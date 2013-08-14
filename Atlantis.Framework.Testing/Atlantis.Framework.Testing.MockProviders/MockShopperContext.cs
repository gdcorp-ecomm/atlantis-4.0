using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockShopperContext : ProviderBase, IShopperContext
  {
    readonly Lazy<ISiteContext> _siteContext;

    public MockShopperContext(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    private string _shopperId = string.Empty;
    private ShopperStatusType _shopperStatus = ShopperStatusType.Public;

    public string ShopperId
    {
      get
      {
        if (_siteContext.Value.Manager != null && _siteContext.Value.Manager.IsManager)
        {
          return _siteContext.Value.Manager.ManagerShopperId;
        }
        return _shopperId;
      }
    }

    public ShopperStatusType ShopperStatus
    {
      get
      {
        if (_siteContext.Value.Manager.IsManager)
        {
          return ShopperStatusType.Manager;
        }

        return _shopperStatus;
      }
    }

    public int ShopperPriceType
    {
      get
      {
        return Container.GetData(MockShopperContextSettings.PriceType, 0);
      }
    }

    public void ClearShopper()
    {
      _shopperId = string.Empty;
      _shopperStatus = ShopperStatusType.Public;
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      _shopperId = shopperId;
      _shopperStatus = ShopperStatusType.Authenticated;
      return true;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      _shopperId = shopperId;
      _shopperStatus = ShopperStatusType.Authenticated;
      return true;
    }

    public void SetNewShopper(string shopperId)
    {
      _shopperId = shopperId;
      _shopperStatus = ShopperStatusType.Public;
    }
  }
}
