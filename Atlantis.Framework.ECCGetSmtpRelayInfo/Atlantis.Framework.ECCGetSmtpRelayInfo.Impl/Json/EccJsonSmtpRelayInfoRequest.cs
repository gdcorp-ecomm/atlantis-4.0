using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetSmtpRelayInfo.Impl.Json
{ 
  [DataContract]
  public class EccJsonSmtpRelayInfoRequest
  {
      [DataMember(Name = "shopper")]
      public string ShopperId { get; set; }

      [DataMember(Name = "reseller")]
      public string ResellerId { get; set; }

      [DataMember(Name = "type")]
      public string EmailType { get; set; }
    }
}
