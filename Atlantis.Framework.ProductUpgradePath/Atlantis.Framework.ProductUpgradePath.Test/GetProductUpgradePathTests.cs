using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.ProductUpgradePath.Impl;
using Atlantis.Framework.ProductUpgradePath.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductUpgradePath.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetProductUpgradePathTests
  {
  
    private const string _shopperId = "";
	
	
    public GetProductUpgradePathTests()
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
    public void ProductUpgradePathTest()
    {
     ProductUpgradePathRequestData request = new ProductUpgradePathRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 );

      ProductUpgradePathResponseData response = (ProductUpgradePathResponseData)Engine.Engine.ProcessRequest(request, 405);
      
	  // Cache call
	  //ProductUpgradePathResponseData response = (ProductUpgradePathResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
