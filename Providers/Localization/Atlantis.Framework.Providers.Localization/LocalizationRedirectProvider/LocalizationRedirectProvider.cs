using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.RuleEngine.Results;
using System;
using System.Linq;
using System.Xml;

namespace Atlantis.Framework.Providers.Localization
{
  public class LocalizationRedirectProvider : ProviderBase, ILocalizationRedirectProvider
  {
    const string _DEFAULT_IP_COUNTRY = "US";

    IGeoProvider _geoIpProvider;
    private IGeoProvider GeoIpProvider
    {
      get
      {
        if (_geoIpProvider == null)
        {
          Container.TryResolve(out _geoIpProvider);
        }

        return _geoIpProvider;
      }
    }

    private ILocalizationProvider _localizationProvider;
    private ILocalizationProvider LocalizationProvider
    {
      get { return _localizationProvider ?? (_localizationProvider = Container.Resolve<ILocalizationProvider>()); }
    }

    public LocalizationRedirectProvider(IProviderContainer container) : base(container) { }

    private string CountryView
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.QueryString["countryview"] ?? string.Empty;
        }

        return result;
      }
    }

    private bool? _isTransPerfectProxy;
    private bool IsTransperfectProxy
    {
      get
      {
        if (!_isTransPerfectProxy.HasValue)
        {
          _isTransPerfectProxy = false;
          IProxyContext proxy;
          if (Container.TryResolve(out proxy))
          {
            _isTransPerfectProxy = proxy.IsProxyActive(ProxyTypes.TransPerfectTranslation);
          }
        }
        return _isTransPerfectProxy.Value;
      }
    }

    private string ValidateCountrySiteResponse(string countrySiteToValidate)
    {
      var validCountrySite = LocalizationProvider.CountrySite;
      
      if (countrySiteToValidate.ToUpperInvariant() == "ES")
      {
        validCountrySite = countrySiteToValidate;
      }
      else if (!countrySiteToValidate.Equals(LocalizationProvider.CountrySite, StringComparison.OrdinalIgnoreCase))
      {
        if (LocalizationProvider.IsValidCountrySubdomain(countrySiteToValidate))
        {
          validCountrySite = countrySiteToValidate;
        }
      }

      return validCountrySite;
    }

    private ILocalizationRedirectResponse GetRedirectResponse(IRuleEngineResult engineResult, string modelId)
    {
      var countrySiteResponse = LocalizationProvider.CountrySite;
      var languageResponse = LocalizationProvider.ShortLanguage;

      if (engineResult.Status != RuleEngineResultStatus.Exception)
      {
        var modelResults = engineResult.ValidationResults;
        var modelFactsResult = modelResults.FirstOrDefault(m => m.ModelId == modelId);
        if (modelFactsResult != null)
        {
          var countrySiteResult = modelFactsResult.Facts.FirstOrDefault(f => f.FactKey == RedirectSiteRuleEngineModel.CURRENT_COUNTRY_SITE_KEY);

          if (countrySiteResult != null)
          {
            countrySiteResponse = Convert.ToString(countrySiteResult.OutputValue);
          }

          var langFact = modelFactsResult.Facts.FirstOrDefault(f => f.FactKey == RedirectSiteRuleEngineModel.LANGUAGE_PREFENCE_KEY);

          if (langFact != null)
          {
            languageResponse = Convert.ToString(langFact.OutputValue);
          }
        }
      }

      countrySiteResponse = ValidateCountrySiteResponse(countrySiteResponse);

      var shouldRedirect = !countrySiteResponse.Equals(LocalizationProvider.CountrySite, StringComparison.OrdinalIgnoreCase);

      ILocalizationRedirectResponse redirectResponse = new LocalizationRedirectResponse(shouldRedirect);
      redirectResponse.CountrySite = countrySiteResponse;
      redirectResponse.ShortLanguage = languageResponse;
      return redirectResponse;
    }


    private ILocalizationRedirectResponse DetermineCountryRedirect()
    {
      var currentCountrySite = LocalizationProvider.CountrySite.ToUpperInvariant();
      ILocalizationRedirectResponse redirectResponse = null;
      if (HttpContext.Current != null && CountryView == "1")
      {
        redirectResponse = new LocalizationRedirectResponse(false);
        redirectResponse.CountrySite = currentCountrySite;
      }
      else if (IsTransperfectProxy)
      {
        redirectResponse = new LocalizationRedirectResponse(false);
        redirectResponse.CountrySite = currentCountrySite;
      }
      else
      {

        var ipCountry = _DEFAULT_IP_COUNTRY;
        if (GeoIpProvider != null)
        {
          ipCountry = GeoIpProvider.RequestCountryCode;

          if (string.IsNullOrEmpty(ipCountry) || ipCountry.Equals("US", StringComparison.OrdinalIgnoreCase))
          {
            ipCountry = _DEFAULT_IP_COUNTRY;
          }
        }

        var currentLanguage = LocalizationProvider.ShortLanguage.ToUpperInvariant();

        var countryPreference = string.Empty;
        if (!string.IsNullOrEmpty(LocalizationProvider.PreviousCountrySiteCookieValue))
        {
          countryPreference = LocalizationProvider.PreviousCountrySiteCookieValue;
        }

        var engineModel = new RedirectSiteRuleEngineModel
          {
            HasCountryCookie = !string.IsNullOrEmpty(countryPreference),
            CountryPreference = countryPreference.ToUpperInvariant(),
            CurrentSubdomainSite = currentCountrySite,
            IPCountry = ipCountry.ToUpperInvariant(),
            LanguagePreference = currentLanguage
          };

        var rules = new XmlDocument();
        rules.LoadXml(CountrySiteRedirectRuleData.CountrySiteRedirectRuleXml);

        var engineResult = RuleEngine.RuleEngine.EvaluateRules(engineModel.Model, rules);

        redirectResponse = GetRedirectResponse(engineResult, engineModel.ModelId);
      }

      return redirectResponse;
    }


    private ILocalizationRedirectResponse _redirectResponse;
    public ILocalizationRedirectResponse RedirectResponse
    {
      get
      {
        if (_redirectResponse == null)
        {
          _redirectResponse = DetermineCountryRedirect();
        }

        return _redirectResponse;
      }
    }
  }
}
