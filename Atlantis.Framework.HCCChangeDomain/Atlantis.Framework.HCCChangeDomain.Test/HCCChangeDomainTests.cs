using System.Diagnostics;
using Atlantis.Framework.HCCChangeDomain.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HCCChangeDomain.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCChangeDomainTests
  {
  
    private const string _shopperId = "12530";
	
	
    public GetHCCChangeDomainTests()
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
    public void HCCChangeDomainTest()
    {
      HCCChangeDomainRequestData request = new HCCChangeDomainRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "e4ca712d-1e92-4aeb-a182-6439464ef879"
        , "ebed6677-ef87-46bf-8d2c-afde3fd2e2c4"
        , "pugmobil.com"
        , false
        , "/");

      HCCChangeDomainResponseData response = (HCCChangeDomainResponseData)Engine.Engine.ProcessRequest(request, 287);
      
	  // Cache call
	  //HCCChangeDomainResponseData response = (HCCChangeDomainResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
