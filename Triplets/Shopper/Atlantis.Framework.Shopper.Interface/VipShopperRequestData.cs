using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VipShopperRequestData : RequestData
  {
    public VipShopperRequestData(string shopperId)
    {
      ShopperID = shopperId ?? string.Empty;
    }

    public override string GetCacheMD5()
    {
      return ShopperID.ToLowerInvariant();
    }
  }
}
