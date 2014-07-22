using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.DomainSearch.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Logging.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.DomainSearch
{
  public class DomainSearchProvider : ProviderBase, IDomainSearchProvider
  {
    private const string _LOGDOMAINSEARCHTRAFFICAPPSETTING = "ATLANTIS.LOGDOMAINSEARCHTRAFFIC";

    private readonly Dictionary<string, IList<IFindResponseDomain>> _emptyResponse = new Dictionary<string, IList<IFindResponseDomain>>(0);

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<ILogDomainSearchResultsProvider> _logDomainSearchResultsProvider;
    private readonly Lazy<IAppSettingsProvider> _appSettingsProvider;
    private readonly Lazy<IGeoProvider> _geoProvider;

    private readonly IList<string> _domainSearchDatabases = new List<string>
    {
      "affix,auctions,cctld,private,premium,similar,crosscheck"
    };

    private const bool INCLUDE_SPINS = true;

    public DomainSearchProvider(IProviderContainer container) : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _logDomainSearchResultsProvider = new Lazy<ILogDomainSearchResultsProvider>(() => Container.CanResolve<ILogDomainSearchResultsProvider>() ? Container.Resolve<ILogDomainSearchResultsProvider>() : null);
      _appSettingsProvider = new Lazy<IAppSettingsProvider>(() => Container.Resolve<IAppSettingsProvider>());
      _geoProvider = new Lazy<IGeoProvider>(() => Container.Resolve<IGeoProvider>());
    }

    private ILocalizationProvider _localization;
    private ILocalizationProvider Localization
    {
      get
      {
        return _localization ?? (_localization = Container.Resolve<ILocalizationProvider>());
      }
    }

    private IProxyContext _proxy;
    private IProxyContext Proxy
    {
      get
      {
        return _proxy ?? (_proxy = Container.Resolve<IProxyContext>());
      }
    }

    private bool? _submitSearchLog;
    private bool SubmitSearchLog
    {
      get
      {
        if (!_submitSearchLog.HasValue)
        {
          _submitSearchLog = "true".Equals(_appSettingsProvider.Value.GetAppSetting(_LOGDOMAINSEARCHTRAFFICAPPSETTING), StringComparison.OrdinalIgnoreCase);
        }
        return _submitSearchLog.Value;
      }
    }

    private Dictionary<string, IList<IFindResponseDomain>> GroupDomainResponse(DomainSearchResponseData responseData)
    {
      var domainResult = new Dictionary<string, IList<IFindResponseDomain>>(responseData.Domains.Count);
      if (responseData.ExactMatchDomains != null)
      {
        domainResult.Add(DomainGroupTypes.EXACT_MATCH, responseData.ExactMatchDomains);
        if (_logDomainSearchResultsProvider.Value != null && SubmitSearchLog)
        {
          foreach (var domain in responseData.ExactMatchDomains)
          {
            LogSearchDomain(domain);
          }
        }
      }

      var responseDomainList = responseData.Domains.ToList();

      domainResult.Add(DomainGroupTypes.All_DOMAINS, responseDomainList);

      var affixDomains = responseDomainList.FindAll(d => d.DomainSearchDataBase != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.AFFIX);
      if (affixDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.AFFIX, affixDomains);
      }

      var auctionDomains = responseDomainList.FindAll(d => d.AuctionType != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.AUCTIONS);
      if (auctionDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.AUCTIONS, auctionDomains);
      }

      var ccTldDomains = responseDomainList.FindAll(d => d.DomainSearchDataBase != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.COUNTRY_CODE_TLD);
      if (ccTldDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.COUNTRY_CODE_TLD, ccTldDomains);
      }

      var premiumDomains = responseDomainList.FindAll(d => d.DomainSearchDataBase != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.PREMIUM);
      if (premiumDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.PREMIUM, premiumDomains);
      }

      var privateDomains = responseDomainList.FindAll(d => d.DomainSearchDataBase != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.PRIVATE);
      if (privateDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.PRIVATE, privateDomains);
      }

      var similiarDomains = responseDomainList.FindAll(d => d.DomainSearchDataBase != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.SIMILIAR);
      if (similiarDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.SIMILIAR, similiarDomains);
      }

      var crossCheckDomains = responseDomainList.FindAll(d => d.DomainSearchDataBase != null && d.DomainSearchDataBase.ToLowerInvariant() == DomainGroupTypes.CROSS_CHECK);
      if (crossCheckDomains.Count != 0)
      {
        domainResult.Add(DomainGroupTypes.CROSS_CHECK, crossCheckDomains);
      }

      return domainResult;
    }

    public IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, ISplitTestInfo splitTestInfo = null)
    {
      return SearchDomain(searchPhrase, sourceCode, sourceUrl, new List<string>(0), splitTestInfo);
    }

    public IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, IList<string> tldsToSearch, ISplitTestInfo splitTestInfo = null)
    {
      IDomainSearchResult domainSearchResult = null;

      var slitId = splitTestInfo == null ? string.Empty : splitTestInfo.SplitTestid;
      var splitSide = splitTestInfo == null ? string.Empty : splitTestInfo.SplitTestSideName;

      for (var i = 1; i < 4; i++)
      {
        if (TryGetSearchResults(searchPhrase, sourceCode, tldsToSearch, sourceUrl, slitId, splitSide, i, out domainSearchResult))
        {
          break;
        }
      }

      return domainSearchResult;
    }

    private void LogSearchDomain(IFindResponseDomain domain)
    {
      if (_logDomainSearchResultsProvider.Value == null) return;

      var domainName = string.Concat(domain.Domain.Sld, ".", domain.Domain.Tld);
      var availability = DomainAvailability.NotAvailable;

      if (domain.IsAvailable)
      {
        availability = DomainAvailability.Available;
      }
      else if (domain.IsBackOrderAvailable)
      {
        availability = DomainAvailability.Backorder;
      }

      _logDomainSearchResultsProvider.Value.SubmitLog(domainName, availability);
    }

    private bool TryGetSearchResults(string searchPhrase, string sourceCode, IList<string> tldsToSearch, string sourceUrl, string splitTestId, string splitTestSideName, int attempt, out IDomainSearchResult searchResult)
    {
      var result = true;
      var geoLocation = _geoProvider.Value.RequestGeoLocation;
      searchResult = new DomainSearchResult(false, _emptyResponse);

      try
      {
        var request = new DomainSearchRequestData(_shopperContext.Value.ShopperId, sourceUrl, string.Empty, _siteContext.Value.Pathway, 0)
        {
          ClientIp = Proxy.OriginIP,
          CountrySite = Localization.CountrySite,
          DomainSearchDataBases = _domainSearchDatabases,
          IncludeSpins = INCLUDE_SPINS,
          Language = Localization.FullLanguage,
          PrivateLabelId = _siteContext.Value.PrivateLabelId,
          SearchPhrase = searchPhrase,
          ShopperStatus = _shopperContext.Value.ShopperStatus,
          SourceCode = sourceCode,
          Tlds = tldsToSearch,
          ClientIpLatitude = geoLocation == null ? 0d : geoLocation.Latitude,
          ClientIpLongitude = geoLocation == null ? 0d : geoLocation.Longitude,
          ClientIpCity = geoLocation == null ? string.Empty : geoLocation.City,
          ClientIpCountry = geoLocation == null ? string.Empty : geoLocation.CountryCode,
          ClientIpRegion = geoLocation == null ? string.Empty : geoLocation.GeoRegionName,
          SplitTestId = splitTestId,
          SplitTestSideName = splitTestSideName,
          RequestTimeout = SearchRequestTimeout
        };

        var requestType = RequestTypeLookUp.GetCurrentRequestType();

        var response = Engine.Engine.ProcessRequest(request, requestType) as DomainSearchResponseData;

        if (response != null)
        {
          var exception = response.GetException();

          if (exception == null)
          {
            var domainResult = GroupDomainResponse(response);
            searchResult = new DomainSearchResult(true, domainResult);

            if (_siteContext.Value.IsRequestInternal)
            {
              searchResult.JsonRequest = string.Empty;
              searchResult.JsonResponse = response.ToJson();
            }
          }
          else
          {
            throw exception;
          }
        }
      }
      catch (Exception ex)
      {
        if (ex.Message.StartsWith("The operation has timed out", StringComparison.OrdinalIgnoreCase))
        {
          result = false;
        }

        LogException(searchPhrase, sourceCode, sourceUrl, splitTestId, splitTestSideName, attempt, ex.Message, ex.StackTrace);
      }

      return result;
    }

    private TimeSpan? _searchRequestTimeout;
    private TimeSpan SearchRequestTimeout
    {
      get
      {
        if (_searchRequestTimeout == null)
        {
          var appSettingValue = _appSettingsProvider.Value.GetAppSetting("ATLANTIS.SEARCHAPI.TIMEOUT");
          var v = 0;

          if (!int.TryParse(appSettingValue, out v))
          {
            v = 5000;
          }

          _searchRequestTimeout = TimeSpan.FromMilliseconds(v);
        }

        return _searchRequestTimeout.Value;
      }
    }

    private void LogException(string searchPhrase, string sourceCode, string sourceUrl, string splitTestId, string splitTestSideName, int attempt, string message, string stackTrace)
    {
      var errorMessage = message + Environment.NewLine + stackTrace;
      var data = string.Format("SearchPhrase: {0}, SourceCode: {1}, SourceUrl: {2}, SplitTest: {3}{4}, Attempt: {5}", searchPhrase, sourceCode, sourceUrl, splitTestId, splitTestSideName, attempt);
      var aex = new AtlantisException("AF.Providers.DomainSearch.TryGetSearchResults", 0, errorMessage, data);
      Engine.Engine.LogAtlantisException(aex);
    }
  }

  internal class DomainAvailability
  {
    public const int NotAvailable = 0;
    public const int Available = 1;
    public const int Backorder = 2;
  }
}