using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IShopperWatchListResponse
  {
    IEnumerable<IDotTypeEoiGtld> Gtlds { get; }

    IDictionary<int, IDotTypeEoiGtld> GtldIdDictionary { get; }

    int GtldCount { get; }
  }
}
