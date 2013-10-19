using System;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Tests.Properties;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  public class MockBasketAddRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (requestData.ShopperID == "ERR")
      {
        throw new Exception("MockBasketAddRequest Error!");
      }

      return BasketAddResponseData.FromResponseXml(Resources.SuccessResponse);
    }
  }
}
