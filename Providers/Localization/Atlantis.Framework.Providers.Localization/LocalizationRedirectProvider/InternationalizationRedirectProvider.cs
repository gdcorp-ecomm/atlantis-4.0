using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public class InternationalizationRedirectProvider : ProviderBase, ILocalizationRedirectProvider
  {
    const string _DEFAULT_COUNTRY_SITE = "www";
    const string _DEFAULT_MARKET_ID = "en-US";    

    public InternationalizationRedirectProvider(IProviderContainer container)
      : base(container)
    {

    }

    private IGeoProvider _geoIpProvider;
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

    private ILanguageUrlRewriteProvider _languageUrlRewriteProvider;
    private ILanguageUrlRewriteProvider LanguageUrlRewriteProvider
    {
      get { return _languageUrlRewriteProvider ?? (_languageUrlRewriteProvider = Container.Resolve<ILanguageUrlRewriteProvider>()); }
    }    

    private string CountryView
    {
      get
      {
        string result = string.Empty;
        HttpContextBase context = HttpContextFactory.GetHttpContext();
        if (context != null)
        {
          result = context.Request.QueryString["countryview"] ?? string.Empty;
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

    #region ILocalizationRedirectProvider Members

    private ILocalizationRedirectResponse _redirectResponse;
    public ILocalizationRedirectResponse RedirectResponse
    {
      get
      {
        if (_redirectResponse == null)
        {
          _redirectResponse = DetermineInternationalizationRedirect();
        }

        return _redirectResponse;
      }
    }

    #endregion

    private ILocalizationRedirectResponse DetermineInternationalizationRedirect()
    {
      ILocalizationRedirectResponse redirectResponse = null;

      if (IsTransperfectProxy)
      {
        redirectResponse = new LocalizationRedirectResponse(false);
        redirectResponse.CountrySite = LocalizationProvider.CountrySite.ToUpperInvariant();
        redirectResponse.MarketId = "es-US";
        redirectResponse.ShortLanguage = "es";
      }
      else if (LocalizationProvider.IsGlobalSite() && !string.IsNullOrEmpty(LocalizationProvider.PreviousLanguageCookieValue) && LocalizationProvider.PreviousLanguageCookieValue.Equals("es", StringComparison.OrdinalIgnoreCase))
      {
        redirectResponse = new LocalizationRedirectResponse(true);
        redirectResponse.CountrySite = LocalizationProvider.CountrySite.ToUpperInvariant();
        redirectResponse.MarketId = "es-US";
        redirectResponse.ShortLanguage = "es";        
      }
      else
      {
        redirectResponse = GetRedirectResponse();
      }

      if (redirectResponse != null && !string.IsNullOrEmpty(redirectResponse.MarketId))
      {
        SetCookies(redirectResponse);
      }

      return redirectResponse;
    }

    private void SetCookies(ILocalizationRedirectResponse redirectResponse)
    {
      //  Set the market id which will set the language cookie
      LocalizationProvider.SetMarket(redirectResponse.MarketId);
    }

    private ILocalizationRedirectResponse GetCountrySiteRedirectResponse()
    {
      ILocalizationRedirectResponse redirectResponse = null;

      //  Check to see if there is a language in the URL path and redirect
      if (TryGetRedirectResponseFromUrlLanguage(out redirectResponse))
      {
        return redirectResponse;
      }

      //  Get the countrysite for redirect process
      string redirectCountrySite = LocalizationProvider.CountrySite.ToLowerInvariant();

      //  Get the market id for redirect process
      string redirectMarketId = GetRedirectMarketId(redirectCountrySite);

      if (!string.IsNullOrEmpty(redirectMarketId) && TryGetRedirectResponse(redirectCountrySite, redirectMarketId, out redirectResponse))
      {
        return redirectResponse;
      }

      //  Get the market id from browser language and try to redirect
      if (TryGetRedirectResponseFromBrowserLanguage(redirectCountrySite, out redirectResponse))
      {
        return redirectResponse;
      }

      //  Get the default market id and try to redirect
      if (TryGetRedirectResponseFromDefaultMarket(redirectCountrySite, out redirectResponse))
      {
        return redirectResponse;
      }

      TryGetRedirectResponse(_DEFAULT_COUNTRY_SITE, _DEFAULT_MARKET_ID, out redirectResponse);
        
      return redirectResponse;
    }

    private ILocalizationRedirectResponse GetGlobalSiteRedirectResponse()
    {
      ILocalizationRedirectResponse redirectResponse = null;

      //  Check to see if there is a language in the URL path and redirect
      if (TryGetRedirectResponseFromUrlLanguage(out redirectResponse))
      {
        return redirectResponse;
      }

      //  Get the countrysite for redirect process
      string redirectCountrySite = GetRedirectCountrySite();

      //  Get the market id for redirect process
      string redirectMarketId = GetRedirectMarketId(redirectCountrySite);

      //  If no previous cookies, check querystring values
      if (string.IsNullOrEmpty(redirectCountrySite) && string.IsNullOrEmpty(redirectMarketId))
      {
        Localization.LocalizationProvider.TryGetRegionSiteFromQueryString(out redirectCountrySite);
        Localization.LocalizationProvider.TryGetMarketIdFromQueryString(out redirectMarketId);        
      }

      //  Try to see if a redirect is needed for the cookie countrysite and market id
      if (TryGetRedirectResponse(redirectCountrySite, redirectMarketId, out redirectResponse))
      {
        return redirectResponse;
      }

      //  Check if redirect countrysite is still missing
      if (string.IsNullOrEmpty(redirectCountrySite))
      {
        //  Get redirect countrysite from IP and try to redirect if it is valid and only has one market site
        if (TryGetRedirectResponseFromIpCountry(out redirectCountrySite, out redirectResponse))
        {
          return redirectResponse;
        }
      }

      //  If we have the country site but didn't find a redirect market, check the browser languages
      if (!string.IsNullOrEmpty(redirectCountrySite))
      {
        //  Get the market id from browser language and try to redirect
        if (TryGetRedirectResponseFromBrowserLanguage(redirectCountrySite, out redirectResponse))
        {
          return redirectResponse;
        }

        //  Get the default market id and ty to redirect
        if (TryGetRedirectResponseFromDefaultMarket(redirectCountrySite, out redirectResponse))
        {
          return redirectResponse;
        }
      }

      //  Try to redirect using the default countrysite and market id
      TryGetRedirectResponse(_DEFAULT_COUNTRY_SITE, _DEFAULT_MARKET_ID, out redirectResponse);

      return redirectResponse;
    }

    /// <summary>
    /// Get market ID for a valid language in the url path.  Return a redirect response if valid combination
    /// If the language in the URL is valid for the LocalizationProvider.CountrySite then no redirect is required.
    /// </summary>
    /// <param name="redirectResponse">ILocalizationRedirectResponse object if valid countrysite and marketid pair</param>
    /// <returns>true if valid countrysite and market id pair</returns>
    private bool TryGetRedirectResponseFromUrlLanguage(out ILocalizationRedirectResponse redirectResponse)
    {
      bool result = false;
      redirectResponse = null;
      string language;
      string redirectMarketId;

      if (LanguageUrlRewriteProvider != null && 
        LanguageUrlRewriteProvider.TryGetUrlLanguageAndMarketId(out language, out redirectMarketId))
      {
        result = true;
        redirectResponse = new LocalizationRedirectResponse(false);
        redirectResponse.CountrySite = LocalizationProvider.CountrySite;
        redirectResponse.MarketId = redirectMarketId;
        redirectResponse.ShortLanguage = language;
      }

      return result;
    }

    private ILocalizationRedirectResponse GetRedirectResponse()
    {
      ILocalizationRedirectResponse result = null;
      if (!LocalizationProvider.IsGlobalSite())
      {
        result = GetCountrySiteRedirectResponse();
      }
      else
      {
        result = GetGlobalSiteRedirectResponse();
      }
      return result;
    }

    /// <summary>
    /// Get the initial market id to redirect to; Adjust language-only values 
    /// </summary>
    /// <param name="redirectCountrySite"></param>
    /// <returns></returns>
    private string GetRedirectMarketId(string redirectCountrySite)
    {
      string marketId = LocalizationProvider.PreviousLanguageCookieValue ?? string.Empty;
      if (marketId.Length == 2)
      {
        if (!string.IsNullOrEmpty(redirectCountrySite))
        {
          if (redirectCountrySite.Equals(_DEFAULT_COUNTRY_SITE, StringComparison.OrdinalIgnoreCase))
          {
            return marketId + "-US";
          }
          
          return marketId + "-" + redirectCountrySite;
        }
        else
        {
          return marketId + "-" + marketId; // es-es; fr-fr; pt-pt
        }
      }
      return marketId;
    }

    /// <summary>
    /// Get the initial country site to redirect to
    /// </summary>
    /// <returns></returns>
    private string GetRedirectCountrySite()
    {
      HttpContextBase context = HttpContextFactory.GetHttpContext();
      if (context != null && CountryView == "1")
      {
        return LocalizationProvider.CountrySite;
      }

      string result = LocalizationProvider.PreviousCountrySiteCookieValue ?? string.Empty;
      return result.Equals("US", StringComparison.OrdinalIgnoreCase) ? _DEFAULT_COUNTRY_SITE : result;
    }

    /// <summary>
    /// Get default market ID for the specified countrysite and return a redirect response if valid combination
    /// </summary>
    /// <param name="redirectCountrySite"></param>
    /// <param name="redirectResponse">ILocalizationRedirectResponse object if valid countrysite and marketid pair</param>
    /// <returns>true if valid countrysite and market id pair</returns>
    private bool TryGetRedirectResponseFromDefaultMarket(string redirectCountrySite, out ILocalizationRedirectResponse redirectResponse)
    {
      bool result = false;
      redirectResponse = null;

      ICountrySite countrySite = LocalizationProvider.TryGetCountrySite(redirectCountrySite);
      if (countrySite != null)
      {
        result = TryGetRedirectResponse(redirectCountrySite, countrySite.DefaultMarketId, out redirectResponse);
      }

      return result;
    }

    /// <summary>
    /// Find redirect market ID from browser languages for the specified countrysite and return a redirect response if valid combination
    /// </summary>
    /// <param name="redirectCountrySite"></param>
    /// <param name="redirectResponse">ILocalizationRedirectResponse object if valid countrysite and marketid pair</param>
    /// <returns>true if valid countrysite and market id pair</returns>
    private bool TryGetRedirectResponseFromBrowserLanguage(string redirectCountrySite, out ILocalizationRedirectResponse redirectResponse)
    {
      bool result = false;
      redirectResponse = null;

      HttpContextBase context = HttpContextFactory.GetHttpContext();

      //  Cannot get any UserLanguage
      if (context == null || context.Request == null ||
          context.Request.UserLanguages == null || context.Request.UserLanguages.Length == 0)
      {
        return result;
      }

      //  There is only one user language so treat it as the default and try to redirect
      if (context.Request.UserLanguages.Length == 1)
      {
        string redirectMarketId = ConvertLanguageToMarketId(context.Request.UserLanguages[0], redirectCountrySite);
        result = TryGetRedirectResponse(redirectCountrySite, redirectMarketId, out redirectResponse);
        return result;
      }

      //  Loop through the UserLanguages to find the first one that is valid for the countrysite
      foreach (string userLanguage in context.Request.UserLanguages)
      {
        //  Remove q value that is passed in by browsers (en-US;q=0.9)
        //  Browsers will pass prioritized values in descending order  
        string marketId = userLanguage.Split(';')[0];

        //  Adjust language-only values
        string redirectMarketId = ConvertLanguageToMarketId(marketId, redirectCountrySite);
        result = TryGetRedirectResponse(redirectCountrySite, redirectMarketId, out redirectResponse);
        if (result)
        {
          break;
        }
      }

      return result;
    }

    /// <summary>
    /// Convert a language-only string into Market ID format
    /// </summary>
    /// <param name="language"></param>
    /// <param name="countrySite"></param>
    /// <returns></returns>
    private string ConvertLanguageToMarketId(string language, string countrySite)
    {
      if (language.Length == 2)
      {
        return language + "-" + (countrySite.Equals(_DEFAULT_COUNTRY_SITE, StringComparison.OrdinalIgnoreCase) ? "us" : countrySite);
      }
      else
      {
        return language;
      }
    }

    /// <summary>
    /// Get countrysite from IP Address then return a redirect response if valid and single-language countrysite
    /// </summary>
    /// <param name="redirectCountrySite">Valid countrysite from IP Address; Default is www</param>
    /// <param name="redirectResponse">ILocalizationRedirectResponse object if valid countrysite and marketid pair</param>
    /// <returns>true if valid countrysite and market id pair</returns>
    private bool TryGetRedirectResponseFromIpCountry(out string redirectCountrySite, out ILocalizationRedirectResponse redirectResponse)
    {
      bool result = false;
      redirectCountrySite = _DEFAULT_COUNTRY_SITE;
      redirectResponse = null;

      string countrySite = GetCountrySiteFromIpCountry();
      if (!string.IsNullOrEmpty(countrySite))
      {
        redirectCountrySite = countrySite;

        //  TODO:  Evaluate to see if it can be refactored for performance;
        IEnumerable<IMarket> markets = LocalizationProvider.GetMarketsForCountryCode(countrySite);
        if (markets.Count() == 1)
        {
          string redirectMarketId = markets.First().Id;
          result = TryGetRedirectResponse(redirectCountrySite, redirectMarketId, out redirectResponse);
        }
      }

      return result;
    }

    /// <summary>
    /// Get redirect response if valid combination of countrysite and market id
    /// </summary>
    /// <param name="redirectCountrySite"></param>
    /// <param name="redirectMarketId"></param>
    /// <param name="redirectResponse">ILocalizationRedirectResponse object if valid countrysite and marketid pair</param>
    /// <returns>true if valid countrysite and market id pair</returns>
    private bool TryGetRedirectResponse(string redirectCountrySite, string redirectMarketId,
                                        out ILocalizationRedirectResponse redirectResponse)
    {
      bool result = false;
      redirectResponse = null;

      if (IsValidMarketForCountrySite(redirectCountrySite, redirectMarketId))
      {
        bool shouldRedirect = !redirectCountrySite.Equals(LocalizationProvider.CountrySite, StringComparison.OrdinalIgnoreCase) ||
          !redirectMarketId.Equals(LocalizationProvider.MarketInfo.Id, StringComparison.OrdinalIgnoreCase);
        redirectResponse = new LocalizationRedirectResponse(shouldRedirect);
        redirectResponse.CountrySite = redirectCountrySite;
        redirectResponse.MarketId = redirectMarketId;
        redirectResponse.ShortLanguage = LocalizationProvider.GetLanguageUrl(redirectCountrySite, redirectMarketId);
        result = true;
      }

      return result;
    }

    private string GetCountrySiteFromIpCountry()
    {
      string result = null;

      if (GeoIpProvider != null)
      {
        result = GeoIpProvider.RequestCountryCode;

        if (!string.IsNullOrEmpty(result))
        {
          if (result.Equals("US", StringComparison.OrdinalIgnoreCase))
          {
            result = _DEFAULT_COUNTRY_SITE;
          }
          else if (!LocalizationProvider.IsValidCountrySubdomain(result))
          {
            result = null;
          }
        }
      }

      return result;
    }

    private bool IsValidMarketForCountrySite(string countrySite, string marketId)
    {
      if (string.IsNullOrEmpty(countrySite) || string.IsNullOrEmpty(marketId))
      {
        return false;
      }

      IMarket market = LocalizationProvider.TryGetMarketForCountrySite(countrySite, marketId);
      return market != null;
    }

  }
}
