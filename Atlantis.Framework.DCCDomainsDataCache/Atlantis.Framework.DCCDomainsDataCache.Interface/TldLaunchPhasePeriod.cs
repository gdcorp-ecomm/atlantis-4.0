using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLaunchPhasePeriod : ITLDLaunchPhasePeriod
  {
    public string Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StopDate { get; set; }
    public bool AvailCheck { get; set; }
  }
}
