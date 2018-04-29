using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TcpServerTest
{
    class Session
    {
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }

        public Session(TcpClient client)
        {
            this.Client = client;
            this.Stream = client.GetStream();
        }
    }
}
