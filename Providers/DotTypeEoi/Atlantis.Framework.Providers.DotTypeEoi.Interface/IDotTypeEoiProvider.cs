﻿namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(out IDotTypeEoiResponse dotTypeEoi);
    bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse);
  }
}