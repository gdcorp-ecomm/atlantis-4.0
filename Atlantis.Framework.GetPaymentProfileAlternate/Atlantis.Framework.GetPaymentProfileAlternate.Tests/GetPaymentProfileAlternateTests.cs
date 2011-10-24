using Atlantis.Framework.GetPaymentProfileAlternate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetPaymentProfileAlternate.Tests
{
  [TestClass]
  public class GetPaymentProfileAlternateTests
  {
    public GetPaymentProfileAlternateTests()
    {
    }

    private string validGoDaddyShopperIdWithAlternatePaymentProfile = "853516";
    private string validGoDaddyShopperIdWithoutAlternatePaymentProfile = "863301";
    private int engineRequestId = 381;

    public TestContext TestContext { get; set; }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestExistingAlternatePaymentProfile()
    {
      var request = new GetPaymentProfileAlternateRequestData(validGoDaddyShopperIdWithAlternatePaymentProfile, string.Empty, string.Empty, string.Empty, 0);
      var response = (GetPaymentProfileAlternateResponseData) Engine.Engine.ProcessRequest(request, engineRequestId);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.PaymentProfileId > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMissingAlternatePaymentProfile()
    {
      var request = new GetPaymentProfileAlternateRequestData(validGoDaddyShopperIdWithoutAlternatePaymentProfile, string.Empty, string.Empty, string.Empty, 0);
      var response = (GetPaymentProfileAlternateResponseData)Engine.Engine.ProcessRequest(request, engineRequestId);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsFalse(response.PaymentProfileId > 0);
    }
  }
}
