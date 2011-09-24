using System.Diagnostics;
using Atlantis.Framework.HCC.Interface.Constants;
using Atlantis.Framework.HCCEditDomain.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HCCEditDomain.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCEditDomainTests
  {
  
    private const string _shopperId = "12530";
	
	
    public GetHCCEditDomainTests()
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
    public void HCCEditDomainTest()
    {
     var request = new HCCEditDomainRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "e4ca712d-1e92-4aeb-a182-6439464ef879" //"b52e14f3-1bf9-44be-87e6-7b17da956673"
        , HCCDomainType.SubDomain
        , "pugmobile.com" //"pugpoggle.com"
        , "app.pugmobile.com" //string.Empty
        , "" //"pugpoggle.biz"//"JABBAJAW.INFO"
        , "/");

      var response = (HCCEditDomainResponseData)Engine.Engine.ProcessRequest(request, 297);
      
	  // Cache call
	  //HCCEditDomainResponseData response = (HCCEditDomainResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
