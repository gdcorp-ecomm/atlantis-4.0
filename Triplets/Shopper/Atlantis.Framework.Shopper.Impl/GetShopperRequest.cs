using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;

namespace Atlantis.Framework.Shopper.Impl
{
  public class GetShopperRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      var requestXml = requestData.ToXML();
      string responseXml = null;

      using (var shopperService = ShopperService.CreateDisposable(requestData, config))
      {
        responseXml = shopperService.Service.GetShopper(requestXml);
      }

      result = GetShopperResponseData.FromShopperXml(responseXml);
      return result;
    }
  }
}
