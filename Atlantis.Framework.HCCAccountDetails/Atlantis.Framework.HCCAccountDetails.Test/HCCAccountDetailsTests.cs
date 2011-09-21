using System.Diagnostics;
using Atlantis.Framework.HCCAccountDetails.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HCCAccountDetails.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCAccountDetailsTests
  {
  
    private const string _shopperId = "";
	
	
    public GetHCCAccountDetailsTests()
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
    public void HCCAccountDetailsTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      string shopperId = "12530";
      string sourceUrl = string.Empty; // TODO: Initialize to an appropriate value
      string orderIo = string.Empty; // TODO: Initialize to an appropriate value
      string pathway = string.Empty; // TODO: Initialize to an appropriate value
      int pageCount = 0; // TODO: Initialize to an appropriate value
      string accountUid = "e4ca712d-1e92-4aeb-a182-6439464ef879";

     HCCAccountDetailsRequestData request = new HCCAccountDetailsRequestData(shopperId, 
      sourceUrl, 
      orderIo, 
      pathway, 
      pageCount, 
      accountUid
     );

      //HCCAccountDetailsResponseData response = (HCCAccountDetailsResponseData)Engine.Engine.ProcessRequest(request, 265);

      var response = SessionCache.SessionCache.GetProcessRequest<HCCAccountDetailsResponseData>(request, 265);
      
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
