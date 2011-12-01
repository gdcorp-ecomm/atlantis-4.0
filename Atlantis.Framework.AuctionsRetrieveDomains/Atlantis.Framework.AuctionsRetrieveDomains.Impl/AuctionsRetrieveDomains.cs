using System;
using System.Data;
using Atlantis.Framework.AuctionsRetrieveDomains.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsRetrieveDomains.Impl
{
  public class AuctionsRetrieveDomainsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DataSet ds = null;
      AuctionsRetrieveDomainsResponseData responseData = null;

      TdnamDomainService.trpLandingDomainsService auctionWS = null;
      try
      {
        AuctionsRetrieveDomainsRequestData auctionData = (AuctionsRetrieveDomainsRequestData)oRequestData;
        auctionWS = new TdnamDomainService.trpLandingDomainsService();
        auctionWS.Url = ((WsConfigElement)oConfig).WSURL;
        auctionWS.Timeout = (int)auctionData.RequestTimeout.TotalMilliseconds;
        ds = auctionWS.RetrieveDomains(auctionData.AuctionCount);
        responseData = new AuctionsRetrieveDomainsResponseData(ds);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuctionsRetrieveDomainsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new AuctionsRetrieveDomainsResponseData(ds, oRequestData, ex);
      }
      finally
      {
        if (auctionWS != null)
        {
          auctionWS.Dispose();
        }
      }
      return responseData;
    }

    #endregion
  }
}
