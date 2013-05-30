using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Impl.Json
{
  [DataContract]
  public class MobilePushEmailSubscribeJsonRequest
  {
    //  REQUIRED OBJECT PROPERTIES:
    //      RequestObj->Id                          The Authentication ID used to identify the caller
    //      RequestObj->Token                       The Token/Password associated with the supplied Authentication ID
    //      RequestObj->Parameters->emailaddress    The email address to be subscribed to RIM
    //      RequestObj->Parameters->callback        The 'X-Call-Back' value from the original request headers from the customer
    //      RequestObj->Parameters->notification    The 'X-Notification-Info' value from the original request headers from the customer

    //  CONTITIONAL OBJECT PROPERTIES:
    //      None of the RequestObj properties are conditionally required for this web service

    //  OPTIONAL OBJECT PROPERTIES:
    //      RequestObj->Parameters->subaccount      The Shopper ID of the customer sub account being queried (overrides the shopper)
    //      RequestObj->Parameters->mobile          Flag to force "mobile" bit in EmailAuth.UserInfo table
    //                                              1 = Set mobile bit ON
    //                                              0 = Set mobile bit OFF
    //                                              Not provided = Ignored

    [DataMember(Name = "emailaddress")]
    public String EmailAddress { get; set; }

    [DataMember(Name = "callback")]
    public string Callback { get; set; }

    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }

    [DataMember(Name = "notification")]
    public string Notification { get; set; }

    [DataMember(Name = "mobile")]
    public string IsMobile { get; set; }
  }
}
