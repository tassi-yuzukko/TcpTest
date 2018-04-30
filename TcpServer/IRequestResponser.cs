using System;
using System.Collections.Generic;
using System.Text;

namespace TcpServerTest
{
    public interface IRequestResponser<TRequest, TResponse>
    {
        /// <summary>
        /// リクエストを受けてレスポンスを返す
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TResponse Response(TRequest request);
    }
}
