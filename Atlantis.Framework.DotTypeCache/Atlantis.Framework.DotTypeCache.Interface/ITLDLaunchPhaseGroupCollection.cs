namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDLaunchPhaseGroupCollection
  {
    bool TryGetGroup(LaunchPhaseGroupTypes groupType, out ITLDLaunchPhaseGroup group);

    int Count { get; }
  }
}
