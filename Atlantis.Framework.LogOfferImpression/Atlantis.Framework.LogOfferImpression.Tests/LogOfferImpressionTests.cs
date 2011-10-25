using System;
using Atlantis.Framework.LogOfferImpression.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.LogOfferImpression.Tests
{
  /// <summary>
  /// Summary description for LogOfferImpressionTests
  /// </summary>
  [TestClass]
  public class LogOfferImpressionTests
  {
    private const string _shopperID = "842749";
    private const string _fbiOfferIdList = "23";
    private const string _visitGuid = "";
    private DateTime _impressionDate = DateTime.Now;
    private const short _applicationID = 18;
    private const int _logOfferImpressionRequest = 91;

    public LogOfferImpressionTests()
    {
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
    public void TestMethod1()
    {
      LogOfferImpressionRequestData _request = new LogOfferImpressionRequestData(_shopperID, string.Empty, string.Empty, string.Empty, 0, _fbiOfferIdList, _applicationID, _impressionDate, _visitGuid);
      LogOfferImpressionResponseData _response = (LogOfferImpressionResponseData)Engine.Engine.ProcessRequest(_request, _logOfferImpressionRequest);
      Assert.IsTrue(_response.IsSuccess);

    }
  }
}
