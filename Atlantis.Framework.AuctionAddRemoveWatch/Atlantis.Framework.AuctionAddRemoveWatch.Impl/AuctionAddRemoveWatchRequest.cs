using System;
using Atlantis.Framework.AuctionAddRemoveWatch.Impl.AuctionAddRemoveWatchWS;
using Atlantis.Framework.AuctionAddRemoveWatch.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionAddRemoveWatch.Impl
{
  public class AuctionAddRemoveWatchRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;

      trpMemberItemService service = null;

      try
      {
        var request = (AuctionAddRemoveWatchRequestData)requestData;

        service = new trpMemberItemService();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string responseXml = service.AddRemoveWatch(request.RequestXml);
        responseData = new AuctionAddRemoveWatchResponseData(responseXml);
      }
      catch (Exception ex)
      {
        responseData = new AuctionAddRemoveWatchResponseData(requestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return responseData;
    }

    #endregion
  }
}