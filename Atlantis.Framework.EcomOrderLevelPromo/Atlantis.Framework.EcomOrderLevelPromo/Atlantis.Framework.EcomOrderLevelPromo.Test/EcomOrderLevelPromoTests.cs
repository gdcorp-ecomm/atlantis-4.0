using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcomOrderLevelPromo.Impl;
using Atlantis.Framework.EcomOrderLevelPromo.Interface;


namespace Atlantis.Framework.EcomOrderLevelPromo.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetEcomOrderLevelPromoTests
  {
  
    private const string _shopperId = "";
    private const int _requestType = 22;
	
    public GetEcomOrderLevelPromoTests()
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
    public void EcomOrderLevelPromoTest()
    {
     EcomOrderLevelPromoRequestData request = new EcomOrderLevelPromoRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0,1 );

      EcomOrderLevelPromoResponseData response = (EcomOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
	  // Cache call
	  //EcomOrderLevelPromoResponseData response = (EcomOrderLevelPromoResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
