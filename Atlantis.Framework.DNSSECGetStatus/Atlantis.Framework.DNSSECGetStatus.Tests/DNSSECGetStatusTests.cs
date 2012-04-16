using System;
using System.Diagnostics;
using Atlantis.Framework.DNSSECGetStatus.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DNSSECGetStatus.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetDNSSECGetStatusTests
  {

    private const string _shopperId = "850659";  // DEV: 856907  TEST: 83439
    private const int _privateLabelId = 1;
    private const int _requestType = 280;


    public GetDNSSECGetStatusTests()
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
    public void DNSSECGetStatusTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      DNSSECGetStatusRequestData request = new DNSSECGetStatusRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _privateLabelId);

      request.RequestTimeout = TimeSpan.FromSeconds(30);
      DNSSECGetStatusResponseData response = SessionCache.SessionCache.GetProcessRequest<DNSSECGetStatusResponseData>(request, _requestType);
      DNSSECGetStatusResponseData response2 = SessionCache.SessionCache.GetProcessRequest<DNSSECGetStatusResponseData>(request, _requestType);
        
      Debug.WriteLine(string.Format("Used DNSSEC: {0}", response.UsedDnsSec));
      Debug.WriteLine(string.Format("Total DNSSEC: {0}", response.TotalDnsSec));
      
      Assert.IsTrue(response.TotalDnsSec == response2.TotalDnsSec);
    }
  }
}
