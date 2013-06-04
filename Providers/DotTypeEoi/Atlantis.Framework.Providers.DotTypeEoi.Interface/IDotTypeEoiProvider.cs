using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, out string displayTime, out IList<IDotTypeEoiGtld> gTlds, out int totalPages);
    bool GetGeneralEoiCategoryList(out Dictionary<int, string> categoryList);
    bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse);
    bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
    bool RemoveFromShopperWatchList(IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
  }
}
