using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Atlantis.Framework.Testing.MockHttpContext;
using System.Web;

namespace Atlantis.Framework.SessionCache.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class SessionCacheTests
  {
    public SessionCacheTests()
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

    [TestInitialize]
    public void InitializeFakeContext()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
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
    public void SessionCacheThirdCallFails()
    {
      HttpContext.Current.Session.Clear();
      TestTripletRequest.Reset();

      TestTripletRequestData request = new TestTripletRequestData(
        string.Empty, string.Empty, string.Empty, string.Empty, 0, 2);
      TestTripletResponseData response = SessionCache.GetProcessRequest<TestTripletResponseData>(request, 999, TimeSpan.FromSeconds(30));
      string firstResult = response.Result;

      response = SessionCache.GetProcessRequest<TestTripletResponseData>(request, 999, TimeSpan.FromSeconds(30));
      string secondResult = response.Result;
      Assert.AreEqual(firstResult, response.Result);

      Thread.Sleep(30000);
      response = SessionCache.GetProcessRequest<TestTripletResponseData>(request, 999, TimeSpan.FromSeconds(30));
      Assert.AreEqual(firstResult, response.Result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SessionCacheExpires()
    {
      HttpContext.Current.Session.Clear();
      TestTripletRequest.Reset();

      TestTripletRequestData request = new TestTripletRequestData(
        string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      TestTripletResponseData response = SessionCache.GetProcessRequest<TestTripletResponseData>(request, 999, TimeSpan.FromSeconds(30));
      string firstResult = response.Result;

      response = SessionCache.GetProcessRequest<TestTripletResponseData>(request, 999, TimeSpan.FromSeconds(30));
      Assert.AreEqual(firstResult, response.Result);

      Thread.Sleep(31000);
      response = SessionCache.GetProcessRequest<TestTripletResponseData>(request, 999, TimeSpan.FromSeconds(30));
      Assert.AreNotEqual(firstResult, response.Result);
    }

    [TestMethod]
    public void SessionCacheNonFrameworkBasic()
    {
      HttpContext.Current.Session.Clear();

      NonFrameworkTestItem itemIn = new NonFrameworkTestItem();
      itemIn.Name2 = "Last";
      itemIn.Name1 = "First";
      SessionCache.SaveToSession(itemIn, "NonFrameworkItem", TimeSpan.FromSeconds(30));

      bool isExpired = false;
      NonFrameworkTestItem itemOut = SessionCache.GetFromSession<NonFrameworkTestItem>("NonFrameworkItem", out isExpired);
      Assert.IsFalse(isExpired);
      Assert.AreEqual(itemIn.Name1, itemOut.Name1);

      Thread.Sleep(31000);
      NonFrameworkTestItem itemOut2 = SessionCache.GetFromSession<NonFrameworkTestItem>("NonFrameworkItem", out isExpired);
      Assert.IsTrue(isExpired);
      Assert.AreEqual(itemIn.Name1, itemOut2.Name1);

    }

  }
}
