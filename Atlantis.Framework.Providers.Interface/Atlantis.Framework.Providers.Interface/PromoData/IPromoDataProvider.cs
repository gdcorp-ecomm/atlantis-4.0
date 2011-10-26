namespace Atlantis.Framework.Providers.Interface.PromoData
{
  public interface IPromoDataProvider
  {
    bool HasPromoCodes { get; }
    void AddPromoCode(string promoCode, string promoType);
    int GetPromoPrice(int productId, string currencyType, out string awardType);
    IPromoData GetProductPromoData();
    bool GetCartItemPromoAttributes(int productId, string currencyType, out string cartItemAttr, out string cartItemAttrValue);
  }
}
