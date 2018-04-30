using CommonLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TcpServerTest.Concretes
{
    class RequestResponserFactoryConcrete : IRequestResponserFactory<Message, Message>
    {
        public IRequestResponser<Message, Message> CreateRequestResponser()
        {
            return new RequestResponserConcrete();
        }
    }
}
