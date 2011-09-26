using System;
using Atlantis.Framework.AuctionGetDetailsBulk.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuctionGetDetailsBulk.Impl.AuctionGetAuctionDetailsBulkWS;

namespace Atlantis.Framework.AuctionGetDetailsBulk.Impl
{
  public class AuctionGetDetailsBulkRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;

      trpMemberItemService service = null;

      try
      {
        var request = (AuctionGetDetailsBulkRequestData)requestData;

        service = new trpMemberItemService();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string responseXml = service.GetAuctionDetailsBulk(request.RequestXml);
        responseData = new AuctionGetDetailsBulkResponseData(responseXml);

      }
      catch (Exception ex)
      {
        responseData = new AuctionGetDetailsBulkResponseData(requestData, ex);
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
