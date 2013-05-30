using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class PushSubscribeDetails
  {
    [DataMember(Name = "SubscriptionId")]
    public string SubscriptionId { get; set; }    
  }
}
