using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLaunchPhasePeriod : ITLDLaunchPhasePeriod
  {
    public DateTime StartDateUtc { get; set; }

    public DateTime StopDateUtc { get; set; }

    public bool IsActive
    {
      get
      {
        DateTime utcDate = DateTime.UtcNow;
        return utcDate >= StartDateUtc && utcDate < StopDateUtc;
      }
    }
  }
}