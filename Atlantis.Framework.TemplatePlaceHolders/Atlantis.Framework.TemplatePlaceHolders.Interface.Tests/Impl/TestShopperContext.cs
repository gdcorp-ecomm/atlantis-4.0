using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  public class TestShopperContext : ProviderBase, IShopperContext
  {
    public string ShopperId { get { return "847235"; } }

    public ShopperStatusType ShopperStatus { get { return ShopperStatusType.Authenticated; } }

    public int ShopperPriceType { get { return 1; } }
    
    public TestShopperContext(IProviderContainer container) : base(container)
    {
    }

    public void ClearShopper()
    {
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      return true;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      return true;
    }

    public void SetNewShopper(string shopperId)
    {
    }
  }
}
