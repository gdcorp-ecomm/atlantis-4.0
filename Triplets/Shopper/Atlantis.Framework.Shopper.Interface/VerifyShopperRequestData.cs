using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VerifyShopperRequestData : RequestData
  {
    public VerifyShopperRequestData(string shopperId)
    {
      ShopperID = shopperId;
    }
  }
}
