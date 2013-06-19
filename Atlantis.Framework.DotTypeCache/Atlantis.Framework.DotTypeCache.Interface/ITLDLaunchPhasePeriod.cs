using System;

namespace Atlantis.Framework.DotTypeCache.Interface
{
// ReSharper disable InconsistentNaming
  public interface ITLDLaunchPhasePeriod
// ReSharper restore InconsistentNaming
  {
    string Type { get; }
    DateTime StartDate { get; }
    DateTime StopDate { get; }
    bool AvailCheck { get; }
  }
}
