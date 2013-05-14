using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Currency.Impl
{
  public class CurrencyTypesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string currencyDataXml;

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        currencyDataXml = comCache.GetCurrencyDataXml();
      }

      return CurrencyTypesResponseData.FromCacheXml(currencyDataXml);
    }
  }
}
