using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using YeelightHelper;

namespace YeelightSsdpTest
{
    class Program
    {
        static void Main(string[] args)
        {

            FakeSsdpHandler ssdpHandler = new FakeSsdpHandler();

            List<string> results = ssdpHandler.FindYeelightDevice();

            foreach(string result in results)
            {
                Console.WriteLine(result);
            }

            Console.ReadKey();
        }
    }
}
