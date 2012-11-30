using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommPrepPayPalExpRedirect.Impl;
using Atlantis.Framework.EcommPrepPayPalExpRedirect.Interface;


namespace Atlantis.Framework.EcommPrepPayPalExpRedirect.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.EcommPrepPayPalExpRedirect.Impl.dll")]
  public class EcommPrepPayPalExpRedirectTests
  {

    private const string _shopperId = "861126";
    public TestContext TestContext { get; set; }

    public EcommPrepPayPalExpRedirectTests()
    {
    }


    [TestMethod]
    public void EcommDelayedPaymentTest()
    {
      var request = new EcommPrepPayPalExpRedirectRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 1, 
          "https://cart.test.godaddy-com.ide/NetGiroPaymentReturn.aspx", "https://cart.test.godaddy-com.ide/NetGiroPaymentCancel.aspx",
          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty );
      
      var response = (EcommPrepPayPalExpRedirectResponseData)Engine.Engine.ProcessRequest(request, 624);
      Debug.WriteLine(response.RedirectURL);
      Debug.WriteLine(response.ErrorDesciption);
      Debug.WriteLine(response.Token);

      // Cache call
      //EcommPrepPayPalExpRedirectResponseData response = (EcommPrepPayPalExpRedirectResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(string.IsNullOrEmpty(response.ErrorDesciption));
    }
  }
}
