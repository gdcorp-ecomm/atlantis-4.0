using System;
using Atlantis.Framework.BPSubscribeRemove.Impl.BPBlogWS;
using Atlantis.Framework.BPSubscribeRemove.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPSubscribeRemove.Impl
{
  public class BPSubscribeRemoveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        BPSubscribeRemoveRequestData blogRequest = (BPSubscribeRemoveRequestData)oRequestData;

        using (BBSoap service = new BBSoap())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          service.Timeout = (int)blogRequest.RequestTimeout.TotalMilliseconds;

          responseText = service.query_subscriber(blogRequest.Email);
          result = new BPSubscribeRemoveResponseData(responseText);

          if ((string.Compare(responseText, "y", true) == 0))
          {
            responseText = service.remove_subscriber(blogRequest.Email);
            result = new BPSubscribeRemoveResponseData(responseText);
          }
        }
      }
      catch (AtlantisException aex)
      {
        result = new BPSubscribeRemoveResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new BPSubscribeRemoveResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }

}
