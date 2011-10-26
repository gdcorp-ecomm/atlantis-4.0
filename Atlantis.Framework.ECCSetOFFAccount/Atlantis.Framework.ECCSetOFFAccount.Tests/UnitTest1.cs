using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.ECCSetOFFAccount.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCSetOFFAccount.Tests
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
      string accountUid = "12b6982d-48c6-11df-b65b-005056956427";
      string password = string.Empty;
      string subaccount = string.Empty;
      string shopperId = "858421";
      string emailAddress = "distro@asdfasdfasdf.com";
      string username = "distro@asdfasdfasdf.com";
      int requestType = 268;
      ECCSetOFFAccountRequestData requestData = new ECCSetOFFAccountRequestData(shopperId,
        string.Empty,
        string.Empty, 
        string.Empty, 
        1, 
        new Guid(accountUid), 
        true, 
        password, 
        username, 
        emailAddress, 
        1, 
        subaccount);

      try
      {
        ECCSetOFFAccountResponseData responseData = (ECCSetOFFAccountResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

        if (!responseData.IsSuccess)
        {
          Debug.WriteLine(responseData.ToXML());
          Assert.IsTrue(!string.IsNullOrEmpty(responseData.Response.Item.Message),"Call successful with errors.");
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
