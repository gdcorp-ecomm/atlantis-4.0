using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IGeneralEoiData
  {
    string DisplayTime { get; }
    IList<IDotTypeEoiGtld> Gtlds { get; }
    int TotalPages { get; }
  }
}
