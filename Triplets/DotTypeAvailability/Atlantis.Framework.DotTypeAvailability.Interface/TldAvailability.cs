using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  public class TldAvailability : ITldAvailability
  {
    public bool HasLeafPage { get; set; }

    public bool IsVisibleInDomainSpins { get; set; }

    public string Name { get; set; }

    public IList<ITldPhase> TldPhases { get; set; }
  }
}
