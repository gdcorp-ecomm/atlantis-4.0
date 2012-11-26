using Atlantis.Framework.Interface;
using System.Web;

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
      get
      {
        int result = 0;
        if (HttpContext.Current != null)
        {
          object priceType = HttpContext.Current.Items[MockShopperContextSettings.PriceType];
          if ((priceType != null) && (priceType is int))
          {
            result = (int)priceType;
          }
        }
        return result;
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
