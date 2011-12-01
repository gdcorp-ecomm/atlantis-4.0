using System.Data;
using Atlantis.Framework.AuctionsRetrieveDomains.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionsRetrieveDomains.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class AuctionsRetrieveDomainsTests
  {
    private const string _shopperId = "842749";
    private const int _auctionCount = 25;
    private const int _auctionsRetrieveDomainsRequest = 215;

    public AuctionsRetrieveDomainsTests()
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
      AuctionsRetrieveDomainsRequestData _request = new AuctionsRetrieveDomainsRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _auctionCount);
      AuctionsRetrieveDomainsResponseData _response = (AuctionsRetrieveDomainsResponseData)Engine.Engine.ProcessRequest(_request, _auctionsRetrieveDomainsRequest);
      DataSet ds = _response.AuctionsList;
      _response = (AuctionsRetrieveDomainsResponseData)DataCache.DataCache.GetProcessRequest(_request, _auctionsRetrieveDomainsRequest);
      ds = _response.AuctionsList;
      //Debug.WriteLine(ds);
      Assert.IsTrue(_response.IsSuccess);
    }
  }
  
}
