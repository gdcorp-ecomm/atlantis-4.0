using System.Diagnostics;
using Atlantis.Framework.GetOffersWithPlId.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetOffersWithPlId.Tests
{
 
  [TestClass]
  public class GetGetOffersWithPlIdTests
  {

    private const string _shopperId = "842749";
    private const int _requestType = 89;
    private const int _privateLabelId = 1;
    private const int _applicationId = 6;
   

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
    public void GetOffersTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      GetOffersWithPlIdRequestData request = new GetOffersWithPlIdRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , _applicationId
        , _privateLabelId);


      GetOffersWithPlIdResponseData response = SessionCache.SessionCache.GetProcessRequest<GetOffersWithPlIdResponseData>(request, _requestType);
      GetOffersWithPlIdResponseData response2 = SessionCache.SessionCache.GetProcessRequest<GetOffersWithPlIdResponseData>(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsNotNull(response.Offers);
      Assert.IsTrue(response.IsSuccess);
      Assert.AreEqual(response.ToXML(), response2.ToXML());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetOffersBadRequestTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      GetOffersWithPlIdRequestData request = new GetOffersWithPlIdRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , _applicationId
        , -33);


      GetOffersWithPlIdResponseData response = SessionCache.SessionCache.GetProcessRequest<GetOffersWithPlIdResponseData>(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);


    }

  }
}
