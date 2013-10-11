using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  [DataContract]
  public class TldAvailability : ITldAvailability
  {
    [DataMember]
    public bool HasLeafPage { get; set; }

    [DataMember]
    public bool IsVisibleInDomainSpins { get; set; }

    [DataMember]
    public string TldName { get; set; }

    [DataMember]
    public string TldPunyCodeName { get; set; }

    [DataMember]
    public IList<ITldPhase> TldPhases { get; set; }
  }
}
