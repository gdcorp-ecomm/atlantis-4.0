using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDPhase
  {
    bool IsPreRegPhaseActive { get; }

    IList<ITLDLaunchPhase> GetAllLaunchPhases(bool activeOnly = true);

    ITLDLaunchPhase GetLaunchPhase(string launchPhaseCode);
  }
}