using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Domains.Interface
{
  public class LaunchPhaseItem
  {
    private LaunchPhaseItem(){}

    public LaunchPhases LaunchPhase { get; private set; }
    public int VendorCost { get; private set; }
    public int? TierId { get; private set; }

    public static LaunchPhaseItem Create(LaunchPhases launchPhase, int? tierId)
    {
      var launchPhaseItem = new LaunchPhaseItem
      {
        LaunchPhase = launchPhase,
        TierId = tierId
      };

      return launchPhaseItem;
    }
  }
}
