using Atlantis.Framework.Basket.Impl.GdBasketService;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Basket.Impl
{
  public class BasketItemCountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (string.IsNullOrEmpty(requestData.ShopperID))
      {
        return BasketItemCountResponseData.Empty;
      }

      string responseText;

      using (var basketService = new WscgdBasketService())
      {
        basketService.Url = ((WsConfigElement) config).WSURL;
        basketService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
        responseText = basketService.GetItemCounts(requestData.ShopperID);
      }

      return BasketItemCountResponseData.FromPipeDelimitedResponseString(responseText);
    }
  }
}
