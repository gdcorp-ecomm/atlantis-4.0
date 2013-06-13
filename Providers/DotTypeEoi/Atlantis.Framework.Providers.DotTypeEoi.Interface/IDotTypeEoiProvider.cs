using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(string languageCode, out IGeneralEoiData generalEoiData);
    bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, string languageCode, out IGeneralGtldData generalGtldData);
    bool GetGeneralEoiCategoryList(string languageCode, out IList<ICategoryData> categoryList);
    bool SearchEoi(string searchString, string languageCode, out IGeneralGtldData generalGtldData);
    bool GetShopperWatchList(string languageCode, out IShopperWatchListResponse shopperWatchListResponse);
    bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, string languageCode, out string responseMessage);
    bool RemoveFromShopperWatchList(IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
  }
}
