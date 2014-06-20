using System;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [ExcludeFromCodeCoverage]
  public class TestBasketProvider : BasketProvider
  {
    public new ISiteContext SiteContext { get { return base.SiteContext; } }
    public new IShopperContext ShopperContext { get { return base.ShopperContext; } }
    public IShopperDataProvider ShopperDataProvider { get { return ShopperData; } }
    
    public TestBasketProvider(IProviderContainer container)
      : base(container)
    {
    }

    public TestBasketProvider(ISiteContext siteContext, IShopperContext shopperContext,
      IShopperDataProvider shopperDataProvider) : base(siteContext, shopperContext, shopperDataProvider)
    {
    }

    internal TestBasketProvider(ISiteContext siteContext, IShopperContext shopperContext,
      IShopperDataProvider shopperDataProvider, IBasketDeleteRequestBuilder basketDeleteRequestBuilder)
      : base(siteContext,
        shopperContext, shopperDataProvider, basketDeleteRequestBuilder)
    {
      
    }

    public new IBasketResponse CreateBasketResponse(BasketResponseStatus responseStatus)
    {
      return base.CreateBasketResponse(responseStatus);
    }

    public new IBasketResponse CreateBasketResponse(Exception exception)
    {
      return base.CreateBasketResponse(exception);
    }
  }
}
