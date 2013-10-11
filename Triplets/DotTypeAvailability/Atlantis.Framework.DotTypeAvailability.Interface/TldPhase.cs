using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  [DataContract]
  public class TldPhase : ITldPhase
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public DateTime StartDate { get; set; }

    [DataMember]
    public DateTime StopDate { get; set; }
  }
}
