using System;
using Atlantis.Framework.AuctionConfirmBuyNow.Impl.AuctionAPIWebService;
using Atlantis.Framework.AuctionConfirmBuyNow.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Impl
{
  public class AuctionConfirmBuyNowRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AuctionConfirmBuyNowResponseData oResponseData = null;

      AuctionConfirmBuyNowRequestData request = (AuctionConfirmBuyNowRequestData) oRequestData;

      trpBiddingService service = new trpBiddingService();
                                    
      try
      {
        service.Url = ((WsConfigElement) oConfig).WSURL;
        service.Timeout = (int) request.RequestTimeout.TotalMilliseconds;
        
        string response = service.ConfirmBuyNowWithSystemId(request.SourceSystemId, request.AuctionItemId, request.ShopperID, request.Comments, request.Isc, request.Itc);

        oResponseData = new AuctionConfirmBuyNowResponseData(response);
      }
      catch (Exception ex)
      {
        oResponseData = new AuctionConfirmBuyNowResponseData(oRequestData, ex);
      }
      finally
      {
        service.Dispose();
      }
      return oResponseData;
    }

    #endregion
  }
}
