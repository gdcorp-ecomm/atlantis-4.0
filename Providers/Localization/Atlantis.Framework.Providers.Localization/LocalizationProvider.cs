using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public abstract class LocalizationProvider : ProviderBase, ILocalizationProvider
  {
    protected const string _WWW = "WWW";

    private readonly Lazy<ICountrySite> _countrySite; 
    private readonly Lazy<IEnumerable<string>> _validCountrySubdomains;
    private readonly Lazy<CountrySitesActiveResponseData> _countrySitesActive;
    private readonly Lazy<MarketsActiveResponseData> _marketsActive; 
    protected Lazy<CountrySiteCookie> CountrySiteCookie;
    private readonly Lazy<string> _proxyLanguage;
    private string _rewrittenUrlLanguage;
    private IMarket _market;
    private string _shortLanguage = null;
    private CultureInfo _cultureInfo = null;

    public LocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _countrySite = new Lazy<ICountrySite>(LoadCountrySiteInfo);
      _validCountrySubdomains = new Lazy<IEnumerable<string>>(LoadValidCountrySubdomains);
      _countrySitesActive = new Lazy<CountrySitesActiveResponseData>(LoadActiveCountrySites);
      _marketsActive = new Lazy<MarketsActiveResponseData>(LoadActiveMarkets);
      _rewrittenUrlLanguage = string.Empty;
      CountrySiteCookie = new Lazy<CountrySiteCookie>(() => new CountrySiteCookie(Container));
      _proxyLanguage = new Lazy<string>(GetProxyLanguage);
    }

    protected abstract string DetermineCountrySite();

    private ICountrySite LoadCountrySiteInfo()
    {
      ICountrySite result;
      if (!_countrySitesActive.Value.TryGetCountrySiteById(DetermineCountrySite(), out result))
      {
        result = CountrySitesActiveResponseData.DefaultCountrySiteInfo;
      }
      return result;
    }

    private CountrySitesActiveResponseData LoadActiveCountrySites()
    {
      CountrySitesActiveResponseData result;

      var request = new CountrySitesActiveRequestData();
      try
      {
        result =
          (CountrySitesActiveResponseData)
          DataCache.DataCache.GetProcessRequest(request, LocalizationProviderEngineRequests.CountrySitesActiveRequest);
      }
      catch
      {
        result = CountrySitesActiveResponseData.DefaultCountrySites;
      }

      return result;
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

    private IEnumerable<string> LoadValidCountrySubdomains()
    {
      var countrySites = new List<string>();
      foreach (ICountrySite countrySite in _countrySitesActive.Value.CountrySites)
      {
        countrySites.Add(countrySite.Id);
      }
      return countrySites;
    }

    public bool IsValidCountrySubdomain(string countryCode)
    {
      return _countrySitesActive.Value.IsValidCountrySite(countryCode);
    }

    public IEnumerable<string> ValidCountrySiteSubdomains
    {
      get { return _validCountrySubdomains.Value; }
    }

    public ICountrySite CountrySiteInfo
    {
      get { return _countrySite.Value; }
    }

    public string CountrySite
    {
      get { return CountrySiteInfo.Id.ToUpperInvariant(); }
    }

    public IMarket MarketInfo
    {
      get
      {
        if (_market == null)
        {
          IMarket defaultMarket;
          string marketId = (ProxyLanguage.Equals("es", StringComparison.OrdinalIgnoreCase) || ProxyLanguage.Equals("es-US", StringComparison.OrdinalIgnoreCase) ? "es-US" : CountrySiteInfo.DefaultMarketId);
          if (!_marketsActive.Value.TryGetMarketById(marketId, out defaultMarket))
          {
            defaultMarket = MarketsActiveResponseData.DefaultMarketInfo;
          }
          _market = defaultMarket;
        }
        
        return _market;
      }
    }

    public bool IsGlobalSite()
    {
      return IsCountrySite(_WWW);
    }

    public bool IsCountrySite(string countryCode)
    {
      return _countrySite.Value.Id.Equals(countryCode, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      return countryCodes.Contains(CountrySite, StringComparer.OrdinalIgnoreCase);
    }

    public string GetCountrySiteLinkType(string baseLinkType)
    {
      string result = baseLinkType;
      if (!_WWW.Equals(CountrySite))
      {
        result = string.Concat(baseLinkType, ".", CountrySite.ToUpperInvariant());
      }

      return result;
    }

    internal string ProxyLanguage
    {
      get { return _proxyLanguage.Value; }
    }

    public string FullLanguage
    {
      get { return MarketInfo.Id; }
    }

    public string ShortLanguage
    {
      get
      {
        if (_shortLanguage == null)
        {
          _shortLanguage = MarketInfo.Id.Split('-')[0]; 
        }
        return _shortLanguage;
      }
    }

    public string RewrittenUrlLanguage
    {
      get { return _rewrittenUrlLanguage; }
      set { _rewrittenUrlLanguage = value; }
    }

    public bool IsActiveLanguage(string language)
    {
      bool result =
        (language.Equals(ShortLanguage, StringComparison.OrdinalIgnoreCase)) ||
        (language.Equals(FullLanguage, StringComparison.OrdinalIgnoreCase));

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

    public void SetMarket(string marketId)
    {
      IMarket market;
      if (_marketsActive.Value.TryGetMarketById(marketId, out market))
      {
        _market = market;
        _cultureInfo = null;
        _shortLanguage = null;
      }
    }

    public CultureInfo CurrentCultureInfo
    {
      get 
      {
        if (_cultureInfo == null)
        {
          _cultureInfo = CultureInfo.GetCultureInfo(MarketInfo.MsCulture);
        }
        return _cultureInfo;
      }
    }

    private string GetProxyLanguage()
    {
      string result = string.Empty;

      IProxyContext proxyContext;
      if (Container.TryResolve(out proxyContext))
      {
        IProxyData languageProxy;
        if (proxyContext.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out languageProxy))
        {
          string language;
          if (languageProxy.TryGetExtendedData("language", out language))
          {
            result = language;
          }
        }
      }

      return (result.Equals("es", StringComparison.OrdinalIgnoreCase) ? "es-US" : result);
    }
  }
}
