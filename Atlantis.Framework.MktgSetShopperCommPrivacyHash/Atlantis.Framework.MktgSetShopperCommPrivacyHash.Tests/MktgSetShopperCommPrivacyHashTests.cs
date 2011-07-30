using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MktgSetShopperCommPrivacyHash.Interface;

namespace Atlantis.Framework.MktgSetShopperCommPrivacyHash.Tests
{
  [TestClass]
  public class MktgSetShopperCommPrivacyHashTests
  {
    [TestMethod]
    public void TestMethod1()
    {
      MktgSetShopperCommPrivacyHashRequestData request =
          new MktgSetShopperCommPrivacyHashRequestData("75866", string.Empty,string.Empty, string.Empty, 0, 3, "311234fasder3");
      MktgSetShopperCommPrivacyHashResponseData response = (MktgSetShopperCommPrivacyHashResponseData)Engine.Engine.ProcessRequest(request, 338);
      Assert.AreEqual(response.IsSuccess, true);

    }
  }
}
