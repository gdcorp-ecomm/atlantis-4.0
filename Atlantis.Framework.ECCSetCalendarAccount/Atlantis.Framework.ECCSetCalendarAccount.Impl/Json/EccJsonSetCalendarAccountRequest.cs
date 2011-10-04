using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCSetCalendarAccount.Impl.Json
{
  [DataContract]
  public class EccJsonSetCalendarAccountRequest
  {
    [DataMember(Name="shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name="reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name="emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name="uid")]
    public string CalendarPlanUid { get; set; }

    [DataMember(Name="message")]
    public string InviteMessage { get; set; }

    [DataMember(Name="subaccount")]
    public string SubAccount { get; set; }
  }
}
