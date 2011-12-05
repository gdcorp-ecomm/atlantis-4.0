using System;
using Atlantis.Framework.Interface;
using System.Net.Cache;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Currency;
using System.Collections.Generic;
using System.Net;
using Atlantis.Framework.FastballContent.Interface.Cookies;

namespace Atlantis.Framework.FastballContent.Interface
{
  public class FastballContentRequestData : RequestData
  {
    private const string _CONTENTRELATIVEFORMAT =
      "fbiChannelIntegrationService/Offer.svc/get/app/{0}/privatelabel/{1}/shopper/{2}/placement/{3}";

    private IProviderContainer _container;
    private string _placement = "banner-iscBannerNoFormat";
    private int _appId;

    public FastballContentRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int fastballAppId, string placement, IProviderContainer container)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CacheLevel = RequestCacheLevel.Default;
      RequestTimeout = TimeSpan.FromSeconds(10);
      _container = container;
      _placement = placement;
      _appId = fastballAppId;
    }

    public RequestCacheLevel CacheLevel { get; set; }

    public IEnumerable<Cookie> GetRequestCookies()
    {
      List<Cookie> result = new List<Cookie>();

      string currencyCookieName = "currency" + PrivateLabelId.ToString();
      string currencyCookieValue = "potableSourceStr=" + CurrencyType;
      result.Add(new Cookie(currencyCookieName, currencyCookieValue, "/"));

      if (!string.IsNullOrEmpty(ShopperID))
      {
        string prefix = string.Empty;
        if (SiteContext.ServerLocation == ServerLocationType.Dev)
        {
          prefix = "dev";
        }
        else if (SiteContext.ServerLocation == ServerLocationType.Test)
        {
          prefix = "test";
        }
        string shopperCookieName = prefix + "Shopper" + PrivateLabelId.ToString();
        string shopperCookieValue = CookieHelper.EncryptCookieValue(ShopperID);
        result.Add(new Cookie(shopperCookieName, shopperCookieValue, "/"));
      }

      return result;
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get
      {
        if ((_siteContext == null) && (_container != null))
        {
          _siteContext = _container.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    public int PrivateLabelId
    {
      get
      {
        int result = 0;
        if (SiteContext != null)
        {
          result = SiteContext.PrivateLabelId;
        }
        return result;
      }
    }

    private ICurrencyProvider _currency;
    private ICurrencyProvider Currency
    {
      get
      {
        if ((_currency == null) && (_container != null))
        {
          _currency = _container.Resolve<ICurrencyProvider>();
        }
        return _currency;
      }
    }

    private string CurrencyType
    {
      get
      {
        string result = "USD";
        if (Currency != null)
        {
          result = Currency.SelectedDisplayCurrencyType;
        }
        return result;
      }
    }



    private string _requestUrl;
    public string RequestUrl
    {
      get
      {
        if (_requestUrl == null)
        {
          _requestUrl = BuildRequestUrl();
        }
        return _requestUrl;
      }
    }

    private string BuildRequestUrl()
    {
      string urlShopperId = ShopperID;
      if (string.IsNullOrEmpty(ShopperID))
      {
        urlShopperId = "0";
      }

      string relativeUrl = string.Format(_CONTENTRELATIVEFORMAT,
        _appId.ToString(),
        PrivateLabelId.ToString(),
        urlShopperId,
        _placement);

      bool secure = SiteContext.ServerLocation != ServerLocationType.Dev;

      ILinkProvider links = _container.Resolve<ILinkProvider>();
      string result = links.GetUrl("TRAFFICURL", relativeUrl, QueryParamMode.CommonParameters, secure);
      return result;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in FastballContentRequestData");
    }
  }
}
