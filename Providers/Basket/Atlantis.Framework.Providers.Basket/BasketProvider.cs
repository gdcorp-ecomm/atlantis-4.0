using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System;

namespace Atlantis.Framework.Providers.Basket
{
  public abstract class BasketProvider : ProviderBase, IBasketProvider
  {
    private Lazy<ISiteContext> _siteContext;

    public BasketProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    protected ISiteContext SiteContext
    {
      get
      {
        return _siteContext.Value;
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

    public IBasketResponse PostToBasket(IBasketAddRequest basketAddRequest)
    {
      throw new NotImplementedException();
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
  }
}
