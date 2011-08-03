using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.UpdateShopper.Interface;

namespace Atlantis.Framework.UpdateShopper.Tests
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
    [DeploymentItem("atlantis.config")]
    public void UpdateShopperMktgData()
    {
      UpdateShopperRequestData requestData = new UpdateShopperRequestData("850774", string.Empty, string.Empty, 
        string.Empty, 0, "172.19.72.107");
      requestData.IPAddress = "172.19.72.107";
      requestData.AddUpdateField("mktg_email", "no");
      requestData.AddUpdateField("mktg_mail", "no");
      requestData.AddUpdateField("mktg_nonpromotional_notices", "no");

      UpdateShopperResponseData response = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(requestData, 61);
      Assert.IsTrue(response.IsSuccess);

      //
      // TODO: Add test logic	here
      //
    }
  }
}
