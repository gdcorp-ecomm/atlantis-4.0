﻿using Atlantis.Framework.PrivacyAppInsertEmailAddress.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PrivacyAppInsertEmailAddress.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class InsertEmailAddress
  {
    public InsertEmailAddress()
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
    public void InsertEmailAddressTest()
    {
      PrivacyAppInsertEmailAddressRequestData request = new PrivacyAppInsertEmailAddressRequestData(
        "850774", string.Empty, string.Empty, string.Empty, 0);

      request.EmailAddress = "wgraham@godaddy.com";

      PrivacyAppInsertEmailAddressResponseData response = (PrivacyAppInsertEmailAddressResponseData)Engine.Engine.ProcessRequest(request, 232);
        
      int result = response.Result;
      string bstrOutput = response.ErrorValue;

      string hash = response.EmailHash;

      Assert.AreEqual(result, 1);

    }
  }
}
