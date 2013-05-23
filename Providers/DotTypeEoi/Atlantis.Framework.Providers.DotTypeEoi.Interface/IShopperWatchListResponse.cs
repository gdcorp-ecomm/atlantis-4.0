using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IShopperWatchListResponse
  {
    IList<IDotTypeEoiGtld> Gtlds { get; set; }
  }
}
