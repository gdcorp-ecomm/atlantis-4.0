using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IGeneralEoiData
  {
    string DisplayTime { get; }

    IList<IDotTypeEoiCategory> Categories { get; }
  }
}
