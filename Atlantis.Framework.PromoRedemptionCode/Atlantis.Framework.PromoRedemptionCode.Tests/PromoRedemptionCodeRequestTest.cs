using System;
using Atlantis.Framework.PromoRedemptionCode.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PromoRedemptionCode.Tests
{
  [TestClass()]
  public class PromoRedemptionCodeRequestTest
  {
    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    /// <summary>
    ///A test for RequestHandler
    ///</summary>
    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.PromoRedemptionCode.Impl.dll")]
    public void Test1()
    {
      PromoRedemptionCodeRequestData request = new PromoRedemptionCodeRequestData("858346", string.Empty, string.Empty, string.Empty, 0, 2, "81014-18158-20715-42180");
      PromoRedemptionCodeResponseData response = (PromoRedemptionCodeResponseData)Engine.Engine.ProcessRequest(request, 633);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsNull(response.CodeStatus);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.PromoRedemptionCode.Impl.dll")]
    public void Test2()
    {
      PromoRedemptionCodeRequestData request = new PromoRedemptionCodeRequestData("858346", string.Empty, string.Empty, string.Empty, 0, 2, "81302-78086-78754-92096");
      PromoRedemptionCodeResponseData response = (PromoRedemptionCodeResponseData)Engine.Engine.ProcessRequest(request, 633);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsNotNull(response.CodeStatus);
      RedemptionCodeStatus testobject = response.CodeStatus;
      Assert.IsTrue(testobject.RedemptionCodeId > 0);
      Assert.IsTrue(testobject.PackageId > 0);
      Assert.AreNotEqual(testobject.CreateDate, new DateTime());
      Assert.IsTrue(testobject.CreateDate < DateTime.Now);
    }
  }
}
