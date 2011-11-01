using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Interface
{
  public class DotTypeStackItem
  {
    public string PromoCode { get; set; }
    public string DotType { get; set; }
    public int Price { get; set; }
    public int StackId { get; set; }

    public DotTypeStackItem(string promoCode, string dotType, int price, int stackId)    
    {
      PromoCode = promoCode;
      DotType = dotType;
      Price = price;
      StackId = stackId;
    }
  }
}
