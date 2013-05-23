using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class ShopperWatchListJsonRequestData : RequestData
  {
    public ShopperWatchListJsonRequestData(string shopperId)
    {
      ShopperID = shopperId;
    }
  }
}
