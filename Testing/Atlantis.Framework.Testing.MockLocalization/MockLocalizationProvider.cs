using System.Globalization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Testing.MockLocalization
{
  public class MockLocalizationProvider : MockProviderBase, ILocalizationProvider
  {
    const string _DEFAULTCOUNTRYSITE = "www";
    const string _DEFAULTLANGUAGE = "en";

    Lazy<string> _countrySite;
    Lazy<string> _fullLanguage;

    public MockLocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _countrySite = new Lazy<string>(() => LoadMockCountrySite());
      _fullLanguage = new Lazy<string>(() => LoadFullLanguage());
    }

    private string LoadFullLanguage()
    {
      string result = _DEFAULTLANGUAGE;

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

      string countrySite = GetMockSetting(MockLocalizationProviderSettings.CountrySite) as string;
      if (countrySite != null)
      {
        result = countrySite;
      }

      return result;
    }

    public string FullLanguage
    {
      get { return _fullLanguage.Value; }
    }

    public string ShortLanguage
    {
      get 
      {
        string result = _fullLanguage.Value;
        if (_fullLanguage.Value != null)
        {
          int dashPosition = _fullLanguage.Value.IndexOf('-');
          if (dashPosition > -1)
          {
            result = _fullLanguage.Value.Substring(0, dashPosition);
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
      get { return _countrySite.Value; }
    }

    public bool IsGlobalSite()
    {
      return _DEFAULTCOUNTRYSITE.Equals(_countrySite.Value, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsCountrySite(string countryCode)
    {
      if (countryCode == null)
      {
        return false;
      }

      return countryCode.Equals(_countrySite.Value, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      bool result = false;
      if (countryCodes != null)
      {
        result = countryCodes.Contains(_countrySite.Value, StringComparer.OrdinalIgnoreCase);
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
      if (!IsGlobalSite() && (_countrySite.Value != null))
      {
        result = string.Concat(baseLinkType, "." + _countrySite.Value.ToUpperInvariant());
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

    public void SetLanguage(string language)
    {
    }

    public CultureInfo CurrentCultureInfo { get; private set; }
  }
}
