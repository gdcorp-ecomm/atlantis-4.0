using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Impl.gdShopperService;

namespace Atlantis.Framework.Shopper.Impl
{
  internal class ShopperService : IDisposable
  {
    private readonly WSCgdShopperService _shopperService;

    internal static ShopperService CreateDisposable(RequestData requestData, ConfigElement configElement)
    {
      return new ShopperService(requestData, configElement);
    }

    private ShopperService(RequestData requestData, ConfigElement configElement)
    {
      _shopperService = new WSCgdShopperService();
      _shopperService.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;
      _shopperService.Url = ((WsConfigElement) configElement).WSURL;
    }

    public WSCgdShopperService Service
    {
      get { return _shopperService; }
    }

    public void Dispose()
    {
      if (_shopperService != null)
      {
        _shopperService.Dispose();
      }
    }
  }
}
