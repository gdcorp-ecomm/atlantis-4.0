using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  // ReSharper disable InconsistentNaming
  public interface ITLDPhase
  // ReSharper restore InconsistentNaming
  {
    Dictionary<string, ITLDLaunchPhase> GetActiveClientRequestPhases();
    ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase);
    bool HasPreRegPhases { get; }
  }
}