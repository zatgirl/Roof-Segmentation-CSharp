using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GlmSharp;
using OSGeo.GDAL;

namespace GdalGeotiff
{
    /// <summary>
    /// Um GeoTIFF é um formato de arquivo .tif que inclui informações espaciais
    /// adicionais (georreferenciamento) incorporadas.
    /// </summary>
    public class GeoTiffRasterFile
    {
        public bool IsOpen { get; private set; }
        public int Width => dataset.RasterXSize;
        public int Height => dataset.RasterYSize;
        public int BandsCount => dataset.RasterCount;
        public string Projection => dataset.GetProjection();
        public double NoDataValue { get { dataset.GetRasterBand(1).GetNoDataValue(out double val, out _); return val; } }
        public double MinValue => minValue;
        public double MaxValue => maxValue;
        public double Mean => mean;
        public double StdDev => stdDev;
        public int HashNoDataValue { get { dataset.GetRasterBand(1).GetNoDataValue(out _, out int has); return has; } }
        public int HashMinValue { get { dataset.GetRasterBand(1).GetMinimum(out _, out int has); return has; } }
        public int HashMaxValue { get { dataset.GetRasterBand(1).GetMaximum(out _, out int has); return has; } }
        public ivec2 RasterSize => new ivec2(dataset.RasterXSize, dataset.RasterYSize);
        public dvec2 PixelSizeGeo => new dvec2(Geotransforms[1], Geotransforms[5]);
        public dvec2 GeographicMin => new dvec2(Geotransforms[0], Geotransforms[3] + Geotransforms[5] * Height);
        public DataType DataType => dataset.GetRasterBand(1).DataType;
        public double[] Geotransforms { get; private set; }

        private readonly Dataset dataset;

        private double minValue = double.NaN;
        private double maxValue = double.NaN;
        private double mean = double.NaN;
        private double stdDev = double.NaN;

        #region CACHE
        private const int CacheCount = 64;
        private const int CacheBlockSize = 512;
        private List<(int blockID, float[] blockData)> cache = new List<(int, float[])>(CacheCount);
        private int cacheHitCounter = 0;
        private int cacheMissCounter = 0;
        private int cacheConsecutiveMissCounter = 0;
        private int cacheMaxConsecutiveMiss = 0;

        private int GetCacheBlockID(int pixelX, int pixelY)
        {
            if (pixelX < 0 || pixelX >= Width || pixelY < 0 || pixelY >= Height)
            {
                throw new Exception(string.Format("Pixel outside the GeoTiff area. ({0}, {1}) / ({2}, {3})", pixelX, pixelY, Width - 1, Height - 1));
            }

            int xCount = (int)Math.Ceiling((float)Width / CacheBlockSize);
            int idX = pixelX / CacheBlockSize;
            int idY = pixelY / CacheBlockSize;
            return idY * xCount + idX;
        }

        private float[] GetFromCache(int pixelX, int pixelY)
        {
            int blockID = GetCacheBlockID(pixelX, pixelY);
            (int id, float[] data) cacheBlock = cache.FirstOrDefault(e => e.blockID == blockID);
            if (cacheBlock.data != null)
            {
                cacheHitCounter++;
                cacheConsecutiveMissCounter = 0;
                return cacheBlock.data;
            }
            cacheMissCounter++;
            cacheConsecutiveMissCounter++;
            cacheMaxConsecutiveMiss = glm.Max(cacheMaxConsecutiveMiss, cacheConsecutiveMissCounter);

            float[] blockData = new float[CacheBlockSize * CacheBlockSize];

            int xOff = pixelX / CacheBlockSize * CacheBlockSize;
            int yOff = pixelY / CacheBlockSize * CacheBlockSize;

            int sizeX = glm.Min(CacheBlockSize, Width - xOff);
            int sizeY = glm.Min(CacheBlockSize, Height - yOff);

            ReadGeoTiffBlock(xOff, yOff, sizeX, sizeY, ref blockData, CacheBlockSize, CacheBlockSize);

            StoreInCache(blockID, blockData);

            return blockData;
        }

        private void StoreInCache(int blockID, float[] data)
        {
            if (cache.Count >= CacheCount)
            {
                cache.RemoveAt(0);
            }

            cache.Add((blockID, data));
        }

        public void PrintsCacheStats()
        {
            //Debug.Log(string.Format("Cache Hits: {0} | Cache Misses: {1} | Cache Max Consecutive Misses: {2}", cacheHitCounter, cacheMissCounter, cacheMaxConsecutiveMiss));
        }
        #endregion

