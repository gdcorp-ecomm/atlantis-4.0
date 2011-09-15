using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccSmtpRelayInfo
  {
    [DataMember(Name = "AccountUid")]
    public string AccountUid { get; set; }
    public Guid AccountGuid { get { return new Guid(AccountUid); } }

    [DataMember]
    public string ProductName { get; set; }
    public EccAccountType ProductType
    {
      get { return (EccAccountType)Enum.Parse(typeof(EccAccountType), ProductName); }
    }

    [DataMember]
    public string Status { get; set; }
    public EccAccountStatus AccountStatus
    {
      get { return (EccAccountStatus)Enum.Parse(typeof(EccAccountStatus), Status); }
    }

    [DataMember(Name = "ExpireDate")]
    public string ExpireDate { get; set; }
    public DateTime ExpirationDate
    {
      get
      {
        DateTime expDate;
        DateTime.TryParse(ExpireDate, out expDate);

        return expDate;
      }
    }

  }
}
