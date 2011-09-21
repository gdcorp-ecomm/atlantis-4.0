using System.Diagnostics;
using Atlantis.Framework.HCCAccountDomains.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HCCAccountDomains.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCAccountDomainsTests
  {
  
    private const string _shopperId = "12530";
	
	
    public GetHCCAccountDomainsTests()
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
    public void HCCAccountDomainsTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      HCCAccountDomainsRequestData request = new HCCAccountDomainsRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "7dc936ec-b8fb-497d-9db2-2b46026b06cc");

      //HCCAccountDomainsResponseData response = (HCCAccountDomainsResponseData)Engine.Engine.ProcessRequest(request, 285);

      // Cache call
      var response = SessionCache.SessionCache.GetProcessRequest<HCCAccountDomainsResponseData>(request, 285);

      //
      // TODO: Add test logic here
      //

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
