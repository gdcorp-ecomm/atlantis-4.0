using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.EcommPaymentProfile.Interface;

namespace Atlantis.Framework.EcommPaymentProfile.Tests
{
  [TestClass]
  public class EcommPaymentProfileTests
  {
    public EcommPaymentProfileTests()
    {}

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EcommPaymentProfileBasic()
    {
      const string shopperId = "840420";
      var request = new EcommPaymentProfileRequestData(
        shopperId, string.Empty, string.Empty, string.Empty, 0, 58071);
      var response = (EcommPaymentProfileResponseData)Engine.Engine.ProcessRequest(request, 379);
      Debug.WriteLine(string.Format("CardType: {0}", response.AccessProfile(shopperId, string.Empty, string.Empty, "EcommPaymentProfileBasic").CreditCardType));
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
