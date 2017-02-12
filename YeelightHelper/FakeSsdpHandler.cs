using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace YeelightHelper
{
    public class FakeSsdpHandler
    {
        public List<string> FindYeelightDevice()
        {
            // Client port
            IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, 59080);

            // Multicast address & port
            IPEndPoint MulticastEndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1982);

            // Init stuff, specify using UDP
            Socket UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // ???
            UdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UdpSocket.Bind(LocalEndPoint);
            UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(MulticastEndPoint.Address, IPAddress.Any));
            UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);

            Console.WriteLine("UDP-Socket setup done...\r\n");

            string SearchString = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1982\r\nMAN:\"ssdp:discover\"\r\nST:wifi_bulb\r\n";

            UdpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, MulticastEndPoint);

            Console.WriteLine("M-Search sent...\r\n");

            byte[] ReceiveBuffer = new byte[64000];

            int ReceivedBytes = 0;
            int EmptyAttempt = 0;

            /*
                    I know it's very stupid to do so, 
                    but I don't know how to deal with async network communications right now...

                    The code below are extremely crappy, but it works anyway...
             */

            List<string> addressList = new List<string>();

            while (true)
            {
                if (UdpSocket.Available > 0)
                {
                    ReceivedBytes = UdpSocket.Receive(ReceiveBuffer, SocketFlags.None);

                    if (ReceivedBytes > 0)
                    {
                        string receivedStr = Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes);
                        string[] splittedLineStr = Regex.Split(receivedStr, "\r\n");
                        
                        foreach(string singleLineStr in splittedLineStr)
                        {
                            if(singleLineStr.Contains("Location:"))
                            {
                                string waitToAddStr = Regex.Split(singleLineStr, @": |://")[2];

                                if (!addressList.Contains(waitToAddStr))
                                {
                                    addressList.Add(waitToAddStr);
                                }
                                
                            }
                        }

                        Debug.WriteLine("[FakeSSDP.Received] Received: " + receivedStr);

                    }
                }
                else
                {
                    EmptyAttempt++;
                    Thread.Sleep(1);

                    if (EmptyAttempt > 1000)
                    {
                        Debug.WriteLine("[FakeSSDP.Protocol] Timeout!");
                        break;
                    }
                }


            }

            return addressList;

        }

        
    }
}
