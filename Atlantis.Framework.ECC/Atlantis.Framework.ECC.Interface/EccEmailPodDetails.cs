using System.Runtime.Serialization;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccEmailPodDetails
  {
    [DataMember(Name = "userId")]
    public string UserId { get; set; }

    [DataMember(Name = "deliveryMode")]
    public string DeliveryMode { get; set; }

    [DataMember(Name = "accountStatus")]
    public string AccountStatus { get; set; } 

    [DataMember(Name = "pod_id")]
    public string PodId { get; set; }

    [DataMember(Name = "pod_type")]
    public string PodType { get; set; }
  }
}
