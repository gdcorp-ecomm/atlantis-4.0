using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;

namespace Atlantis.Framework.Shopper.Impl
{
  class CreateShopperRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      var requestXml = requestData.ToXML();
      string responseXml = null;

      using (var shopperService = ShopperService.CreateDisposable(requestData, config))
      {
        responseXml = shopperService.Service.CreateShopper(requestXml);
      }

      result = CreateShopperResponseData.FromShopperXml(responseXml);
      return result;
    }
  }
}
