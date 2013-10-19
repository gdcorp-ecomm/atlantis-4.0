using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  public abstract class BasketProvider : ProviderBase, IBasketProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<IShopperDataProvider> _shopperDataProvider; 

    public BasketProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _shopperDataProvider = new Lazy<IShopperDataProvider>(() => Container.Resolve<IShopperDataProvider>());
    }

    protected ISiteContext SiteContext
    {
      get
      {
        return _siteContext.Value;
      }
    }

    protected IShopperContext ShopperContext
    {
      get
      {
        return _shopperContext.Value;
      }
    }

    protected IShopperDataProvider ShopperData
    {
      get
      {
        return _shopperDataProvider.Value;
      }
    }

    public IBasketAddRequest NewAddRequest()
    {
      var result = new BasketAddRequest();
      SetInitialAddRequestProperties(result);
      return result;
    }

    protected virtual void SetInitialAddRequestProperties(IBasketAddRequest basketAddRequest)
    {
      basketAddRequest.ISC = SiteContext.ISC;
    }

    public IBasketAddItem NewBasketAddItem(int unifiedProductId, string itemTrackingCode)
    {
      var result = new BasketAddItem(Container, unifiedProductId, itemTrackingCode);
      SetInitialAddItemProperties(result);
      return result;
    }

    protected virtual void SetInitialAddItemProperties(IBasketAddItem basketAddItem)
    {
    }

    public IBasketResponse PostToBasket(IBasketAddRequest basketAddRequest)
    {
      if (basketAddRequest == null)
      {
        throw new ArgumentException("IBasketAddRequest cannot be null.");
      }

      try
      {
        VerifyShopperForCart();

        var request = BasketAddRequestBuilder.FromBasketAddRequest(ShopperContext.ShopperId, basketAddRequest);
        var response = (BasketAddResponseData) Engine.Engine.ProcessRequest(request, BasketEngineRequests.AddItems);

        return ProviderBasketResponse.FromBasketResponseStatus(response.Status);
      }
      catch (Exception ex)
      {
        return ProviderBasketResponse.FromException(ex);
      }
    }

    protected virtual void VerifyShopperForCart()
    {
      bool validShopper = false;
      if (!string.IsNullOrEmpty(ShopperContext.ShopperId))
      {
        validShopper = IsValidShopperId();
      }

      if (!validShopper)
      {
        CreateNewShopperForCart();
      }
    }

    private bool IsValidShopperId()
    {
      bool result = false;
      try
      {
        result = ShopperData.IsShopperValid();
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("BasketProvider.IsValidShopperId", 0, ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    private void CreateNewShopperForCart()
    {
      try
      {
        ShopperData.TryCreateNewShopper();
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("BasketProvider.CreateNewShopperForCart", 0, ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(exception);
      }
    }



  }
}
