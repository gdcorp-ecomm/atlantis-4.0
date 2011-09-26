using System;
using Atlantis.Framework.AuctionPlaceBid.Impl.AuctionAPIWebService;
using Atlantis.Framework.AuctionPlaceBid.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionPlaceBid.Impl
{
  public class AuctionPlaceBidRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AuctionPlaceBidResponseData oResponseData = null;

      AuctionPlaceBidRequestData request = (AuctionPlaceBidRequestData)oRequestData;

      trpBiddingService service = new trpBiddingService();


      try
      {

        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        string response = service.ConfirmBid(request.RequestorInformation.SourceSystemId, request.AuctionItemId, request.ShopperID, request.BidAmount, request.Comments);

        oResponseData = new AuctionPlaceBidResponseData(response);
      }
      catch (Exception ex)
      {
        oResponseData = new AuctionPlaceBidResponseData(oRequestData, ex);
      }
      finally
      {
        service.Dispose();
      }

      return oResponseData;
    }
  }
}
