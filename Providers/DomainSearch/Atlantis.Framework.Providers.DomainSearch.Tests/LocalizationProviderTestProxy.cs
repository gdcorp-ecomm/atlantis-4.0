using System;
using System.Collections.Generic;
using System.Globalization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
  public class LocalizationProviderTestProxy : ProviderBase, ILocalizationProvider // Framework providers should implement a corresponding interface
  {
    public LocalizationProviderTestProxy(IProviderContainer container)
      : base(container)
    {
    }

    public IEnumerable<IMarket> GetMappedMarketsForCountrySite(string countrySite, bool includeInternalOnly) { throw new NotImplementedException(); }

    public string FullLanguage
    {
      get { return "en-us"; }
    }

    public string ShortLanguage
    {
      get { return "en"; }
    }

    public bool IsActiveLanguage(string language)
    {
      return language == "en";
    }

    public string CountrySite
    {
      get { return "www"; }
    }

    public IMarket TryGetMarketForCountrySite(string countrySiteId, string marketId)
    {
      throw new NotImplementedException();
    }

    public bool IsGlobalSite()
    {
      return true;
    }

    public bool IsGlobalSite(string countrySiteId)
    {
      throw new NotImplementedException();
    }

    public bool IsCountrySite(string countryCode)
    {
      return countryCode.ToLowerInvariant() != "us";
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

    public string PreviousLanguageCookieValue { get; set; }

    public bool IsValidCountrySubdomain(string countryCode)
    {
      throw new NotImplementedException();
    }

    public void SetLanguage(string language)
    {
      throw new NotImplementedException();
    }

    public CultureInfo CurrentCultureInfo
    {
      get { throw new NotImplementedException(); }
    }

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

    public IEnumerable<IMarket> GetMarketsForCountryCode(string countryCode)
    {
      throw new NotImplementedException();
    }

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

    public ICountrySite CountrySiteInfo
    {
      get { throw new NotImplementedException(); }
    }

    public IMarket MarketInfo
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

    public IMarket GetMarketForCountrySite(string countrySiteId, string marketId)
    {
      throw new NotImplementedException();
    }

    public void SetMarket(string marketId)
    {
      throw new NotImplementedException();
    }
  }
}