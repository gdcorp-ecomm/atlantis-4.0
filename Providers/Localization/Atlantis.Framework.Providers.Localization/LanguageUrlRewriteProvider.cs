using System;
using System.Text.RegularExpressions;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public class LanguageUrlRewriteProvider : ProviderBase, ILanguageUrlRewriteProvider
  {
    private const string REQUEST_SAVE_KEY = "REWRITTENURLLANGUAGEINFO";
    public static string ROOT_DEFAULT_DOCUMENT = "default.aspx";

    private Lazy<ISiteContext> _siteContext;
    private Lazy<MarketsActiveResponseData> _activeMarkets; 
    private Lazy<CountrySiteMarketMappingsResponseData> _countrySiteMarketMappings;
    private Lazy<ILocalizationProvider> _localizationProvider;

    public LanguageUrlRewriteProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _localizationProvider = new Lazy<ILocalizationProvider>(
        () => { return Container.CanResolve<ILocalizationProvider>() ? Container.Resolve<ILocalizationProvider>() : null; });
      _countrySiteMarketMappings = new Lazy<CountrySiteMarketMappingsResponseData>(LoadCountrySiteMarketMappings);
      _activeMarkets = new Lazy<MarketsActiveResponseData>(LoadActiveMarkets);
    }

    protected ISiteContext SiteContext
    {
      get { return _siteContext.Value; }
    }

    private MarketsActiveResponseData LoadActiveMarkets()
    {
      MarketsActiveResponseData result;

      var request = new MarketsActiveRequestData();
      try
      {
        result =
          (MarketsActiveResponseData)
          DataCache.DataCache.GetProcessRequest(request, LocalizationProviderEngineRequests.MarketsActiveRequest);
      }
      catch
      {
        result = MarketsActiveResponseData.DefaultMarkets;
      }

      return result;
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
        if (!IsValidHost() || IsTransperfectProxyActive() || _localizationProvider.Value == null)
        {
          return;
        }

        string validMarketId;
        string urlLanguage = GetUrlLanguage(out validMarketId);

        if (String.IsNullOrEmpty(urlLanguage))
        {
          if (!string.IsNullOrEmpty(validMarketId))
          {
            RedirectToDefaultUrl(validMarketId);
          }
          else
          {
            _localizationProvider.Value.SetMarket(_localizationProvider.Value.CountrySiteInfo.DefaultMarketId);
          }
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
      HttpContextFactory.GetHttpContext().Response.Cache.SetCacheability(HttpCacheability.NoCache);
      HttpContextFactory.GetHttpContext().Response.RedirectPermanent(newUrl, false);
      HttpContextFactory.GetHttpContext().ApplicationInstance.CompleteRequest();
    }

    private void RewriteRequestUrl(string urlLanguage)
    {
      _localizationProvider.Value.RewrittenUrlLanguage = urlLanguage;
      string newPath = GetLanguageFreeUrlPath(urlLanguage);

      //  Rewrite the path only if language URL was stripped off
      if (!newPath.Equals(HttpContextFactory.GetHttpContext().Request.Path, StringComparison.OrdinalIgnoreCase))
      {
        HttpContextFactory.GetHttpContext().RewritePath(AdjustForDefaultDocument(newPath));
      }

      string marketId;
      if (_countrySiteMarketMappings.Value.TryGetMarketIdByCountrySiteAndUrlLanguage(urlLanguage, out marketId))
      {
        _localizationProvider.Value.SetMarket(marketId);
      }

      //  Save the rewritten values in case they are lost due to child request processing.
      SaveRequestLanguageUrlInfo(urlLanguage, _localizationProvider.Value.MarketInfo.Id);
    }

    private void SaveRequestLanguageUrlInfo(string language, string marketId)
    {
      HttpContextFactory.GetHttpContext().Request.Headers[REQUEST_SAVE_KEY] = language + "|" + marketId;
    }

    private string AdjustForDefaultDocument(string newPath)
    {
      if (newPath == "/" || newPath.Equals(HttpContextFactory.GetHttpContext().Request.ApplicationPath + "/", StringComparison.OrdinalIgnoreCase))
      {
        return string.Format("{0}{1}", newPath, ROOT_DEFAULT_DOCUMENT);
      }
      
      if (newPath == String.Empty || newPath.Equals(HttpContextFactory.GetHttpContext().Request.ApplicationPath, StringComparison.OrdinalIgnoreCase))
      {
        return string.Format("{0}/{1}", newPath, ROOT_DEFAULT_DOCUMENT);
      }

      return newPath;
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
    private bool IsValidHost()
    {
      try
      {
        var baseUri = string.Concat("http://", HttpContext.Current.Request.Headers["Host"], "/");
        Uri hostUri;
        return Uri.TryCreate(baseUri, UriKind.Absolute, out hostUri);
      }
      catch (Exception)
      {
        return false;
      }
    }

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
      //  Get RawUrl path because it will not contain virtual directory info IF added by a url rewrite
      string rawUrlPath = HttpContextFactory.GetHttpContext().Request.RawUrl.Split('?')[0];
      string updatedPath = RemoveLanguageCode(rawUrlPath, urlLanguage);

      UriBuilder newUrlBuilder = new UriBuilder(HttpContextFactory.GetHttpContext().Request.Url);
      newUrlBuilder.Path = updatedPath;
      return newUrlBuilder.Uri.ToString();
    }

    private string RemoveLanguageCode(string path, string languageCode)
    {
      Regex re = new Regex("/" + languageCode + "\\b", RegexOptions.IgnoreCase);
      return re.Replace(path, "", 1);
    }

    private string GetLanguageFreeUrlPath(string languageCode = null)
    {
      string validMarketId;
      string result;

      if (String.IsNullOrWhiteSpace(languageCode))
      {
        languageCode = GetUrlLanguage(out validMarketId);
      }

      if (!string.IsNullOrWhiteSpace(languageCode))
      {
        result = RemoveLanguageCode(HttpContextFactory.GetHttpContext().Request.Path, languageCode);
      }
      else
      {
        result = HttpContextFactory.GetHttpContext().Request.Path;
      }

      return result;
    }

    private string GetUrlLanguage(out string validMarketId)
    {
      string result;

      TryGetUrlLanguageAndMarketId(out result, out validMarketId);
      
      return result;
    }

    public bool TryGetSavedRequestLanguageUrlInfo(out string savedLanguage, out string savedMarketId)
    {
      bool result = false;
      savedLanguage = null;
      savedMarketId = null;

      string headerValue = HttpContextFactory.GetHttpContext().Request.Headers[REQUEST_SAVE_KEY];
      if (!string.IsNullOrEmpty(headerValue))
      {
        string[] languageMarket = headerValue.Split('|');
        if (languageMarket.Length == 2)
        {
          savedLanguage = languageMarket[0];
          savedMarketId = languageMarket[1];
          result = true;
        }        
      }
      return result;
    }

    public bool IsValidLanguageCodeForRequest(string language)
    {
      bool result = false;

      if (_countrySiteMarketMappings.Value != null)
      {
        result = _countrySiteMarketMappings.Value.IsValidUrlLanguageForCountrySite(language, _siteContext.Value.IsRequestInternal);
      }

      return result;
    }

    public bool TryGetUrlLanguageAndMarketId(out string language, out string validMarketId)
    {
      bool result = false;
      validMarketId = String.Empty;
      language = String.Empty;

      HttpContextBase context = HttpContextFactory.GetHttpContext();
      if (context.Request.AppRelativeCurrentExecutionFilePath != null)
      {
        string appRelativePath = context.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", string.Empty);
        if (appRelativePath != String.Empty)
        {
          string[] segments = appRelativePath.Split('/');
          if (segments.Length > 0)
          {
            IMarket market;
            string firstSegment = segments[0];
            if (IsValidLanguageCodeForRequest(firstSegment))
            {
              result = true;
              language = firstSegment;
              _countrySiteMarketMappings.Value.TryGetMarketIdByCountrySiteAndUrlLanguage(language, out validMarketId);
            }
            else if (_activeMarkets.Value.TryGetMarketById(firstSegment, out market) && market != null)
            {
              validMarketId = firstSegment;
            }
          }
        }
      }
      
      if (result) return result;

      //  If no valid language was found then check for saved language info
      string savedLanguage;
      string savedMarketId;
      if (TryGetSavedRequestLanguageUrlInfo(out savedLanguage, out savedMarketId))
      {
        result = true;
        language = savedLanguage;
        validMarketId = savedMarketId;
      }

      return result;
    }
    #endregion
  }
}
