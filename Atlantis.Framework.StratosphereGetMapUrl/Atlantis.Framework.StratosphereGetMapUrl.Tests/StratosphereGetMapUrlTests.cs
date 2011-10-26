using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.StratosphereGetMapUrl.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.StratosphereGetMapUrl.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetStratosphereGetMapUrlTests
  {
    private const string _shopperId = "856907";

    public GetStratosphereGetMapUrlTests()
    { }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    public void StratosphereGetShopperMapUrlTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "corp.web.mya.dev.client.godaddy.com");

      StratosphereGetMapUrlRequestData request = new StratosphereGetMapUrlRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "shopper"
         , null
         , cert);

      StratosphereGetMapUrlResponseData response = SessionCache.SessionCache.GetProcessRequest<StratosphereGetMapUrlResponseData>(request, request.StratosphereGetMapUrlRequestType);
        
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void StratosphereGetDomainMapUrlTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "corp.web.mya.dev.client.godaddy.com");

      StratosphereGetMapUrlRequestData request = new StratosphereGetMapUrlRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "domain"
         , "intrepidkjs.com"
         , cert);

      StratosphereGetMapUrlResponseData response = SessionCache.SessionCache.GetProcessRequest<StratosphereGetMapUrlResponseData>(request, request.StratosphereGetMapUrlRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //public void StratosphereGetInvalidMapUrlTest()
    //{
    //  SessionCache.ContextWrapper.UseMockContext = true;
    //  X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "corp.web.mya.dev.client.godaddy.com");

    //  StratosphereGetMapUrlRequestData request = new StratosphereGetMapUrlRequestData(_shopperId
    //     , string.Empty
    //     , string.Empty
    //     , string.Empty
    //     , 0
    //     , "kent"
    //     , null
    //     , cert);

    //  StratosphereGetMapUrlResponseData response = SessionCache.SessionCache.GetProcessRequest<StratosphereGetMapUrlResponseData>(request, _requestType);

    //  Debug.WriteLine(response.ToXML());
    //  Assert.IsFalse(response.IsSuccess);
    //}
  
    private X509Certificate2 FindCertificate(StoreLocation location, StoreName name, X509FindType findType, string findValue)
    {
      X509Store store = new X509Store(name, location);

      try
      {
        // create and open store for read-only access
        store.Open(OpenFlags.ReadOnly);

        // search store
        X509Certificate2Collection col = store.Certificates.Find(findType, findValue, true);

        // return first certificate found
        return col[0];
      }
      // always close the store
      finally
      {
        store.Close();
      }
    }
  }
}
