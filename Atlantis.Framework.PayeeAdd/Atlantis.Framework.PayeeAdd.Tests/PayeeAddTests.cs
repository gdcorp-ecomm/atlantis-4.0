using System.Diagnostics;
using Atlantis.Framework.PayeeAdd.Interface;
using Atlantis.Framework.PayeeProfileClass.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PayeeAdd.Tests
{
  [TestClass]
  public class GetPayeeAddTests
  {
    private const string _shopperId = "";
    private const int _requestType = 478;


    public GetPayeeAddTests()
    { }

    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    public void PayeeAddTest()
    {
      PayeeProfile payee = BuildPayee();

      PayeeAddRequestData request = new PayeeAddRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , payee);

      PayeeAddResponseData response = (PayeeAddResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(string.Format("CapID: {0}", response.CapId));
      Assert.IsTrue(response.IsSuccess);
    }

    #region BuildPayee
    private PayeeProfile BuildPayee()
    {
      PayeeProfile payee = new PayeeProfile();
      payee.FriendlyName = "PayeeACHTest2";
      payee.TaxDeclarationTypeID = "1";
      payee.TaxStatusTypeID = "1";
      payee.TaxStatusText = string.Empty;
      payee.TaxID = "123456789";
      payee.TaxIDTypeID = "1";
      payee.TaxExemptTypeID = "3";
      payee.TaxCertificationTypeID = "7";
      payee.SubmitterName = "Kent Searle";
      payee.SubmitterTitle = "";
      payee.PaymentMethodTypeID = "2";
      payee.ACH.AchBankName = "Kent Searle";
      payee.ACH.AchRTN = "155487006";
      payee.ACH.AccountNumber = "8227444441";
      payee.ACH.AccountOrganizationTypeID = "1";
      payee.ACH.AccountTypeID = "1";
      //payee.PayPal.PayPalEmail = "ksearle@godaddy.com";

      PayeeProfile.AddressClass w9 = new PayeeProfile.AddressClass();
      PayeeProfile.AddressClass payment = new PayeeProfile.AddressClass();

      w9.AddressType = "W9";
      w9.ContactName = "Joe Blow";
      w9.Address1 = "123 Main St";
      w9.Address2 = "Apt. 25B";
      w9.City = "Scottsdale";
      w9.StateOrProvince = "AZ";
      w9.PostalCode = "85260";
      w9.Country = "USA";
      w9.Phone1 = "480-505-8888";

      payment.AddressType = "Payment";
      payment.ContactName = "Jane Doe";
      payment.Address1 = "32123 Main St";
      payment.Address2 = string.Empty;
      payment.City = "Phoenix";
      payment.StateOrProvince = "AZ";
      payment.PostalCode = "85255";
      payment.Country = "USA";
      payment.Phone1 = "480-505-8888";
      payment.Phone2 = "480-255-9992";
      payment.Fax = "480-255-9993";

      payee.Address.Add(payment);
      payee.Address.Add(w9);

      return payee;
    }
    #endregion
  }
}
