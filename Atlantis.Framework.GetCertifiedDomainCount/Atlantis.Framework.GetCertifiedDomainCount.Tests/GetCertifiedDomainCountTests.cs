using System.Diagnostics;
using Atlantis.Framework.GetCertifiedDomainCount.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetCertifiedDomainCount.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetGetCertifiedDomainCountTests
  {

    private const string _shopperId = "842749";
    private const int _certifiedDomainsCountRequest = 95;


    public GetGetCertifiedDomainCountTests()
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
    public void GetCertifiedDomainCountTest()
    {
      GetCertifiedDomainCountRequestData request = new GetCertifiedDomainCountRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      GetCertifiedDomainCountResponseData response = (GetCertifiedDomainCountResponseData)Engine.Engine.ProcessRequest(request, _certifiedDomainsCountRequest);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine("CertifiedDomainCount: " + response.CertifiedDomainsCount.ToString());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SerializeTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      GetCertifiedDomainCountRequestData request = new GetCertifiedDomainCountRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      GetCertifiedDomainCountResponseData response = SessionCache.SessionCache.GetProcessRequest<GetCertifiedDomainCountResponseData>(request, _certifiedDomainsCountRequest);
      GetCertifiedDomainCountResponseData response2 = SessionCache.SessionCache.GetProcessRequest<GetCertifiedDomainCountResponseData>(request, _certifiedDomainsCountRequest);

      string xml = response.SerializeSessionData();
      Debug.WriteLine(xml);
      Debug.WriteLine("CertifiedDomainCount: " + response.CertifiedDomainsCount.ToString());

      Assert.AreEqual(response.CertifiedDomainsCount, response2.CertifiedDomainsCount);
      Assert.IsTrue(response2.IsSuccess);
    }
  }
}
