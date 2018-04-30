using System;
using System.Collections.Generic;
using System.Text;

namespace TcpServerTest
{
    public interface IRequestResponserFactory<TRequest, TResponse>
    {
        IRequestResponser<TRequest, TResponse> CreateRequestResponser();
    }
}
