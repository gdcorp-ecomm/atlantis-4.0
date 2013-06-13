using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IGeneralGtldData
  {
    string DisplayTime { get; }
    IList<IDotTypeEoiGtld> Gtlds { get; }
    int TotalPages { get; }
  }
}
