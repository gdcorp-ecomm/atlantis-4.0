using System;
using Atlantis.Framework.AuctionConfirmAcceptOffer.Impl.AuctionConfirmAcceptOfferWS;
using Atlantis.Framework.AuctionConfirmAcceptOffer.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmAcceptOffer.Impl
{
  public class AuctionConfirmAcceptOfferRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      trpBiddingService service = null;

      try
      {
        var request = (AuctionConfirmAcceptOfferRequestData) requestData;

        service = new trpBiddingService();
        service.Url = ((WsConfigElement) config).WSURL;
        service.Timeout = (int) request.RequestTimeout.TotalMilliseconds;

        string responseXml = service.ConfirmAcceptOffer(request.AuctionItemId, request.BidId, request.ShopperID);
        responseData = new AuctionConfirmAcceptOfferResponseData(responseXml);
      }
      catch (Exception ex)
      {
        responseData = new AuctionConfirmAcceptOfferResponseData(requestData, ex);
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
