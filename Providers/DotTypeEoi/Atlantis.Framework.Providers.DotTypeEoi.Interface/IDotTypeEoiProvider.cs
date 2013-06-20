using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(out IGeneralEoiData generalEoiData);

    bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, out IGeneralGtldData generalGtldData);

    bool GetGeneralEoiCategoryList(out IList<ICategoryData> categoryList);

    bool SearchEoi(string searchString, out IGeneralGtldData generalGtldData);

    bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse);

    bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, out string responseMessage);

    bool RemoveFromShopperWatchList(IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
  }
}
