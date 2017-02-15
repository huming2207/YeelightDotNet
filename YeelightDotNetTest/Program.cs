using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using YeelightHelper;

namespace YeelightDotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Info] Test starts now.");

            // Test the Yeelight's "Fake SSDP protocol"
            FakeSsdpHandler ssdpHandler = new FakeSsdpHandler();

            // So far the SSDP handler will block the thread for about 2 seconds.
            // I'll add a event-based method later if possible.
            //
            // Here it returns a List<string>, containing IP and port strings.
            List<string> results = ssdpHandler.FindYeelightDevice();

            string firstDeviceIp = string.Empty;
            if(results.Count > 0)
            {
                foreach(string result in results)
                {
                    Console.WriteLine("[FakeSSDP.Results] Found device at " + result);
                }

                Console.WriteLine("[FakeSSDP.Results] Pick the first device, at " + results[0]);
                firstDeviceIp = results[0].Split(':')[0];
            }
            else
            {
                Console.WriteLine("[Error] No device found, please turn on your device & check your network, then try again.");
                Console.WriteLine("[Error] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            Thread.Sleep(2000);

            // Initialize the command helper
            YeelightCommand yeelightCommand = new YeelightCommand(firstDeviceIp);

            Console.WriteLine("[Test] #1 Set maximum brightness");
            bool testResult1 = yeelightCommand.SetBrightness(99, YeelightEffect.SmoothChange, 500).Result;

            Thread.Sleep(3000);

            Console.WriteLine("[Test] #2 Turn off the light~");
            bool testResult2 = yeelightCommand.SetPower(false, YeelightEffect.SmoothChange, 500).Result;

            Thread.Sleep(3000);

            Console.WriteLine("[Test] #3 Turn on the light again lol~");
            bool testResult3 = yeelightCommand.SetPower(true, YeelightEffect.SmoothChange, 500).Result;

            Thread.Sleep(3000); 

            Console.WriteLine("[Test] Final: Get all status");
            var testResultStatus = yeelightCommand.GetAllStatus().Result;

            Console.WriteLine("\r\n\r\n[Result] Test 1 returns {0}, Test 2 returns {1}, Test 3 returns {2}, ", 
               testResult1.ToString(), testResult2.ToString(), testResult3.ToString());
            Console.WriteLine("[Result] Powered on: " + testResultStatus.IsPowerOn.ToString());
            Console.WriteLine("[Result] Brightness: " + testResultStatus.Brightness.ToString());
            Console.WriteLine("[Result] Model type: " + Enum.GetName(typeof(YeelightModel), testResultStatus.Model));

            Console.ReadKey();
        }
    }
}
