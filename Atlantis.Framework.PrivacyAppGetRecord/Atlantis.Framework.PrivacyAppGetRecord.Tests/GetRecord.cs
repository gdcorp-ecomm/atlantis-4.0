﻿using Atlantis.Framework.PrivacyAppGetRecord.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PrivacyAppGetRecord.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetRecord
  {
    public GetRecord()
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
    public void GetRecordTest()
    {
      PrivacyAppGetRecordRequestData request = new PrivacyAppGetRecordRequestData(
        "850774", string.Empty, string.Empty, string.Empty, 0);
      request.HashKey = "wabesctdufbgbdsdzbdjmjjhwfgbkhmcdfthjdmadgrfdjujeeehfguesjbhrfqh";
      request.ApplicationId = 7;

      PrivacyAppGetRecordResponseData response = (PrivacyAppGetRecordResponseData)Engine.Engine.ProcessRequest(request, 207);
      string test = response.ResponseXML;
      Assert.IsTrue(response.IsValid);

    }
  }
}
