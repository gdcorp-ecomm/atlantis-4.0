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
    [DeploymentItem("Atlantis.Framework.EcommDelayedPayment.Interface.dll")]
    [DeploymentItem("Atlantis.Framework.EcommDelayedPayment.Impl.dll")]
    public void EcommDelayedPaymentTest()
    {
      EcommDelayedPaymentRequestData request = new EcommDelayedPaymentRequestData("75866", string.Empty, "443734", string.Empty, 0, "https://cart.test.godaddy-com.ide/NetGiroPaymentReturn.aspx", "AliPay");
      EcommDelayedPaymentResponseData response = (EcommDelayedPaymentResponseData)Engine.Engine.ProcessRequest(request, 432);

      request.Payments.CurrentCCPayment.AccountName = "srdttt";
      request.Payments.CurrentCCPayment.Amount = "5600";
      request.Payments.CurrentCCPayment.AccountNumber = "5105105105105100";
      request.Payments.CurrentCCPayment.CCType = "MasterCard";
      request.Payments.CurrentCCPayment.Cvv = string.Empty;
      request.Payments.CurrentCCPayment.IssuerId = "24";
      request.Payments.CurrentCCPayment.ExpMonth = "06";
      request.Payments.CurrentCCPayment.ExpYear = "2016";
      request.Payments.CurrentCCPayment.PredictedMerchantCountry = "US";
      request.Payments.CurrentCCPayment.CardSpecificGateWayId = "1";
      request.Payments.CurrentCCPayment.RequireCvv = "0";

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

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("Atlantis.Framework.EcommDelayedPayment.Interface.dll")]
    //[DeploymentItem("Atlantis.Framework.EcommDelayedPayment.Impl.dll")]
    //public void EcommDelayedPaymentIndiaTest()
    //{
    //  EcommDelayedPaymentRequestData request = new EcommDelayedPaymentRequestData("850774", string.Empty, "1548535", string.Empty, 0, "https://cart.dev-godaddy.com/Actions/DelayedPaymentReturn.aspx", "CCAvenue", "en-US");


    //  request.Payment.Order_Billing = "foreign";
    //  request.Payment.Order_Source = "Online";
    //  request.Payment.Remote_Addr = "127.0.0.1";
    //  request.Payment.Remote_Host = "127.0.0.1";
    //  request.Payment.Pathway = "df53a795-2fd6-4917-97e8-bc0c43295455";
    //  request.Payment.TranslationLanguage = "en";
    //  request.Payment.CurrencyDisplay = "INR";
    //  request.Payment.ReqISCAmount = "0";

    //  request.Billing.City = "abc";
    //  request.Billing.Company = " ";
    //  request.Billing.Country = "in";
    //  request.Billing.Email = "sthota@godaddy.com";
    //  request.Billing.First_Name = "sss";
    //  request.Billing.Last_Name = "ttt";
    //  request.Billing.Phone2 = "+91.2354584524";
    //  request.Billing.State = "AN";
    //  request.Billing.Street1 = "abc lane";
    //  request.Billing.Zip = "56776";

    //  request.Payments.CurrentCCPayment.AccountName = "srdttt";
    //  request.Payments.CurrentCCPayment.Amount = "5600";
    //  request.Payments.CurrentCCPayment.AccountNumber = "5105105105105100";
    //  request.Payments.CurrentCCPayment.CCType = "MasterCard";
    //  request.Payments.CurrentCCPayment.Cvv = string.Empty;
    //  request.Payments.CurrentCCPayment.ExpMonth = "06";
    //  request.Payments.CurrentCCPayment.ExpYear = "2016";
    //  request.Payments.CurrentCCPayment.PredictedMerchantCountry = "US";
    //  request.Payments.CurrentCCPayment.RequireCvv = "0";


    //  EcommDelayedPaymentResponseData response = (EcommDelayedPaymentResponseData)Engine.Engine.ProcessRequest(request, 432);
    //  System.Diagnostics.Debug.WriteLine(response.InvoiceID);
    //  System.Diagnostics.Debug.WriteLine(response.RedirectURL);
    //  // Cache call
    //  //EcommDelayedPaymentResponseData response = (EcommDelayedPaymentResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

    //  //
    //  // TODO: Add test logic here
    //  //

    //  Debug.WriteLine(response.ToXML());
    //  Assert.IsTrue(response.IsSuccess);
    //}
  }
}
