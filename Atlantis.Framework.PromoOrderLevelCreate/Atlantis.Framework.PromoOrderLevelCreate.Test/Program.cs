using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.PromoOrderLevelCreate.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      PromoOrderLevelCreateTest test = new PromoOrderLevelCreateTest();
      test.CreateOrderLevelPromoPL_Success();

      test.CreateOrderLevelPromoPL_ExpiredEndDate();

      test.CreateOrderLevelPromoPL_PromoCodeInUse();

      test.CreateOrderLevelPromoPL_PromoDollarAwardGreaterThanAllowed();

      test.CreateOrderLevelPromoPL_PromoInvalidCurrency();

      test.CreateOrderLevelPromoPL_PromoPctAwardGreaterThanAllowed();
    }
  }
}
