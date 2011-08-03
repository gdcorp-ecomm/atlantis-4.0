using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSubscribeRemove.Interface;
using Atlantis.Framework.MktgSubscribeRemove.Impl.UnsubscribeWS;

namespace Atlantis.Framework.MktgSubscribeRemove.Impl
{
  public class MktgSubscribeRemoveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        MktgSubscribeRemoveRequestData mktgRequest = (MktgSubscribeRemoveRequestData)oRequestData;

        using (Service service = new Service())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          service.Timeout = (int)mktgRequest.RequestTimeout.TotalMilliseconds;
          responseText = service.Unsubscribe(mktgRequest.Email, mktgRequest.PublicationId, mktgRequest.PrivateLabelId, mktgRequest.RequestedBy, mktgRequest.IPAddress);
        }

        result = new MktgSubscribeRemoveResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new MktgSubscribeRemoveResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new MktgSubscribeRemoveResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
