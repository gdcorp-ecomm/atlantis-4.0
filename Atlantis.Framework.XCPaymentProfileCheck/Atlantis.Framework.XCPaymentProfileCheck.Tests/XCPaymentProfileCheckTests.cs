using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.XCPaymentProfileCheck.Interface;


namespace Atlantis.Framework.XCPaymentProfileCheckCheck.Tests
{
  [TestClass]
  public class GetXCPaymentProfileCheckTests
  {

    private const string _shopperIdWithPP = "839409";
    private const string _shopperIdWithoutPP = "857517";
    private const int _paymentProfileCheckRequestType = 175;

    public GetXCPaymentProfileCheckTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void XCPaymentProfileCheckTestShopperWithPP()
    {
      var request = new XCPaymentProfileCheckRequestData(_shopperIdWithPP
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      var response = (XCPaymentProfileCheckResponseData)Engine.Engine.ProcessRequest(request, _paymentProfileCheckRequestType);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(string.Format("Shopper's Has Instant Purchase: {0}", response.HasInstantPurchasePayment));
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void XCPaymentProfileCheckTestShopperWithoutPP()
    {
      var request = new XCPaymentProfileCheckRequestData(_shopperIdWithoutPP
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      var response = (XCPaymentProfileCheckResponseData)Engine.Engine.ProcessRequest(request, _paymentProfileCheckRequestType);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(string.Format("Shopper's Has Instant Purchase: {0}", response.HasInstantPurchasePayment));
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
