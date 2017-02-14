using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;

namespace YeelightHelper
{
    public class YeelightCommand
    {
        private string _ipAddress = string.Empty;

        public YeelightCommand(string ip, int port = 55443)
        {
            _ipAddress = ip;
        }

        public YeelightStatusResponse GetAllStatus()
        {
            YeelightJsonPayload jsonPayload = new YeelightJsonPayload()
            {
                PayloadId = 1,
                PayloadMethod = "get_prop"
            };

            jsonPayload.PayloadParameters.AddRange(
                new string[] 
                {
                    "id",
                    "model",
                    "fw_ver",
                    "power",
                    "bright",
                    "color_mode",
                    "ct",
                    "rgb",
                    "hue",
                    "sat",
                    "name"
                });

            // Convert the payload to JSON and wait to send
            string jsonPayloadStr = JsonConvert.SerializeObject(jsonPayload);

            // Send and wait for response
            string responseJsonStr = YeelightTcpHandler.SendAndReceive(jsonPayloadStr, _ipAddress);

            // Create a new received JSON object buffer and deserialize into this object
            YeelightJsonPayload responseJson = JsonConvert.DeserializeObject<YeelightJsonPayload>(responseJsonStr);

            // Get the response array and write to StatusResponse object
            YeelightStatusResponse statusResponse = new YeelightStatusResponse()
            {
                DeviceId = responseJson.PayloadParameters[0].ToString(),
                Model = (YeelightModel)(Convert.ToInt32(responseJson.PayloadParameters[1])),
                FirmwareVersion = Convert.ToInt32(responseJson.PayloadParameters[2]),
                IsPowerOn = (responseJson.PayloadParameters[3].ToString() == "on"),
                Brightness = Convert.ToInt32(responseJson.PayloadParameters[4]),
                ColorTemperature = Convert.ToInt32(responseJson.PayloadParameters[5]),
                RGBValue = Convert.ToInt32(responseJson.PayloadParameters[6]),
                HueValue = Convert.ToInt32(responseJson.PayloadParameters[7]),
                SaturationValue = Convert.ToInt32(responseJson.PayloadParameters[8]),
                DeviceName = responseJson.PayloadParameters[9].ToString(),
            };

            return statusResponse;
        }

        /// <summary>
        /// Set the color temperature
        /// </summary>
        /// <param name="colorTemp">Color temperature, from 1700 to 6500</param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public bool SetColorTemperature(int colorTemp, string effect, int duration)
        {
            return AdjustColorOrPower(colorTemp, effect, duration, "set_ct_abx");
        }

        /// <summary>
        /// Set the color in RGB mode
        /// </summary>
        /// <param name="rgbValue"></param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public bool SetColorInRGB(int rgbValue, string effect, int duration)
        {
            return AdjustColorOrPower(rgbValue, effect, duration, "set_rgb");
        }

        /// <summary>
        /// Set the color in HSV mode
        /// </summary>
        /// <param name="hueValue"></param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public bool SetColorInHSV(int hueValue, string effect, int duration)
        {
            return AdjustColorOrPower(hueValue, effect, duration, "set_hsv");
        }

        /// <summary>
        /// Set the brightness
        /// </summary>
        /// <param name="brightness">Brightness from 1 to 100</param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public bool SetBrightness(int brightness, string effect, int duration)
        {
            return AdjustColorOrPower(brightness, effect, duration, "set_bright");
        }

        /// <summary>
        /// Power on or off the light bulb
        /// </summary>
        /// <param name="isPowerOn"></param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public bool SetPower(bool isPowerOn, string effect, int duration)
        {
            string isPowerOnStr = isPowerOn ? "on" : "off";
            return SendSettingsAndWaitResult("set_power", new List<object> { isPowerOnStr, effect, duration });
        }

        /// <summary>
        /// Toggle the Yeelight
        /// </summary>
        /// <returns>Return true if it gets a successful response</returns>
        public bool Toggle()
        {
            return SendSettingsAndWaitResult("toggle", new List<object> { });
        }

        /// <summary>
        /// Set current status to default
        /// </summary>
        /// <returns></returns>
        public bool SetDefault()
        {
            return SendSettingsAndWaitResult("set_default", new List<object> { });
        }

