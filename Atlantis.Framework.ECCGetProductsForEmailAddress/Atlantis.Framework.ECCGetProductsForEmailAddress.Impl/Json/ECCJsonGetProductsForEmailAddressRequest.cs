using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetProductsForEmailAddress.Impl.Json
{
  [DataContract]
  public class ECCJsonGetProductsForEmailAddressRequest
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
