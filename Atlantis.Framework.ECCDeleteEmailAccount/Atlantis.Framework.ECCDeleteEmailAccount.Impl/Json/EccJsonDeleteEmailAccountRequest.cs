using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCSetEmailAccount.Impl.Json
{

  [DataContract]
  public class EccJsonDeleteEmailAccountRequest
  {    
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }
  
    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }  
  }
}
