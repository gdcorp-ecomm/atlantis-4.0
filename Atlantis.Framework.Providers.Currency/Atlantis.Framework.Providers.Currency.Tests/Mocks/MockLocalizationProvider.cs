using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockProviders;
using System.Globalization;

namespace Atlantis.Framework.Providers.Currency.Tests.Mocks
{
  public class MockLocalizationProvider : ProviderBase, ILocalizationProvider
  {
    private Lazy<ISiteContext> _siteContext;
    public MockLocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    #region ILocalizationProvider Members

    public string FullLanguage
    {
      get { throw new NotImplementedException(); }
    }

    public string ShortLanguage
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsActiveLanguage(string language)
    {
      throw new NotImplementedException();
    }

    public string CountrySite
    {
      get 
      {
        return Container.GetData("Localization.CountrySite", "us");
      }
    }

    public bool IsGlobalSite()
    {
      throw new NotImplementedException();
    }

    public bool IsCountrySite(string countryCode)
    {
      throw new NotImplementedException();
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> ValidCountrySiteSubdomains
    {
      get { throw new NotImplementedException(); }
    }

    public string GetCountrySiteLinkType(string baseLinkType)
    {
      throw new NotImplementedException();
    }

    public string PreviousCountrySiteCookieValue
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsValidCountrySubdomain(string countryCode)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region ILocalizationProvider Members


    public string RewrittenUrlLanguage
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public void SetMarket(string marketId)
    {
      throw new NotImplementedException();
    }

    public System.Globalization.CultureInfo CurrentCultureInfo
    {
      get { return Container.GetData<CultureInfo>("Localization.CultureInfo", CultureInfo.CurrentCulture); }
    }

    #endregion

    #region ILocalizationProvider Members

    public IMarket MarketInfo
    {
      get { throw new NotImplementedException(); }
    }

    public ICountrySite CountrySiteInfo
    {
      get
      {
        return Container.GetData<ICountrySite>("Localization.CountrySiteInfo", null);
      }
    }

    #endregion


    public string GetLanguageUrl()
    {
      throw new NotImplementedException();
    }

    public string GetLanguageUrl(string marketId)
    {
      throw new NotImplementedException();
    }

    public string GetLanguageUrl(string countrySiteId, string marketId)
    {
      throw new NotImplementedException();
    }

    public IMarket GetMarketForCountrySite(string countrySiteId, string marketId)
    {
        throw new NotImplementedException();
    }

    public bool IsGlobalSite(string countrySiteId)
    {
        throw new NotImplementedException();
    }

    public ICountrySite TryGetCountrySite(string countrySiteId)
    {
        throw new NotImplementedException();
    }

    public IMarket TryGetMarket(string marketId)
    {
        throw new NotImplementedException();
    }

    public IMarket TryGetMarketForCountrySite(string countrySiteId, string marketId)
    {
        throw new NotImplementedException();
    }
  }
}
