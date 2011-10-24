using Atlantis.Framework.MyaMirageData.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Atlantis.Framework.MyaMirageData.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MyaMirageDataTests
  {
    public MyaMirageDataTests()
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
    public void MirageDataBasic()
    {
      MyaMirageDataRequestData request = new MyaMirageDataRequestData("856907", string.Empty, string.Empty, string.Empty, 0);
      MyaMirageDataResponseData response = (MyaMirageDataResponseData)Engine.Engine.ProcessRequest(request, 386);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MirageDataSession()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://blue.com/default.aspx", string.Empty);
      MyaMirageDataRequestData request = new MyaMirageDataRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      MyaMirageDataResponseData response = SessionCache.SessionCache.GetProcessRequest<MyaMirageDataResponseData>(request, 386);
      
      Assert.IsTrue(response.IsSuccess);
      int quantity = response.GetProductNamespaceTotal("photoalbum");

      response = SessionCache.SessionCache.GetProcessRequest<MyaMirageDataResponseData>(request, 386);
      Assert.AreEqual(quantity, response.GetProductNamespaceTotal("photoalbum"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void OwnedNamespaces()
    {
      MyaMirageDataRequestData request = new MyaMirageDataRequestData("856907", string.Empty, string.Empty, string.Empty, 0);
      MyaMirageDataResponseData response = (MyaMirageDataResponseData)Engine.Engine.ProcessRequest(request, 386);

      foreach (string namespacekey in response.GetOwnedNamespaceList())
      {
        Debug.WriteLine(namespacekey);
      }

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
