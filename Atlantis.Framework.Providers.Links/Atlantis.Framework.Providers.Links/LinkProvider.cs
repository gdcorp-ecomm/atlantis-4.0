using System.Diagnostics;
using System.Linq;
using Atlantis.Framework.DotNetExtensions.StringBuilder;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Atlantis.Framework.Providers.Links
{
  public class LinkProvider : ProviderBase, ILinkProvider
  {
    private static bool _lowerCaseRelativeUrlsForSEO = false;
    /// <summary>
    /// Setting this to true will lowercase all urls that don't use GetURL (same site urls) to improve SEO
    /// </summary>
    public static bool LowerCaseRelativeUrlsForSEO
    {
      get { return _lowerCaseRelativeUrlsForSEO; }
      set { _lowerCaseRelativeUrlsForSEO = value; }
    }

    private const int _RESELLERCONTEXTID = 6;

    /// <summary>
    /// Runs a standard UrlEncode and then also encodes single quotes to %27
    /// </summary>
    /// <param name="text">String to encode</param>
    /// <returns>Url encoded string</returns>
    public static string ImprovedUrlEncode(string text)
    {
      string result = HttpUtility.UrlEncode(text);
      if (result != null)
      {
        result = result.Replace("'", "%27");
      }

      return result;
    }

    private const string _schemeNonSecure = "http://";
    private const string _schemeSecure = "https://";
    private const string _schemeNone = "//";

    private readonly Dictionary<int, IDictionary<string, ILinkInfo>> _linkMaps;
    private readonly ISiteContext _siteContext;
    private Lazy<bool> _useC3ImageUrls;
    private Lazy<ILocalizationProvider> _localizationProvider;

    public LinkProvider(IProviderContainer container)
      : base(container)
    {
      _linkMaps = new Dictionary<int, IDictionary<string, ILinkInfo>>();
      _siteContext = container.Resolve<ISiteContext>();
      _useC3ImageUrls = new Lazy<bool>(() => { return GetUseC3ImageUrls(); });
      _localizationProvider = new Lazy<ILocalizationProvider>(
        () => { return Container.CanResolve<ILocalizationProvider>() ? Container.Resolve<ILocalizationProvider>() : null; });
    }

    private bool GetUseC3ImageUrls()
    {
      string useC3ImageUrlsSetting = DataCache.DataCache.GetAppSetting("LINKPROVIDER.USEC3IMAGEURLS");
      return "true".Equals(useC3ImageUrlsSetting, StringComparison.OrdinalIgnoreCase);
    }

    private string _contextHost;
    private string ContextHost
    {
      get
      {
        if (_contextHost == null)
        {
          _contextHost = HttpContext.Current.Request.Url.Host.ToLowerInvariant();
          IProxyContext proxy;
          if (Container.TryResolve(out proxy))
          {
            _contextHost = proxy.ContextHost;
          }
        }
        return _contextHost;
      }
    }

    private bool? _isTransperfectProxy;
    private bool IsTransperfectProxy
    {
      get
      {
        if (!_isTransperfectProxy.HasValue)
        {
          _isTransperfectProxy = false;
          IProxyContext proxyContext;
          if (Container.TryResolve(out proxyContext))
          {
            _isTransperfectProxy = proxyContext.IsProxyActive(ProxyTypes.TransPerfectTranslation);
          }
        }
        return _isTransperfectProxy.Value;
      }
    }

    public bool IsDebugInternal()
    {
      return _siteContext.IsRequestInternal &&
             (ContextHost.Contains(".debug.") || ContextHost.StartsWith("debug."));
    }

    private bool IsLocalImageServerUrl(string relativePath)
    {
      return (IsDebugInternal() && relativePath.Contains("/_ImageServer/"));
    }

    private bool IsSecureConnection
    {
      get
      {
        bool isSecure = false;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request != null)
          {
            isSecure = HttpContext.Current.Request.IsSecureConnection;
          }
        }
        return isSecure;
      }
    }

    private IDictionary<string, ILinkInfo> GetLinkMap(int contextId)
    {
      IDictionary<string, ILinkInfo> result;
      if (!_linkMaps.TryGetValue(contextId, out result))
      {
        var request = new LinkInfoRequestData(contextId);
        var response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, LinkProviderEngineRequests.LinkInfo);

        result = response.Links;
        _linkMaps[contextId] = result;
      }

      return result;
    }

    private static NameValueCollection ConvertStringArrayToQueryMap(string[] queryParams)
    {
      int capacity = 0;
      if (queryParams != null)
        capacity = Math.Abs(queryParams.Length / 2);

      var result = new NameValueCollection(capacity);

      if (queryParams != null)
      {
        for (int i = 1; i < queryParams.Length; i = i + 2)
        {
          string key = queryParams[i - 1];
          string value = queryParams[i];

          if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(value)))
          {
            result[key] = value;
          }
        }
      }

      return result;
    }

    private string _defaultScheme;
    /// <summary>
    /// Returns http:// or https:// based on the current request
    /// </summary>
    public string DefaultScheme
    {
      get
      {
        if (_defaultScheme == null)
        {
          _defaultScheme = IsSecureConnection ? _schemeSecure : _schemeNonSecure;
        }
        return _defaultScheme;
      }
    }

    private string _defaultRootLink;
    private string DefaultRootLink
    {
      get
      {
        if (_defaultRootLink == null)
        {
          if (HttpContext.Current != null && HttpContext.Current.Request != null)
          {
            _defaultRootLink = HttpContext.Current.Request.Url.Authority;
            if (ContextHost == "localhost")
            {
              _defaultRootLink = _defaultRootLink + HttpContext.Current.Request.Url.Segments[0] +
                HttpContext.Current.Request.Url.Segments[1];
            }

            if (LowerCaseRelativeUrlsForSEO)
            {
              _defaultRootLink = _defaultRootLink.ToLowerInvariant();
            }
          }
          else
          {
            _defaultRootLink = string.Empty;
          }
        }
        return _defaultRootLink;
      }
    }

    private StringBuilder BuildInitialRelativeUrl(string relativePath, LinkProviderOptions options)
    {
      string scheme;
      ComputeFromOptions(options, true, out scheme);
      var urlStringBuilder = new StringBuilder(scheme.Length + DefaultRootLink.Length + relativePath.Length);
      urlStringBuilder.Append(scheme);
      urlStringBuilder.Append(DefaultRootLink);

      string path = ResolveUrl(AppendUrlLanguage(relativePath));
      bool bBaseUrlMissingSlash = (urlStringBuilder.Length > 0 && urlStringBuilder[urlStringBuilder.Length - 1] != '/') ||
                                  (urlStringBuilder.Length == 0);
      bool bPathMissingSlash = (path.Length > 0 && path[0] != '/');
      if (bBaseUrlMissingSlash && bPathMissingSlash)
      {
        urlStringBuilder.Append('/');
      }
      urlStringBuilder.Append(path);

      return urlStringBuilder;
    }

    private string AppendUrlLanguage(string relativePath)
    {
      if (_localizationProvider.Value == null || _localizationProvider.Value.RewrittenUrlLanguage == String.Empty ||
          IsLocalImageServerUrl(relativePath))
      {
        return relativePath;
      }

      if (relativePath.StartsWith("~/"))
      {
        return relativePath.Insert(1, String.Format("/{0}", _localizationProvider.Value.RewrittenUrlLanguage));
      }

      if (relativePath.StartsWith("/"))
      {
        return String.Format("/{0}{1}", _localizationProvider.Value.RewrittenUrlLanguage, relativePath);
      }

      if (string.IsNullOrEmpty(relativePath))
      {
        return String.Format("/{0}", _localizationProvider.Value.RewrittenUrlLanguage);
      }

      return String.Format("/{0}/{1}", _localizationProvider.Value.RewrittenUrlLanguage, relativePath);
    }

    private static string ResolveUrl(string url)
    {
      string result = url;
      if ((!string.IsNullOrEmpty(url)) && (url.StartsWith("~")))
      {
        result = VirtualPathUtility.ToAbsolute(url);
      }

      if (LowerCaseRelativeUrlsForSEO)
      {
        result = result.ToLowerInvariant();
      }

      return result;
    }

    private StringBuilder BuildInitialUrl(int contextId, string linkName, string relativePath, LinkProviderOptions options, ref string countrySiteId, ref string marketId, out ILinkInfo linkInfo)
    {
      string scheme;
      ComputeFromOptions(options, false, out scheme);
      var urlStringBuilder = new StringBuilder(scheme);

      bool isPage = false;
      if (linkName.Length > 0 && GetLinkMap(contextId).TryGetValue(linkName, out linkInfo))
      {
        int iStartOfNamedLink = urlStringBuilder.Length;

        string namedLink = linkInfo.BaseUrl;
        urlStringBuilder.Append(namedLink);

        // if the current site is global, then the target site is global
        // use this as a flag to not change the database's linkinfo url
        bool bTargetSiteIsGlobal;
        string targetSiteDefaultMarketId;
        if (_localizationProvider.Value != null && (linkInfo.CountrySupportType != LinkTypeCountrySupport.NoSupport || linkInfo.LanguageSupportType != LinkTypeLanguageSupport.NoSupport))
        {
          var countrySite = _localizationProvider.Value.TryGetCountrySite(countrySiteId ?? _localizationProvider.Value.CountrySite) ?? _localizationProvider.Value.CountrySiteInfo;
          countrySiteId = countrySite.Id;

          targetSiteDefaultMarketId = countrySite.DefaultMarketId;
          bTargetSiteIsGlobal = _localizationProvider.Value.IsGlobalSite(countrySiteId);

          var market = _localizationProvider.Value.GetMarketForCountrySite(countrySiteId, marketId ?? _localizationProvider.Value.MarketInfo.Id);
          marketId = market.Id;
        }
        else
        {
          bTargetSiteIsGlobal = true;
          targetSiteDefaultMarketId = String.Empty;
        }

        // update host w/country
        if (!bTargetSiteIsGlobal)
        {
          if (linkInfo.CountrySupportType == LinkTypeCountrySupport.ReplaceHostNameSupport)
          {
            int iEndOfHostName = urlStringBuilder.IndexOf('.', iStartOfNamedLink);
            if (iEndOfHostName != -1)
            {
              string newHostname = countrySiteId.ToLowerInvariant();
              urlStringBuilder.Remove(iStartOfNamedLink, iEndOfHostName - iStartOfNamedLink);
              urlStringBuilder.Insert(iStartOfNamedLink, newHostname);
            }
          }
          else if (linkInfo.CountrySupportType == LinkTypeCountrySupport.PrefixHostNameSupport)
          {
            string newHostname = countrySiteId.ToLowerInvariant();
            urlStringBuilder.Insert(iStartOfNamedLink, '.');
            urlStringBuilder.Insert(iStartOfNamedLink, newHostname);
          }
        }

        int lastDot = namedLink.LastIndexOf('.');
        int lastSlash = namedLink.LastIndexOf('/');
        isPage = (lastSlash < lastDot) && (lastSlash > 0);

        // add language as required
        if (_localizationProvider.Value != null && linkInfo.LanguageSupportType == LinkTypeLanguageSupport.PrefixPathSupport)
        {
          bool bIsDefaultMarket = String.Equals(targetSiteDefaultMarketId, marketId, StringComparison.OrdinalIgnoreCase);
          if (!bIsDefaultMarket && !IsTransperfectProxy)
          {
            string langUrl = _localizationProvider.Value.GetLanguageUrl(countrySiteId, marketId);

            if (!String.IsNullOrEmpty(langUrl))
            {
              // if there is no slash then add a slash
              int iFirstSlash = urlStringBuilder.IndexOf('/', iStartOfNamedLink);
              if (iFirstSlash == -1)
              {
                iFirstSlash = urlStringBuilder.Length;
                urlStringBuilder.Append('/');
              }

              // if there is a slash then insert <language>/ just after the slash
              urlStringBuilder.Insert(iFirstSlash + 1, langUrl);
            }
          }
        }

      }
      else
      {
        urlStringBuilder.Append(DefaultRootLink);
        linkInfo = null;
      }

      bool bBaseUrlMissingSlash = (urlStringBuilder.Length > 0 && urlStringBuilder[urlStringBuilder.Length - 1] != '/' && !isPage) ||
                                  (urlStringBuilder.Length == 0);
      bool bPathMissingSlash = (relativePath.Length > 0 && relativePath[0] != '/');
      if (bBaseUrlMissingSlash && bPathMissingSlash)
      {
        urlStringBuilder.Append('/');
      }

      urlStringBuilder.Append(relativePath);

      return urlStringBuilder;
    }

    private static void BuildUrlParameters(StringBuilder initialUrl, NameValueCollection queryParameters)
    {
      char delimiter = '?';
      const string REGIONSITE_KEY = "regionsite";

      if (queryParameters.AllKeys.Contains(REGIONSITE_KEY, StringComparer.OrdinalIgnoreCase) && !string.IsNullOrEmpty(queryParameters[REGIONSITE_KEY]))
      {
        initialUrl.Append(delimiter);
        initialUrl.Append(REGIONSITE_KEY);
        initialUrl.Append("=");
        initialUrl.Append(queryParameters[REGIONSITE_KEY]);
        delimiter = '&';
      }

      foreach (string key in queryParameters.Keys)
      {
        if (key != null && !REGIONSITE_KEY.Equals(key, StringComparison.OrdinalIgnoreCase))
        {
          string value = queryParameters[key];
          if (!string.IsNullOrEmpty(value))
          {
            initialUrl.Append(delimiter);
            if (delimiter == '?')
              delimiter = '&';

            initialUrl.Append(ImprovedUrlEncode(key));
            initialUrl.Append('=');
            initialUrl.Append(ImprovedUrlEncode(value));
          }
        }
      }
    }

    private string BuildUrl(
      string linkName,
      string relativePath,
      LinkProviderOptions options,
      string countrySiteId, string marketId,
      int? contextId,
      NameValueCollection queryParameters)
    {
      relativePath = relativePath ?? String.Empty;
      if (relativePath.Contains("?") || relativePath.Contains("&"))
      {
        throw new ArgumentException("Relative path cannot contain query string.");
      }

      ILinkInfo linkInfo;
      var siteContextId = contextId.HasValue ? contextId.Value : _siteContext.ContextId;
      var urlStringBuilder = BuildInitialUrl(siteContextId, linkName, relativePath, options, ref countrySiteId, ref marketId, out linkInfo);
      HandleCommonParameters(queryParameters, options, countrySiteId, marketId, linkInfo);
      BuildUrlParameters(urlStringBuilder, queryParameters);

      return urlStringBuilder.ToString();
    }

    private void HandleCommonParameters(NameValueCollection queryMap, LinkProviderOptions options, string countrySiteId = null, string marketId = null, ILinkInfo linkInfo = null)
    {
      if (!options.HasFlag(LinkProviderOptions.QueryStringExplicitParameters) && linkInfo != null)
      {
        string langParm = linkInfo.LanguageParameter;
        if (linkInfo.LanguageSupportType == LinkTypeLanguageSupport.QueryStringSupport && !String.IsNullOrEmpty(langParm))
        {
          if (!String.IsNullOrEmpty(marketId))
          {
            queryMap[langParm] = marketId;
          }
        }

        string cntryParm = linkInfo.CountryParameter;
        if (linkInfo.CountrySupportType == LinkTypeCountrySupport.QueryStringSupport && !String.IsNullOrEmpty(cntryParm))
        {
          if (!String.IsNullOrEmpty(countrySiteId))
          {
            queryMap[cntryParm] = countrySiteId;
          }
        }
      }

      if (!options.HasFlag(LinkProviderOptions.QueryStringExplicitParameters) && !options.HasFlag(LinkProviderOptions.QueryStringExplicitWithLocalizationParameters))
      {
        if ((LinkProviderCommonParameters.HandleProgId) && (_siteContext.ContextId == _RESELLERCONTEXTID))
        {
          queryMap["prog_id"] = _siteContext.ProgId;
        }

        if ((LinkProviderCommonParameters.HandlePlId) && (_siteContext.ContextId == _RESELLERCONTEXTID))
        {
          queryMap["pl_id"] = _siteContext.PrivateLabelId.ToString();
        }

        if ((LinkProviderCommonParameters.HandleManager) && (_siteContext.Manager.IsManager))
        {
          foreach (string key in _siteContext.Manager.ManagerQuery.Keys)
          {
            queryMap[key] = _siteContext.Manager.ManagerQuery[key];
          }
        }

        if (LinkProviderCommonParameters.HandleISC)
        {
          string givenIsc = queryMap["isc"];
          if (string.IsNullOrEmpty(givenIsc))
          {
            queryMap["isc"] = _siteContext.ISC;
          }
        }

        // Use event for application to customize remaining querystring parameters
        LinkProviderCommonParameters.OnAddCommonParameters(Container, queryMap);
      }

    }

    private string BuildRelativeUrl(
      string relativePath,
      LinkProviderOptions options,
      NameValueCollection queryParameters)
    {
      relativePath = relativePath ?? String.Empty;
      if (relativePath.Contains("?") || relativePath.Contains("&"))
      {
        throw new ArgumentException("Relative path cannot contain query string.");
      }

      var urlStringBuilder = BuildInitialRelativeUrl(relativePath, options);
      HandleCommonParameters(queryParameters, options);
      BuildUrlParameters(urlStringBuilder, queryParameters);

      return urlStringBuilder.ToString();
    }

    #region Relative Site Urls

    /// <summary>
    /// Gets a relative url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath)
    {
      return GetRelativeUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, new NameValueCollection());
    }

    /// <summary>
    /// Gets a relative url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, params string[] additionalQueryParameters)
    {
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetRelativeUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, queryMap);
    }

    /// <summary>
    /// Gets a relative url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetRelativeUrl_Int(relativePath, options, queryMap);
    }

    /// <summary>
    /// Gets a relative url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, NameValueCollection queryMap)
    {
      var localQueryMap = new NameValueCollection(queryMap);
      return GetRelativeUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, localQueryMap);
    }

    /// <summary>
    /// Gets a relative url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var localQueryMap = new NameValueCollection(queryMap);
      return GetRelativeUrl_Int(relativePath, options, localQueryMap);
    }

    /// <summary>
    /// Gets a relative url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      return GetRelativeUrl_Int(relativePath, options, new NameValueCollection());
    }

    //    public string GetRelativeUrl(string relativePath, LinkProviderOptions options = LinkProviderOptions.DefaultOptions, params string[] additionalQueryParameters)
    //    {
    //      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
    //      return GetRelativeUrl_Int(relativePath, options, queryMap);
    //    }

    public string GetRelativeUrl(string relativePath = null, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions)
    {
      queryMap = queryMap == null ? new NameValueCollection() : new NameValueCollection(queryMap);
      return GetRelativeUrl_Int(relativePath, options, queryMap);
    }

    private string GetRelativeUrl_Int(string relativePath, LinkProviderOptions options, NameValueCollection queryMap)
    {
      return BuildRelativeUrl(relativePath, options, queryMap);
    }

    #endregion

    #region Full Urls

    #region Http

    /// <summary>
    /// Gets a full http: url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath)
    {
      var queryMap = new NameValueCollection();
      return GetFullUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, queryMap);
    }

    /// <summary>
    /// Gets a full http: url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath, params string[] additionalQueryParameters)
    {
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, queryMap);
    }

    /// <summary>
    /// Gets a full http: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullUrl_Int(relativePath, options, queryMap);
    }

    /// <summary>
    /// Gets a full http: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var localQueryMap = new NameValueCollection(queryMap);
      return GetFullUrl_Int(relativePath, options, localQueryMap);
    }

    /// <summary>
    /// Gets a full http: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath, QueryParamMode queryParamMode)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      return GetFullUrl_Int(relativePath, options, new NameValueCollection());
    }

    private string GetFullUrl_Int(string relativePath, LinkProviderOptions options, NameValueCollection queryMap)
    {
      return BuildRelativeUrl(relativePath, options, queryMap);
    }

    #endregion

    #region Https

    /// <summary>
    /// Gets a full https: url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath)
    {
      var queryMap = new NameValueCollection();
      return GetFullSecureUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, queryMap);
    }

    /// <summary>
    /// Gets a full https: url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath, params string[] additionalQueryParameters)
    {
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullSecureUrl_Int(relativePath, LinkProviderOptions.DefaultOptions, queryMap);
    }

    /// <summary>
    /// Gets a full https: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullSecureUrl_Int(relativePath, options, queryMap);
    }

    /// <summary>
    /// Gets a full https: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var localQueryMap = new NameValueCollection(queryMap);
      return GetFullSecureUrl_Int(relativePath, options, localQueryMap);
    }

    /// <summary>
    /// Gets a full https: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      return GetFullSecureUrl_Int(relativePath, options, new NameValueCollection());
    }

    private string GetFullSecureUrl_Int(string relativePath, LinkProviderOptions options, NameValueCollection queryMap)
    {
      return BuildRelativeUrl(relativePath, options | LinkProviderOptions.ProtocolHttps, queryMap);
    }

    #endregion

    #endregion

    #region Named Urls

    /// <summary>
    /// Gets a named link url, includes common parameters automatically, and uses the same scheme as current request (http or https)
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath)
    {
      var queryMap = new NameValueCollection();
      return GetUrl_Int(linkName, relativePath, queryMap, LinkProviderOptions.DefaultOptions, null, null, null);
    }

    /// <summary>
    /// Gets a named link url, includes common parameters automatically, and uses the same scheme as current request (http or https)
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath, params string[] additionalQueryParameters)
    {
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetUrl_Int(linkName, relativePath, queryMap, LinkProviderOptions.DefaultOptions, null, null, null);
    }

    /// <summary>
    /// Gets a named link url, and uses the same scheme as current request (http or https)
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetUrl_Int(linkName, relativePath, queryMap, options, null, null, null);
    }

    /// <summary>
    /// Gets a named link url
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="isSecure">When true uses https: instead of http:</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, params string[] additionalQueryParameters)
    {
      var options = isSecure ? LinkProviderOptions.ProtocolHttps : LinkProviderOptions.ProtocolHttp;
      AddToOptions(queryParamMode, ref options);
      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetUrl_Int(linkName, relativePath, queryMap, options, null, null, null);
    }

    /// <summary>
    /// Gets a named link url, includes common parameters automatically, and uses the same scheme as current request (http or https)
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath, NameValueCollection queryMap)
    {
      var localQueryMap = new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, localQueryMap, LinkProviderOptions.DefaultOptions, null, null, null);
    }

    /// <summary>
    /// Gets a named link url
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="isSecure">When true uses https: instead of http:</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, NameValueCollection queryMap)
    {
      var options = isSecure ? LinkProviderOptions.ProtocolHttps : LinkProviderOptions.ProtocolHttp;
      AddToOptions(queryParamMode, ref options);
      var localQueryMap = new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, localQueryMap, options, null, null, null);
    }

    /// <summary>
    /// Gets a named link url
    /// </summary>
    /// <param name="linkName">Link name</param>
    /// <param name="relativePath">Relative path to append to named link with no tilda (ex. products/somepage.aspx)</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="isSecure">When true uses https: instead of http:</param>
    /// <returns>Full url to a named link</returns>
    public string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure)
    {
      var options = isSecure ? LinkProviderOptions.ProtocolHttps : LinkProviderOptions.ProtocolHttp;
      AddToOptions(queryParamMode, ref options);
      return GetUrl_Int(linkName, relativePath, new NameValueCollection(), options, null, null, null);
    }

    //    public string GetUrl(string linkName, string relativePath, LinkProviderOptions options, string countrySiteId, string marketId, params string[] additionalQueryParameters)
    //    {
    //      var queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
    //      return GetUrl_Int(linkName, relativePath, queryMap, options, countrySiteId, marketId);
    //    }

    public string GetUrl(string linkName, string relativePath = null, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions)
    {
      var localQueryMap = queryMap == null ? new NameValueCollection() : new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, localQueryMap, options, null, null, null);
    }

    public string GetSpecificMarketUrl(string linkName, string relativePath, string countrySiteId, string marketId, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions)
    {
      var localQueryMap = queryMap == null ? new NameValueCollection() : new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, localQueryMap, options, countrySiteId, marketId, null);
    }

    public string GetSpecificContextUrl(string linkName, string relativePath, int contextId, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions)
    {
      var localQueryMap = queryMap == null ? new NameValueCollection() : new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, localQueryMap, options, null, null, contextId);
    }

    //    public string GetUrl(string linkName, string relativePath = null, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions, string countrySiteId = null, string marketId = null)
    //    {
    //      var localQueryMap = queryMap == null ? new NameValueCollection() : new NameValueCollection(queryMap);
    //      return GetUrl_Int(linkName, relativePath, localQueryMap, options, countrySiteId, marketId);
    //    }

    private string GetUrl_Int(string linkName, string relativePath, NameValueCollection queryMap, LinkProviderOptions options, string countrySiteId, string marketId, int? contextId)
    {
      if (!string.IsNullOrEmpty(ContextHost))
      {
        bool isSecureServerRequest = ContextHost.Contains("secureserver.net");

        if (isSecureServerRequest)
        {
          string linkSecureServerCounterPart = string.Concat(linkName, ".SS");
          var siteContextId = contextId.HasValue ? contextId.Value : _siteContext.ContextId;
          bool linkTypeMapForContextExists = GetLinkMap(siteContextId).Count > 0;

          if (linkTypeMapForContextExists && GetLinkMap(siteContextId).ContainsKey(linkSecureServerCounterPart))
          {
            linkName = linkSecureServerCounterPart;
          }
        }
      }

      return BuildUrl(linkName, relativePath, options, countrySiteId, marketId, contextId, queryMap);
    }

    #endregion

    #region Url Args

    /// <summary>
    /// Gets a querystring (including the ?).  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryMap">Additional query string parameters that should be added to the query string.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(NameValueCollection queryMap, QueryParamMode queryParamMode)
    {
      var localQueryMap = new NameValueCollection(queryMap);
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      return GetUrlArguments_Int(localQueryMap, options);
    }

    /// <summary>
    /// Gets a querystring (including the ?) including common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryMap">Additional query string parameters that should be added to the query string.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(NameValueCollection queryMap)
    {
      var localQueryMap = new NameValueCollection(queryMap);
      return GetUrlArguments_Int(localQueryMap, LinkProviderOptions.DefaultOptions);
    }

    /// <summary>
    /// Gets a querystring (including the ?) including common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(QueryParamMode queryParamMode)
    {
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      return GetUrlArguments_Int(new NameValueCollection(), options);
    }

    /// <summary>
    /// Gets a querystring (including the ?) with common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments()
    {
      return GetUrlArguments_Int(new NameValueCollection(), LinkProviderOptions.DefaultOptions);
    }

    /// <summary>
    /// Gets a querystring (including the ?).  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="queryParameters">Additional query string parameters that should be added to the query string.</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(QueryParamMode queryParamMode, params string[] queryParameters)
    {
      var queryMap = ConvertStringArrayToQueryMap(queryParameters);
      var options = LinkProviderOptions.DefaultOptions;
      AddToOptions(queryParamMode, ref options);
      return GetUrlArguments_Int(queryMap, options);
    }

    /// <summary>
    /// Gets a querystring (including the ?) including common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryParameters">Additional query string parameters that should be added to the query string.</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(params string[] queryParameters)
    {
      var queryMap = ConvertStringArrayToQueryMap(queryParameters);
      return GetUrlArguments_Int(queryMap, LinkProviderOptions.DefaultOptions);
    }

    public string GetUrlArguments(NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions)
    {
      queryMap = queryMap == null ? new NameValueCollection() : new NameValueCollection(queryMap);
      return GetUrlArguments_Int(queryMap, options);
    }

    //    public string GetUrlArguments(LinkProviderOptions options, params string[] queryParameters)
    //    {
    //      var queryMap = ConvertStringArrayToQueryMap(queryParameters);
    //      return GetUrlArguments_Int(queryMap, options);
    //    }

    private string GetUrlArguments_Int(NameValueCollection queryMap, LinkProviderOptions options)
    {
      var urlBuilder = new StringBuilder();
      HandleCommonParameters(queryMap, options);
      BuildUrlParameters(urlBuilder, queryMap);
      return urlBuilder.ToString();
    }

    #endregion

    #region Image Root

    private string ChooseLinkType(string nonManagerType, string managerType)
    {
      string result = nonManagerType;
      if ((_useC3ImageUrls.Value) && (_siteContext.Manager.IsManager))
      {
        result = managerType;
      }

      return result;
    }

    private string _imageRoot;
    public string ImageRoot
    {
      get
      {
        if (_imageRoot == null)
        {
          string scheme = IsSecureConnection ? _schemeSecure : _schemeNonSecure;
          string linkType = ChooseLinkType(LinkTypes.Image, LinkTypes.C3Image);
          _imageRoot = String.Concat(scheme, this[linkType], "/");
        }
        return _imageRoot;
      }
    }

    private string _cssRoot;
    public string CssRoot
    {
      get
      {
        if (_cssRoot == null)
        {
          string scheme = IsSecureConnection ? _schemeSecure : _schemeNonSecure;
          string linkType = ChooseLinkType(LinkTypes.ExternalCSS, LinkTypes.C3ExternalCSS);
          _cssRoot = String.Concat(scheme, this[linkType], "/");
        }
        return _cssRoot;
      }
    }

    private string _javascriptRoot;
    public string JavascriptRoot
    {
      get
      {
        if (_javascriptRoot == null)
        {
          string scheme = IsSecureConnection ? _schemeSecure : _schemeNonSecure;
          string linkType = ChooseLinkType(LinkTypes.ExternalScript, LinkTypes.C3ExternalScript);
          _javascriptRoot = String.Concat(scheme, this[linkType], "/");
        }
        return _javascriptRoot;
      }
    }

    private string _largeImagesRoot;
    public string LargeImagesRoot
    {
      get
      {
        if (_largeImagesRoot == null)
        {
          string scheme = IsSecureConnection ? _schemeSecure : _schemeNonSecure;
          string linkType = ChooseLinkType(LinkTypes.ExternalBigImage1, LinkTypes.C3ExternalBigImage1);
          _largeImagesRoot = String.Concat(scheme, this[linkType], "/");
        }
        return _largeImagesRoot;
      }
    }

    private string _presentationCentralImagesRoot;
    public string PresentationCentralImagesRoot
    {
      get
      {
        if (_presentationCentralImagesRoot == null)
        {
          string scheme = IsSecureConnection ? _schemeSecure : _schemeNonSecure;
          string linkType = ChooseLinkType(LinkTypes.ExternalBigImage2, LinkTypes.C3ExternalBigImage2);
          _presentationCentralImagesRoot = String.Concat(scheme, this[linkType], "/");
        }
        return _presentationCentralImagesRoot;
      }
    }

    #endregion

    #region Default Indexer

    /// <summary>
    /// Gets a named link for a given contextId
    /// </summary>
    /// <param name="linkName">link name</param>
    /// <param name="contextId">context Id</param>
    /// <returns>A named link</returns>
    public string this[string linkName, int contextId]
    {
      get
      {
        string result = string.Empty;
        var linkMap = GetLinkMap(contextId);
        if ((linkMap != null) && (linkMap.ContainsKey(linkName)))
        {
          result = linkMap[linkName].BaseUrl;
        }
        return result;
      }
    }

    /// <summary>
    /// Gets a named link for the current request's contextId
    /// </summary>
    /// <param name="linkName">link name</param>
    /// <returns>A named link</returns>
    public string this[string linkName]
    {
      get
      {
        return this[linkName, _siteContext.ContextId];
      }
    }

    #endregion

    #region Videos and other Media

    private string _videoRoot;

    /// <summary>
    /// Returns the FOS video root. Listens to the app setting COMMERCIALS_URL_TYPE.  Always non-secure (http)
    /// </summary>
    public string VideoRoot
    {
      get
      {
        string commercialsUrlType = DataCache.DataCache.GetAppSetting("COMMERCIALS_URL_TYPE").ToUpperInvariant();

        if (_videoRoot == null)
        {
          if (commercialsUrlType == "LIMELIGHT_HTTP")
          {
            _videoRoot = _schemeNonSecure + this[LinkTypes.VideoLimeLight] + "/";
          }
          else if (commercialsUrlType == "IMAGES")
          {
            _videoRoot = _schemeNonSecure + this[LinkTypes.VideoLocal] + "/";
          }
          else
          {
            _videoRoot = _schemeNonSecure + this[LinkTypes.VideoAkamai] + "/";
          }
        }
        return _videoRoot;
      }
    }

    private string _videoMeRoot;
    /// <summary>
    /// Returns the Video.ME root.  Always non-secure (http)
    /// </summary>
    public string VideoMeRoot
    {
      get
      {
        if (_videoMeRoot == null)
        {
          _videoMeRoot = _schemeNonSecure + this[LinkTypes.VideoMe] + "/";
        }
        return _videoMeRoot;
      }
    }

    #endregion

    #region Enum conversion helpers
    /* commented out this currently unused private method to achieve better code coverage scores
    private void ComputeFromOptions(LinkProviderOptions options, out UrlRootMode rootMode)
    {
      if (options.HasFlag(LinkProviderOptions.FormatRelativeUrl))
      {
        rootMode = UrlRootMode.NoRoot;
      }
      else
      {
        if (options.HasFlag(LinkProviderOptions.ProtocolHttp))
        {
          rootMode = UrlRootMode.HttpRoot;
        }
        else if (options.HasFlag(LinkProviderOptions.ProtocolHttps))
        {
          rootMode = UrlRootMode.HttpsRoot;
        }
        else if (options.HasFlag(LinkProviderOptions.ProtocolAgnostic))
        {
          rootMode = UrlRootMode.NoProtocol;
        }
        else
        {
          rootMode = UrlRootMode.DefaultRoot;
        }
      }
    }
    */

    /* commented out this currently unused private method to achieve better code coverage scores
    private void ComputeFromOptions(LinkProviderOptions options, out QueryParamMode queryParamMode)
    {
      if (options.HasFlag(LinkProviderOptions.QueryStringExplicitParameters))
      {
        queryParamMode = QueryParamMode.ExplicitParameters;
      }
      else if (options.HasFlag(LinkProviderOptions.QueryStringExplicitWithLocalizationParameters))
      {
        queryParamMode = QueryParamMode.ExplicitWithLocalizationParameters;
      }
      else
      {
        queryParamMode = QueryParamMode.CommonParameters;
      }
    }
    */

    private void ComputeFromOptions(LinkProviderOptions options, bool isRelative, out string scheme)
    {
      if (options.HasFlag(LinkProviderOptions.ProtocolHttp))
      {
        scheme = _schemeNonSecure;
      }
      else if (options.HasFlag(LinkProviderOptions.ProtocolHttps))
      {
        if (isRelative && IsDebugInternal())
        {
          scheme = _schemeNonSecure;
        }
        else
        {
          scheme = _schemeSecure;
        }
      }
      else if (options.HasFlag(LinkProviderOptions.ProtocolAgnostic))
      {
        scheme = _schemeNone;
      }
      else
      {
        scheme = DefaultScheme;
      }
    }

    /* commented out this currently unused private method to achieve better code coverage scores
    private void ComputeFromOptions(LinkProviderOptions options, out UrlRootMode rootMode, out QueryParamMode queryParamMode)
    {
      ComputeFromOptions(options, out rootMode);
      ComputeFromOptions(options, out queryParamMode);
    }
    */

    private void AddToOptions(QueryParamMode queryParamMode, ref LinkProviderOptions options)
    {
      switch (queryParamMode)
      {
        case QueryParamMode.ExplicitParameters:
          options |= LinkProviderOptions.QueryStringExplicitParameters;
          break;
        case QueryParamMode.ExplicitWithLocalizationParameters:
          options |= LinkProviderOptions.QueryStringExplicitWithLocalizationParameters;
          break;
      }
    }

    /* commented out this currently unused private method to achieve better code coverage scores
    private void AddToOptions(UrlRootMode rootMode, ref LinkProviderOptions options)
    {
      switch (rootMode)
      {
        case UrlRootMode.HttpRoot:
          options |= LinkProviderOptions.ProtocolHttp;
          break;
        case UrlRootMode.HttpsRoot:
          options |= LinkProviderOptions.ProtocolHttps;
          break;
        case UrlRootMode.NoProtocol:
          options |= LinkProviderOptions.ProtocolAgnostic;
          break;
        case UrlRootMode.NoRoot:
          options |= LinkProviderOptions.FormatRelativeUrl;
          break;
      }
    }
    */
    #endregion

  }
}
