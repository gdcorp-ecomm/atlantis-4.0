using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atlantis.Framework.Testing.MockLocalization
{
  public class MockLocalizationProvider : MockProviderBase, ILocalizationProvider
  {
    const string _DEFAULTCOUNTRYSITE = "www";
    const string _DEFAULTLANGUAGE = "en";

    string _countrySite;
    string _fullLanguage;
    IMarket _marketInfo = null;
    ICountrySite _countrySiteInfo = null;
    private CultureInfo _cultureInfo = null;
    string _rewrittenUrlLanguage = String.Empty;

    public MockLocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _countrySiteInfo = LoadCountySiteInfo();
      _marketInfo = LoadMarketInfo();
      _countrySite = LoadMockCountrySite();
      _fullLanguage = LoadFullLanguage();
    }

    private ICountrySite LoadCountySiteInfo()
    {
      ICountrySite result = null;

      ICountrySite countrySite = GetMockSetting(MockLocalizationProviderSettings.CountrySiteInfo) as ICountrySite;
      if (countrySite != null)
      {
        result = countrySite;
      }

      return result;
    }

    private IMarket LoadMarketInfo()
    {
      IMarket result = null;

      IMarket marketInfo = GetMockSetting(MockLocalizationProviderSettings.MarketInfo) as IMarket;
      if (marketInfo != null)
      {
        result = marketInfo;
      }

      return result;
    }

    private string LoadFullLanguage()
    {
      string result = _DEFAULTLANGUAGE;

      if (MarketInfo != null)
      {
        return MarketInfo.Id;
      }

      string fullLanguage = GetMockSetting(MockLocalizationProviderSettings.FullLanguage) as string;
      if (fullLanguage != null)
      {
        result = fullLanguage;
      }

      return result;
    }

    private string LoadMockCountrySite()
    {
      string result = _DEFAULTCOUNTRYSITE;

      if (CountrySiteInfo != null)
      {
        return CountrySiteInfo.Id;
      }

      string countrySite = GetMockSetting(MockLocalizationProviderSettings.CountrySite) as string;
      if (countrySite != null)
      {
        result = countrySite;
      }

      return result;
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

    #region ILocalizationProvider Members


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

    #endregion
  }
}
