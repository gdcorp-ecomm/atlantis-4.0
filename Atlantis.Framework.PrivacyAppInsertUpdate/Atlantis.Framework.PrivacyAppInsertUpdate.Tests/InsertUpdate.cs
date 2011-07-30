using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.PrivacyAppInsertUpdate.Interface;

namespace Atlantis.Framework.PrivacyAppInsertUpdate.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class InsertUpdate
  {
    public InsertUpdate()
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
    public void InsertUpdateTest()
    {
      PrivacyAppInsertUpdateRequestData request = new PrivacyAppInsertUpdateRequestData(
        "850774", string.Empty, string.Empty, string.Empty, 0);

      request.PrivacyXML = "<gdPrivacyApp applicationID=\"10\"><firstName value=\"sri\"/></gdPrivacyApp>";

      //invalid schema test
      //request.PrivacyXML = "<gdPrivacyApp applicationID=\"10\"><firstName value=\"sri\"/><gdg>dfdf</gdg></gdPrivacyApp>"; 

      PrivacyAppInsertUpdateResponseData response = (PrivacyAppInsertUpdateResponseData)Engine.Engine.ProcessRequest(request, 221);
      int result = response.Result;
      string bstrOutput = response.OutputValue;

      bool hasErr = response.HasError;
      string errtxt = response.ErrorDescription;
      int statusid = response.StatusID;
      string hash = response.Hash;

      Assert.IsFalse(hasErr);

    }
  }
}
