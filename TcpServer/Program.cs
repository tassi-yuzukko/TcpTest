using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLib;

namespace TcpServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // サーバが接続を待つエンドポイント
            var endpoint = new IPEndPoint(IPAddress.Loopback, 54321);

            // サーバ
            var server = new Server<Message, Message>(
                endpoint,
                // リクエストからレスポンスを作る処理
                request => new Message
                {
                    Id = request.Id,
                    // メッセージの文字列を逆順にする
                    Content = new string(request.Content.Reverse().ToArray()),
                });
            // 接続を待機
            var task = Task.Run(() => server.Listen());

            Console.WriteLine("終了する場合は何かキーを押してください...");
            Console.ReadKey();

            // サーバを終了
            server.Close();
            // サーバの終了処理、Taskの管理、エラー処理あたりが微妙
        }
    }
}
