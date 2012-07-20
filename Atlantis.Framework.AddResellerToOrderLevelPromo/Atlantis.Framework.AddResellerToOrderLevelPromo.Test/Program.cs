using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CreatePLOrderLevelPromo.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      CreatePLOrderLevelPromoTest test = new CreatePLOrderLevelPromoTest();
      test.CreatePLOrderLevelPromoSingleReseller_Success();

      test.CreatePLOrderLevelPromoSingleReseller_FailedPromoDoesntExist();
    }
  }
}
