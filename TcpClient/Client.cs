using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace TcpClientTest
{
    // クライアント
    public class Client<TRequest, TResponse> : IDisposable
    {
        // 接続先のエンドポイント
        private readonly IPEndPoint _endpoint;
        private TcpClient client;
        private NetworkStream stream;

        public Client(IPEndPoint endpoint)
        {
            _endpoint = endpoint;
            client = new TcpClient();
        }

        // サーバにリクエストを送信してレスポンスを受信する
        public TResponse SendMessage(TRequest request)
        {
            // 2. サーバにリクエストを送信する
            Console.WriteLine($"Client send: {request}");
            stream.WriteObject(request);

            // 3. サーバからレスポンスを受信する
            var response = stream.ReadObject<TResponse>();
            Console.WriteLine($"Client received: {response}");

            return response;
        }

        public async Task ConnectAsync()
        {
            await client.ConnectAsync(_endpoint.Address, _endpoint.Port);
            stream = client.GetStream();
        }

        public void Dispose()
        {
            client?.Dispose();
            stream?.Dispose();
        }
    }
}