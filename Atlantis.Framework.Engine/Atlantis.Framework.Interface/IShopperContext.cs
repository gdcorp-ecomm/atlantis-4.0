
namespace Atlantis.Framework.Interface
{
  public interface IShopperContext
  {
    string ShopperId { get; }
    ShopperStatusType ShopperStatus { get; }
    int ShopperPriceType { get; }

    void ClearShopper();
    bool SetLoggedInShopper(string shopperId);
    bool SetLoggedInShopperWithCookieOverride(string shopperId);
    void SetNewShopper(string shopperId);
    
  }
}
