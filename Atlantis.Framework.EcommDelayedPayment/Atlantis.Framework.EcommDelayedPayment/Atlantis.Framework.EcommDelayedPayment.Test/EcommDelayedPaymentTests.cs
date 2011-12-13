using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommDelayedPayment.Impl;
using Atlantis.Framework.EcommDelayedPayment.Interface;


namespace Atlantis.Framework.EcommDelayedPayment.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetEcommDelayedPaymentTests
  {

    private const string _shopperId = "";


    public GetEcommDelayedPaymentTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EcommDelayedPaymentTest()
    {
      EcommDelayedPaymentRequestData request = new EcommDelayedPaymentRequestData("75866", string.Empty, "373735", string.Empty, 0, "https://cart.test.godaddy-com.ide/NetGiroPaymentReturn.aspx", "AliPay");
      EcommDelayedPaymentResponseData response = (EcommDelayedPaymentResponseData)Engine.Engine.ProcessRequest(request, 432);
      System.Diagnostics.Debug.WriteLine(response.InvoiceID);
      System.Diagnostics.Debug.WriteLine(response.RedirectURL);
      // Cache call
      //EcommDelayedPaymentResponseData response = (EcommDelayedPaymentResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
