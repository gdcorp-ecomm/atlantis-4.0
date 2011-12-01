using System;
using Atlantis.Framework.AuctionRecommendations.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionRecommendations.Impl
{
  public class AuctionRecommendationsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AuctionRecommendationsResponseData oReponseData = null;

      gdAuctionsLeprechaunWS.gdAuctionsLeprechaunWS service = null;
      try
      {
        AuctionRecommendationsRequestData request = (AuctionRecommendationsRequestData)oRequestData;
        service = new gdAuctionsLeprechaunWS.gdAuctionsLeprechaunWS();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)Math.Truncate(request.RequestTimeout.TotalMilliseconds);
        string responseXML = service.AuctionRecommendationsSync(request.RequestXML);
        oReponseData = new AuctionRecommendationsResponseData(responseXML);
      }
      catch (Exception ex)
      {
        oReponseData = new AuctionRecommendationsResponseData(oRequestData, ex);
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
