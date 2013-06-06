using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiResponse
  {
    string DisplayTime { get; set; }
    IList<IDotTypeEoiCategory> Categories { get; }
  }
}
