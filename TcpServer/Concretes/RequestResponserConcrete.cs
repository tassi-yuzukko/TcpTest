using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpServerTest.Concretes
{
    class RequestResponserConcrete : IRequestResponser<Message, Message>
    {
        public Message Response(Message request)
        {
            return new Message
            {
                Id = request.Id,
                // メッセージの文字列を逆順にする
                Content = new string(request.Content.Reverse().ToArray()),
            };
        }
    }
}
