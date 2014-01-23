using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Products.Handlers
{
  internal class O365ProductGroupOfferedHandler : ProductGroupOfferedHandler
  {
    private readonly Lazy<IGeoProvider> _geoProvider;
    private readonly Lazy<IShopperDataProvider> _shopperDataProvider;

    public O365ProductGroupOfferedHandler(IProviderContainer container) : base(container)
    {
      _geoProvider = new Lazy<IGeoProvider>(() => (container != null && container.CanResolve<IGeoProvider>()) ? container.Resolve<IGeoProvider>() : null);
      _shopperDataProvider = new Lazy<IShopperDataProvider>(() => (container != null && container.CanResolve<IShopperDataProvider>()) ? container.Resolve<IShopperDataProvider>() : null);
    }

    private static ProductGroupOfferedCountriesResponseData LoadCountriesOffered(int productGroupType)
    {
      var request = new ProductGroupOfferedCountriesRequestData(productGroupType);
      return (ProductGroupOfferedCountriesResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.ProductOfferCountries);
    }

    public override bool IsProductGroupOffered(int productGroupType, ProductGroupsOfferedResponseData responseData)
    {
      var offered = base.IsProductGroupOffered(productGroupType, responseData);

      if (offered)
      {
        string shopperCountry;
        if (_shopperDataProvider.Value != null && _shopperDataProvider.Value.TryGetField(ShopperDataFields.Country, out shopperCountry) && !string.IsNullOrEmpty(shopperCountry))
        {
          var countriesOfferedResponse = LoadCountriesOffered(productGroupType);
          offered = countriesOfferedResponse.ContainsCountry(shopperCountry);
        }
        else if(_geoProvider.Value != null)
        {
          var countriesOfferedResponse = LoadCountriesOffered(productGroupType);
          offered = countriesOfferedResponse.ContainsCountry(_geoProvider.Value.RequestCountryCode);
        }
      }

      return offered;
    }
  }
}