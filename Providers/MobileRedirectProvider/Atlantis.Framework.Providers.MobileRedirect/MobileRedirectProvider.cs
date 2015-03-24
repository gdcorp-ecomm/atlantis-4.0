using System;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.BasePages;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.MobileRedirect.Interface;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;

namespace Atlantis.Framework.Providers.MobileRedirect
{
  public class MobileRedirectProvider : ProviderBase, IMobileRedirectProvider
  {
    private const int RESELLER_MOBILE_SITE_ENABLED_CATEGORY = 388;
    private const string MOBILE_SITE_LINK_TYPE = "MDOTMOBILEURL";

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

    private bool? GetFullSiteQueryStringValue()
    {
      bool? fullSiteValue = null;

      string showFullSiteQueryStringValue = HttpContext.Current.Request.QueryString[QueryStringParameters.FullSite];

      if (!string.IsNullOrEmpty(showFullSiteQueryStringValue))
      {
        if (showFullSiteQueryStringValue == "1" || showFullSiteQueryStringValue.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          fullSiteValue = true;
        }
        else
        {
          fullSiteValue = false;
        }
      }

      return fullSiteValue;
    }

    private void SetRedirectBrowserCookie(bool redirectBrowser)
    {
      bool didCookieChange;
      string cookieSetValue = redirectBrowser ? "1" : "0";

      HttpCookie redirectBrowsreCookieFromRequest = HttpContext.Current.Request.Cookies[CookieNames.MobileRedirectBrowser];

      if (redirectBrowsreCookieFromRequest == null)
      {
        didCookieChange = true;
      }
      else
      {
        didCookieChange = redirectBrowsreCookieFromRequest.Value != cookieSetValue;
      }

      if (didCookieChange)
      {
        HttpCookie responseCookie = SiteContext.NewCrossDomainMemCookie(CookieNames.MobileRedirectBrowser);

        responseCookie.Value = cookieSetValue;

        HttpContext.Current.Response.Cookies.Set(responseCookie);
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

    private bool ShouldRedirectUserAgent()
    {
      bool shouldRedirect;

      HttpCookie redirectBrowserCookie = HttpContext.Current.Request.Cookies[CookieNames.MobileRedirectBrowser];

      if (redirectBrowserCookie == null)
      {
        bool redirectBrowser = IsMobileSiteEnabled && !IsNoRedirectBrowser && !IsSearchEngineBot && (IsMobileDevice || IsOutDatedBrowser);
        SetRedirectBrowserCookie(redirectBrowser);
        
        shouldRedirect = redirectBrowser;
      }
      else
      {
        shouldRedirect = redirectBrowserCookie.Value == "1";
      }

      return shouldRedirect;
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
      bool redirectRequired;

      bool? fullSiteQueryStringValue = GetFullSiteQueryStringValue();

      if (fullSiteQueryStringValue.HasValue)
      {
        SetRedirectBrowserCookie(!fullSiteQueryStringValue.Value);
        redirectRequired = !fullSiteQueryStringValue.Value;
      }
      else
      {
        redirectRequired = ShouldRedirectUserAgent();
      }

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
