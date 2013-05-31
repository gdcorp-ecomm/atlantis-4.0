using System;
using Atlantis.Framework.AuctionConfirmBuyNow.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Impl
{
  public class AuctionConfirmBuyNowBulkRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData;

      try
      {
        AuctionConfirmBuyNowBulkRequestData auctionConfirmBuyNowBulkRequestData = (AuctionConfirmBuyNowBulkRequestData)requestData;
        WsConfigElement wsConfig = ((WsConfigElement)configElement);

        using (AuctionAPIWebService.trpBiddingService service = new AuctionAPIWebService.trpBiddingService())
        {
          service.Url = wsConfig.WSURL;
          service.Timeout = (int)Math.Truncate(auctionConfirmBuyNowBulkRequestData.RequestTimeout.TotalMilliseconds);

          string response = service.ConfirmBuyNowBulk(requestData.ToXML());
          responseData = new AuctionConfirmBuyNowBulkResponseData(response);
        }
      }
      catch (Exception ex)
      {
        responseData = new AuctionConfirmBuyNowBulkResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
