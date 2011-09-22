using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetCalendarDetails.Impl.Json
{
  [DataContract]
  public class ECCJsonGetCalendarDetailsRequest
  {
    [DataMember(Name="shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name="reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name="active")]
    private int active { get; set; }

    public bool IncludeActiveOnly
    {
      get 
      { 
        bool parse;
        bool.TryParse(active.ToString(), out parse);
        return parse;
      }
      set { active = (value ? 1 : 0); }
    }

    [DataMember(Name="emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }
  }
}
