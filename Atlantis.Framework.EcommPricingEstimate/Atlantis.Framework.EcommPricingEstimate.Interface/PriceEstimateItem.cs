using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.EcommPricingEstimate.Interface
{
  public class PriceEstimateItem
  {
    public PriceEstimateItem(int Pf_ID, string Discount_Code)
    {
      pf_id = Pf_ID;
      discount_code = Discount_Code;
    }

    public int pf_id { get; set; }
    public string discount_code { get; set; }
    public string name { get; set; }
    public int list_price { get; set; }
    public int adjusted_price { get; set; }
    public int icann_fee { get; set; }
  }
}
