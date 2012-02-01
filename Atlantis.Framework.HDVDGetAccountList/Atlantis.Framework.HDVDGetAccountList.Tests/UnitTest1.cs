using System;
using System.Diagnostics;
using Atlantis.Framework.HDVDGetAccountList.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDGetAccountList.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {

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
    public void GetAccountList()
    {
      const string APP_NAME = "HDVDGetAccountList_UNITTESTS";
      string _shopperId = "90611";
      int requestId = 400;
      HDVDGetAccountListRequestData request = new HDVDGetAccountListRequestData(
        _shopperId, 
        string.Empty, 
        string.Empty,
        string.Empty, 
        1,
        "all", 
        15, 
        1,
        string.Empty, 
        string.Empty, 
        string.Empty,
        APP_NAME
      );

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetAccountListResponseData response = Engine.Engine.ProcessRequest(request, requestId) as HDVDGetAccountListResponseData;
      Assert.IsTrue(response.IsSuccess);
      Debug.WriteLine(response.ToXML());

    }

  }
}
