using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDLaunchPhaseGroup
  {
    IList<ITLDLaunchPhase> Phases { get; } 
  }
}
