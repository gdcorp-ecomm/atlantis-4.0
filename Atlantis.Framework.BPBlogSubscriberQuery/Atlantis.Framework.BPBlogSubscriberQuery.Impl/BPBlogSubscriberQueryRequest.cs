using System;
using Atlantis.Framework.BPBlogSubscriberQuery.Impl.BPBlogWS;
using Atlantis.Framework.BPBlogSubscriberQuery.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPBlogSubscriberQuery.Impl
{
  public class BPBlogSubscriberQueryRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        BPBlogSubscriberQueryRequestData blogRequest = (BPBlogSubscriberQueryRequestData)oRequestData;

        using (BBSoap service = new BBSoap())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          service.Timeout = (int)blogRequest.RequestTimeout.TotalMilliseconds;

          responseText = service.query_subscriber(blogRequest.Email);
          result = new BPBlogSubscriberQueryResponseData(responseText);
        }
      }
      catch (AtlantisException aex)
      {
        result = new BPBlogSubscriberQueryResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new BPBlogSubscriberQueryResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
