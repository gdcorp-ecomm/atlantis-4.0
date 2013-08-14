using System;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.BasePages;
using Atlantis.Framework.BasePages.Cookies;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.MobileRedirect.Interface;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;

namespace Atlantis.Framework.Providers.MobileRedirect
{
  public class MobileRedirectProvider : ProviderBase, IMobileRedirectProvider
  {
    private const int RESELLER_MOBILE_SITE_ENABLED_CATEGORY = 388;
    public const string MOBILE_SITE_LINK_TYPE = "MDOTMOBILEURL";

    public MobileRedirectProvider(IProviderContainer container) : base(container)
    {
    }

    private ILinkProvider _linkProvider;
    private ILinkProvider LinkProvider
    {
      get { return _linkProvider ?? (_linkProvider = Container.Resolve<ILinkProvider>()); }
    }

    private IUserAgentDetectionProvider _userAgentDetectionProvider;
    private IUserAgentDetectionProvider UserAgentDetectionProvider
    {
      get { return _userAgentDetectionProvider ?? (_userAgentDetectionProvider = Container.Resolve<IUserAgentDetectionProvider>()); }
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext ?? (_siteContext = Container.Resolve<ISiteContext>()); } 
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get { return _shopperContext ?? (_shopperContext = Container.Resolve<IShopperContext>()); }
    }

    private bool IsMobileSiteEnabled
    {
      get
      {
        bool isMobileSiteEnabled;

        if (SiteContext.ContextId == ContextIds.GoDaddy)
        {
          isMobileSiteEnabled = true;
        }
        else
        {
          isMobileSiteEnabled = DataCache.DataCache.GetPLData(SiteContext.PrivateLabelId, RESELLER_MOBILE_SITE_ENABLED_CATEGORY) == "1";
        }

        return isMobileSiteEnabled;
      }
    }

    private bool? _showFullSiteFromQueryString;
    private bool ShowFullSiteFromQueryString
    {
      get
      {
        if (!_showFullSiteFromQueryString.HasValue)
        {
          _showFullSiteFromQueryString = false;

          string showFullSiteQueryStringValue = HttpContext.Current.Request.QueryString[QueryStringParameters.FullSite];

          if (showFullSiteQueryStringValue != null)
          {
            if (showFullSiteQueryStringValue == "1" || showFullSiteQueryStringValue.ToLower() == "true")
            {
              _showFullSiteFromQueryString = true;
            }
          }
        }

        return _showFullSiteFromQueryString.Value;
      }
    }

    private bool IsNoRedirectBrowser
    {
      get { return UserAgentDetectionProvider.IsNoRedirectBrowser(HttpContext.Current.Request.UserAgent); }
    }

    private bool IsMobileDevice
    {
      get { return UserAgentDetectionProvider.IsMobileDevice(HttpContext.Current.Request.UserAgent); }
    }

    private bool IsOutDatedBrowser
    {
      get { return UserAgentDetectionProvider.IsOutDatedBrowser(HttpContext.Current.Request.UserAgent); }
    }

    private bool IsSearchEngineBot
    {
      get { return UserAgentDetectionProvider.IsSearchEngineBot(HttpContext.Current.Request.UserAgent); }
    }

    private bool HasUserAgentBeenChecked
    {
      get { return RedirectBrowserCookie != null; }
    }

    private HttpCookie _redirectBrowserCookie;
    private HttpCookie RedirectBrowserCookie
    {
      get { return _redirectBrowserCookie ?? (_redirectBrowserCookie = HttpContext.Current.Request.Cookies[CookieNames.MobileRedirectBrowser]); }
      set { _redirectBrowserCookie = value; }
    }

    private bool IsRedirectBrowserFromCookie()
    {
      bool isRedirectBrowserFromCookie = false;

      if (RedirectBrowserCookie != null)
      {
        isRedirectBrowserFromCookie = RedirectBrowserCookie.Value == "1";
      }

      return isRedirectBrowserFromCookie;
    }

    private void SetIsRedirectBrowserCookie(bool isRedirectUserAgent)
    {
      bool didCookieChange;
      string cookieSetValue = isRedirectUserAgent ? "1" : "0";

      if (RedirectBrowserCookie == null)
      {
        RedirectBrowserCookie = SiteContext.NewCrossDomainMemCookie(CookieNames.MobileRedirectBrowser);
        didCookieChange = true;
      }
      else
      {
        didCookieChange = RedirectBrowserCookie.Value != cookieSetValue;
      }

      if (didCookieChange)
      {
        RedirectBrowserCookie.Value = cookieSetValue;

        HttpCookie responseCookie = HttpContext.Current.Response.Cookies.Get(CookieNames.MobileRedirectBrowser) ?? SiteContext.NewCrossDomainMemCookie(CookieNames.MobileRedirectBrowser);

        responseCookie.Value = RedirectBrowserCookie.Value;

        HttpContext.Current.Response.Cookies.Set(responseCookie);
      }
    }

