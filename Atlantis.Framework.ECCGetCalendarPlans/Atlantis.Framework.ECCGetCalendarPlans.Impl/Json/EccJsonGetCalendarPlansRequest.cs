using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetCalendarPlans.Impl.Json
{
  [DataContract]
  public class EccJsonGetCalendarPlansRequest
  {
    [DataMember(Name="shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name="reseller")]
    public string ResellerId { get; set; }
    
    [DataMember(Name="uid")]
    public string PlanUid { get; set; }

    [DataMember(Name="subaccount")]
    public string SubAccount { get; set; }
  }
}
