using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DataProvider.Interface;

namespace Atlantis.Framework.DataProvider.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DataProviderTests
  {
    public DataProviderTests()
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
    [DeploymentItem("DataProvider.xml")]
    public void DataProviderProcBasic()
    {
      Dictionary<string, object> parameters = new Dictionary<string, object>(1);
      //parameters["shopper_id"] = "832652";
      parameters["url"] = string.Empty;
      parameters["displayName"] = "h45yehy";
      parameters["title"] = "jryjrer";
      parameters["category"] = 0;
      parameters["description"] = string.Empty;
      parameters["screenshotSelection"] = 0;
      parameters["agreement"] = 1;
      parameters["shopper_id"] = "832652";
      parameters["gdshop_socialMediaVideoApprovalStatusID"] = 1;
      parameters["gdshop_socialMediaVideoID"] = 0;
      parameters["gdshop_socialMediaCompetitionID"] = 1;
      DataProviderRequestData request = new DataProviderRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, "sb10videocontestinsert", parameters);
      DataProviderResponseData response = (DataProviderResponseData)Engine.Engine.ProcessRequest(request, 35);
      object result = response.GetResponseObject();
      Assert.IsNotNull(result);
    }
  }
}
