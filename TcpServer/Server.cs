using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLib;

namespace TcpServerTest
{
    // サーバ
    public class Server : IDisposable
    {
        readonly CancellationTokenSource tokenSource;
        readonly IList<IListener> listeners;

        public Server()
        {
            tokenSource = new CancellationTokenSource();
            listeners = new List<IListener>();
        }

        public void AddListener<TRequest, TResponse>(
            IPEndPoint endpoint,
            IRequestResponserFactory<TRequest, TResponse> requestResponserFactory)
        {
            var listener = new Listener<TRequest, TResponse>(
                endpoint,
                requestResponserFactory,
                tokenSource.Token);

            listener.StartListening();

            listeners.Add(listener);
        }

        public void Dispose()
        {
            tokenSource.Cancel();

            try
            {
                Task.WaitAll(
                    listeners.Select(i => i.ListenerTask).Where(i => i != null).ToArray());
            }
            finally
            {
                tokenSource.Dispose();
            }
        }
    }
}