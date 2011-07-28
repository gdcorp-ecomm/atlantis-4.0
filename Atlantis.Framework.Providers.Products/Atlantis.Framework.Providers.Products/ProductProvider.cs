using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductOffer.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using System.Web;

namespace Atlantis.Framework.Providers.Products
{
  /// <summary>
  /// Product Provider.  A page should only have one of these, and pass it to any user controls that need it.
  /// It provides page cached- product classes, so expensive calls are minimized to once per page.
  /// </summary>
  public class ProductProvider : ProviderBase, IProductProvider
  {
    private const string GET_UNIFIED_ID_BY_PFID_REQUEST_FORMAT = "<GetUnifiedIDByPFID><param name=\"n_pf_id\" value=\"{0}\"/><param name=\"n_privateLabelID\" value=\"{1}\"/></GetUnifiedIDByPFID>";
    
    private Dictionary<int, IProduct> _products;

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get
      {
        if(_siteContext == null)
        {
          _siteContext = Container.Resolve<ISiteContext>();
        }

        return _siteContext;
      }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = Container.Resolve<IShopperContext>();
        }

        return _shopperContext;
      }
    }

    private ICurrencyProvider _currencyProvider;
    private ICurrencyProvider CurrencyProvider
    {
      get
      {
        if (_currencyProvider == null)
        {
          _currencyProvider = Container.Resolve<ICurrencyProvider>();
        }

        return _currencyProvider;
      }
    }

    private Dictionary<string, int> _pfidToUnifiedProductIdLookup;
    private Dictionary<string, int> PfidToUnifiedProductIdLookup
    {
      get
      {
        if (_pfidToUnifiedProductIdLookup == null)
        {
          _pfidToUnifiedProductIdLookup = new Dictionary<string, int>();
        }
        return _pfidToUnifiedProductIdLookup;
      }
    }
    
    public ProductProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
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
        result = new Product(productId, SiteContext.PrivateLabelId, CurrencyProvider);
        _products[productId] = result;
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
        resultProductViewList.Add(new ProductView(GetProduct(productId), false, priceRoundingMethod, savingsRoundingMethod));
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
        resultProductViewList.Add(new ProductView(GetProduct(productId), isDefault, priceRoundingMethod, savingsRoundingMethod));
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
          resultProductViewDictionary.Add(keys[i], new ProductView(GetProduct(productIds[i]), false, priceRoundingMethod, savingsRoundingMethod));
        }
      }
      return resultProductViewDictionary;
    }

    public IProductView NewProductView(IProduct product, bool isDefault, int quantity, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      IProductView result = new ProductView(product, isDefault, quantity, priceRoundingMethod, savingsRoundingMethod);
      return result;
    }

    public IProductView NewProductView(IProduct product)
    {
      IProductView result = new ProductView(product, false, 1, PriceRoundingType.RoundFractionsUpProperly, SavingsRoundingType.FloorSavingsProperly);
      return result;
    }

    public IProductView NewProductView(IProduct product, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      IProductView result = new ProductView(product, false, 1, priceRoundingMethod, savingsRoundingMethod);
      return result;
    }

 
    #endregion

    #region Products Offered

    public bool IsProductGroupOffered(int productGroupType)
    {
      return ProductGroupsOffered.ContainsKey(productGroupType);
    }

    private Dictionary<int, string> _productsGroupsOffered;
    private Dictionary<int, string> ProductGroupsOffered
    {
      get
      {
        if (_productsGroupsOffered == null)
        {
          _productsGroupsOffered = GetOfferedProducts();
        }
        return _productsGroupsOffered;
      }
    }

    private string RequestUrl
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.RawUrl;
        }
        return result;
      }
    }

    private Dictionary<int, string> GetOfferedProducts()
    {
      ProductOfferRequestData request = new ProductOfferRequestData(ShopperContext.ShopperId, 
                                                                    RequestUrl, 
                                                                    string.Empty,
                                                                    SiteContext.Pathway, 
                                                                    SiteContext.PageCount, 
                                                                    SiteContext.PrivateLabelId);

      ProductOfferResponseData response = (ProductOfferResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.ProductOffer);

      return response.ProductOfferings;
    }

    #endregion

    #region UnifiedProductId Lookup

    private int GetUnifiedProductIdByPfidFromDataCache(int pfid, int privateLabelId)
    {
      int unifiedProductId = int.MinValue;
      string requestXml = string.Format(GET_UNIFIED_ID_BY_PFID_REQUEST_FORMAT, pfid, privateLabelId);

      try
      {
        string resultXml = DataCache.DataCache.GetCacheData(requestXml);

        if (!string.IsNullOrEmpty(resultXml))
        {
          XDocument xdoc = XDocument.Parse(resultXml);

          string resultProductIdString = (from item in xdoc.Descendants("item")
                                          select item.Attribute("gdshop_product_unifiedProductID").Value).FirstOrDefault();

          if (!string.IsNullOrEmpty(resultProductIdString))
          {
            if (!int.TryParse(resultProductIdString, out unifiedProductId))
            {
              unifiedProductId = int.MinValue;
            }
            else
            {
              PfidToUnifiedProductIdLookup.Add(GetUnifiedProductIdLookupKey(pfid, privateLabelId), unifiedProductId);
            }
          }
        }
      }
      catch(Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(
          "ProductProvider.GetUnifiedProductIdByPfidFromDataCache", "0", message, requestXml, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(aex);
      }
      
      return unifiedProductId;
    }

    private static string GetUnifiedProductIdLookupKey(int pfid, int privateLabelId)
    {
      return string.Concat(pfid.ToString(), "-", privateLabelId.ToString());
    }

    public int GetUnifiedProductIdByPfid(int pfid, int privateLabelId)
    {
      int result;

      string key = GetUnifiedProductIdLookupKey(pfid, privateLabelId);
      if (PfidToUnifiedProductIdLookup.ContainsKey(key))
      {
        result = PfidToUnifiedProductIdLookup[key];
      }
      else
      {
        result = GetUnifiedProductIdByPfidFromDataCache(pfid, privateLabelId);
      }

      return result;
    }

    #endregion


  }
}