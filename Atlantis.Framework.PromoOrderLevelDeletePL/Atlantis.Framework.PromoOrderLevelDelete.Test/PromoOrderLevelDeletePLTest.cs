using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.PromoOrderLevelDeletePL.Interface;
using Atlantis.Framework.PromoOrderLevelDeletePL.Impl;

namespace Atlantis.Framework.PromoOrderLevelDelete.Test
{
  [TestClass]
  public class PromoOrderLevelDeletePLTest
  {
    [DeploymentItem("atlantis.config")]
    [TestMethod]
    public void TestPromoOrderLevelDeletePL()
    {
      PromoOrderLevelDeletePLRequestData request = new PromoOrderLevelDeletePLRequestData(string.Empty, string.Empty, 0, "NEEL2343", 4103309);
      PromoOrderLevelDeletePLResponseData response = Engine.Engine.ProcessRequest(request, 582) as PromoOrderLevelDeletePLResponseData;
      Assert.IsTrue(response.IsSuccess() == true);
    }
  }
}
