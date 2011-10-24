using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Atlantis.Framework.DomainCheck.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainCheck.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DomainCheckTests
  {
    public DomainCheckTests()
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
    public void DomainCheckBasicTest()
    {
      DomainToCheck domain = new DomainToCheck("miccobluered7.com", false);
      DomainCheckRequestData request = new DomainCheckRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domain, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);

      DomainCheckResponseData response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainCheckTimeoutTest()
    {
      List<DomainToCheck> domains = new List<DomainToCheck>();
      domains.Add(new DomainToCheck("micco.com", false));
      domains.Add(new DomainToCheck("micco1.com", false));
      domains.Add(new DomainToCheck("micco2.com", false));
      domains.Add(new DomainToCheck("micco3.com", false));
      domains.Add(new DomainToCheck("micco4.com", false));

      DomainCheckRequestData request = new DomainCheckRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domains, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);

      DomainCheckResponseData response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsFalse(response.IsSuccess);
      Assert.AreEqual(response.ServiceExceptionStatus, WebExceptionStatus.Timeout);

    }

    private volatile bool _asyncSearchComplete = false;
    private DomainCheckResponseData _asyncResponse = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainCheckAsyncTest()
    {
      _asyncResponse = null;

      List<DomainToCheck> domains = new List<DomainToCheck>();
      domains.Add(new DomainToCheck("micco.com", false));
      domains.Add(new DomainToCheck("micco1.com", false));
      domains.Add(new DomainToCheck("micco2.com", false));
      domains.Add(new DomainToCheck("micco3.com", false));
      domains.Add(new DomainToCheck("micco4.com", false));

      DomainCheckRequestData request = new DomainCheckRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domains, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(request, 21, EndDomainCheckAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndDomainCheckAsyncTest(IAsyncResult result)
    {
      _asyncResponse = Engine.Engine.EndProcessRequest(result) as DomainCheckResponseData;
      _asyncSearchComplete = true;
    }
  }
}
