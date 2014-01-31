using Atlantis.Framework.BonsaiRenewalsByProductId.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Tests
{
  [TestClass]
  public class BonsaiProductRenewalTests
  {
    public TestContext TestContext { get; set; }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.BonsaiRenewalsByProductId.Impl.dll")]
    public void TestGetProductRenewal()
    {
      var requestData = new BonsaiProductRenewalRequestData(2712, 1);
      var responseData = (BonsaiProductRenewalResponseData)Engine.Engine.ProcessRequest(requestData, 796);

      Assert.IsNull(responseData.AtlantisException);
      Assert.IsTrue(responseData.RenewalProductId == 2713);
    }
  }
}