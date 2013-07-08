using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Localization
{
  public class CountryCookieLocalizationProvider : LocalizationProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;

    public CountryCookieLocalizationProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    protected override string DetermineCountrySite()
    {
      string result = _WWW;

      if ((HttpContext.Current != null) && (_siteContext.Value.ContextId == 1))
      {
        if ((CountrySiteCookie.Value.HasValue) && (IsValidCountrySubdomain(CountrySiteCookie.Value.Value)))
        {
          result = CountrySiteCookie.Value.Value;
        }
      }

      return result;
    }
  }
}
