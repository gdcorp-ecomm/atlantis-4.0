using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products.Factories;

namespace Atlantis.Framework.Providers.Products
{
  /// <summary>
  /// Product Provider.  A page should only have one of these, and pass it to any user controls that need it.
  /// It provides page cached- product classes, so expensive calls are minimized to once per page.
  /// </summary>
  public class ProductProvider : ProviderBase, IProductProvider
  {
    private Dictionary<int, IProduct> _products;

    private readonly Lazy<ProductGroupsOfferedResponseData> _productGroupsOffered;
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ICurrencyProvider> _currency;

    public ProductProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _currency = new Lazy<ICurrencyProvider>(() => Container.Resolve<ICurrencyProvider>());
      _productGroupsOffered = new Lazy<ProductGroupsOfferedResponseData>(LoadOfferedProducts);
    }

    private ProductGroupsOfferedResponseData LoadOfferedProducts()
    {
      ProductGroupsOfferedRequestData request = new ProductGroupsOfferedRequestData(_siteContext.Value.PrivateLabelId);
      return (ProductGroupsOfferedResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.ProductOffer);
    }

    public IProduct GetProduct(int productId)
    {
      IProduct result;

      if (_products == null)
      {
        _products = new Dictionary<int, IProduct>(32);
      }

      if (!_products.TryGetValue(productId, out result))
      {
        result = new Product(productId, Container);
        _products[productId] = result;
      }
      return result;
    }

    public List<IProduct> NewProductList(IEnumerable<int> productIds)
    {
      List<IProduct> result = new List<IProduct>();
      foreach (int productId in productIds)
      {
        result.Add(GetProduct(productId));
      }
      return result;
    }

    #region ProductView Creation

    public virtual List<IProductView> NewProductViewList(IEnumerable<int> productIds)
    {
      return NewProductViewList(productIds, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
    }

    public virtual List<IProductView> NewProductViewList(IEnumerable<int> productIds, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      List<IProductView> resultProductViewList = new List<IProductView>();

      foreach (int productId in productIds)
      {
        resultProductViewList.Add(new ProductView(_currency.Value, GetProduct(productId), false, priceRoundingMethod, savingsRoundingMethod));
      }

      return resultProductViewList;
    }

    public virtual List<IProductView> NewProductViewList(IEnumerable<int> productIds, int defaultId)
    {
      return NewProductViewList(productIds, defaultId, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
    }

    public virtual List<IProductView> NewProductViewList(IEnumerable<int> productIds, int defaultId, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      List<IProductView> resultProductViewList = new List<IProductView>();

      bool defaultSet = false;
      foreach (int productId in productIds)
      {
        bool isDefault = (productId == defaultId);
        resultProductViewList.Add(new ProductView(_currency.Value, GetProduct(productId), isDefault, priceRoundingMethod, savingsRoundingMethod));
        if (isDefault)
        {
          defaultSet = true;
        }
      }

      if (!defaultSet && (resultProductViewList.Count > 0))
      {
        resultProductViewList[0].IsDefault = true;
      }

      return resultProductViewList;
    }

    public virtual Dictionary<T, IProductView> NewProductViewDictionary<T>(IList<T> keys, IList<int> productIds, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      Dictionary<T, IProductView> resultProductViewDictionary = null;
      if (keys.Count == productIds.Count)
      {
        resultProductViewDictionary = new Dictionary<T, IProductView>();
        for (int i = 0; i < keys.Count; i++)
        {
          resultProductViewDictionary.Add(keys[i], new ProductView(_currency.Value, GetProduct(productIds[i]), false, priceRoundingMethod, savingsRoundingMethod));
        }
      }
      return resultProductViewDictionary;
    }

    public IProductView NewProductView(IProduct product, bool isDefault, int quantity, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      IProductView result = new ProductView(_currency.Value, product, isDefault, quantity, priceRoundingMethod, savingsRoundingMethod);
      return result;
    }

    public IProductView NewProductView(IProduct product)
    {
      IProductView result = new ProductView(_currency.Value, product, false, 1, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
      return result;
    }

    public IProductView NewProductView(IProduct product, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      IProductView result = new ProductView(_currency.Value, product, false, 1, priceRoundingMethod, savingsRoundingMethod);
      return result;
    }

 
    #endregion

    public bool IsProductGroupOffered(int productGroupType)
    {
      var productGroupOfferedHandler = ProductGroupOfferedFactory.GetHandler(Container, productGroupType);
      return productGroupOfferedHandler.IsProductGroupOffered(productGroupType, _productGroupsOffered.Value);
    }

    public int GetUnifiedProductIdByPfid(int pfid)
    {
      var request = new UnifiedProductIdRequestData(pfid, _siteContext.Value.PrivateLabelId);
      var response = (UnifiedProductIdResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.UnifiedProductId);
      return response.UnifiedProductId;
    }

    public int GetNonUnifiedPfid(int unifiedProductId)
    {
      var request = new NonUnifiedPfidRequestData(unifiedProductId, _siteContext.Value.PrivateLabelId);
      var response = (NonUnifiedPfidResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.NonUnifiedProductId);
      return response.NonUnifiedPfid;
    }
  }
}