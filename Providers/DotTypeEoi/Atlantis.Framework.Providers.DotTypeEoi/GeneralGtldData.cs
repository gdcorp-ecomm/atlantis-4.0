using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.Providers.DotTypeEoi
{
  class GeneralGtldData : IGeneralGtldData
  {
    public GeneralGtldData(string displayTime, IList<IDotTypeEoiGtld> gtlds, int totalPages)
    {
      DisplayTime = displayTime;
      Gtlds = gtlds;
      TotalPages = totalPages;
    }

    public string DisplayTime { get; set; }
    public IList<IDotTypeEoiGtld> Gtlds { get; set; }
    public int TotalPages { get; set; }
  }
}
