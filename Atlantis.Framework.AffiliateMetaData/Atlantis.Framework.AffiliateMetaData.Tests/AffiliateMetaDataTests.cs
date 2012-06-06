using System.Diagnostics;
using Atlantis.Framework.AffiliateMetaData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AffiliateMetaData.Tests
{
  [TestClass]
  public class GetAffiliateMetaDataTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 532;


    public GetAffiliateMetaDataTests()
    { }

    private TestContext testContextInstance;

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
    [DeploymentItem("atlantis.framework.affiliatemetadata.impl.dll")]
    public void AffiliateMetaDataTest()
    {
      AffiliateMetaDataRequestData request = new AffiliateMetaDataRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1);

      AffiliateMetaDataResponseData response = (AffiliateMetaDataResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("atlantis.framework.affiliatemetadata.impl.dll")]
    public void AffiliateMetaDataMD5Test()
    {
      AffiliateMetaDataRequestData request1 = new AffiliateMetaDataRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1);

      AffiliateMetaDataRequestData request2 = new AffiliateMetaDataRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 2);

      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }
  }
}
