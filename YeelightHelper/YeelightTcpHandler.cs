using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using PrimS;
using PrimS.Telnet;

namespace YeelightHelper
{
    internal class YeelightTcpHandler
    {
        internal static async Task<string> SendAndReceive(string payload, string ip, int port = 55443)
        {
            Client client = new Client(ip, 55443, new CancellationToken());
            await client.WriteLine(payload);
            string responseString = await client.ReadAsync(TimeSpan.FromSeconds(60));
            return responseString;
        }
    }
}
