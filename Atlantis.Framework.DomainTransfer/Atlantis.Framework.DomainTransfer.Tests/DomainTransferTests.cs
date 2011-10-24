using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Atlantis.Framework.DomainTransfer.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainTransfer.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DomainTransferTests
  {
    public DomainTransferTests()
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
    public void DomainTransferBasicTest()
    {
      TransferToCheck domain = new TransferToCheck("test.com", false);
      DomainTransferRequestData request = new DomainTransferRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domain, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);

      DomainTransferResponseData response = (DomainTransferResponseData)Engine.Engine.ProcessRequest(request, 32);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainTransferTimeoutTest()
    {
      List<TransferToCheck> domains = new List<TransferToCheck>();
      domains.Add(new TransferToCheck("micco.com", false));
      domains.Add(new TransferToCheck("micco1.com", false));
      domains.Add(new TransferToCheck("micco2.com", false));
      domains.Add(new TransferToCheck("micco3.com", false));
      domains.Add(new TransferToCheck("micco4.com", false));

      DomainTransferRequestData request = new DomainTransferRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domains, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);

      DomainTransferResponseData response = (DomainTransferResponseData)Engine.Engine.ProcessRequest(request, 32);
      Assert.IsFalse(response.IsSuccess);
      Assert.AreEqual(response.ServiceExceptionStatus, WebExceptionStatus.Timeout);

    }

    private volatile bool _asyncSearchComplete = false;
    private DomainTransferResponseData _asyncResponse = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainTransferAsyncTest()
    {
      _asyncResponse = null;

      List<TransferToCheck> domains = new List<TransferToCheck>();
      domains.Add(new TransferToCheck("micco.com", false));
      domains.Add(new TransferToCheck("micco1.com", false));
      domains.Add(new TransferToCheck("micco2.com", false));
      domains.Add(new TransferToCheck("micco3.com", false));
      domains.Add(new TransferToCheck("micco4.com", false));

      DomainTransferRequestData request = new DomainTransferRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domains, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(request, 33, EndDomainCheckAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndDomainCheckAsyncTest(IAsyncResult result)
    {
      _asyncResponse = Engine.Engine.EndProcessRequest(result) as DomainTransferResponseData;
      _asyncSearchComplete = true;
    }
  }
}
