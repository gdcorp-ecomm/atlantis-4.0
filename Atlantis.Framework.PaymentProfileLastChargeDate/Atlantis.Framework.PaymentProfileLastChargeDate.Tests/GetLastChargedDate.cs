using Atlantis.Framework.PaymentProfileLastChargeDate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PaymentProfileLastChargeDate.Tests
{
  [TestClass]
  public class GetLastChargedDate
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    [DeploymentItem("Atlantis.Framework.PaymentProfileLastChargeDate.Impl.dll")]
    public void TestMethod1()
    {
      PaymentProfileLastChargeDateRequestData request = new PaymentProfileLastChargeDateRequestData("856907", string.Empty, string.Empty, string.Empty, 0, 58628);
      PaymentProfileLastChargeDateResponseData response =
        (PaymentProfileLastChargeDateResponseData)Engine.Engine.ProcessRequest(request, 642);

      Assert.IsTrue(response.IsSuccess);

    }
  }
}
