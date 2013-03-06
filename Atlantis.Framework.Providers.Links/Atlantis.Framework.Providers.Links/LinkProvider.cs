using Atlantis.Framework.Interface;
using Atlantis.Framework.LinkInfo.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Atlantis.Framework.Providers.Links
{
  public class LinkProvider : ProviderBase, ILinkProvider
  {
    private static bool _allowRelativeUrls = false;
    /// <summary>
    /// Setting this to true will allow the GetRelativeUrl methods to return relative urls "/example.aspx" instead of fully qualified urls
    /// </summary>
    public static bool AllowRelativeUrls
    {
      get { return _allowRelativeUrls; }
      set { _allowRelativeUrls = value; }
    }

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

    private readonly Dictionary<int, Dictionary<string, string>> _linkMaps;
    private readonly ISiteContext _siteContext;
    private Lazy<bool> _useC3ImageUrls;

    private enum UrlRootMode
    {
      NoRoot = 0,
      DefaultRoot = 1,
      HttpRoot = 2,
      HttpsRoot = 3
    }

    public LinkProvider(IProviderContainer container)
      : base(container)
    {
      _linkMaps = new Dictionary<int, Dictionary<string, string>>(1);
      _siteContext = container.Resolve<ISiteContext>();
      _useC3ImageUrls = new Lazy<bool>(() => { return GetUseC3ImageUrls(); });
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
          if (Container.CanResolve<IProxyContext>())
          {
            IProxyContext proxy = Container.Resolve<IProxyContext>();
            if (proxy.IsLocalARR)
            {
              _contextHost = proxy.ARRHost;
            }
          }
        }
        return _contextHost;
      }
    }

    private string _originIP;
    private string OriginIP
    {
      get
      {
        if (_originIP == null)
        {
          _originIP = HttpContext.Current.Request.UserHostAddress;
          if (Container.CanResolve<IProxyContext>())
          {
            IProxyContext proxy = Container.Resolve<IProxyContext>();
            _originIP = proxy.OriginIP;
          }
        }
        return _originIP;
      }
    }

    public bool IsDebugInternal()
    {
      return _siteContext.IsRequestInternal &&
             (ContextHost.Contains(".debug.") || ContextHost.StartsWith("debug."));
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

    private Dictionary<string, string> GetLinkMap(int contextId)
    {
      Dictionary<string, string> result;
      if (!_linkMaps.TryGetValue(contextId, out result))
      {
        string requestURL = string.Empty;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request != null)
          {
            requestURL = HttpContext.Current.Request.Url.ToString();
          }
        }
        LinkInfoRequestData request = new LinkInfoRequestData(
          string.Empty, requestURL,
          string.Empty, _siteContext.Pathway, _siteContext.PageCount, contextId);

        LinkInfoResponseData response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(
          request, LinkProviderEngineRequests.LinkInfo);

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

      NameValueCollection result = new NameValueCollection(capacity);

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

    private StringBuilder BuildInitialUrl(string relativePath, UrlRootMode rootMode)
    {
      StringBuilder urlStringBuilder = new StringBuilder();
      if (rootMode != UrlRootMode.NoRoot)
      {
        string scheme = DefaultScheme;
        if ((rootMode == UrlRootMode.HttpsRoot) && (!IsDebugInternal()))
        {
          scheme = _schemeSecure;
        }
        else if (rootMode == UrlRootMode.HttpRoot)
        {
          scheme = _schemeNonSecure;
        }

        urlStringBuilder.Append(scheme);
        urlStringBuilder.Append(DefaultRootLink);
      }

      urlStringBuilder.Append(ResolveUrl(relativePath));
      return urlStringBuilder;
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

    private StringBuilder BuildInitialUrl(int contextId, string linkName, string relativePath, string scheme)
    {
      StringBuilder urlStringBuilder = new StringBuilder(scheme);

      bool isPage = false;
      string namedLink;
      if ((linkName.Length > 0) && (GetLinkMap(contextId).TryGetValue(linkName, out namedLink)))
      {
        urlStringBuilder.Append(namedLink);

        int lastDot = namedLink.LastIndexOf('.');
        int lastSlash = namedLink.LastIndexOf('/');
        isPage = ((lastSlash < lastDot) && (lastSlash > 0));
      }
      else
      {
        urlStringBuilder.Append(DefaultRootLink);
      }

      if (relativePath.Length > 0)
      {
        if (relativePath[0] != '/')
        {
          urlStringBuilder.Append("/");
        }
        urlStringBuilder.Append(relativePath);
      }
      else if (!isPage)
      {
        urlStringBuilder.Append("/");
      }

      return urlStringBuilder;
    }

    private static void BuildUrlParameters(StringBuilder initialUrl, NameValueCollection queryParameters)
    {
      char delimiter = '?';

      foreach (string key in queryParameters.Keys)
      {
        if (key != null)
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
      bool isSecure,
      QueryParamMode queryParamMode,
      NameValueCollection queryParameters)
    {
      string scheme = isSecure ? _schemeSecure : _schemeNonSecure;

      if (relativePath.Contains("?") || relativePath.Contains("&"))
      {
        throw new ArgumentException("Relative path cannot contain query string.");
      }

      StringBuilder urlStringBuilder = BuildInitialUrl(_siteContext.ContextId, linkName, relativePath, scheme);
      HandleCommonParameters(queryParameters, queryParamMode);
      BuildUrlParameters(urlStringBuilder, queryParameters);

      return urlStringBuilder.ToString();
    }

    private void HandleCommonParameters(NameValueCollection queryMap, QueryParamMode queryParamMode)
    {
      if (queryParamMode == QueryParamMode.CommonParameters)
      {
        if ((LinkProviderCommonParameters.HandleProgId) && (_siteContext.ContextId == _RESELLERCONTEXTID))
        { 
          queryMap["prog_id"] = _siteContext.ProgId;
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
        LinkProviderCommonParameters.OnAddCommonParameters(_siteContext, queryMap);

      }
    }

    private string BuildRelativeUrl(
      string relativePath,
      QueryParamMode queryParamMode,
      NameValueCollection queryParameters)
    {
      return BuildRelativeUrl(relativePath, queryParamMode, queryParameters, UrlRootMode.NoRoot);
    }

    private string BuildRelativeUrl(
      string relativePath,
      QueryParamMode queryParamMode,
      NameValueCollection queryParameters,
      UrlRootMode rootMode)
    {
      if (relativePath.Contains("?") || relativePath.Contains("&"))
      {
        throw new ArgumentException("Relative path cannot contain query string.");
      }

      StringBuilder urlStringBuilder = BuildInitialUrl(relativePath, rootMode);
      HandleCommonParameters(queryParameters, queryParamMode);
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
      return GetRelativeUrl_Int(relativePath, QueryParamMode.CommonParameters, new NameValueCollection());
    }

    /// <summary>
    /// Gets a relative url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, params string[] additionalQueryParameters)
    {
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetRelativeUrl_Int(relativePath, QueryParamMode.CommonParameters, queryMap);
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
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetRelativeUrl_Int(relativePath, queryParamMode, queryMap);
    }

    /// <summary>
    /// Gets a relative url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="queryMap">Additional query string parameters that should be added to the url.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, NameValueCollection queryMap)
    {
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetRelativeUrl_Int(relativePath, QueryParamMode.CommonParameters, localQueryMap);
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
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetRelativeUrl_Int(relativePath, queryParamMode, localQueryMap);
    }

    /// <summary>
    /// Gets a relative url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will remain unchanged.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Relative url in the current site</returns>
    public string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode)
    {
      return GetRelativeUrl_Int(relativePath, queryParamMode, new NameValueCollection());
    }

    private string GetRelativeUrl_Int(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap)
    {
      string result;
      if (AllowRelativeUrls)
      {
        result = BuildRelativeUrl(relativePath, queryParamMode, queryMap);
      }
      else if (HttpContext.Current.Request.IsSecureConnection)
      {
        result = GetFullSecureUrl_Int(relativePath, queryParamMode, queryMap);
      }
      else
      {
        result = GetFullUrl_Int(relativePath, queryParamMode, queryMap);
      }
      return result;
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
      NameValueCollection queryMap = new NameValueCollection();
      return GetFullUrl_Int(relativePath, QueryParamMode.CommonParameters, queryMap);
    }

    /// <summary>
    /// Gets a full http: url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath, params string[] additionalQueryParameters)
    {
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullUrl_Int(relativePath, QueryParamMode.CommonParameters, queryMap);
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
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullUrl_Int(relativePath, queryParamMode, queryMap);
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
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetFullUrl_Int(relativePath, queryParamMode, localQueryMap);
    }

    /// <summary>
    /// Gets a full http: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Full http: url in the current site</returns>
    public string GetFullUrl(string relativePath, QueryParamMode queryParamMode)
    {
      return GetFullUrl_Int(relativePath, queryParamMode, new NameValueCollection());
    }

    private string GetFullUrl_Int(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap)
    {
      return BuildRelativeUrl(relativePath, queryParamMode, queryMap, UrlRootMode.DefaultRoot);
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
      NameValueCollection queryMap = new NameValueCollection();
      return GetFullSecureUrl_Int(relativePath, QueryParamMode.CommonParameters, queryMap);
    }

    /// <summary>
    /// Gets a full https: url to the current site, and includes common parameters automatically.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="additionalQueryParameters">Additional query string parameters that should be added to the url</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath, params string[] additionalQueryParameters)
    {
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullSecureUrl_Int(relativePath, QueryParamMode.CommonParameters, queryMap);
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
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetFullSecureUrl_Int(relativePath, queryParamMode, queryMap);
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
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetFullSecureUrl_Int(relativePath, queryParamMode, localQueryMap);
    }

    /// <summary>
    /// Gets a full https: url to the current site.
    /// </summary>
    /// <param name="relativePath">Relative path using tilda server form (~/path/etc). If ~ is not used input path will just be appended to the default root or the request.</param>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Full https: url in the current site</returns>
    public string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode)
    {
      return GetFullSecureUrl_Int(relativePath, queryParamMode, new NameValueCollection());
    }

    private string GetFullSecureUrl_Int(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap)
    {
      return BuildRelativeUrl(relativePath, queryParamMode, queryMap, UrlRootMode.HttpsRoot);
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
      NameValueCollection queryMap = new NameValueCollection();
      return GetUrl_Int(linkName, relativePath, QueryParamMode.CommonParameters, IsSecureConnection, queryMap);
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
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetUrl_Int(linkName, relativePath, QueryParamMode.CommonParameters, IsSecureConnection, queryMap);
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
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetUrl_Int(linkName, relativePath, queryParamMode, IsSecureConnection, queryMap);
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
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(additionalQueryParameters);
      return GetUrl_Int(linkName, relativePath, queryParamMode, isSecure, queryMap);
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
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, QueryParamMode.CommonParameters, IsSecureConnection, localQueryMap);
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
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetUrl_Int(linkName, relativePath, queryParamMode, isSecure, localQueryMap);
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
      return GetUrl_Int(linkName, relativePath, queryParamMode, isSecure, new NameValueCollection());
    }

    private string GetUrl_Int(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, NameValueCollection queryMap)
    {
      return BuildUrl(linkName, relativePath, isSecure, queryParamMode, queryMap);
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
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetUrlArguments_Int(localQueryMap, queryParamMode);
    }

    /// <summary>
    /// Gets a querystring (including the ?) including common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryMap">Additional query string parameters that should be added to the query string.  Note. Request.QueryString is a NameValueCollection.</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(NameValueCollection queryMap)
    {
      NameValueCollection localQueryMap = new NameValueCollection(queryMap);
      return GetUrlArguments_Int(localQueryMap, QueryParamMode.CommonParameters);
    }

    /// <summary>
    /// Gets a querystring (including the ?) including common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(QueryParamMode queryParamMode)
    {
      return GetUrlArguments_Int(new NameValueCollection(), queryParamMode);
    }

    /// <summary>
    /// Gets a querystring (including the ?) with common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments()
    {
      return GetUrlArguments_Int(new NameValueCollection(), QueryParamMode.CommonParameters);
    }

    /// <summary>
    /// Gets a querystring (including the ?).  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryParamMode">When set to Explicit, no common query string parameters are automatically added. (even prog_id is not added automatically)</param>
    /// <param name="queryParameters">Additional query string parameters that should be added to the query string.</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(QueryParamMode queryParamMode, params string[] queryParameters)
    {
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(queryParameters);
      return GetUrlArguments_Int(queryMap, queryParamMode);
    }

    /// <summary>
    /// Gets a querystring (including the ?) including common parameters.  Ensures all parameters are url encoded
    /// </summary>
    /// <param name="queryParameters">Additional query string parameters that should be added to the query string.</param>
    /// <returns>Querystring in the form ?arg=val&arg=val</returns>
    public string GetUrlArguments(params string[] queryParameters)
    {
      NameValueCollection queryMap = ConvertStringArrayToQueryMap(queryParameters);
      return GetUrlArguments_Int(queryMap, QueryParamMode.CommonParameters);
    }

    private string GetUrlArguments_Int(NameValueCollection queryMap, QueryParamMode queryParamMode)
    {
      StringBuilder urlBuilder = new StringBuilder();
      HandleCommonParameters(queryMap, queryParamMode);
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
          _imageRoot = scheme + this[linkType] + "/";
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
          _cssRoot = scheme + this[linkType] + "/";
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
          _javascriptRoot = scheme + this[linkType] + "/";
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
          _largeImagesRoot = scheme + this[linkType] + "/";
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
          _presentationCentralImagesRoot = scheme + this[linkType] + "/";
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
        Dictionary<string, string> linkMap = GetLinkMap(contextId);
        if ((linkMap != null) && (linkMap.ContainsKey(linkName)))
        {
          result = linkMap[linkName];
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
  }
}
