using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerTest
{
    interface IListener
    {
        void StartListening();

        Task ListenerTask { get; }
    }
}
