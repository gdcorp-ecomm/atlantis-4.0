using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;

namespace Atlantis.Framework.Shopper.Impl
{
  public class SearchShoppersRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      var requestXml = requestData.ToXML();
      string responseXml = null;

      using (var shopperService = ShopperService.CreateDisposable(requestData, config))
      {
        responseXml = shopperService.Service.SearchShoppers(requestXml);
      }

      result = SearchShoppersResponseData.FromShopperSearchXml(responseXml);
      return result;
    }
  }
}