        public static GeoTiffRasterFile Create(string path, GeoTiffRasterFile file)
        {
            //Debug.Assert(path != null);
            //Debug.Assert(file != null);

            return Create(path, file.Width, file.Height, file.Geotransforms, file.Projection, file.NoDataValue, file.DataType);
        }

        public static GeoTiffRasterFile Create(string path, int width, int height, double[] adfGeoTransform, string projection, double noDataValue, DataType dataType)
        {
            //Debug.Assert(path != null);
            //Debug.Assert(width > 0);
            //Debug.Assert(height > 0);
            //Debug.Assert(adfGeoTransform != null);
            //Debug.Assert(projection != null);

            try
            {
                Gdal.AllRegister();
                Driver drv = Gdal.GetDriverByName("GTiff");
                if (drv == null)
                {
                    throw new Exception("Can't get driver!");
                }

                path = Encoding.UTF8.GetString(Encoding.Default.GetBytes(path));
                string[] options = { /*"TILED=YES", "BLOCKXSIZE=128", "BLOCKYSIZE=128",*/ "BIGTIFF=YES", "COMPRESS=DEFLATE" };
                Dataset ds = drv.Create(path, width, height, 1, dataType, options);
                if (ds == null)
                {
                    throw new Exception("Can't create geotiff!");
                }

                ds.SetGeoTransform(adfGeoTransform);
                ds.SetProjection(projection);

                Band b = ds.GetRasterBand(1);
                b.SetNoDataValue(noDataValue);
                b.SetRasterColorInterpretation(ColorInterp.GCI_GrayIndex);
                drv.Dispose();

                GeoTiffRasterFile file = new GeoTiffRasterFile(ds);

                return file;
            }
            catch (Exception e)
            {
                throw new Exception("Application error: " + e.Message);
            }
        }

        public static GeoTiffRasterFile Open(string path)
        {
            //Debug.Assert(path != null);

            if (!File.Exists(path))
            {
                //Debug.LogError("File \'" + path + "\' not found!");
                return null;
            }

            try
            {
                Gdal.AllRegister();
                path = Encoding.UTF8.GetString(Encoding.Default.GetBytes(path));
                Dataset ds = Gdal.Open(path, Access.GA_ReadOnly);
                if (ds == null)
                {
                    throw new Exception("Can't open geotiff!");
                }

                GeoTiffRasterFile file = new GeoTiffRasterFile(ds);
                return file;
            }
            catch (Exception e)
            {
                throw new Exception("Application error: " + e.Message);
            }
        }

        public GeoTiffRasterFile(Dataset dataset)
        {
            this.dataset = dataset ?? throw new Exception("Null dataset!");
            Geotransforms = new double[6];
            dataset.GetGeoTransform(Geotransforms);
            //Debug.Assert(Geotransforms[2] == 0.0 && Geotransforms[4] == 0.0, "File is not north up!");
            IsOpen = true;
        }

