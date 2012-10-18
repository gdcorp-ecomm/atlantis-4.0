using Atlantis.Framework.LogDollarValueRestrictions.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.LogDollarValueRestrictions.Tests
{
  /// <summary>
  /// Summary description for LogDollarValueRestrictionsTests
  /// </summary>
  [TestClass]
  public class LogDollarValueRestrictionsTests
  {
    private const string _shopperID = "842749";
    private const int _logDollarValueRestrictionsRequest = 583;
    private const int _totalPrice = 50;
    private const bool _isManager = false;

    public LogDollarValueRestrictionsTests()
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
    [DeploymentItem("App.config")]
    public void TestMethod1()
    {
      LogDollarValueRestrictionsRequestData _request = new LogDollarValueRestrictionsRequestData(_shopperID, string.Empty, string.Empty, string.Empty, 0, _totalPrice, _isManager);
      LogDollarValueRestrictionsResponseData _response = (LogDollarValueRestrictionsResponseData)Engine.Engine.ProcessRequest(_request, _logDollarValueRestrictionsRequest);
      Assert.IsTrue(_response.IsSuccess);

    }
  }
}
