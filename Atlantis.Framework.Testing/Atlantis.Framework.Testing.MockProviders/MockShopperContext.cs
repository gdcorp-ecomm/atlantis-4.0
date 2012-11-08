using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockShopperContext : ProviderBase, IShopperContext
  {
    public MockShopperContext(IProviderContainer container)
      : base(container)
    {
    }

    private string _shopperId = string.Empty;
    private ShopperStatusType _shopperStatus = ShopperStatusType.Public;
    private int _shopperPriceType = 0;

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return _shopperStatus; }
    }

    public int ShopperPriceType
    {
      get { return _shopperPriceType; }
    }

    public void ClearShopper()
    {
      _shopperPriceType = 0;
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
