using System;
using GlmSharp;
using OSGeo.GDAL;

namespace GdalGeotiff
{
    /// <summary>
    /// Estrutura que contém o Header Geotiff.
    /// </summary>
    public struct GeotiffAttribs
    {
        public string projection;
        public double[] adfGeoTransform;

        //Raster params.  
        public int xSize, ySize;   //no imagem sem overview
        public int overviewCount;
        public int bandCount;    //1 = gray, 3 = RGB, 4 = RGBX
        public DataType bandDataType; //float, int, uint, byte, double,  etc. 
        public string bandDataTypeString;
        public string bandColorInterpretationString; //nome de cada banda
        public int bandBytesPerSample; //bytes em cada componente da banda
        public double bandMinValue;
        public double bandMaxValue;
        public double noDataValue;
        public int hasNoDataValue;
        public int hasPalette;
        public string compressionType;
    }

    /// <summary>
    /// Classe para fazer a leitura de arquivos Geotiff dos mais variados
    /// formatos.
    /// </summary>
    /// <remarks>
    /// https://github.com/TUW-GEO/OGRSpatialRef3D/tree/master/gdal-1.10.0/swig/csharp/apps
    /// https://trac.osgeo.org/gdal/wiki/GdalOgrCsharpRaster
    /// </remarks>
    /// <author="Cesar Tadeu Pozzer"></author>
    /// <mod="23/06/2019"></mod>
    public class GDALGeotiffReader
    {
        public GeotiffAttribs geotiffAttribs;
        public Dataset ds;
        private bool isOpen = false;
        public string fileName;

        //abre o arquivo geotiff e le todos os atributos. 
        public GDALGeotiffReader(string _fileName)
        {
            fileName = _fileName;
            try
            {
                
                Gdal.AllRegister();
                ds = Gdal.Open(_fileName, Access.GA_Update);

                if (ds == null)
                {
                    throw new Exception("Can't open " + _fileName);
                }

                ReadAllAttribs();

                //UIDebug.LogWarning("O arquivo sera lido em formato float[]. Poderia ser em outro formato dependendo do tipo do arquivo");

                isOpen = true;
            }
            catch (Exception e)
            {
                throw new Exception("Application error: " + e.Message);
            }
        }

        public void Dispose()
        {
            ds.Dispose();
        }

