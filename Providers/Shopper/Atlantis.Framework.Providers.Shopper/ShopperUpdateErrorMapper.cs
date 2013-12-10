using System.Collections.Generic;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Shopper
{
  internal static class ShopperUpdateErrorMapper
  {
    internal static Dictionary<string, ShopperUpdateResultType> ShopperUpdateErrorMap
    {
      get
      {
        var map = new Dictionary<string, ShopperUpdateResultType>(10);

        map.Add("0xC0044A10", ShopperUpdateResultType.InvalidShopperXml);
        map.Add("0xC0044A13", ShopperUpdateResultType.InvalidRequestField);
        map.Add("0xC0044A15", ShopperUpdateResultType.ShopperNotFound);
        map.Add("0xC0044A1A", ShopperUpdateResultType.InvalidShopperId);
        map.Add("0xC0044A1E", ShopperUpdateResultType.CountryMarketIdNotCompatible);
        map.Add("0xC0044A20", ShopperUpdateResultType.PasswordUnacceptable);
        map.Add("0xC0044A21", ShopperUpdateResultType.PinUnacceptable);
        map.Add("0xC0044A22", ShopperUpdateResultType.HintMatchesPassword);
        map.Add("0x80040E2F", ShopperUpdateResultType.LoginNameAlreadyExists);

        return map;
      }
    }
  }
}
