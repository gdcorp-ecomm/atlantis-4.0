using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.GetExpiringProfiles.Interface;
using Atlantis.Framework.Testing.MockHttpContext;

namespace Atlantis.Framework.GetExpiringProfiles.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetGetExpiringProfilesTests
  {

    private const string _shopperId = "856907";
    private const string _requestedBy = "Henry";
    private const int _daysBefore = 1;
    private const int _daysAfter = 2;
    private const int _requestType = 92;
	
    public GetGetExpiringProfilesTests()
    {
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
    public void GetExpiringProfilesTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);
      
      GetExpiringProfilesRequestData request = new GetExpiringProfilesRequestData(_shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        0,
        _daysBefore,
        _daysAfter);


      GetExpiringProfilesResponseData response = SessionCache.SessionCache.GetProcessRequest<GetExpiringProfilesResponseData>(request, _requestType);
      GetExpiringProfilesResponseData response2 = SessionCache.SessionCache.GetProcessRequest<GetExpiringProfilesResponseData>(request, _requestType);
        
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.AreEqual(response.ToXML(), response2.ToXML());
    }
  }  
}
