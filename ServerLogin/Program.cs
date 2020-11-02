using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerLibrary.AsyncServer server = new ServerLibrary.AsyncServer(IPAddress.Parse("127.0.0.1"), 2048);
            while (true)
            {
                server.Start();
            }
        }
    }
}
