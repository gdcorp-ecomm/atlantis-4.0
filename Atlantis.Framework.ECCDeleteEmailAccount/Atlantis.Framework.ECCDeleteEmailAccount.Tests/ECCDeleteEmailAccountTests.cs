using System;
using System.Diagnostics;
using Atlantis.Framework.ECCSetEmailAccount.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCSetEmailAccount.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Deletes or sets the test context which provides
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
    [DeploymentItem("app.config")]
    public void EccDeleteEmailAccountForShopperTest()
    {
      string shopperId = "858421"; //"85408"; // //"858965"; 

      int requestType = 247;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new ECCDeleteEmailAccountRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1
                                                                                );


     
      ((ECCDeleteEmailAccountRequestData)requestData).EmailAddress = "kklink@asdfasdfasdf.com";
      ((ECCDeleteEmailAccountRequestData)requestData).RequestTimeout = new TimeSpan(0, 0, 0, 10);
      ((ECCDeleteEmailAccountRequestData)requestData).ResellerId = 1;
      ((ECCDeleteEmailAccountRequestData)requestData).Subaccount = "";

      try
      {
        var deleteEmailAccountResponseData = (ECCDeleteEmailAccountResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Debug.WriteLine(deleteEmailAccountResponseData.ToXML());
        Assert.IsTrue(deleteEmailAccountResponseData.IsSuccess);

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

  }
}
