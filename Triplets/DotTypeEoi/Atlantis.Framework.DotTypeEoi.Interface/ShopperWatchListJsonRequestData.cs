using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class ShopperWatchListJsonRequestData : RequestData
  {
    public ShopperWatchListJsonRequestData()
    {
    }

    public override string GetCacheMD5()
    {
      return "ShopperWatchListJsonRequestData";
    }
  }
}
