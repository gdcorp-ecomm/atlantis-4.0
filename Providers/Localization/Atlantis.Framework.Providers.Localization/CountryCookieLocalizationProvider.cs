using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Localization
{
  public class CountryCookieLocalizationProvider : LocalizationProvider, ILocalizationProvider
  {
    private Lazy<ISiteContext> _siteContext;
    private Lazy<CountrySiteCookie> _countrySiteCookie;

    public CountryCookieLocalizationProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _countrySiteCookie = new Lazy<CountrySiteCookie>(() => { return new CountrySiteCookie(Container); });
    }

    protected override string DetermineCountrySite()
    {
      string result = _WWW;

      if ((HttpContext.Current != null) && (_siteContext.Value.ContextId == 1))
      {
        if ((_countrySiteCookie.Value.HasValue) && (IsValidCountrySubdomain(_countrySiteCookie.Value.Value)))
        {
          result = _countrySiteCookie.Value.Value;
        }
      }

      return result;
    }
  }
}
