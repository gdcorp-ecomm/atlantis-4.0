using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.LogOfferClick.Interface;

namespace Atlantis.Framework.LogOfferClick.Tests
{
  /// <summary>
  /// Summary description for LogOfferClickTests
  /// </summary>
  [TestClass]
  public class LogOfferClickTests
  {
    private const string _shopperID = "842749";
    private const string _fbiOfferID = "23";
    private const string _visitGuid = "";
    private DateTime _clickDate = DateTime.Now;
    private const short _applicationID = 18;
    private const int _logOfferClickRequest = 90;

    public LogOfferClickTests()
    {
    }

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
    public void WebServiceResponseTest()
    {
      LogOfferClickRequestData _request = new LogOfferClickRequestData(_shopperID, string.Empty, string.Empty, string.Empty, 0, _fbiOfferID, _visitGuid, _clickDate, _applicationID);
      LogOfferClickResponseData _response = (LogOfferClickResponseData)Engine.Engine.ProcessRequest(_request, _logOfferClickRequest);
      Assert.IsTrue(_response.IsSuccess);
    }
  }
}
