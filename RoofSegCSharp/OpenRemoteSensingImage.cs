using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using GdalGeotiff;

namespace RoofSegCSharp
{    
    class OpenRemoteSensingImage
    {
        private static GDALGeotiffReader remoteSensingImageReader = null;
        private string path;

        public void Open(string path)
        {
            remoteSensingImageReader = new GDALGeotiffReader(path);
        }
    }
}
