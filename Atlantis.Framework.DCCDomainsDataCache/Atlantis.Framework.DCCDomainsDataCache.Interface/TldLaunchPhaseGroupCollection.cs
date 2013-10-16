using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLaunchPhaseGroupCollection : ITLDLaunchPhaseGroupCollection
  {
    private static readonly IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> _emptyCollection = new Dictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup>(0);

    private readonly IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> _launchPhaseGroupDictionary;

    private TldLaunchPhaseGroupCollection()
    {
      _launchPhaseGroupDictionary = _emptyCollection;
    }

    private TldLaunchPhaseGroupCollection(IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> launchPhaseGroupDictionary)
    {
      _launchPhaseGroupDictionary = launchPhaseGroupDictionary ?? _emptyCollection;
    }

    public static ITLDLaunchPhaseGroupCollection EmptyCollection()
    {
      return new TldLaunchPhaseGroupCollection();
    }

    public static ITLDLaunchPhaseGroupCollection CreateCollection(IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> launchPhaseGroupDictionary)
    {
      return new TldLaunchPhaseGroupCollection(launchPhaseGroupDictionary);
    }

    public bool TryGetGroup(LaunchPhaseGroupTypes groupType, out ITLDLaunchPhaseGroup group)
    {
      return _launchPhaseGroupDictionary.TryGetValue(groupType, out group);
    }

    public int Count
    {
      get { return _launchPhaseGroupDictionary.Count; }
    }
  }
}