        /// <summary>
        /// Set and run a active color flow
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <param name="flowExpression"></param>
        /// <returns></returns>
        public bool StartColorFlow(YeelightAction action, List<ColorFlowType> flows)
        {
            return SendSettingsAndWaitResult("start_cf", GetColorFlowParameters(action, flows));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StopColorFlow()
        {
            return SendSettingsAndWaitResult("stop_cf", new List<object> { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rgbColor"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public bool SetSceneInRgb(int rgbColor, int brightness)
        {
            return SendSettingsAndWaitResult("set_scene", 
                new List<object> { "color", rgbColor, brightness });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorTempValue"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public bool SetSceneInColorTemp(int colorTempValue, int brightness)
        {
            return SendSettingsAndWaitResult("set_scene", 
                new List<object> { "color", colorTempValue, brightness });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hueValue"></param>
        /// <param name="saturationValue"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public bool SetSceneInHsv(int hueValue, int saturationValue, int brightness)
        {
            return SendSettingsAndWaitResult("set_scene", 
                new List<object> { "hsv", hueValue, saturationValue, brightness });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="flows"></param>
        /// <returns></returns>
        public bool SetSceneInColorFlow(YeelightAction action, List<ColorFlowType> flows)
        {
            return SendSettingsAndWaitResult("set_scene",
               new List<object> { "cf", GetColorFlowParameters(action, flows)});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brightness"></param>
        /// <param name="timerInMinutes"></param>
        /// <returns></returns>
        public bool SetSceneInAutoOff(int brightness, int timerInMinutes)
        {
            return SendSettingsAndWaitResult("set_scene",
                new List<object> { "auto_delay_off", brightness, timerInMinutes });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timerInMinutes"></param>
        /// <returns></returns>
        public bool AddCron(CronType type, int timerInMinutes)
        {
            return SendSettingsAndWaitResult("cron_add", new List<object> { (int)type, timerInMinutes });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CronJsonResult> GetCron(CronType type)
        {
            YeelightJsonPayload jsonPayload = SendSettingAndWaitJsonPayload("cron_get", new List<object> { (int)type });
            List<CronJsonResult> cronJsonResults = new List<CronJsonResult>();

            foreach (string jsonResult in jsonPayload.PayloadParameters.OfType<string>())
            {
                CronJsonResult cronJsonResult = JsonConvert.DeserializeObject<CronJsonResult>(jsonResult);
                cronJsonResults.Add(cronJsonResult);
            }

            return cronJsonResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool DeleteCron(CronType type)
        {
            return SendSettingsAndWaitResult("cron_del", new List<object> { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public bool AdjustSetting(string action, string prop)
        {
            return SendSettingsAndWaitResult("set_adjust", new List<object> { action, prop });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="hostIP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool SetMusic(MusicSettingAction action, string hostIP, string port)
        {
            return SendSettingsAndWaitResult("set_music", new List<object> { (int)action, hostIP, port });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SetName(string name)
        {
            return SendSettingsAndWaitResult("set_name", new List<object> { name });
        }

        private bool AdjustColorOrPower(int value, string effect, int duration, string payloadMethod)
        {
            return SendSettingsAndWaitResult(payloadMethod, new List<object> { value, effect, duration });
        }

        private List<object> GetColorFlowParameters(YeelightAction action, List<ColorFlowType> flows)
        {
            string flowExpStr = string.Empty;

            foreach (ColorFlowType flowArray in flows)
            {
                flowExpStr += flowArray.Duration.ToString() + ",";
                flowExpStr += flowArray.Mode.ToString() + ",";
                flowExpStr += flowArray.Value.ToString() + ",";
                flowExpStr += flowArray.Brightness.ToString() + ",";
            }

            // Remove the last comma (,)
            flowExpStr.Remove(flowExpStr.Length - 1);

            return (new List<object> { flows.Count, (int)action, flowExpStr });
        }
        

        private bool SendSettingsAndWaitResult(string payloadMethod, List<object> payloadParameter)
        {
            YeelightJsonPayload responseJson = SendSettingAndWaitJsonPayload(payloadMethod, payloadParameter);

            if (responseJson.PayloadParameters.Contains("ok"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private YeelightJsonPayload SendSettingAndWaitJsonPayload(string payloadMethod, List<object> payloadParameter)
        {
            YeelightJsonPayload jsonPayload = new YeelightJsonPayload()
            {
                PayloadId = 1,
                PayloadMethod = payloadMethod
            };

            jsonPayload.PayloadParameters.AddRange(payloadParameter);

            // Convert the payload to JSON and wait to send
            string jsonPayloadStr = JsonConvert.SerializeObject(jsonPayload);

            // Send and wait for response
            string responseJsonStr = YeelightTcpHandler.SendAndReceive(jsonPayloadStr, _ipAddress);

            // Create a new received JSON object buffer and deserialize into this object
            YeelightJsonPayload responseJson = JsonConvert.DeserializeObject<YeelightJsonPayload>(responseJsonStr);

            return responseJson;
        }
    }
}
