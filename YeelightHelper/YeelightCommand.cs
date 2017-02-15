using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using LiteGuard;
using System.Text.RegularExpressions;

namespace YeelightHelper
{
    public class YeelightCommand
    {
        private string _ipAddress = string.Empty;

        public YeelightCommand(string ip)
        {
            _ipAddress = ip;
        }

        public async Task<YeelightStatusResponse> GetAllStatus()
        {
            var responseJson = await SendSettingAndWaitJsonPayload("get_prop", new List<object>()
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

            // Get the response array and write to StatusResponse object
            YeelightStatusResponse statusResponse = new YeelightStatusResponse()
            {
                DeviceId = responseJson.PayloadParameters[0]?.ToString(),
                Model = (YeelightModel)Enum.Parse(typeof(YeelightModel), responseJson.PayloadParameters[1].ToString(), true),
                FirmwareVersion = Convert.ToInt32(responseJson.PayloadParameters[2]),
                IsPowerOn = (responseJson.PayloadParameters[3].ToString() == "on"),
                Brightness = Convert.ToInt32(responseJson.PayloadParameters[4]),
            };

            // If the lightbulb is not Single-color (Mono) version, then find out more information
            if(statusResponse.Model != YeelightModel.Mono)
            {
                statusResponse.ColorTemperature = Convert.ToInt32(responseJson.PayloadParameters[5]);
                statusResponse.RGBValue = Convert.ToInt32(responseJson.PayloadParameters[6]);
                statusResponse.HueValue = Convert.ToInt32(responseJson.PayloadParameters[7]);
                statusResponse.SaturationValue = Convert.ToInt32(responseJson.PayloadParameters[8]);
                statusResponse.DeviceName = responseJson.PayloadParameters[9].ToString();
            }

            return statusResponse;
        }

        /// <summary>
        /// Set the color temperature
        /// </summary>
        /// <param name="colorTemp">Color temperature, from 1700 to 6500</param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public async Task<bool> SetColorTemperature(int colorTemp, string effect, int duration)
        {
            return await AdjustColorOrPower(colorTemp, effect, duration, "set_ct_abx");
        }

        /// <summary>
        /// Set the color in RGB mode
        /// </summary>
        /// <param name="rgbValue"></param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public async Task<bool> SetColorInRGB(int rgbValue, string effect, int duration)
        {
            return await AdjustColorOrPower(rgbValue, effect, duration, "set_rgb");
        }

        /// <summary>
        /// Set the color in HSV mode
        /// </summary>
        /// <param name="hueValue"></param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public async Task<bool> SetColorInHSV(int hueValue, string effect, int duration)
        {
            return await AdjustColorOrPower(hueValue, effect, duration, "set_hsv");
        }

        /// <summary>
        /// Set the brightness
        /// </summary>
        /// <param name="brightness">Brightness from 1 to 100</param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public async Task<bool> SetBrightness(int brightness, string effect, int duration)
        {
            return await AdjustColorOrPower(brightness, effect, duration, "set_bright");
        }

        /// <summary>
        /// Power on or off the light bulb
        /// </summary>
        /// <param name="isPowerOn"></param>
        /// <param name="effect">Color changing effect, Smooth or Fast</param>
        /// <param name="duration">Duration in millisecond</param>
        /// <returns>Return true if it gets a successful response</returns>
        public async Task<bool> SetPower(bool isPowerOn, string effect, int duration)
        {
            string isPowerOnStr = isPowerOn ? "on" : "off";
            return await SendSettingsAndWaitResult("set_power", new List<object> { isPowerOnStr, effect, duration });
        }

        /// <summary>
        /// Toggle the Yeelight
        /// </summary>
        /// <returns>Return true if it gets a successful response</returns>
        public async Task<bool> Toggle()
        {
            return await SendSettingsAndWaitResult("toggle", new List<object> {  });
        }

        /// <summary>
        /// Set current status to default
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SetDefault()
        {
            return await SendSettingsAndWaitResult("set_default", new List<object> { });
        }

        /// <summary>
        /// Set and run a active color flow
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <param name="flowExpression"></param>
        /// <returns></returns>
        public async Task<bool> StartColorFlow(YeelightAction action, List<ColorFlowType> flows)
        {
            return await SendSettingsAndWaitResult("start_cf", GetColorFlowParameters(action, flows));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StopColorFlow()
        {
            return await SendSettingsAndWaitResult("stop_cf", new List<object> { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rgbColor"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public async Task<bool> SetSceneInRgb(int rgbColor, int brightness)
        {
            return await SendSettingsAndWaitResult("set_scene", 
                new List<object> { "color", rgbColor, brightness });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorTempValue"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public async Task<bool> SetSceneInColorTemp(int colorTempValue, int brightness)
        {
            return await SendSettingsAndWaitResult("set_scene", 
                new List<object> { "color", colorTempValue, brightness });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hueValue"></param>
        /// <param name="saturationValue"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public async Task<bool> SetSceneInHsv(int hueValue, int saturationValue, int brightness)
        {
            return await SendSettingsAndWaitResult("set_scene", 
                new List<object> { "hsv", hueValue, saturationValue, brightness });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="flows"></param>
        /// <returns></returns>
        public async Task<bool> SetSceneInColorFlow(YeelightAction action, List<ColorFlowType> flows)
        {
            return await SendSettingsAndWaitResult("set_scene",
               new List<object> { "cf", GetColorFlowParameters(action, flows)});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brightness"></param>
        /// <param name="timerInMinutes"></param>
        /// <returns></returns>
        public async Task<bool> SetSceneInAutoOff(int brightness, int timerInMinutes)
        {
            return await SendSettingsAndWaitResult("set_scene",
                new List<object> { "auto_delay_off", brightness, timerInMinutes });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timerInMinutes"></param>
        /// <returns></returns>
        public async Task<bool> AddCron(CronType type, int timerInMinutes)
        {
            return await SendSettingsAndWaitResult("cron_add", new List<object> { (int)type, timerInMinutes });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<CronJsonResult>> GetCron(CronType type)
        {
            var jsonPayload = await SendSettingAndWaitJsonPayload("cron_get", new List<object> { (int)type });
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
        public async Task<bool> DeleteCron(CronType type)
        {
            return await SendSettingsAndWaitResult("cron_del", new List<object> { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public async Task<bool> AdjustSetting(string action, string prop)
        {
            return await SendSettingsAndWaitResult("set_adjust", new List<object> { action, prop });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="hostIP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task<bool> SetMusic(MusicSettingAction action, string hostIP, string port)
        {
            return await SendSettingsAndWaitResult("set_music", new List<object> { (int)action, hostIP, port });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> SetName(string name)
        {
            return await SendSettingsAndWaitResult("set_name", new List<object> { name });
        }

        private async Task<bool> AdjustColorOrPower(int value, string effect, int duration, string payloadMethod)
        {
            return await SendSettingsAndWaitResult(payloadMethod, new List<object> { value, effect, duration });
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
        

        private async Task<bool> SendSettingsAndWaitResult(string payloadMethod, List<object> payloadParameter)
        {
            var responseJson = await SendSettingAndWaitJsonPayload(payloadMethod, payloadParameter);

            if (responseJson.PayloadParameters.Contains("ok"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<YeelightResultJsonPayload> SendSettingAndWaitJsonPayload(string payloadMethod, List<object> payloadParameter)
        {
            YeelightJsonPayload jsonPayload = new YeelightJsonPayload()
            {
                PayloadId = 1,
                PayloadMethod = payloadMethod
            };

            jsonPayload.PayloadParameters = payloadParameter;

            // Convert the payload to JSON and wait to send
            string jsonPayloadStr = JsonConvert.SerializeObject(jsonPayload) + "\r\n";

            // Send and wait for response
            string responseJsonStr = await YeelightTcpHandler.SendAndReceive(jsonPayloadStr, _ipAddress);

            var responseJsonSettings = new JsonSerializerSettings
            {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include
            };

            // Somehow Xiaomi designed their API to return some extra contents, which we don't need that and meanwhile it can also causes exceptions.
            // As a result, here I only reserve the first line of the content, then ignore the rest of them if exists.
            string responseJsonStrCleaned = Regex.Split(responseJsonStr, "\r\n")[0];

            // Create a new received JSON object buffer and deserialize into this object
            var responseJson = JsonConvert.DeserializeObject<YeelightResultJsonPayload>(responseJsonStrCleaned, responseJsonSettings);

            return responseJson;
        }
    }
}
