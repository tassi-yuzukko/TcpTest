using System;
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
    class Listener<TRequest, TResponse> : IListener
    {
        // 接続を待つエンドポイント
        readonly IPEndPoint endpoint;

        // リクエストからレスポンスを作成するクラスのファクトリ
        readonly IRequestResponserFactory<TRequest, TResponse> requestResponserFactory;

        // TCPリスナー
        readonly TcpListener listener;

        readonly IList<Session<TRequest, TResponse>> sessions = new List<Session<TRequest, TResponse>>();

        readonly CancellationToken cancellationToken;

        public Task ListenerTask => _litenerTask;
        Task _litenerTask;

        public Listener(
            IPEndPoint endpoint,
            IRequestResponserFactory<TRequest, TResponse> requestResponserFactory,
            CancellationToken cancellationToken)
        {
            this.endpoint = endpoint;
            this.requestResponserFactory = requestResponserFactory;
            this.cancellationToken = cancellationToken;

            listener = new TcpListener(this.endpoint);

            // 1. クライアントからの接続を待つ
            listener.Start();

            // 終了処理を予約しておく
            cancellationToken.Register(() =>
            {
                _litenerTask.Wait();

                listener.Stop();
            });
        }

        // 接続待ちを開始
        public void StartListening()
        {
            Console.WriteLine($"Listener listen:");

            _litenerTask = new Task(async () =>
            {
                while (true)
                {
                    // 2. クライアントからの接続を受け入れる
                    if (listener.Pending())
                    {
                        var client = await Task.Run(
                            () => listener.AcceptTcpClientAsync(),
                            cancellationToken);

                        var session = new Session<TRequest, TResponse>(
                            client,
                            requestResponserFactory.CreateRequestResponser(),
                            cancellationToken);

                        sessions.Add(session);

                        Console.WriteLine($"Listener accepted:");
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            });

            ListenerTask.Start();
        }
    }
}