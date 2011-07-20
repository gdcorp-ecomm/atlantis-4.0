using System.Diagnostics;
using Atlantis.Framework.AuctionsAreaBySection.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionsAreaBySection.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class AuctionsAreaBySectionTests
  {
    private const string _shopperId = "856907";   
    private const string _membersAreaID = "2";    //membersAreaID = 2 for Bidding
    private const string _returnsBid = "1";
    private const int _auctionsAreaBySectionRequest = 87;

    public AuctionsAreaBySectionTests()
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
    public void WebServiceResponseTest()
    {
      AuctionsAreaBySectionRequestData _request = new AuctionsAreaBySectionRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _membersAreaID, _returnsBid);
      AuctionsAreaBySectionResponseData _response = (AuctionsAreaBySectionResponseData)Engine.Engine.ProcessRequest(_request, _auctionsAreaBySectionRequest);
      Debug.WriteLine(_response.ToXML());
      Assert.IsTrue(_response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SerializeTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      AuctionsAreaBySectionRequestData _request = new AuctionsAreaBySectionRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _membersAreaID, _returnsBid);
      AuctionsAreaBySectionResponseData _response = SessionCache.SessionCache.GetProcessRequest<AuctionsAreaBySectionResponseData>(_request, _auctionsAreaBySectionRequest);
      AuctionsAreaBySectionResponseData _response2 = SessionCache.SessionCache.GetProcessRequest<AuctionsAreaBySectionResponseData>(_request, _auctionsAreaBySectionRequest);

      string xml = _response.SerializeSessionData();
      Debug.WriteLine(xml);
      Assert.IsTrue(_response.IsSuccess);
      Assert.AreEqual(_response.ToXML(), _response2.ToXML());
    }
  }
}
