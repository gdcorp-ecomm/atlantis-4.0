using System;

namespace Atlantis.Framework.Interface
{
  public interface IAsyncRequest
  {
    IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state);
    IResponseData EndHandleRequest(IAsyncResult asyncResult);
  }
}
