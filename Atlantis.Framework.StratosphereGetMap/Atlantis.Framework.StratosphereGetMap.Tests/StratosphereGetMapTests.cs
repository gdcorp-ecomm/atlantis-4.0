using System;
using System.Diagnostics;
using System.Threading;
using Atlantis.Framework.StratosphereGetMap.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.StratosphereGetMap.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetStratosphereGetMapTests
  {

    private const string _shopperId = "83439";  //DEV: 856907  TEST: 83439
    private const int _requestType = 237;
    private const int _asyncRequestType = 241;

    public GetStratosphereGetMapTests()
    { }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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
    public void StratosphereGetShopperMapTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      StratosphereGetMapRequestData request = new StratosphereGetMapRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "shopper"
         , null
         , 3);

      StratosphereGetMapResponseData response = SessionCache.SessionCache.GetProcessRequest<StratosphereGetMapResponseData>(request, _requestType);
        
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void StratosphereGetDomainMapTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      StratosphereGetMapRequestData request = new StratosphereGetMapRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "domain"
         , "intrepidkjs.com"
         , null);

      StratosphereGetMapResponseData response = SessionCache.SessionCache.GetProcessRequest<StratosphereGetMapResponseData>(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    private volatile bool _asyncSearchComplete = false;
    private StratosphereGetMapResponseData _asyncResponse = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void StratosphereGetDomainMapAsyncTest()
    {
      _asyncResponse = null;
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      StratosphereGetMapRequestData request = new StratosphereGetMapRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "domain"
         , "intrepidkjs.com"
         , null);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(request, _asyncRequestType, EndStratosphereGetMapAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Debug.WriteLine(_asyncResponse.ToXML());
      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndStratosphereGetMapAsyncTest(IAsyncResult result)
    {
      _asyncResponse = (StratosphereGetMapResponseData)Engine.Engine.EndProcessRequest(result);
      _asyncSearchComplete = true;
    }

  }
}
