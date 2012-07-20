using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.AddOrderLevelPromoPL.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      AddOrderLevelPromoPLTest test = new AddOrderLevelPromoPLTest();
      test.CreateOrderLevelPromoPL_Success();

      test.CreateOrderLevelPromoPL_ExpiredEndDate();

      test.CreateOrderLevelPromoPL_PromoCodeInUse();

      test.CreateOrderLevelPromoPL_PromoDollarAwardGreaterThanAllowed();

      test.CreateOrderLevelPromoPL_PromoInvalidCurrency();

      test.CreateOrderLevelPromoPL_PromoPctAwardGreaterThanAllowed();
    }
  }
}
