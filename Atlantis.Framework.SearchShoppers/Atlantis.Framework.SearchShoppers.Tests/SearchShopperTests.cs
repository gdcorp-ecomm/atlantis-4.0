using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.SearchShoppers.Interface;

namespace Atlantis.Framework.SearchShoppers.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetShopperMarketingData
  {
    public GetShopperMarketingData()
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
    public void GetShopperMktgData()
    {

      SearchShoppersRequestData requestData = new SearchShoppersRequestData(
        "850774", string.Empty, string.Empty, string.Empty, 0, "172.19.72.107");
      requestData.IPAddress = "172.19.72.107";
      requestData.AddSearchField("email", "sthota@godaddy.com");
      requestData.AddSearchField("privateLabelID", "1");
      requestData.AddReturnField("mktg_email");
      requestData.AddReturnField("mktg_mail");
      requestData.AddReturnField("mktg_nonpromotional_notices");

      SearchShoppersResponseData response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(requestData, 2);


      //
      // TODO: Add test logic	here
      //
    }
  }
}
