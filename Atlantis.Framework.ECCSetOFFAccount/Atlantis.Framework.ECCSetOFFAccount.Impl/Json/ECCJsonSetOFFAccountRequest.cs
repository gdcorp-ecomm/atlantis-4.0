using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atlantis.Framework.ECCSetOFFAccount.Impl.Json
{
  [DataContract(Namespace = "")]
  public class ECCJsonSetOFFAccountRequest
  {
    /// <summary>
    /// The OFF Account user. If a value is provided for this parameter, the result will include only the OFF Account identified by this user name.
    /// </summary>
    [DataMember(Name = "user")]
    public string Username { get; set; }

    [DataMember(Name = "password")]
    public string Password { get; set; }

    [DataMember(Name = "uid")]
    private string accountUid { get; set; }
    public Guid AccountUid { 
      get { return new Guid(accountUid);}
      set { accountUid = value.ToString(); }
    }

    /// <summary>
    /// If the email address parameter is not supplied, no change to the current email address association will occur for an existing OFF 
    /// If the email address parameter is supplied, but empty; the OFF account will have no email associated.
    /// If the email address parameter is supplied, it cannot be assigned to any other OFF account.
    /// </summary>
    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

  }
}
