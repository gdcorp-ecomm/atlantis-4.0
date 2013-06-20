using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.Providers.DotTypeEoi
{
  class GeneralEoiData : IGeneralEoiData
  {
    public GeneralEoiData(string displayTime, IList<IDotTypeEoiCategory> categories)
    {
      DisplayTime = displayTime;
      Categories = categories;
    }

    public string DisplayTime { get; set; }

    public IList<IDotTypeEoiCategory> Categories { get; set; }
  }
}
