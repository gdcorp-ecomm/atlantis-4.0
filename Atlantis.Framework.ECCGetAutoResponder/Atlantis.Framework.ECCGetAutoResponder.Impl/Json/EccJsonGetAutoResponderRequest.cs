using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetAutoResponder.Impl.Json
{
  [DataContract]
  public class EccJsonGetAutoResponderRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }
  }
}
