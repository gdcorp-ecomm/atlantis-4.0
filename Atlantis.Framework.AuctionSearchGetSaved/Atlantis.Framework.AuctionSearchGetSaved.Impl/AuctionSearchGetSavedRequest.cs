using System;
using Atlantis.Framework.AuctionSearchGetSaved.Impl.AuctionSearchWS;
using Atlantis.Framework.AuctionSearchGetSaved.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionSearchGetSaved.Impl
{
  public class AuctionSearchGetSavedRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      trpAdminUtilsService auctionsSearch = null;

      try
      {
        var request = (AuctionSearchGetSavedRequestData)requestData;
        auctionsSearch = new trpAdminUtilsService
                           {
                             Url = ((WsConfigElement) config).WSURL,
                             Timeout = (int) request.RequestTimeout.TotalMilliseconds
                           };

        var responseXml = auctionsSearch.GetMemberSavedSearches(request.RequestXml);
        responseData = new AuctionSearchGetSavedResponseData(responseXml);
      }
      catch (Exception ex)
      {
        responseData = new AuctionSearchGetSavedResponseData(requestData, ex);
      }
      finally
      {
        if (auctionsSearch != null)
        {
          auctionsSearch.Dispose();
        }
      }
       
      return responseData;
    }
  }
}