    private void CheckForFullSiteRequest()
    {
      if (ShowFullSiteFromQueryString)
      {
        SetIsRedirectBrowserCookie(false);
      }
    }

    private string GetAffiliatePublisherHash(out DateTime expiration)
    {
      expiration = DateTime.MinValue;
      string affilatePublisherHash = string.Empty;
      if (SiteContext.ContextId == ContextIds.GoDaddy)
      {
        HttpCookie publisherHashCookie = HttpContext.Current.Request.Cookies[CookieNames.PublisherHash];
        if (publisherHashCookie != null && !string.IsNullOrEmpty(publisherHashCookie.Value))
        {
          HttpCookie publisherExpireCookie = HttpContext.Current.Request.Cookies[CookieNames.PublisherExpirationDate];
          if (publisherExpireCookie != null && DateTime.TryParse(publisherExpireCookie.Value, out expiration))
          {
            affilatePublisherHash = publisherHashCookie.Value;
          }
        }
      }

      return affilatePublisherHash;
    }

    private void CheckUserAgent()
    {
      if (!HasUserAgentBeenChecked)
      {
        bool isRedirectUserAgent = IsMobileSiteEnabled && !IsNoRedirectBrowser && !IsSearchEngineBot && (IsMobileDevice || IsOutDatedBrowser);
        SetIsRedirectBrowserCookie(isRedirectUserAgent);
      }
    }

    private bool TryGetPublisherHash(out string publisherHashAndDate)
    {
      publisherHashAndDate = string.Empty;

      if (SiteContext.ContextId == ContextIds.GoDaddy)
      {
        DateTime expires;
        string publisherHash = GetAffiliatePublisherHash(out expires);

        if (!string.IsNullOrEmpty(publisherHash) && expires > DateTime.Now)
        {
          publisherHashAndDate = string.Format("{0}|{1}", publisherHash, expires.ToLocalTime());
        }
      }

      return publisherHashAndDate != string.Empty;
    }

    private NameValueCollection GetRedirectQueryParameters(string redirectKey, NameValueCollection additionalQueryParameters)
    {
      if (additionalQueryParameters == null)
      {
        additionalQueryParameters = new NameValueCollection(0);
      }

      NameValueCollection redirectQueryParameters = new NameValueCollection(additionalQueryParameters);

      string mrs = string.Empty;
      if (!string.IsNullOrEmpty(ShopperContext.ShopperId))
      {
        mrs = CookieHelper.EncryptCookieValue(ShopperContext.ShopperId);
      }

      string mrf;
      try
      {
        mrf = HttpContext.Current.Request.Url.PathAndQuery;
      }
      catch
      {
        mrf = string.Empty;
      }

      redirectQueryParameters.Add("mrk", redirectKey ?? string.Empty);
      redirectQueryParameters.Add("mrs", mrs);
      redirectQueryParameters.Add("mrf", mrf);

      string publisherHashAndDate;
      if (TryGetPublisherHash(out publisherHashAndDate))
      {
        redirectQueryParameters.Add(QueryStringParameters.PublisherHash, publisherHashAndDate);
      }

      if (IsOutDatedBrowser)
      {
        redirectQueryParameters.Add(QueryStringParameters.OutDatedBrowser, "1");
      }

      return redirectQueryParameters;
    }

    public bool IsRedirectRequired()
    {
      CheckForFullSiteRequest();
      CheckUserAgent();

      bool redirectRequired = IsRedirectBrowserFromCookie();

      return redirectRequired;
    }

    public string GetRedirectUrl(string redirectKey, NameValueCollection additionalQueryParameters)
    {
      string redirectUrl = string.Empty;

      if (IsRedirectRequired())
      {
        NameValueCollection redirectQueryParameters = GetRedirectQueryParameters(redirectKey, additionalQueryParameters);
        redirectUrl = LinkProvider.GetUrl(MOBILE_SITE_LINK_TYPE, string.Empty, redirectQueryParameters);
      }

      return redirectUrl;
    }
  }
}
