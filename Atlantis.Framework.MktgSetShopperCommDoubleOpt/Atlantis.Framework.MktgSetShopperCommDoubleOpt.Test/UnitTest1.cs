using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MktgSetShopperCommDoubleOpt.Interface;

namespace Atlantis.Framework.MktgSetShopperCommDoubleOpt.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      MktgSetShopperCommDoubleOptRequestData request = new MktgSetShopperCommDoubleOptRequestData("850774", string.Empty, string.Empty, string.Empty,0,3);
      MktgSetShopperCommDoubleOptResponseData response = (MktgSetShopperCommDoubleOptResponseData)Engine.Engine.ProcessRequest(request, 341);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
