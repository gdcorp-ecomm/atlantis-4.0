using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  public interface ITldAvailability
  {
    bool HasLeafPage { get; set; }

    bool IsVisibleInDomainSpins { get; set; }

    string Name { get; set; }

    IList<ITldPhase> TldPhases { get; set; }
  }
}
