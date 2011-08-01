using Atlantis.Framework.EcommSwitchPaymentProfile.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommSwitchPaymentProfile.Tests
{
  [TestClass]
  public class SwitchPaymentProfileTests
  {
    public SwitchPaymentProfileTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SwitchResourceProfileValid()
    {
      //switch to profileid X
      var request = new EcommSwitchPaymentProfileRequestData("840420", "sourceUrl", "orderId", "pathway", 1, "418729", "virhost", "billing", 60814, "127.0.0.1");
      var response = Engine.Engine.ProcessRequest(request, 397) as EcommSwitchPaymentProfileResponseData;

      Assert.IsNotNull(response);
      Assert.IsTrue(response.Successful);

      //switch back to profileid Y
      var request2 = new EcommSwitchPaymentProfileRequestData("840420", "sourceUrl", "orderId", "pathway", 1, "418729", "virhost", "billing", 58071, "127.0.0.1");
      var response2 = Engine.Engine.ProcessRequest(request2, 397) as EcommSwitchPaymentProfileResponseData;

      Assert.IsNotNull(response2);
      Assert.IsTrue(response2.Successful);
    }
  }
}
