using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Products.Handlers
{
  internal class ProductGroupOfferedHandler : IProductGroupOfferedHandler
  {
    private readonly Lazy<ILocalizationProvider> _localization;

    public ProductGroupOfferedHandler(IProviderContainer container)
    {
      _localization = new Lazy<ILocalizationProvider>(() => (container != null && container.CanResolve<ILocalizationProvider>()) ? container.Resolve<ILocalizationProvider>() : null);
    }

    private static ProductGroupsOfferedMarketsResponseData LoadMarketsOffered()
    {
      var request = new ProductGroupsOfferedMarketsRequestData();
      return (ProductGroupsOfferedMarketsResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.ProductOfferMarkets);
    }

    public virtual bool IsProductGroupOffered(int productGroupType, ProductGroupsOfferedResponseData responseData)
    {
      var offered = responseData.IsOffered(productGroupType);

      if (offered && _localization.Value != null)
      {
        var marketsOfferedResponse = LoadMarketsOffered();
        ProductGroupMarketData marketData;

        if (marketsOfferedResponse.TryGetMarketData(productGroupType, out marketData))
        {
          offered = marketData.ContainsMarket(_localization.Value.MarketInfo.Id);
        }
        else
        {
          return true;
        }
      }

      return offered;
    }
  }
}