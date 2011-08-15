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
    public void TestGetRenewals()
    {
      var requestData = new BonsaiRenewalsRequestData("857527", "http://localhost", string.Empty, string.Empty, 0, 42114, 1);
      var responseData = (BonsaiRenewalsResponseData) Engine.Engine.ProcessRequest(requestData, 403);

      Assert.IsNull(responseData.AtlantisException);
      Assert.IsNotNull(responseData.RenewalOptions);
      Assert.IsTrue(responseData.RenewalOptions.Count > 0);
    }
  }
}
