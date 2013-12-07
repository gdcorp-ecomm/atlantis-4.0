using System;
using System.Collections.Generic;
using System.Globalization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Support.Tests
{
  public class MockLocalizationProvider : ProviderBase, ILocalizationProvider
  {
    public const string COUNTRY_SITE_NAME = "MockLocalizationProvider.CountrySite";
    public const string IS_GLOBAL_SITE_NAME = "MockLocalizationProvider.IsGlobalSite";
    public const string MARKET_INFO = "MockLocalizationProvider.MarketInfo";

    public MockLocalizationProvider(IProviderContainer container) : base(container)
    {
    }

    public string FullLanguage { get; private set; }
    public string ShortLanguage { get; private set; }
    public string RewrittenUrlLanguage { get; set; }
    public bool IsActiveLanguage(string language)
    {
      throw new NotImplementedException();
    }

    public ICountrySite CountrySiteInfo { get; private set; }

    public string CountrySite
    {
      get
      {
        return Container.GetData(COUNTRY_SITE_NAME, "us");
      }
    }

    public IMarket MarketInfo
    {
      get { return Container.GetData<IMarket>(MARKET_INFO, null); }       
    }

    public bool IsGlobalSite()
    {
      return Container.GetData(IS_GLOBAL_SITE_NAME, "www").ToLowerInvariant() == "www";
    }

    public bool IsCountrySite(string countryCode)
    {
      throw new NotImplementedException();
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> ValidCountrySiteSubdomains { get; private set; }
    public string GetCountrySiteLinkType(string baseLinkType)
    {
      throw new NotImplementedException();
    }

    public string PreviousCountrySiteCookieValue { get; private set; }
    public bool IsValidCountrySubdomain(string countryCode)
    {
      throw new NotImplementedException();
    }

    public void SetMarket(string marketId)
    {
      throw new NotImplementedException();
    }

    public CultureInfo CurrentCultureInfo { get; private set; }
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

    public IEnumerable<IMarket> GetMarketsForCountryCode(string countryCode)
    {
      throw new NotImplementedException();
    }

    public bool IsGlobalSite(string countrySiteId)
    {
      throw new NotImplementedException();
    }

    public string PreviousLanguageCookieValue
    {
      get { throw new NotImplementedException(); }
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
