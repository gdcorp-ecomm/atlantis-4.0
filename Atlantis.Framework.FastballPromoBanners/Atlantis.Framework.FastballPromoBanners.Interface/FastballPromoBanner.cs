using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballPromoBanners.Interface
{
  [DataContract]
  public class FastballPromoBanner
  {
    [DataMember(Name = "ofid")]
    public string OfferId { get; set; }

    [DataMember(Name = "disc")]
    public string Discount { get; set; }

    [DataMember(Name = "odisc")]
    public string OrderDiscount { get; set; }

    public string Isc { get; set; }

    [DataMember(Name = "btext")]
    public string BannerText { get; set; }

    [DataMember(Name = "ci")]
    public string CiCode { get; set; }

    [DataMember(Name = "product")]
    public string Product { get; set; }

    [DataMember(Name = "start")]
    public DateTime StartDate { get; set; }

    [DataMember(Name = "end")]
    public DateTime EndDate { get; set; }
  }
}
