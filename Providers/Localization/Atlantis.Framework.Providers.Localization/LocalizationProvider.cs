using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Providers.Localization
{
  public abstract class LocalizationProvider : ProviderBase, ILocalizationProvider
  {
    static Dictionary<string, string> _englishDefaultLanguages;

    static LocalizationProvider()
    {
      _englishDefaultLanguages = new Dictionary<string, string>(5, StringComparer.OrdinalIgnoreCase);
      _englishDefaultLanguages[_WWW] = "en";
      _englishDefaultLanguages["au"] = "en-au";
      _englishDefaultLanguages["ca"] = "en-ca";
      _englishDefaultLanguages["in"] = "en-in";
      _englishDefaultLanguages["uk"] = "en-gb";
    }

    protected const string _WWW = "WWW";

    private Lazy<string> _countrySite;
    private Lazy<ValidCountrySubdomainsResponseData> _validCountrySubdomains;
    private Lazy<string> _fullLanguage;
    private Lazy<string> _shortLanguage;

    public LocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _countrySite = new Lazy<string>(() => { return DetermineCountrySite(); });
      _validCountrySubdomains = new Lazy<ValidCountrySubdomainsResponseData>(() => { return LoadValidCountrySubdomains(); });
      _fullLanguage = new Lazy<string>(() => { return DetermineFullLanguage(); });
      _shortLanguage = new Lazy<string>(() => { return DetermineShortLanguage(); });
    }

    protected abstract string DetermineCountrySite();

    private ValidCountrySubdomainsResponseData LoadValidCountrySubdomains()
    {
      var request = new ValidCountrySubdomainsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidCountrySubdomainsResponseData)DataCache.DataCache.GetProcessRequest(request, LocalizationProviderEngineRequests.ValidCountrySubdomains);
    }

    protected bool IsValidCountrySubdomain(string countryCode)
    {
      return _validCountrySubdomains.Value.IsValidCountrySubdomain(countryCode);
    }

    public IEnumerable<string> ValidCountrySiteSubdomains
    {
      get { return _validCountrySubdomains.Value.ValidCountrySubdomains; }
    }

    public string CountrySite
    {
      get { return _countrySite.Value; }
    }

    public bool IsGlobalSite()
    {
      return IsCountrySite(_WWW);
    }

    public bool IsCountrySite(string countryCode)
    {
      return _countrySite.Value.Equals(countryCode, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      return countryCodes.Contains(_countrySite.Value, StringComparer.OrdinalIgnoreCase);
    }

    public string GetCountrySiteLinkType(string baseLinkType)
    {
      string result = baseLinkType;
      if (!_WWW.Equals(_countrySite.Value))
      {
        result = string.Concat(baseLinkType, ".", _countrySite.Value.ToUpperInvariant());
      }

      return result;
    }

    private string DetermineFullLanguage()
    {
      string result = _englishDefaultLanguages[_WWW];

      if (!IsGlobalSite())
      {
        string englishVariation;
        if (_englishDefaultLanguages.TryGetValue(_countrySite.Value, out englishVariation))
        {
          result = englishVariation;
        }
      }

      IProxyContext proxyContext;
      if (Container.TryResolve(out proxyContext))
      {
        IProxyData languageProxy;
        if (proxyContext.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out languageProxy))
        {
          string language;
          if (languageProxy.TryGetExtendedData("language", out language))
          {
            result = language;
          }
        }
        else if (proxyContext.TryGetActiveProxy(ProxyTypes.SmartlingTranslation, out languageProxy))
        {
          string language;
          if (languageProxy.TryGetExtendedData("language", out language))
          {
            result = language;
          }
        }
      }

      return result;
    }

    private string DetermineShortLanguage()
    {
      string result = _fullLanguage.Value;
      if (result != null)
      {
        int indexDash = result.IndexOf('-');
        if (indexDash > 0)
        {
          result = result.Substring(0, indexDash);
        }
      }
      return result;
    }

    public string FullLanguage
    {
      get 
      {
        return _fullLanguage.Value;
      }
    }

    public string ShortLanguage
    {
      get 
      {
        return _shortLanguage.Value;
      }
    }

    public bool IsActiveLanguage(string language)
    {
      bool result =
        (language.Equals(_shortLanguage.Value, StringComparison.OrdinalIgnoreCase)) ||
        (language.Equals(_fullLanguage.Value, StringComparison.OrdinalIgnoreCase));

      return result;
    }


  }
}