        public void WriteGeotiffBlock(int xOff, int yOff, int xSize, int ySize, float[] arr, int arrXSize, int arrYSize)
        {
            if (!IsOpen)
            {
                throw new Exception("Closed file");
            }

            if (dataset == null)
            {
                throw new Exception("Invalid dataset");
            }
            try
            {
                int[] iBandMap = new int[] { 1 };

                //http://gdal.org/python/osgeo.gdal.Dataset-class.html
                CPLErr err = dataset.WriteRaster(xOff, yOff, xSize, ySize, arr, arrXSize, arrYSize, 1, iBandMap, 0, 0, 0);

                if (err != CPLErr.CE_None)
                {
                    throw new Exception(string.Format("Error while writing block! :: {0} | xOff: {1} | yOff: {2} | xSize: {3} | ySize: {4} ",
                        err.ToString(), xOff, yOff, xSize, ySize));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Writing error: " + e.Message);
            }
        }

        public void WriteGeotiffBlock(int xOff, int yOff, int xSize, int ySize, int[] arr, int arrXSize, int arrYSize)
        {
            if (!IsOpen)
            {
                throw new Exception("Closed file");
            }

            if (dataset == null)
            {
                throw new Exception("Invalid dataset");
            }
            try
            {
                int[] iBandMap = new int[] { 1 };

                //http://gdal.org/python/osgeo.gdal.Dataset-class.html
                CPLErr err = dataset.WriteRaster(xOff, yOff, xSize, ySize, arr, arrXSize, arrYSize, 1, iBandMap, 0, 0, 0);

                if (err != CPLErr.CE_None)
                {
                    throw new Exception(string.Format("Error while writing block! :: {0} | xOff: {1} | yOff: {2} | xSize: {3} | ySize: {4} ",
                        err.ToString(), xOff, yOff, xSize, ySize));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Writing error: " + e.Message);
            }
        }

        public void ReadGeoTiffBlock(int xOff, int yOff, int xSize, int ySize, ref float[] arr, int arrXSize, int arrYSize)
        {
            if (!IsOpen)
            {
                throw new Exception("Closed file");
            }

            //Debug.Assert(arr != null && (xSize * ySize) == arr.Length, "Invalid data array");

            int[] iBandMap = new int[] { 1 };

            if (dataset == null)
            {
                throw new Exception("Invalid dataset");
            }

            try
            {
                CPLErr err = dataset.ReadRaster(xOff, yOff, xSize, ySize, arr, arrXSize, arrYSize, 1, iBandMap, 0, 0, 0);
                if (err != CPLErr.CE_None)
                {
                    throw new Exception(string.Format("Error while reading block! :: {0} | xOff: {1} | yOff: {2} | xSize: {3} | ySize: {4} ",
                        err.ToString(), xOff, yOff, xSize, ySize));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Reading error: " + e.Message);
            }
        }

        public void ReadGeoTiffBlock(int xOff, int yOff, int xSize, int ySize, ref int[] arr, int arrXSize, int arrYSize)
        {
            if (!IsOpen)
            {
                throw new Exception("Closed file");
            }

            //Debug.Assert(arr != null && (xSize * ySize) == arr.Length, "Invalid data array");

            int[] iBandMap = new int[] { 1 };

            if (dataset == null)
            {
                throw new Exception("Invalid dataset");
            }

            try
            {
                CPLErr err = dataset.ReadRaster(xOff, yOff, xSize, ySize, arr, arrXSize, arrYSize, 1, iBandMap, 0, 0, 0);
                if (err != CPLErr.CE_None)
                {
                    throw new Exception(string.Format("Error while reading block! :: {0} | xOff: {1} | yOff: {2} | xSize: {3} | ySize: {4} ",
                        err.ToString(), xOff, yOff, xSize, ySize));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Reading error: " + e.Message);
            }
        }

        public float GetPixel(int x, int y)
        {
            float[] data = GetFromCache(x, y);

            x = x % CacheBlockSize;
            y = y % CacheBlockSize;

            return data[y * CacheBlockSize + x];
        }

        public void Close(bool buildOverviews, bool computeStatistics)
        {
            if (!IsOpen)
            {
                throw new Exception("Closed file!");
            }

            if (buildOverviews)
            {
                BuildOverviews();
            }

            if (computeStatistics)
            {
                ComputeStatistics();
            }

            dataset.FlushCache();
            dataset.Dispose();
            IsOpen = false;
            //Debug.Log("File closed.");
        }

        public void BuildOverviews()
        {
            try
            {
                const int BlockSize = 128;
                int minRasterSize = Math.Min(Width, Height);
                int overviewCount = (int)MathF.Floor(MathF.Log((float)minRasterSize / BlockSize, 2.0f));

                if (overviewCount <= 0)
                {
                    //Debug.Log("No overviews build.");
                    return;
                }

                string overviewText = string.Empty;
                List<int> overviewList = new List<int>();
                for (int i = 1; i <= overviewCount; i++)
                {
                    int ov = glm.Pow(2, i);
                    overviewList.Add(ov);
                    overviewText += ov;
                    if (i < overviewCount)
                    {
                        overviewText += ", ";
                    }
                }

                CPLErr err = (CPLErr)dataset.BuildOverviews("CUBICSPLINE", overviewList.ToArray());

                if (err != CPLErr.CE_None)
                {
                    throw new Exception(err.ToString());
                }

                //Debug.Log("Overviews built: " + overviewText);
            }
            catch (Exception e)
            {
                //Debug.LogError("Error building overviews! :: " + e.Message);
            }
        }

        public void ComputeStatistics()
        {
            try
            {
                CPLErr err = dataset.GetRasterBand(1).ComputeStatistics(false, out minValue, out maxValue, out mean, out stdDev, null, null);

                if (err != CPLErr.CE_None)
                {
                    throw new Exception(err.ToString());
                }

                //Debug.Log("File statistics: MIN: " + minValue + " | MAX: " + maxValue + " | MEAN: " + mean + " | STDDEV: " + stdDev);
            }
            catch (Exception e)
            {
                //Debug.LogError("Error computing file statistics! :: " + e.Message);
            }
        }
    }
}