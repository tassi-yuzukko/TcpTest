using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace TcpServerTest
{
    // サーバ
    public class Server<TRequest, TResponse>
    {
        // 接続を待つエンドポイント
        private readonly IPEndPoint _endpoint;

        // リクエストからレスポンスを作成する処理
        private readonly Func<TRequest, TResponse> _processor;

        // TCPリスナー
        private readonly TcpListener _listener;

        List<Session> sessions = new List<Session>();

        public Server(IPEndPoint endpoint, Func<TRequest, TResponse> processor)
        {
            _endpoint = endpoint;
            _processor = processor;
            _listener = new TcpListener(_endpoint);
        }

        // クライアントからリクエストを受信してレスポンスを送信する
        private void Receive(Session session)
        {
            // 3. クライアントからリクエストを受信する
            var request = session.Stream.ReadObject<TRequest>();

            // 4. リクエストを処理してレスポンスを作る
            var response = _processor(request);

            // 5. クライアントにレスポンスを送信する
            session.Stream.WriteObject(response);
        }

        // 接続を待つ
        public async Task Listen()
        {
            Console.WriteLine($"Server listen:");
            // 1. クライアントからの接続を待つ
            _listener.Start();

            while (true)
            {
                // 2. クライアントからの接続を受け入れる
                var client = await _listener.AcceptTcpClientAsync();
                var session = new Session(client);
                sessions.Add(session);
                Console.WriteLine($"Server accepted:");

                var task = Task.Run(() => Receive(session));

                // Taskの管理やエラー処理は省略
            }
        }

        // 終了する
        public void Close()
        {
            _listener.Stop();
        }
    }
}