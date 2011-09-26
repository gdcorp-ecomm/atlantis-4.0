using System;
using Atlantis.Framework.AuctionSearch.Impl.AuctionSearchWS;
using Atlantis.Framework.AuctionSearch.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionSearch.Impl
{
  public class AuctionSearchRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oReponseData;
      trpAdminUtilsService auctionsSearch = null;

      try
      {
        AuctionSearchRequestData request = (AuctionSearchRequestData)oRequestData;
        auctionsSearch = new trpAdminUtilsService();
        auctionsSearch.Url = ((WsConfigElement)oConfig).WSURL;
        auctionsSearch.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string responseXml = auctionsSearch.AuctionSearch(request.RequestXml);
        oReponseData = new AuctionSearchResponseData(request, responseXml);
      }
      catch (Exception ex)
      {
        oReponseData = new AuctionSearchResponseData(oRequestData, ex);
      }
      finally
      {
        if(auctionsSearch != null)
        {
          auctionsSearch.Dispose();
        }
      }

      return oReponseData;
    }
  }
}
