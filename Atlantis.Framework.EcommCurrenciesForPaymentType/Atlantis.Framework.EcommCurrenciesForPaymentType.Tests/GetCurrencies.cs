using System.Collections.Generic;
using Atlantis.Framework.EcommCurrenciesForPaymentType.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommCurrenciesForPaymentType.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetCurrencies
  {
    public GetCurrencies()
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
    public void TestMethod1()
    {
      EcommCurrenciesForPaymentTypeRequestData request = new EcommCurrenciesForPaymentTypeRequestData("850774", string.Empty, string.Empty, string.Empty, 0);
      //request.BasketType = "gdshop"; //default value "gdshop". See method description onhover
      //request.PaymentType = "credit_card"; //default value credit_card. See method description onhover
      request.PaymentSubType = "visa";

      EcommCurrenciesForPaymentTypeResponseData response = (EcommCurrenciesForPaymentTypeResponseData)Engine.Engine.ProcessRequest(request, 553);

      List<string> currencyList = new List<string>();
      currencyList = response.AvaiblableCurrencyList;

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
