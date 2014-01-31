using Atlantis.Framework.BonsaiRenewalsByProductId.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Tests
{
  [TestClass]
  public class BonsaiRenewalsByProductIdTests
  {
    public BonsaiRenewalsByProductIdTests()
    {
    }

    public TestContext TestContext { get; set; }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.BonsaiRenewalsByProductId.Impl.dll")]
    public void TestGetRenewals()
    {
      var requestData = new BonsaiRenewalsRequestData("840420", "http://localhost", string.Empty, string.Empty, 0, 2712, 1);
      var responseData = (BonsaiRenewalsResponseData) Engine.Engine.ProcessRequest(requestData, 403);

      Assert.IsNull(responseData.AtlantisException);
      Assert.IsNotNull(responseData.RenewalOptions);
      Assert.IsTrue(responseData.RenewalOptions.Count > 0);
    }
  }
}