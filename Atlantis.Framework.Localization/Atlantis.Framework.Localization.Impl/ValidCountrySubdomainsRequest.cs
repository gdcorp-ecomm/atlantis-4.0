using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class ValidCountrySubdomainsRequest : IRequest
  {
    const string _APPSETTINGNAME = "SALES_VALID_COUNTRY_SUBDOMAINS";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string value;

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        value = comCache.GetAppSetting(_APPSETTINGNAME);
      }

      return ValidCountrySubdomainsResponseData.FromDelimitedSetting(value);
    }
  }
}
