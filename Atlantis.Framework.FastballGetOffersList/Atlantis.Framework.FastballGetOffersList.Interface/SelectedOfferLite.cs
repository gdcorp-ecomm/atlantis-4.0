using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class SelectedOfferLite
  {
    public string fbiOfferId { get; set; }
    public string DiscountType { get; set; }
    public string FastballDiscount { get; set; }
    public string FastballOrderDiscount { get; set; }
    public string ProductGroupCode { get; set; }
    public DateTime TargetDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsActive
    {
      get { return DateTime.Now >= TargetDate && DateTime.Now <= EndDate; }
    }

    public static bool TryParse(XElement xmlElement, out SelectedOfferLite selectedOffer)
    {
      bool result = false;
      selectedOffer = null;
      


      return result;
    }
  }
}
