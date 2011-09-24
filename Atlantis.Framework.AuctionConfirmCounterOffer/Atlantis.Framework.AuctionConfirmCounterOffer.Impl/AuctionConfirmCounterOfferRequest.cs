using System;
using Atlantis.Framework.AuctionConfirmCounterOffer.Impl.AuctionConfirmCounterOfferWS;
using Atlantis.Framework.AuctionConfirmCounterOffer.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmCounterOffer.Impl
{
  public class AuctionConfirmCounterOfferRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;

      trpBiddingService service = null;

      try
      {
        var request = (AuctionConfirmCounterOfferRequestData)requestData;

        service = new trpBiddingService();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string responseXml = service.ConfirmCounterOffer(request.AuctionItemId, request.BidId, request.ShopperID, request.BidAmount, request.Comments);
        responseData = new AuctionConfirmCounterOfferResponseData(responseXml);
      }
      catch (Exception ex)
      {
        responseData = new AuctionConfirmCounterOfferResponseData(requestData, ex);
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
