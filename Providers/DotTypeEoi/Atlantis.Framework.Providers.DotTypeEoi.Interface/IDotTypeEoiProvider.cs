using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(out IDotTypeEoiResponse dotTypeEoi);
    bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse);
    bool AddToShopperWatchList(IList<string> gTldIds, out string responseMessage);
    bool RemoveFromShopperWatchList(IList<string> gTldIds, out string responseMessage);
  }
}
