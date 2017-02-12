using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace YeelightHelper
{
    internal class YeelightTcpHandler
    {
        internal static string SendAndReceive(string payload, string ip, int port = 55443)
        {
            // Parse and create IP address
            IPAddress ipAddr = IPAddress.Parse(ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            // Set to TCP Stream (telnet) and connect
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Connect(ipEndPoint);

            // Generate payload byte array and send the payload out
            byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);
            tcpSocket.Send(payloadBytes);

            // Then wait for the response
            byte[] receivedBuffer = new byte[8192];
            tcpSocket.Receive(receivedBuffer);

            return Encoding.ASCII.GetString(receivedBuffer);
        }
    }
}
