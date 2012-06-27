using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSubscribeRemove.Impl.UnsubscribeWS;
using Atlantis.Framework.MktgSubscribeRemove.Interface;

namespace Atlantis.Framework.MktgSubscribeRemove.Impl
{
  public class MktgSubscribeRemoveRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;

      try
      {
        MktgSubscribeRemoveRequestData mktgRequest = (MktgSubscribeRemoveRequestData)oRequestData;

        string responseText;
        using (Service service = new Service())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          service.Timeout = (int)mktgRequest.RequestTimeout.TotalMilliseconds;
          
          responseText = service.Unsubscribe(mktgRequest.Email, 
                                             mktgRequest.PublicationId, 
                                             mktgRequest.PrivateLabelId, 
                                             mktgRequest.RequestedBy, 
                                             mktgRequest.IpAddress);
        }

        result = new MktgSubscribeRemoveResponseData(responseText);
      }
      catch (Exception ex)
      {
        result = new MktgSubscribeRemoveResponseData(oRequestData, ex);
      }

      return result;
    }
  }
}
