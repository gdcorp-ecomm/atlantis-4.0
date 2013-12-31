using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atlantis.Framework.Testing.MockLocalization
{
  public class MockLocalizationProvider : ProviderBase, ILocalizationProvider
  {
    const string _DEFAULTCOUNTRYSITE = "www";
    const string _DEFAULTLANGUAGE = "en";

    readonly string _countrySite;
    string _fullLanguage;
    IMarket _marketInfo;
    readonly ICountrySite _countrySiteInfo;
    private CultureInfo _cultureInfo;
    string _rewrittenUrlLanguage = String.Empty;

    public MockLocalizationProvider(IProviderContainer container) : base(container)
    {
      _countrySiteInfo = LoadCountySiteInfo();
      _marketInfo = LoadMarketInfo();
      _countrySite = LoadMockCountrySite();
      _fullLanguage = LoadFullLanguage();
    }

    private ICountrySite LoadCountySiteInfo()
    {
      return Container.GetData<ICountrySite>(MockLocalizationProviderSettings.CountrySiteInfo, null);
    }

    private IMarket LoadMarketInfo()
    {
      return Container.GetData<IMarket>(MockLocalizationProviderSettings.MarketInfo, null);
    }

    private string LoadFullLanguage()
    {
      if (MarketInfo != null)
      {
        return MarketInfo.Id;
      }

      return Container.GetData(MockLocalizationProviderSettings.FullLanguage, _DEFAULTLANGUAGE);
    }

    private string LoadMockCountrySite()
    {
      if (CountrySiteInfo != null)
      {
        return CountrySiteInfo.Id;
      }

      return Container.GetData(MockLocalizationProviderSettings.CountrySite, _DEFAULTCOUNTRYSITE);
    }

    public IEnumerable<IMarket> GetMarketsForCountryCode(string countryCode)
    {
      IEnumerable<IMarket> markets;

      var marketDictionary = Container.GetData<IDictionary<string, IEnumerable<IMarket>>>(MockLocalizationProviderSettings.MarketInfoForCountryCode, null);

      if (marketDictionary == null || !marketDictionary.TryGetValue(countryCode, out markets))
      {
        markets = new IMarket[0];
      }

      return markets;
    }

    public string FullLanguage
    {
      get { return _fullLanguage; }
    }

    public string ShortLanguage
    {
      get
      {
        string result = _fullLanguage;
        if (_fullLanguage != null)
        {
          int dashPosition = _fullLanguage.IndexOf('-');
          if (dashPosition > -1)
          {
            result = _fullLanguage.Substring(0, dashPosition);
          }
        }
        return result;
      }
    }

    public bool IsActiveLanguage(string language)
    {
      if (language == null)
      {
        return false;
      }

      return (language.Equals(ShortLanguage, StringComparison.OrdinalIgnoreCase) || (language.Equals(FullLanguage, StringComparison.OrdinalIgnoreCase)));
    }

    public string CountrySite
    {
      get { return _countrySite; }
    }

    public bool IsGlobalSite()
    {
      return _DEFAULTCOUNTRYSITE.Equals(_countrySite, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsCountrySite(string countryCode)
    {
      if (countryCode == null)
      {
        return false;
      }

      return countryCode.Equals(_countrySite, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      bool result = false;
      if (countryCodes != null)
      {
        result = countryCodes.Contains(_countrySite, StringComparer.OrdinalIgnoreCase);
      }
      return result;
    }

    public IEnumerable<string> ValidCountrySiteSubdomains
    {
      get { return new HashSet<string>(); }
    }

    public string GetCountrySiteLinkType(string baseLinkType)
    {
      string result = baseLinkType;
      if (!IsGlobalSite() && (_countrySite != null))
      {
        result = string.Concat(baseLinkType, "." + _countrySite.ToUpperInvariant());
      }
      return result;
    }


    public string PreviousCountrySiteCookieValue
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsValidCountrySubdomain(string countryCode)
    {
      return true;
    }

    public CultureInfo CurrentCultureInfo
    {
      get
      {
        if (_cultureInfo == null)
        {
          _cultureInfo = DetermineCultureInfo();
        }
        return _cultureInfo;
      }
    }

    private CultureInfo DetermineCultureInfo()
    {
      CultureInfo result = CultureInfo.CurrentCulture;

      try
      {
        CultureInfo localizedCulture = CultureInfo.GetCultureInfo(_fullLanguage);
        result = localizedCulture;
      }
      catch (CultureNotFoundException)
      {
      }

      return result;
    }

    public string RewrittenUrlLanguage
    {
      get { return _rewrittenUrlLanguage; }
      set { _rewrittenUrlLanguage = value; }
    }

    public ICountrySite CountrySiteInfo
    {
      get { return _countrySiteInfo; }
    }

    public IMarket MarketInfo
    {
      get { return _marketInfo; }
    }

    public void SetMarket(string marketId)
    {
      _marketInfo = new MockMarketInfo(marketId, marketId, marketId, false);
      _fullLanguage = marketId;
      _cultureInfo = null;
    }

    public string GetLanguageUrl()
    {
      return GetLanguageUrl(CountrySite, MarketInfo.Id);
    }

    public string GetLanguageUrl(string marketId)
    {
      return GetLanguageUrl(CountrySite, marketId);
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

    public string PreviousLanguageCookieValue
    {
      get { throw new NotImplementedException(); }
    }

    public IEnumerable<IMarket> GetMappedMarketsForCountrySite(string countrySite, bool includeInternalOnly)
    {
      throw new NotImplementedException();
    }
  }
}
