using Atlantis.Framework.Basket.Impl.GdBasketService;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Basket.Impl
{
  public class BasketAddRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      BasketAddRequestData request = (BasketAddRequestData)requestData;

      string responseXml;
      using (var basketService = new WscgdBasketService())
      {
        responseXml = basketService.AddItem(request.ShopperID, request.ToXML());
      }

      return BasketAddResponseData.FromResponseXml(responseXml);
    }
  }
}
