using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.PromoOrderLevelAddPL.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      PromoOrderLevelAddPLTest test = new PromoOrderLevelAddPLTest();
      test.CreatePLOrderLevelPromoSingleReseller_Success();

      test.CreatePLOrderLevelPromoSingleReseller_FailedPromoDoesntExist();
    }
  }
}
