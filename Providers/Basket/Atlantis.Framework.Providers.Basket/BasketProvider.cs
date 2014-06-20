using System.Linq;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  public abstract class BasketProvider: ProviderBase, IBasketProvider
  {
    private readonly ISiteContext _siteContext;
    private readonly IShopperContext _shopperContext;
    private readonly IShopperDataProvider _shopperDataProvider;
    private readonly IBasketDeleteRequestBuilder _basketDeleteRequestBuilder;

    protected BasketProvider(IProviderContainer container) : base(container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _shopperContext = container.Resolve<IShopperContext>();
      _shopperDataProvider = container.Resolve<IShopperDataProvider>();
    }

    protected BasketProvider(ISiteContext siteContext, IShopperContext shopperContext,
      IShopperDataProvider shopperDataProvider) : this(siteContext, shopperContext, shopperDataProvider, null)
    {
    }

    internal BasketProvider(ISiteContext siteContext, IShopperContext shopperContext,
      IShopperDataProvider shopperDataProvider, IBasketDeleteRequestBuilder basketDeleteRequestBuilder)
      : base(null)
    {
      _siteContext = siteContext;
      _shopperContext = shopperContext;
      _shopperDataProvider = shopperDataProvider;
      _basketDeleteRequestBuilder = basketDeleteRequestBuilder ?? new BasketDeleteRequestBuilder();
    }

    protected ISiteContext SiteContext
    {
      get
      {
        return _siteContext;
      }
    }

    protected IShopperContext ShopperContext
    {
      get
      {
        return _shopperContext;
      }
    }

    protected IShopperDataProvider ShopperData
    {
      get
      {
        return _shopperDataProvider;
      }
    }

    public IBasketAddRequest NewAddRequest()
    {
      var result = new BasketAddRequest();
      SetInitialAddRequestProperties(result);
      return result;
    }

    public IBasketDeleteRequest NewDeleteRequest()
    {
      return new BasketDeleteRequest();
    }

    protected virtual void SetInitialAddRequestProperties(IBasketAddRequest basketAddRequest)
    {
      basketAddRequest.ISC = SiteContext.ISC;
    }

    public IBasketResponse DeleteFromBasket(IBasketDeleteRequest deleteRequest)
    {
      if (deleteRequest == null)
      {
        throw new ArgumentException("deleteRequest is null");
      }

      if (deleteRequest.ItemsToDelete.Any())
      {
        if (string.IsNullOrEmpty(ShopperContext.ShopperId))
        {
          return CreateBasketResponse(new ArgumentException("Shopper id is null or empty"));
        }

        try
        {
          var request = _basketDeleteRequestBuilder.BuildRequestData(ShopperContext.ShopperId, deleteRequest);
          var response =
            (BasketDeleteResponseData) Engine.Engine.ProcessRequest(request, BasketEngineRequests.DeleteItem);

          return CreateBasketResponse(response.Status);
        }
        catch (Exception ex)
        {
          return CreateBasketResponse(ex);
        }
      }

      return ProviderBasketResponse.FromException(new ArgumentException("Items to delete in delete request is empty"));
    }

    protected virtual IBasketResponse CreateBasketResponse(BasketResponseStatus status)
    {
      return ProviderBasketResponse.FromBasketResponseStatus(status);
    }

    protected virtual IBasketResponse CreateBasketResponse(Exception exception)
    {
      return ProviderBasketResponse.FromException(exception);
    }

    public IBasketAddItem NewBasketAddItem(int unifiedProductId, string itemTrackingCode)
    {
      var result = new BasketAddItem(_siteContext, unifiedProductId, itemTrackingCode);
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

    private int? _totalItems;
    public int TotalItems
    {
      get
      {
        if (!_totalItems.HasValue)
        {
          _totalItems = LoadBasketItemCount();
        }
        return _totalItems.GetValueOrDefault();
      }
    }

    private int LoadBasketItemCount()
    {
      if (string.IsNullOrEmpty(ShopperContext.ShopperId))
      {
        return 0;
      }

      var request = new BasketItemCountRequestData(ShopperContext.ShopperId);
      try
      {
        var response = (BasketItemCountResponseData) Engine.Engine.ProcessRequest(request, BasketEngineRequests.ItemCount);
        return response.TotalItems;
      }
      catch // Engine will log
      {
        return 0;
      }
    }
  }
}
