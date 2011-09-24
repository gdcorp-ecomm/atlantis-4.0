using System.Diagnostics;
using Atlantis.Framework.HCC.Interface.Constants;
using Atlantis.Framework.HCCRemoveDomain.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HCCRemoveDomain.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCRemoveDomainTests
  {
  
    private const string _shopperId = "";
	
	
    public GetHCCRemoveDomainTests()
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
    public void HCCRemoveDomainTest()
    {
     var request = new HCCRemoveDomainRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "7dc936ec-b8fb-497d-9db2-2b46026b06cc"
        , HCCDomainType.SubDomain
        , "JABBAJAW.INFO"
        , "shark.JABBAJAW");

      var response = (HCCRemoveDomainResponseData)Engine.Engine.ProcessRequest(request, 298);
      
	  // Cache call
	  //HCCRemoveDomainResponseData response = (HCCRemoveDomainResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
