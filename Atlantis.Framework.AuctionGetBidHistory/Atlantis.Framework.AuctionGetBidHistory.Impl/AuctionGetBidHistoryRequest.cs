using System;
using Atlantis.Framework.AuctionGetBidHistory.Impl.AuctionGetBidHistoryWS;
using Atlantis.Framework.AuctionGetBidHistory.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetBidHistory.Impl
{
  public class AuctionGetBidHistoryRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oReponseData = null;

      trpBiddingService service = null;

      try
      {
        AuctionGetBidHistoryRequestData request = (AuctionGetBidHistoryRequestData)oRequestData;

        service = new trpBiddingService();
        
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        
        string responseXml = service.GetBidHistory(request.RequestXml);
        oReponseData = new AuctionGetBidHistoryResponseData(responseXml, request.IsMemberArea);
        
      }
      catch (Exception ex)
      {
        oReponseData = new AuctionGetBidHistoryResponseData(oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }
      
      return oReponseData;
    }
  }
}
