using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeelightHelper
{
    public sealed class YeelightStatusResponse
    {
        public string           CacheControl              { get; set; }
        public string           Location                  { get; set; }
        public string           DeviceId                  { get; set; }
        public YeelightModel    Model                     { get; set; }
        public int              FirmwareVersion           { get; set; }
        public List<string>     SupportCommand            { get; set; }
        public bool             IsPowerOn                 { get; set; }
        public int              Brightness                { get; set; }
        public ColorMode        ColorMode                 { get; set; }
        public int              ColorTemperature          { get; set; }
        public int              RGBValue                  { get; set; }
        public int              HueValue                  { get; set; }
        public int              SaturationValue           { get; set; }
        public string           DeviceName                { get; set; }              
    }
}
