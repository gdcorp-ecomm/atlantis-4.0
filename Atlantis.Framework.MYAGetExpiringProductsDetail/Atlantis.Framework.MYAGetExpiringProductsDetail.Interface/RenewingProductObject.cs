using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.MYAGetExpiringProductsDetail.Interface
{
  [Serializable]
  [DataContract(Name = "renprodobj")]
  public class RenewingProductObject
  {
    [DataMember(Name = "id")]
    public int? Id { get; set; }

    [DataMember(Name = "brid")]
    public int? BillingResourceId { get; set; }

    [DataMember(Name = "desc")]
    public string Description { get; set; }

    [DataMember(Name = "pfid")]
    public int? PFID { get; set; }

    [DataMember(Name = "urenpid")]
    public int? UnifiedRenewalProductId { get; set; }

    [DataMember(Name = "upid")]
    public int? UnifiedProductID { get; set; }

    [DataMember(Name = "domid")]
    public int? DomainID { get; set; }

    [DataMember(Name = "cnam")]
    public string CommonName { get; set; }

    [DataMember(Name = "battmp")]
    public int? BillingAttempt { get; set; }

    [DataMember(Name = "dsync")]
    public bool? DontSync { get; set; }

    [DataMember(Name = "ns")]
    public string Namespace { get; set; }

    [DataMember(Name = "dispimg")]
    public bool? DisplayImageFlag { get; set; }

    [DataMember(Name = "isrnprclk")]
    public bool IsRenewalPriceLocked { get; set; }

    [DataMember(Name = "ishstprd")]
    public bool IsHostingProduct { get; set; }

    [DataMember(Name = "ispsdu")]
    public bool IsPastDue { get; set; }

    [DataMember(Name = "hsadn")]
    public bool? HasAddon { get; set; }

    [DataMember(Name = "rcrpmt")]
    public string RecurringPayment { get; set; }

    [DataMember(Name = "orgprc")]
    public int? OriginalListPrice { get; set; }

    [DataMember(Name = "ptyp")]
    public int? ProductTypeID { get; set; }

    [DataMember(Name = "actexp")]
    public DateTime? AccountExpirationDate { get; set; }

    [DataMember(Name = "atren")]
    public bool AutoRenewFlag { get; set; }
  }
}
