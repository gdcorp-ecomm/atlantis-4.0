using System;
using Atlantis.Framework.AuctionBidCloseOuts.Impl.AuctionsBiddingWS;
using Atlantis.Framework.AuctionBidCloseOuts.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionBidCloseOuts.Impl
{
  public class AuctionBidCloseOutsRequest : IRequest
  {
    private const int _MAX_SERVICETIMEOUT_MILLISECONDS = 300000;// in miliseconds

    public IResponseData RequestHandler(RequestData auctionBidCloseOutsRequestData,
      ConfigElement configElement)
    {
      IResponseData responseData = null;

      try
      {

        AuctionBidCloseOutsRequestData requestData
          = (AuctionBidCloseOutsRequestData)auctionBidCloseOutsRequestData;

        string responseXML = string.Empty;
        using (var auctionsBidding = new trpBiddingService())
        {
          auctionsBidding.Url = ((WsConfigElement)configElement).WSURL;

          if (requestData.RequestTimeout.TotalMilliseconds > _MAX_SERVICETIMEOUT_MILLISECONDS)
          {
            auctionsBidding.Timeout = _MAX_SERVICETIMEOUT_MILLISECONDS;
          }
          else
          {
            auctionsBidding.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          }

          responseXML = auctionsBidding.PlaceBidCloseOuts(requestData.ShopperID, requestData.IsShopperAuth,
            requestData.DomainName);

          if (responseXML == null)
          {
            throw new Exception("AuctionBidCloseOuts returned null response.");
          }
          else
          {
            responseData = new AuctionBidCloseOutsResponseData(responseXML);
          }
        }
      }
      catch (AtlantisException atlantisException)
      {
        responseData = new AuctionBidCloseOutsResponseData(atlantisException);
      }
      catch (Exception exception)
      {
        responseData = new AuctionBidCloseOutsResponseData(auctionBidCloseOutsRequestData, exception);
      }

      return responseData;
    }
  }
}
