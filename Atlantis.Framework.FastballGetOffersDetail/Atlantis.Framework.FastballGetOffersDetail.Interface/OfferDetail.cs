using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class OfferDetail
  {
    public string fbiOfferId { get; set; }
    public string DiscountType { get; set; }
    public string FastballDiscount { get; set; }
    public string FastballOrderDiscount { get; set; }
    public string ProductGroupCode { get; set; }
    public DateTime TargetDate { get; set; }
    public DateTime EndDate { get; set; }

    public List<DataItem> DataItems { get; set; }
    public List<ProductGroupAttrib> ProductGroupAttributes { get; set; }
    
    public bool IsActive
    {
      get { return DateTime.Now >= TargetDate && DateTime.Now <= EndDate; }
    }

  }
}
