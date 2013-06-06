using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, string languageCode, out IGeneralEoiData generalEoiData);
    bool GetGeneralEoiCategoryList(string languageCode, out IList<ICategoryData> categoryList);
    bool GetShopperWatchList(string languageCode, out IShopperWatchListResponse shopperWatchListResponse);
    bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, string languageCode, out string responseMessage);
    bool RemoveFromShopperWatchList(IList<IDotTypeEoiGtld> gTlds, out string responseMessage);
  }
}
