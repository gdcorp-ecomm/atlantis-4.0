using System;
using Atlantis.Framework.BPBlogSubscriberAdd.Impl.BPBlogWS;
using Atlantis.Framework.BPBlogSubscriberAdd.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPBlogSubscriberAdd.Impl
{
  public class BPBlogSubscriberAddRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        BPBlogSubscriberAddRequestData blogRequest = (BPBlogSubscriberAddRequestData)oRequestData;

        using (BBSoap service = new BBSoap())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          service.Timeout = (int)blogRequest.RequestTimeout.TotalMilliseconds;

          int confirmedInt = blogRequest.Confirmed ? 1 : 0;
          responseText = service.add_subscriber(blogRequest.Email, blogRequest.FirstName, blogRequest.LastName, confirmedInt);
          result = new BPBlogSubscriberAddResponseData(responseText);
        }
      }
      catch (AtlantisException aex)
      {
        result = new BPBlogSubscriberAddResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new BPBlogSubscriberAddResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
