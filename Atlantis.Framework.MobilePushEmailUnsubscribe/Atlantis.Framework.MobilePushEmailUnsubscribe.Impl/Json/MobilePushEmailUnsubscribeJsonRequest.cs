using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.MobilePushEmailUnsubscribe.Impl.Json
{
/*
Unsubscribe an Email Address from RIM

 @param RequestObj $aRequest   Standard inbound request object
   REQUIRED OBJECT PROPERTIES:
       RequestObj->Id                          The Authentication ID used to identify the caller
       RequestObj->Token                       The Token/Password associated with the supplied Authentication ID
       RequestObj->Parameters->emailaddress    The email address being unsubscribed from RIM
       RequestObj->Parameters->subscription    The 'X-Subscription-Id' value from the original request headers from the customer

   CONTITIONAL OBJECT PROPERTIES:
       None of the RequestObj properties are conditional for this web service

   OPTIONAL OBJECT PROPERTIES:
       RequestObj->Parameters->subaccount      The Shopper ID of the customer sub account being queried (overrides the shopper)
       RequestObj->Parameters->mobile          Flag to force "mobile" bit in EmailAuth.UserInfo table
                                               1 = Set mobile bit ON
                                               0 = Set mobile bit OFF
                                               Not provided = Ignored

   IGNORED OBJECT PROPERTIES:
       RequestObj->Parameters->shopper         The Shopper ID of the customer account being queried
       RequestObj->Parameters->reseller        The Reseller ID associated to the Shopper ID
       RequestObj->Return->PageNumber
       RequestObj->Return->ResultsPerPage
       RequestObj->Return->OrderBy
       RequestObj->Return->SortOrder

*/
  [DataContract]
  public class MobilePushEmailUnsubscribeJsonRequest
  {
    [DataMember(Name = "emailaddress")]
    public String EmailAddress { get; set; }

    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }

    [DataMember(Name = "subscription")]
    public string SubscriptionId { get; set; }

    [DataMember(Name = "mobile")]
    public string IsMobile { get; set; }
  }
}