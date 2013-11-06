using System;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  public class MockBasketItemCountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (string.IsNullOrEmpty(requestData.ShopperID))
      {
        return BasketItemCountResponseData.Empty;
      }

      if (requestData.ShopperID == "ERR")
      {
        throw new Exception("Boom");
      }

      return BasketItemCountResponseData.FromPipeDelimitedResponseString("4|0");
    }
  }
}
