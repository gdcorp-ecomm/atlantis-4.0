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
    internal const string QS_COUNTRYSITE_KEY = "regionsite";
    internal const string QS_MARKETID_KEY = "marketid";

    private readonly Lazy<ICountrySite> _countrySite; 
    private readonly Lazy<IEnumerable<string>> _validCountrySubdomains;
    private readonly Lazy<CountrySitesActiveResponseData> _countrySitesActive;
    private readonly Lazy<MarketsActiveResponseData> _marketsActive; 
    protected Lazy<CountrySiteCookie> CountrySiteCookie;
    private Lazy<ISiteContext> _siteContext;
    protected Lazy<LanguageCookie> LanguageCookie; 
    private readonly Lazy<string> _proxyLanguage;
    private string _rewrittenUrlLanguage;
    private IMarket _market;
    private string _shortLanguage = null;
    private CultureInfo _cultureInfo = null;
    private readonly Dictionary<string,CountrySiteMarketMappingsResponseData> _countrySiteMarketMappings;

    protected LocalizationProvider(IProviderContainer container)
      : base(container)
    {
      _countrySite = new Lazy<ICountrySite>(LoadCountrySiteInfo);
      _validCountrySubdomains = new Lazy<IEnumerable<string>>(LoadValidCountrySubdomains);
      _countrySitesActive = new Lazy<CountrySitesActiveResponseData>(LoadActiveCountrySites);
      _marketsActive = new Lazy<MarketsActiveResponseData>(LoadActiveMarkets);
      _rewrittenUrlLanguage = string.Empty;
      CountrySiteCookie = new Lazy<CountrySiteCookie>(() => new CountrySiteCookie(Container));
      LanguageCookie = new Lazy<LanguageCookie>(() => new LanguageCookie(Container));
      _proxyLanguage = new Lazy<string>(GetProxyLanguage);
      _countrySiteMarketMappings = new Dictionary<string, CountrySiteMarketMappingsResponseData>();
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
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

    public ICountrySite TryGetCountrySite(string countrySiteId)
    {
      ICountrySite countrySite;
      _countrySitesActive.Value.TryGetCountrySiteById(countrySiteId, out countrySite);
      return countrySite;
    }

    public IMarket TryGetMarket(string marketId)
    {
      IMarket market;
      _marketsActive.Value.TryGetMarketById(marketId, out market);
      return market;
    }

    public IMarket GetMarketForCountrySite(string countrySiteId, string marketId)
    {
      IMarket market = null;
      // try to get the mapping given the supplied countrySiteId
      var marketMappings = GetOrLoadMarketMappingsForCountrySite(countrySiteId);
      if ( marketMappings != null ) // if we found one, then the countrysiteid must be 'ok'
      {
        bool isMapping = marketMappings.IsMapping(marketId, _siteContext.Value.IsRequestInternal);
        if (isMapping) // if mapping exists, we are done
        {
          market = TryGetMarket(marketId);
        }
        // if not, then we had a good countrySiteId, but bad marketid
      }
      else
      {
        // countrySiteId must have been invalid, so use the request's id... which must have already been validated
        countrySiteId = CountrySite;
      }

      // we have a valid countrySiteId now, and if the market wasn't computable from marketid, then
      // we revert to the default market id
      if (market == null)
      {
        var countrySite = TryGetCountrySite(countrySiteId);

        // if we are here, we must have a valid countrySiteId, so no need to validate
        marketId = countrySite.DefaultMarketId;
        _marketsActive.Value.TryGetMarketById(marketId, out market);
        // no need to check for IsInternal/public because we are using the default... which needs to always be not internal
      }

      return market;
    }

    public IMarket TryGetMarketForCountrySite(string countrySiteId, string marketId)
    {
      IMarket market = null;
      var marketMappings = GetOrLoadMarketMappingsForCountrySite(countrySiteId);
      if ( marketMappings != null )
      {
        bool isMapping = marketMappings.IsMapping(marketId, _siteContext.Value.IsRequestInternal);
        if (isMapping)
        {
          market = TryGetMarket(marketId);
        }
      }

      return market;
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
      return _countrySitesActive.Value.IsValidCountrySite(countryCode, _siteContext.Value.IsRequestInternal);
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

          //  Transperfect
          string marketId = (ProxyLanguage.Equals("es", StringComparison.OrdinalIgnoreCase) || ProxyLanguage.Equals("es-US", StringComparison.OrdinalIgnoreCase) ? "es-US" : string.Empty);
          if (marketId != string.Empty && _marketsActive.Value.TryGetMarketById(marketId, out defaultMarket))
          {
            _market = defaultMarket;
            SetLanguageCookie("es");
            return _market;
          }

          //  Try to get marketid querystring value
          if (LocalizationProvider.TryGetMarketIdFromQueryString(out marketId))
          {
            //  Make sure it's valid for the CountrySite
            defaultMarket = TryGetMarketForCountrySite(CountrySite, marketId);
            if (defaultMarket != null)
            {
              _market = defaultMarket;
              SetLanguageCookie(_market.Id);
              return _market;
            }
          }

          //  Default market id for countrysite
          if (_marketsActive.Value.TryGetMarketById(CountrySiteInfo.DefaultMarketId, out defaultMarket))
          {
            _market = defaultMarket;
            SetLanguageCookie(_market.Id);
            return _market;
          }
          
          //  Default market 
          _market = MarketsActiveResponseData.DefaultMarketInfo;
          SetLanguageCookie(_market.Id);
        }

        return _market;
      }
    }

    public bool IsGlobalSite(string countrySiteId)
    {
      return _WWW.Equals(countrySiteId, StringComparison.OrdinalIgnoreCase);
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

    public virtual string PreviousLanguageCookieValue
    {
      get
      {
        string result = null;
        if (LanguageCookie.Value.HasValue)
        {
          result = LanguageCookie.Value.Value;
        }

        return result;
      }
    }

    public void SetMarket(string marketId)
    {
      IMarket market;
      bool proxyActive = IsTransperfectProxyActive();
      
      if (_marketsActive.Value.TryGetMarketById((proxyActive ? "es-US" : marketId), out market))
      {
        _market = market;
        LanguageCookie.Value.Value = (proxyActive ? "es" : market.Id);
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
          _cultureInfo = DetermineCultureInfo();
        }
        return _cultureInfo;
      }
    }

    
    private CultureInfo DetermineCultureInfo()
    {
      CultureInfo result;

      try
      {
        result = SupportedCultures.GetByName(MarketInfo.MsCulture) ?? CultureInfo.CurrentCulture;        
      }
      catch (CultureNotFoundException ex)
      {
        AtlantisException aex = new AtlantisException("LocalizationProvider.DetermineCultureInfo", 0, ex.Message + ex.StackTrace, MarketInfo.MsCulture);
        Engine.Engine.LogAtlantisException(aex);
        result = CultureInfo.CurrentCulture;
      }

      return result;
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

    private bool IsTransperfectProxyActive()
    {
      return ProxyLanguage.Equals("es", StringComparison.OrdinalIgnoreCase) || ProxyLanguage.Equals("es-us", StringComparison.OrdinalIgnoreCase);
    }

    public string GetLanguageUrl()
    {
      return GetLanguageUrl(CountrySite, MarketInfo.Id);
    }

    public string GetLanguageUrl(string marketId)
    {
      return GetLanguageUrl(CountrySite, marketId);
    }

    public string GetLanguageUrl(string countrySiteId, string marketId)
    {
      var marketMappings = GetOrLoadMarketMappingsForCountrySite(countrySiteId);
      return marketMappings.GetLanguageUrl(marketId);
    }

    public IEnumerable<IMarket> GetMarketsForCountryCode(string countryCode)
    {
      var marketList = new List<IMarket>();
      CountrySiteMarketMappingsResponseData marketMappings;
      string validCountryCode = IsValidCountrySubdomain(countryCode) ? countryCode : _WWW;

      marketMappings = GetOrLoadMarketMappingsForCountrySite(validCountryCode);

      if (_countrySiteMarketMappings.Count == 0)
      {
         marketMappings = GetOrLoadMarketMappingsForCountrySite(_WWW);
      }

      IEnumerable<string> marketIds = marketMappings.GetMarketIdsForCountry();

      foreach (var marketId in marketIds)
      {
        if (marketMappings.IsPublicMapping(marketId))
        {
          IMarket market = TryGetMarket(marketId);
          marketList.Add(market);
        }
      }

      return marketList;
    } 

    private CountrySiteMarketMappingsResponseData GetOrLoadMarketMappingsForCountrySite(string countrySiteId)
    {
      CountrySiteMarketMappingsResponseData marketMappings;
      _countrySiteMarketMappings.TryGetValue(countrySiteId, out marketMappings);
      if (marketMappings == null)
      {
        marketMappings = LoadCountrySiteMarketMappings(countrySiteId);
        _countrySiteMarketMappings[countrySiteId] = marketMappings;
      }
      return marketMappings;
    }

    public IEnumerable<IMarket> GetMappedMarketsForCountrySite(string countrySite, bool includeInternalOnly)
    {
      var marketList = new List<IMarket>();
      CountrySiteMarketMappingsResponseData marketMappings = GetOrLoadMarketMappingsForCountrySite(countrySite);
      if (marketMappings != null)
      {
        foreach (var marketId in marketMappings.GetMarketIdsForCountry())
        {
          if (includeInternalOnly || marketMappings.IsPublicMapping(marketId))
          {
            IMarket market = TryGetMarket(marketId);
            if (market != null && (includeInternalOnly || !market.IsInternalOnly))
            {
              marketList.Add(market);
            }
          }
        }  
      }

      return marketList;
    }

    private CountrySiteMarketMappingsResponseData LoadCountrySiteMarketMappings(string countrySiteId)
    {
      CountrySiteMarketMappingsResponseData result;

      var request = new CountrySiteMarketMappingsRequestData(countrySiteId);
      try
      {
        result = (CountrySiteMarketMappingsResponseData)DataCache.DataCache.GetProcessRequest(request,LocalizationProviderEngineRequests.CountrySiteMarketMappingsRequest);
      }
      catch
      {
        result = CountrySiteMarketMappingsResponseData.NoMappingsResponse;
      }

      return result;
    }

    public static bool TryGetRegionSiteFromQueryString(out string regionSite)
    {
      try
      {
        regionSite = HttpContextFactory.GetHttpContext().Request.QueryString[QS_COUNTRYSITE_KEY] ?? string.Empty;
        return !string.IsNullOrEmpty(regionSite);
      }
      catch
      {
        regionSite = string.Empty;
        return false;
      }  
    }

    public static bool TryGetMarketIdFromQueryString(out string marketId)
    {
      try
      {
        marketId = HttpContextFactory.GetHttpContext().Request.QueryString[QS_MARKETID_KEY];
        return !string.IsNullOrEmpty(marketId);
      }
      catch
      {
        marketId = string.Empty;
        return false;
      }  
    }

    protected void SetCountrySiteCookie(string value)
    {
      if ((!CountrySiteCookie.Value.HasValue) || !CountrySiteCookie.Value.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
      {
        CountrySiteCookie.Value.Value = value;
      }
    }

    protected void SetLanguageCookie(string value)
    {
      if ((!LanguageCookie.Value.HasValue) || !LanguageCookie.Value.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
      {
        LanguageCookie.Value.Value = value;
      }
    }
  }
}
