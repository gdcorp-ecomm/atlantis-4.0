using System;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public class LanguageUrlRewriteProvider : ProviderBase, ILanguageUrlRewriteProvider
  {
    public static string URL_LANGUAGE_CONTEXT_KEY = "ATLANTIS_URL_LANGUAGE";

    private Lazy<ISiteContext> _siteContext;
    private Lazy<CountrySiteMarketMappingsResponseData> _countrySiteMarketMappings;
    private Lazy<ILocalizationProvider> _localizationProvider;

    public LanguageUrlRewriteProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
     _localizationProvider = new Lazy<ILocalizationProvider>(
        () => { return Container.CanResolve<ILocalizationProvider>() ? Container.Resolve<ILocalizationProvider>() : null; });
      _countrySiteMarketMappings = new Lazy<CountrySiteMarketMappingsResponseData>(LoadCountrySiteMarketMappings);
    }

    protected ILocalizationProvider LocalizationProvider
    {
      get { return _localizationProvider.Value; }
    }

    protected ISiteContext SiteContext
    {
      get { return _siteContext.Value; }
    }

    private CountrySiteMarketMappingsResponseData LoadCountrySiteMarketMappings()
    {
      CountrySiteMarketMappingsResponseData result;

      var request = new CountrySiteMarketMappingsRequestData(_localizationProvider.Value.CountrySite);
      try
      {
        result = (CountrySiteMarketMappingsResponseData)DataCache.DataCache.GetProcessRequest(request, LocalizationProviderEngineRequests.CountrySiteMarketMappingsRequest);
        if (result.NoMappings)
        {
          result = CountrySiteMarketMappingsResponseData.FromCountrySiteAndMarketId(_localizationProvider.Value.CountrySite,
                                                                      _localizationProvider.Value.MarketInfo.Id);
        }
      }
      catch
      {
        result = CountrySiteMarketMappingsResponseData.FromCountrySiteAndMarketId(_localizationProvider.Value.CountrySite,
                                                                      _localizationProvider.Value.MarketInfo.Id);
      }

      return result;
    }

    public void ProcessLanguageUrl()
    {
      try
      {
        if (IsTransperfectProxyActive() || _localizationProvider.Value == null)
        {
          return;
        }

        string urlLanguage = GetUrlLanguage();

        if (String.IsNullOrEmpty(urlLanguage))
        {
          _localizationProvider.Value.SetMarket(_localizationProvider.Value.CountrySiteInfo.DefaultMarketId);
        }
        else if (IsDefaultLanguageUrl(urlLanguage) && HttpContextFactory.GetHttpContext().Request.HttpMethod == "GET")
        {
          RedirectToDefaultUrl(urlLanguage);
        }
        else
        {
          RewriteRequestUrl(urlLanguage);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("LanguageUrlRewriteProvider.ProcessLanguageUrl", "0", "Error processing language url.", message, null, null);
        Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
      }
    }

    private void RedirectToDefaultUrl(string urlLanguage)
    {
      string newUrl = GetPermanentRedirectUrl(urlLanguage);
      HttpContextFactory.GetHttpContext().Response.RedirectPermanent(newUrl);
    }

    private void RewriteRequestUrl(string urlLanguage)
    {
      string newPath = RemoveLanguageCodeFromUrlPath(urlLanguage);
      _localizationProvider.Value.RewrittenUrlLanguage = urlLanguage;
      HttpContextFactory.GetHttpContext().RewritePath(newPath);
      string marketId;
      if (_countrySiteMarketMappings.Value.TryGetMarketIdByCountrySiteAndUrlLanguage(urlLanguage, out marketId))
      {
        _localizationProvider.Value.SetMarket(marketId);
      }
    }

    private bool IsTransperfectProxyActive()
    {
      bool result = false;

      IProxyContext proxyContext;
      if (Container.TryResolve(out proxyContext))
      {
        result = proxyContext.IsProxyActive(ProxyTypes.TransPerfectTranslation);
      }

      return result;
    }

    #region Helper methods
    private bool IsDefaultLanguageUrl(string language)
    {
      bool result = false;

      if (_localizationProvider.Value != null)
      {
        string marketId;
        if (_countrySiteMarketMappings.Value.TryGetMarketIdByCountrySiteAndUrlLanguage(language, out marketId))
        {
          result = marketId.Equals(_localizationProvider.Value.CountrySiteInfo.DefaultMarketId,
                                   StringComparison.OrdinalIgnoreCase);
        }
      }

      return result;
    }

    private string GetPermanentRedirectUrl(string urlLanguage)
    {
      //  Get RawUrl path because it will not contain virtual directory info added by a url rewrite
      string rawUrlPath = HttpContextFactory.GetHttpContext().Request.RawUrl.Split('?')[0];
      string updatedPath = RemoveLanguageCode(rawUrlPath, urlLanguage);

      UriBuilder newUrlBuilder = new UriBuilder(HttpContextFactory.GetHttpContext().Request.Url);
      newUrlBuilder.Path = updatedPath;
      return newUrlBuilder.Uri.ToString();
    }

    private string RemoveLanguageCode(string path, string languageCode)
    {
      if (String.IsNullOrWhiteSpace(languageCode))
      {
        return path;
      }

      Regex re = new Regex("/" + languageCode + "/", RegexOptions.IgnoreCase);
      return re.Replace(path, "/", 1);
    }

    private string RemoveLanguageCodeFromUrlPath(string languageCode = null)
    {
      if (String.IsNullOrWhiteSpace(languageCode))
      {
        languageCode = GetUrlLanguage();
      }

      UriBuilder newUrlBuilder = new UriBuilder(HttpContextFactory.GetHttpContext().Request.Url);
      if (!string.IsNullOrWhiteSpace(languageCode))
      {
        return RemoveLanguageCode(newUrlBuilder.Path, languageCode);
      }
      else
      {
        return newUrlBuilder.Path;
      }
    }

    private string GetUrlLanguage()
    {
      string result = string.Empty;

      if (HttpContextFactory.GetHttpContext().Request.AppRelativeCurrentExecutionFilePath != null)
      {
        string appRelativePath = HttpContextFactory.GetHttpContext().Request.AppRelativeCurrentExecutionFilePath.Replace("~/", string.Empty);
        if (appRelativePath != String.Empty)
        {
          string[] segments = appRelativePath.Split('/');
          if (segments.Length > 0)
          {
            string firstSegment = segments[0];
            if (IsValidLanguageCode(firstSegment))
            {
              result = firstSegment;
            }
          }
        }
      }
      
      return result;
    }

    private bool IsValidLanguageCode(string language)
    {
      bool result = false;

      if (_countrySiteMarketMappings.Value != null)
      {
        result = _countrySiteMarketMappings.Value.IsValidUrlLanguageForCountrySite(language);
      }

      return result;
    }

    #endregion
  }
}
