using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl.Json
{
  [DataContract]
  public class MobilePushEmailGetSubscriptionJsonRequest
  {
    [DataMember(Name = "emailaddress")]
    public String EmailAddress { get; set; }

    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }

    [DataMember(Name = "subid")]
    public string SubscriptionId { get; set; }
  }
}
