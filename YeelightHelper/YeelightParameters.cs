using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeelightHelper
{
    public class YeelightParameters
    {
        public static readonly string GetProperties = "get_prop";
        public static readonly string SetColorTemp = "set_ct_abx";
        public static readonly string SetRGB = "set_rgb";
        public static readonly string SetHSV = "set_hsv";
        public static readonly string SetBright = "set_bright";
        public static readonly string SetPower = "set_power";
        public static readonly string Toggle = "toggle";
        public static readonly string SetDefault = "set_default";
        public static readonly string StartColorFlow = "set_cf";
        public static readonly string StopColorFlow = "stop_cf";
        public static readonly string SetScene = "set_scene";
        public static readonly string AddCron = "cron_add";
        public static readonly string GetCron = "cron_get";
        public static readonly string DeleteCron = "cron_del";
        public static readonly string AdjustSetting = "set_adjust";
        public static readonly string SetMusic = "set_music";
        public static readonly string SetName = "set_name";
    }
}
