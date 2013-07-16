using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.DomainSearch.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.DomainSearch
{
  public class DomainSearchProvider : ProviderBase, IDomainSearchProvider 
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;

    private readonly IList<string> _domainSearchDatabases = new List<string> { "affix,auctions,cctld,private,premium,similar" };
    private const bool INCLUDE_SPINS = true;

    public DomainSearchProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
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

    private static Dictionary<string, IEnumerable<IFindResponseDomain>> GroupDomainResponse(DomainSearchResponseData responseData)
    {
     var domainResult = new Dictionary<string, IEnumerable<IFindResponseDomain>>(responseData.Domains.Count);
      if (responseData.ExactMatchDomains != null)
      {
        domainResult.Add(DomainGroupTypes.EXACT_MATCH, responseData.ExactMatchDomains);
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

      return domainResult;
    }

    public bool SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, out IDomainSearchResult domainSearchResult)
    {
      var success = false;
      domainSearchResult = null;

      try
      {
        var request = new DomainSearchRequestData(_shopperContext.Value.ShopperId, sourceUrl, string.Empty, _siteContext.Value.Pathway, 0)
                                          {
                                            ClientIp = Proxy.OriginIP,
                                            CountrySite = Localization.CountrySite,
                                            DomainSearchDataBases = _domainSearchDatabases,
                                            IncludeSpins = INCLUDE_SPINS,
                                            Language = Localization.ShortLanguage,
                                            PrivateLabelId = _siteContext.Value.PrivateLabelId,
                                            SearchPhrase = searchPhrase,
                                            ShopperStatus = _shopperContext.Value.ShopperStatus,
                                            SourceCode = sourceCode

                                          };

        var response = Engine.Engine.ProcessRequest(request, 714) as DomainSearchResponseData;

        if (response != null)
        {
          var exception = response.GetException();

          if (exception == null)
          {
           var domainResult =  GroupDomainResponse(response);

            domainSearchResult = new DomainSearchResult(domainResult, response.ToJson());
            success = true;
          }
          else
          {
            throw exception; 
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        var data = string.Format("searchPhrase:{0}, sourceCode:{1}, sourceUrl:{2}", searchPhrase, sourceCode, sourceUrl);
        var aex = new AtlantisException("Atlantis.Framework.Providers.DomainSearch.SearchDomain", "0", message, data, _siteContext.Value, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(aex);

      }

      return success;
    }
  }
}
