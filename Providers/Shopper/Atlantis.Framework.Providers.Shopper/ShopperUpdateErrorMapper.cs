using System.Collections.Generic;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Shopper
{
  internal static class ShopperUpdateErrorMapper
  {
    private static readonly Dictionary<string, ShopperUpdateResultType> _updateErrorMap;
 
    static ShopperUpdateErrorMapper()
    {
      _updateErrorMap = new Dictionary<string, ShopperUpdateResultType>(10)
      {
        {"0xC0044A10", ShopperUpdateResultType.InvalidShopperXml},
        {"0xC0044A13", ShopperUpdateResultType.InvalidRequestField},
        {"0xC0044A15", ShopperUpdateResultType.ShopperNotFound},
        {"0xC0044A1A", ShopperUpdateResultType.InvalidShopperId},
        {"0xC0044A1E", ShopperUpdateResultType.CountryMarketIdNotCompatible},
        {"0xC0044A20", ShopperUpdateResultType.PasswordUnacceptable},
        {"0xC0044A21", ShopperUpdateResultType.PinUnacceptable},
        {"0xC0044A22", ShopperUpdateResultType.HintMatchesPassword},
        {"0x80040E2F", ShopperUpdateResultType.LoginNameAlreadyExists},
        {"0xC0044A1D", ShopperUpdateResultType.LoginNameNumericShouldBeCustomerNumber}
      };
    }

    internal static ShopperUpdateResultType GetUpdateResultType(string serviceErrorCode)
    {
      ShopperUpdateResultType result;
      if (!_updateErrorMap.TryGetValue(serviceErrorCode, out result))
      {
        result = ShopperUpdateResultType.UnknownError;
      }
      return result;
    }
  }
}
