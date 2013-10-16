using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLaunchPhaseGroup : ITLDLaunchPhaseGroup
  {
    private TldLaunchPhaseGroup()
    {
      Phases = new List<ITLDLaunchPhase>(16);  
    }

    public static ITLDLaunchPhaseGroup CreateEmptyGroup()
    {
      return new TldLaunchPhaseGroup();
    }

    public static ITLDLaunchPhaseGroup CreateGroup(ITLDLaunchPhase phase)
    {
      ITLDLaunchPhaseGroup tldLaunchPhaseGroup = new TldLaunchPhaseGroup();
      tldLaunchPhaseGroup.Phases.Add(phase);

      return tldLaunchPhaseGroup;
    }

    public IList<ITLDLaunchPhase> Phases { get; private set; }
  }
}