        private void ReadAllAttribs()
        {
            geotiffAttribs.adfGeoTransform = new double[6];

            geotiffAttribs.projection = ds.GetProjectionRef();
            geotiffAttribs.bandCount = ds.RasterCount;
            geotiffAttribs.xSize = ds.RasterXSize;
            geotiffAttribs.ySize = ds.RasterYSize;

            ds.GetRasterBand(1).GetNoDataValue(out geotiffAttribs.noDataValue, out geotiffAttribs.hasNoDataValue);
            ds.GetGeoTransform(geotiffAttribs.adfGeoTransform);

            geotiffAttribs.overviewCount = ds.GetRasterBand(1).GetOverviewCount(); //atributo somente disponivel dentro bandas. 
            geotiffAttribs.bandDataType = ds.GetRasterBand(1).DataType;

            int bits = Gdal.GetDataTypeSize(geotiffAttribs.bandDataType);
            if (bits % 8 != 0)
            {
                throw new Exception("Numero de bits nao multiplo de 8");
            }

            geotiffAttribs.bandBytesPerSample = bits / 8; //A funcao retorna o numero de bits/sample. 

            //cor em cada banda 
            geotiffAttribs.bandColorInterpretationString = "( ";
            for (int iBand = 1; iBand <= ds.RasterCount; iBand++)
            {
                Band band = ds.GetRasterBand(iBand);
                if (iBand == 1)
                {
                    geotiffAttribs.bandColorInterpretationString += Gdal.GetColorInterpretationName(band.GetRasterColorInterpretation());
                }
                else
                {
                    geotiffAttribs.bandColorInterpretationString += "  " + Gdal.GetColorInterpretationName(band.GetRasterColorInterpretation());
                }
            }
            geotiffAttribs.bandColorInterpretationString += " )";

            geotiffAttribs.bandDataTypeString = Gdal.GetDataTypeName(geotiffAttribs.bandDataType);

            //Parametro de compressao da imagem. 
            string[] metadata = ds.GetMetadata("IMAGE_STRUCTURE");
            if (metadata.Length > 0)
            {
                for (int iMeta = 0; iMeta < metadata.Length; iMeta++)
                {
                    string str = metadata[iMeta];
                    if (str.Contains("COMPRESSION"))
                    {
                        geotiffAttribs.compressionType = metadata[iMeta];
                    }
                }
            }

            //paleta de cores
            geotiffAttribs.hasPalette = 0;
            if (ds.GetRasterBand(1).GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
            {
                geotiffAttribs.hasPalette = 1;
            }
        }

        //imprime e retorna os atributos do arquivo. 
        public string PrintAttribs()
        {
            string overviewsMsg = "";
            if (geotiffAttribs.overviewCount == 0)
            {
                overviewsMsg = "  -> Imagem nao pode ser lida com Overviews (GetRasterBand().GetOverview())";
            }

            string attribs = "GeoTiff attribs: " + fileName +
                      "\nSize: (" + geotiffAttribs.xSize + ", " + geotiffAttribs.ySize + ")" +
                      "\nOverviews: " + geotiffAttribs.overviewCount + overviewsMsg +
                      "\nBands: " + geotiffAttribs.bandCount +
                      "\nbandDataType: " + geotiffAttribs.bandDataTypeString +
                      "\nbandBytesPerSample: " + geotiffAttribs.bandBytesPerSample +
                      "\nbandColorInterpretation: " + geotiffAttribs.bandColorInterpretationString +
                      "\nHasNoData: " + geotiffAttribs.hasNoDataValue +
                      "\nNoDataValue:  " + geotiffAttribs.noDataValue +
                      "\nHasPalette:  " + geotiffAttribs.hasPalette +
                      "\nGeoTransforms coords: (" + geotiffAttribs.adfGeoTransform[0] + ", " + geotiffAttribs.adfGeoTransform[3] + ")   pixelSize: (" + geotiffAttribs.adfGeoTransform[1].ToString("F9") + ", " + geotiffAttribs.adfGeoTransform[5].ToString("F9") + ")   rotation: (" + geotiffAttribs.adfGeoTransform[2] + ", " + geotiffAttribs.adfGeoTransform[4] + ")" +
                      "\nCompressionType: " + geotiffAttribs.compressionType +
                      "\nProjection: " + geotiffAttribs.projection +
                      "\n ";

            return attribs;
        }


        //le area selecionada do arquivo e retorna cada componente em um vetor de float, que ja deve estar alocado. 
        //OBS: o tipo float tem representacao precisa para os seguintes formados de dados do GeoTiff:
        //     - GDT_Byte
        //     - GDT_Int16
        //     - GDT_UInt16
        //     - GDT_Float32
        public void Read(float[] arrFloat, int xOffset, int yOffset, int xSize, int ySize, int xBuffSize, int yBuffSize)
        {
            if (isOpen == false)
            {
                throw new Exception("Arquivo Geotiff nao carregado");
            }

            if (xOffset + xSize > geotiffAttribs.xSize)
            {
                throw new ArgumentOutOfRangeException("Read: xOffset + xSize > Geotiff.XSize");
            }
            if (yOffset + ySize > geotiffAttribs.ySize)
            {
                throw new ArgumentOutOfRangeException("Read: yOffset + ySize > Geotiff.YSize");
            }

            //verificacao do tipo de dado usado para leitura. Nao se pode usar um tipo com menos bits que o presente no arquivo geotiff. 
            if (geotiffAttribs.bandDataType == DataType.GDT_Byte)
            {
                //pode ler com os tipos byte, short, int, float ou double do C#
                //DebugLogWarning("Warning: Lendo arquivo em formato float[]. Poderia ser usado o tipo byte[]");
            }
            else if (geotiffAttribs.bandDataType == DataType.GDT_Int16)
            {
                //pode ler com os tipos short, int, float ou double do C#
                //DebugLogWarning("Warning: Lendo arquivo em formato float[]. Poderia ser usado o tipo short[]");
            }
            else if (geotiffAttribs.bandDataType == DataType.GDT_UInt16)
            {
                //pode ler com os tipos float ou double do C#
                //DebugLogWarning("Warning: Lendo arquivo em formato float[]. Poderia ser usado o tipo int[]");
            }
            else if (geotiffAttribs.bandDataType == DataType.GDT_Float32) //FORMATO DEFAULT DE LEITURA DE TODOS OS ARQUIVOS GEOTIFF
            {
                //pode ler com os tipos float ou double do C#
                //DebugLogWarning("Warning: Lendo arquivo em formato float[]");
            }
            else if (geotiffAttribs.bandDataType == DataType.GDT_Int32)
            {
                //pode ler com o tipo int ou double do C#
                throw new Exception("Erro: O arquivo deve ser lido com formato int ou double para nao perder precisao");
            }
            else if (geotiffAttribs.bandDataType == DataType.GDT_Int32 || geotiffAttribs.bandDataType == DataType.GDT_UInt32 || geotiffAttribs.bandDataType == DataType.GDT_Float64)
            {
                //pode ler somente com o tipo double do C#
                throw new Exception("Erro: O arquivo deve ser lido com formato double para nao perder precisao");
            }
            else
            {
                //formatos complexos 
                throw new Exception("Erro: Formato nao suportado: GDT_CInt16, GDT_CInt32, GDT_CFloat64");
            }

            //escolhe a leitura pelo numero de bandas e nao pelo tipo de cada banda (muitos arquivos possuem bandas do tipo unknown e sao RGB). 
            if (geotiffAttribs.bandCount == 3 || geotiffAttribs.bandCount == 4)
            {
                int[] iBandMap = { 1, 2, 3 };
                if (ds.GetRasterBand(1).GetRasterColorInterpretation() != ColorInterp.GCI_RedBand ||
                    ds.GetRasterBand(2).GetRasterColorInterpretation() != ColorInterp.GCI_GreenBand ||
                    ds.GetRasterBand(3).GetRasterColorInterpretation() != ColorInterp.GCI_BlueBand)
                {
                    //Debug.LogErrorFormat(" Warning: Imagem com {0} bandas, porem nao em formato RGB. O arquivo sera lido como RGB", geotiffAttribs.bandCount);
                }
                //Debug.Log("Vai ler em formato RGB");
                ReadBands(arrFloat, iBandMap, xOffset, yOffset, xSize, ySize, xBuffSize, yBuffSize);
            }
            //GrayScale ou palette
            else if (geotiffAttribs.bandCount == 1)
            {
                if (ds.GetRasterBand(1).GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
                {
                    int[] iBandMap = { 1 };
                    ReadBands(arrFloat, iBandMap, xOffset, yOffset, xSize, ySize, xBuffSize, yBuffSize);
                }
                else if (ds.GetRasterBand(1).GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
                {
                    //DebugLog("Vai ler em formato Paleta de cores");
                    ReadBandPalette(arrFloat, xOffset, yOffset, xSize, ySize, xBuffSize, yBuffSize);
                }
                else
                {
                    throw new Exception("GdalGeotiff: ColorInterp nao suportado no arquivo geotiff de entrada");
                }
            }
            else
            {
                throw new Exception("Erro: Formato desconhecido");
            }
        }

        public int PixelBandCount()
        {
            //Ex: arquivos em formato RFloat32 (DEM) ou Grayscale 8 bits . 
            if (geotiffAttribs.hasPalette == 0 && geotiffAttribs.bandCount == 1)
            {
                return 1;
            }

            //todo arquivo com paleta tem 1 banda, mas por default gera-se 3 bandas RGB no arquivo de saida
            if (geotiffAttribs.hasPalette == 1)
            {
                return 3; //
            }

            //arquivos RGBx sao convertidos para RGB
            if (geotiffAttribs.bandCount == 3 || geotiffAttribs.bandCount == 4)
            {
                return 3;  //ignora a quarta, caso existir
            }

            throw new Exception("Nao calculou o numero de Componentes");
        }

        //Numero de componentes de cor de toda imagem que a funcao Read() vai retornar. 
        //Se a imagem for RGB,     retorna x * y * 3. 
        //Se a imagem for RFloat,  retorna x * y * 1. 
        //Se a imagem for Pallete, retorna x * y * 3. 
        public long ImageComponentsCount(int xBuffSize, int yBuffSize)
        {
            return (long)xBuffSize * yBuffSize * PixelBandCount();
        }

        //Le os Canais indicados em iBandMap[]
        //se o geotiff for lido com ds.ReadRaster() em 3 bandas, os dados ficam organizados em um unico vetor da seguinte forma: [RRRRRR GGGGGG BBBBBB]
        //se o geotiff for lido com xBand.ReadRaster() em 3 bandas, os dados ficam organizados em tres vetores da seguinte forma: [RRRRRR] [GGGGGG] [BBBBBB]
        //Se o dado do geotiff for GDT_UInt16, deve-se utilizar um formato do C#/Gdal que suporte 16 bits com sinal, nesse caso somente int, float ou double. O formato short nao pode ser usado. 
        //Essa funcao esta lendo sempre em formato float, mas poderia ser outro formato. Cada componente da imagem eh armazenada em uma posicao do array. 
        //Se (xBuffSize,yBuffSize) forem diferentes de (xSize,ySize), a imagem eh reescalada pelo proprio GDAL. 
        private void ReadBands(float[] arrFloat, int[] iBandMap, int xOffset, int yOffset, int xSize, int ySize, int xBuffSize, int yBuffSize)
        {
            //leitura em Float32
            int componentsCount = xBuffSize * yBuffSize * iBandMap.Length;
            int pixelCount = xBuffSize * yBuffSize;
            float[] floatArray = new float[componentsCount];
            ds.ReadRaster(xOffset, yOffset, xSize, ySize, floatArray, xBuffSize, yBuffSize, iBandMap.Length, iBandMap, 0, 0, 0);
            ProcessBands(arrFloat, floatArray, pixelCount, iBandMap.Length);
        }

        //faz a leitura de geotiff com paleta de cores. Usa os mesmos argumentos da funcao ReadBands()
        private void ReadBandPalette(float[] arr, int xOffset, int yOffset, int xSize, int ySize, int xBuffSize, int yBuffSize)
        {
            Band band = ds.GetRasterBand(1);
            ColorTable ct = band.GetRasterColorTable();
            if (ct == null)
            {
                throw new Exception("Band has no color table!");
            }

            DebugLog("ColorTable entries: " + ct.GetCount() + "  Interpret: " + Gdal.GetPaletteInterpretationName(ct.GetPaletteInterpretation()));

            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                throw new Exception("Esta implementacao somente le paleta de cores em RGB. Eh facil adaptar para Grayscale ou outro formato");
            }

            int pixelCount = xBuffSize * yBuffSize;
            float[] floatArray = new float[pixelCount];

            //le apenas os indices das cores. 
            band.ReadRaster(xOffset, yOffset, xSize, ySize, floatArray, xBuffSize, yBuffSize, 0, 0);

            //cria uma color tabela mais eficiente
            int[,] colorTable = new int[ct.GetCount(), 3];
            for (int i = 0; i < ct.GetCount(); i++)
            {
                ColorEntry ce = ct.GetColorEntry(i);
                colorTable[i, 0] = ce.c1;
                colorTable[i, 1] = ce.c2;
                colorTable[i, 2] = ce.c3;
            }

            //converte os indices em cores RGB
            int cont = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                arr[cont] = colorTable[(int)floatArray[i], 0];
                arr[cont + 1] = colorTable[(int)floatArray[i], 1];
                arr[cont + 2] = colorTable[(int)floatArray[i], 2];
                cont += 3;
            }
        }

        //Se o arquivo for 3 canais RGB16, os dados sao lidos no formato [RR RR RR  GG GG GG  BB BB BB] e devem ser convertidos para [RRGGBB RRGGBB RRGGBB]
        //Se o arquivo for 3 canais RGB8, os dados sao lidos no formato [R R R R R R   G G G G G G   B B B B B B] e devem ser convertidos para [RGB RGB RGB RGB RGB RGB]
        //Se o arquivo for Float32, os dados nao precisam ser convertidos, apenas copiados para o array de saida. 
        //Se o arquivo for UInt16, os dados nao precisam ser convertidos, apenas copiados para o array de saida. 
        private void ProcessBands(float[] tgt, float[] src, int pixelCount, int bandCount)
        {
            for (int i = 0; i < pixelCount; i++)
            {
                for (int band = 0; band < bandCount; band++)
                {
                    tgt[i * bandCount + band] = src[i + band * pixelCount];
                }
            }
        }

        protected void DebugLogWarning(string msg)
        {
#if EnableWarnings
        Debug.Log(msg);
#endif
        }

        protected void DebugLog(string msg)
        {
#if EnableMessages
        Debug.Log(msg);
#endif
        }

        public string Projection => geotiffAttribs.projection;

        public int XSize => geotiffAttribs.xSize;

        public int YSize => geotiffAttribs.ySize;

        public int OverviewCount => geotiffAttribs.overviewCount;

        public int BandCount
        {
            get => geotiffAttribs.bandCount;
            set => geotiffAttribs.bandCount = value;
        }

        public string BandDataTypeString => geotiffAttribs.bandDataTypeString;

        public DataType BandDataType => geotiffAttribs.bandDataType;

        public int BandBytesPerSample => geotiffAttribs.bandBytesPerSample;

        public double NoDataValue => geotiffAttribs.noDataValue;

        public int HasNoDataValue => geotiffAttribs.hasNoDataValue;

        public int HasPalette => geotiffAttribs.hasPalette;

        public double[] AdGeoTransform => geotiffAttribs.adfGeoTransform;

        public dvec2 GetPosition(double x, double y)
        {
            double dfGeoX, dfGeoY;

            dfGeoX = geotiffAttribs.adfGeoTransform[0] + geotiffAttribs.adfGeoTransform[1] * x + geotiffAttribs.adfGeoTransform[2] * y;
            dfGeoY = geotiffAttribs.adfGeoTransform[3] + geotiffAttribs.adfGeoTransform[5] * y + geotiffAttribs.adfGeoTransform[4] * x;

            return new dvec2(dfGeoX, dfGeoY);
        }

        public void GetGeoTransform(double[] arr)
        {
            ds.GetGeoTransform(arr);
        }
    }
}