using System;
using Atlantis.Framework.AuctionPlaceBidBulk.Impl.AuctionPlaceBidBulkWS;
using Atlantis.Framework.AuctionPlaceBidBulk.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionPlaceBidBulk.Impl
{
  public class AuctionPlaceBidBulkRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      trpBiddingService service = null;

      try
      {
        var request = (AuctionPlaceBidBulkRequestData) oRequestData;

        service = new trpBiddingService();
        
        service.Url = ((WsConfigElement) oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        
        string responseXml = service.ConfirmBidBulk(request.RequestXml);
        oResponseData = new AuctionPlaceBidBulkResponseData(responseXml);
        
      }
      catch (Exception ex)
      {
        oResponseData = new AuctionPlaceBidBulkResponseData(oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }
      return oResponseData;
    }

    #endregion
  }
}
