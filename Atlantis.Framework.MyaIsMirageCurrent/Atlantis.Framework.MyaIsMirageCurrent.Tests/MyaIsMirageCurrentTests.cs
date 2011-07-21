using Atlantis.Framework.MyaIsMirageCurrent.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaIsMirageCurrent.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MyaIsMirageCurrentTests
  {
    public MyaIsMirageCurrentTests()
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
    public void MyaIsMirageCurrentBasic()
    {
      MyaIsMirageCurrentRequestData request =
        new MyaIsMirageCurrentRequestData("859161", string.Empty, string.Empty, string.Empty, 0);
      MyaIsMirageCurrentResponseData response = (MyaIsMirageCurrentResponseData)Engine.Engine.ProcessRequest(request, 162);
      Assert.IsNull(response.RequestException);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MyaIsMirageCurrentRO()
    {
      MyaIsMirageCurrentRequestData request =
        new MyaIsMirageCurrentRequestData("859161", string.Empty, string.Empty, string.Empty, 0);
      MyaIsMirageCurrentResponseData response = (MyaIsMirageCurrentResponseData)Engine.Engine.ProcessRequest(request, 9162);
      Assert.IsNull(response.RequestException);
    }

  }
}
