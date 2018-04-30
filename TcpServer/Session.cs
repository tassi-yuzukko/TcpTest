using CommonLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServerTest
{
    class Session<TRequest, TResponse>
    {
        readonly TcpClient client;
        readonly NetworkStream stream;

        // リクエストからレスポンスを作成する処理
        readonly IRequestResponser<TRequest, TResponse> requestResponser;

        readonly CancellationToken cancellationToken;

        public Session(
            TcpClient client,
            IRequestResponser<TRequest, TResponse> requestResponser,
            CancellationToken cancellationToken)
        {
            this.client = client;
            this.requestResponser = requestResponser;
            this.cancellationToken = cancellationToken;

            stream = client.GetStream();
            cancellationToken.Register(() => stream.Dispose()); // 終了処理を予約しておく

            Task.Run(() => Receive(), cancellationToken);
        }

        // クライアントからリクエストを受信してレスポンスを送信する
        private void Receive()
        {
            while (true)
            {
                if (IsAvailable())
                {
                    // 3. クライアントからリクエストを受信する
                    var request = stream.ReadObject<TRequest>();

                    // 4. リクエストを処理してレスポンスを作る
                    var response = requestResponser.Response(request);

                    // 5. クライアントにレスポンスを送信する
                    stream.WriteObject(response);
                }

                Thread.Sleep(10);
            }
        }

        bool IsAvailable()
        {
            return client?.Available > 0;
        }
    }
}
