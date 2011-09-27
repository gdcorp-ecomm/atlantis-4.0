using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;
using System.Text;

namespace Atlantis.Framework.EccGetEmailAddressInfo.Impl.Json
{
  [DataContract(Namespace = "")]
  public class EccJsonGetEmailAccountInfoRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "type")]
    private string email_type { get; set; }
    public EmailTypes Type {
      get { return (EmailTypes) Enum.Parse(typeof (EmailTypes), email_type); }
      set { email_type = ((int)value).ToString(); }
    }

    [DataMember]
    private int active { get; set; }
    public bool IncludeActiveOnly {
      get
      {
        bool parse;
        bool.TryParse(active.ToString(), out parse);
        return parse;
      }
      set { active = (value ? 1 : 0); }
    }

    [DataMember]
    private int dynamic_data { get; set; }
    public bool IncludeDynamicData {
      get
      {
        bool parse;
        bool.TryParse(dynamic_data.ToString(), out parse);
        return parse;
      }
      set { dynamic_data = (value ? 1 : 0); }
    }

    [DataMember(Name = "subaccount")]
    public string Subaccount{ get; set; }
  }
}
