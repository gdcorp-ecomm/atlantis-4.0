using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.ECCGetEmailAddressesForDomain.Impl.Json
{
  [DataContract]
  public class ECCJsonGetEmailAddressesForDomainRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "domain")]
    public string DomainName { get; set; }

    [DataMember(Name = "type")]
    private string type { get; set; }
    public EmailTypes AccountType
    {
      get { return (EmailTypes)Enum.Parse(typeof(EmailTypes), this.type.ToString()); }
      set { type = value.ToString(); }
    }

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
