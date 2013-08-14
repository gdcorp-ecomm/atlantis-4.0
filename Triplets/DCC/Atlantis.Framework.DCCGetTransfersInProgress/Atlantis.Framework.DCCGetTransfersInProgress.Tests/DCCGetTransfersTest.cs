using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCGetTransfersInProgress.Interface;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DCCGetTransfersTest
  {
    public TestContext TestContext { get; set; }

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
    [DeploymentItem("Atlantis.Framework.DCCGetTransfersInProgress.Impl.dll")]
    public void GetTransfers()
    {

      var request = new DCCGetTransfersInProgressRequestData("856907", 5, "sthota");
      var response = (DCCGetTransfersInProgressResponseData)Engine.Engine.ProcessRequest(request, 445);

      string xml = response.ResponseXml;
      DomainTransferCollection xferCol = response.TransferCollection;
      if (xferCol != null)
      {
        List<DomainTransfer> tList = xferCol.DomainTransferList;
        string domainName = tList[0].DomainName;
        int a = xferCol.ResultCount;
      }
      Assert.IsTrue(response.IsSuccess);
      
    }
  }
}
