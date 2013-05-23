using Atlantis.Framework.Currency.Interface;

namespace Atlantis.Framework.PromoOrderLevelCreate.Interface
{
  internal class CurrencyValidaton
  {
    internal static bool IsCurrencyValid(string currencyType, int currencyTypesRequestType)
    {
      var request = new CurrencyTypesRequestData();
      var response = (CurrencyTypesResponseData)DataCache.DataCache.GetProcessRequest(request, currencyTypesRequestType);
      return response.Contains(currencyType);
    }
  }
}
