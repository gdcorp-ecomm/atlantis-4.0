using System;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDLaunchPhasePeriod
  {
    DateTime StartDateUtc { get; }

    DateTime StopDateUtc { get; }

    bool IsActive { get; }
  }
}