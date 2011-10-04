using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetOFFAccountsForShopper.Impl.Json
{
  [DataContract(Namespace = "")]
  public class ECCJsonGetOFFAccountsForShopperRequest
  {
    /// <summary>
    /// The OFF Account user. If a value is provided for this parameter, the result will include only the OFF Account identified by this user name.
    /// </summary>
    [DataMember(Name = "user")]
    public string Username { get; set; }

    /// <summary>
    ///  The email address associated to the account. If a value is provided for this parameter, the result will include only the OFF Account associated with this email address.
    /// </summary>
    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "active")]
    private string active { get; set; }
    public bool ActiveOnly
    {
      get
      {
        bool parse;
        bool.TryParse(active, out parse);
        return parse;
      }
      set { active = (value ? "1" : "0"); }
    }
  }
}
