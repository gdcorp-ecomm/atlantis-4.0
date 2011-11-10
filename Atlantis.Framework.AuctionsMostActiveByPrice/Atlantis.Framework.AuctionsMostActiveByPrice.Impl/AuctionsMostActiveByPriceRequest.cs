using System;
using Atlantis.Framework.AuctionsMostActiveByPrice.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsMostActiveByPrice.Impl
{
  public class AuctionsMostActiveByPriceRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      string responseXML = string.Empty;
      AuctionsMostActiveByPriceResponseData responseData = null;

      try
      {
        AuctionsMostActiveByPriceRequestData auctionData = (AuctionsMostActiveByPriceRequestData)oRequestData;
        using (TdnamDomainService.trpLandingDomainsService auctionWS = new TdnamDomainService.trpLandingDomainsService())
        {
          auctionWS.Url = ((WsConfigElement)oConfig).WSURL;
          auctionWS.Timeout = (int)auctionData.RequestTimeout.TotalMilliseconds;
          responseXML = auctionWS.RetrieveMostActiveByPrice(auctionData.AuctionCount);
        }
        responseData = new AuctionsMostActiveByPriceResponseData(responseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuctionsMostActiveByPriceResponseData(responseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new AuctionsMostActiveByPriceResponseData(responseXML, oRequestData, ex);
      }
      return responseData;
    }

    #endregion
  }
}
