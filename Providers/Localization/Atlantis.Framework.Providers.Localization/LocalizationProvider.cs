using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atlantis.Framework.Providers.Localization
{
  public abstract class LocalizationProvider : ProviderBase, ILocalizationProvider
  {
    protected const string _WWW = "WWW";

    private readonly Lazy<string> _countrySite;
    private readonly Lazy<ValidCountrySubdomainsResponseData> _validCountrySubdomains;
    protected Lazy<CountrySiteCookie> CountrySiteCookie;

    private LanguageLocale _languageLocale = null;
    private CultureInfo _cultureInfo = null;

    public LocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _countrySite = new Lazy<string>(DetermineCountrySite);
      _validCountrySubdomains = new Lazy<ValidCountrySubdomainsResponseData>(LoadValidCountrySubdomains);
      CountrySiteCookie = new Lazy<CountrySiteCookie>(() => new CountrySiteCookie(Container));
    }

    protected abstract string DetermineCountrySite();

    private ValidCountrySubdomainsResponseData LoadValidCountrySubdomains()
    {
      var request = new ValidCountrySubdomainsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidCountrySubdomainsResponseData)DataCache.DataCache.GetProcessRequest(request, LocalizationProviderEngineRequests.ValidCountrySubdomains);
    }

    public bool IsValidCountrySubdomain(string countryCode)
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

    private LanguageLocale Language
    {
      get
      {
        if (_languageLocale == null)
        {
          string shortLanguage = "en";

          IProxyContext proxyContext;
          if (Container.TryResolve(out proxyContext))
          {
            IProxyData languageProxy;
            if (proxyContext.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out languageProxy))
            {
              string language;
              if (languageProxy.TryGetExtendedData("language", out language))
              {
                shortLanguage = language;
              }
            }
            else if (proxyContext.TryGetActiveProxy(ProxyTypes.SmartlingTranslation, out languageProxy))
            {
              string language;
              if (languageProxy.TryGetExtendedData("language", out language))
              {
                shortLanguage = language;
              }
            }
          }

          _languageLocale = LanguageLocale.FromLanguageAndCountrySite(shortLanguage, _countrySite.Value);
        }

        return _languageLocale;
      }
    }

    public string FullLanguage
    {
      get 
      {
        return Language.FullLanguage;
      }
    }

    public string ShortLanguage
    {
      get 
      {
        return Language.ShortLanguage;
      }
    }

    public bool IsActiveLanguage(string language)
    {
      bool result =
        (language.Equals(Language.ShortLanguage, StringComparison.OrdinalIgnoreCase)) ||
        (language.Equals(Language.FullLanguage, StringComparison.OrdinalIgnoreCase));

      return result;
    }

    public virtual string PreviousCountrySiteCookieValue
    {
      get
      {
        string result = null;
        if (CountrySiteCookie.Value.HasValue)
        {
          result = CountrySiteCookie.Value.Value;
        }

        return result;
      }
    }

    public void SetLanguage(string language)
    {
      _languageLocale = LanguageLocale.FromLanguageAndCountrySite(language, _countrySite.Value);
      _cultureInfo = null;
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
        CultureInfo localizedCulture = CultureInfo.GetCultureInfo(Language.FullLanguage);
        result = localizedCulture;
      }
      catch (CultureNotFoundException ex)
      {
        AtlantisException aex = new AtlantisException("LocalizationProvider.DetermineCultureInfo", 0, ex.Message + ex.StackTrace, Language.FullLanguage);
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }
  }
}
