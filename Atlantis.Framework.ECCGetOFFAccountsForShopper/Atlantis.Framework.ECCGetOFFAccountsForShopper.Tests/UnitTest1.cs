using System;
using System.Diagnostics;
using Atlantis.Framework.ECCGetOFFAccountsForShopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetOFFAccountsForShopper.Tests
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
    [DeploymentItem("App.config")]
    [DeploymentItem("Atlantis.config")]
    public void TestMethod1()
    {
      string shopperId = "858421";
      string emailAddress = "smoketest3@imkcars.com"; //null; //"distro@asdfasdfasdf.com";
      string username = "smoketest3@imkcars.com"; //string.Empty;
      int requestType = 269;
      ECCGetOFFAccountsForShopperRequestData requestData = new ECCGetOFFAccountsForShopperRequestData(shopperId,
                                                                                                      string.Empty,
                                                                                                      string.Empty,
                                                                                                      string.Empty,
                                                                                                      1,
                                                                                                      username,
                                                                                                      false,
                                                                                                      emailAddress,
                                                                                                      1);

      try
      {
        ECCGetOFFAccountsForShopperResponseData responseData = (ECCGetOFFAccountsForShopperResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsNotNull(responseData.ToString());
          Debug.WriteLine(responseData.ToXML());
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
