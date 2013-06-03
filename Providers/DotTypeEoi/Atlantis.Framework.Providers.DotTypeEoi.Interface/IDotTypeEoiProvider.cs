using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(out IDotTypeEoiResponse dotTypeEoi);
    bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse);
    bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
    bool RemoveFromShopperWatchList(IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
  }
}
