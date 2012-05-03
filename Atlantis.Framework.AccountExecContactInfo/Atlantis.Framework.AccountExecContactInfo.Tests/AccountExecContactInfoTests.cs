using System.Diagnostics;
using Atlantis.Framework.AccountExecContactInfo.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AccountExecContactInfo.Tests
{
  [TestClass]
  public class GetAccountExecContactInfoTests
  {
    private const string _vipShopperId = "823393";
    private const string _ordinaryShopperId = "856907";
    private const int _requestType = 527;


    public GetAccountExecContactInfoTests()
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
    [DeploymentItem("atlantis.framework.accountexeccontactinfo.impl.dll")]
    public void AccountExecContactInfoVIPShopperTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      AccountExecContactInfoRequestData request = new AccountExecContactInfoRequestData(_vipShopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      AccountExecContactInfoResponseData response1 = SessionCache.SessionCache.GetProcessRequest<AccountExecContactInfoResponseData>(request, _requestType);
      AccountExecContactInfoResponseData response2 = SessionCache.SessionCache.GetProcessRequest<AccountExecContactInfoResponseData>(request, _requestType);

      Debug.WriteLine(response1.ToXML());
      Assert.IsTrue(response1.IsSuccess);

      string xml = response2.SerializeSessionData();
      Debug.WriteLine("Serialized Response");
      Debug.WriteLine(xml);

      Assert.AreEqual(response1.VipRepInfo.RepPhoneExtension, response2.VipRepInfo.RepPhoneExtension);
      Assert.IsTrue(response2.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("atlantis.framework.accountexeccontactinfo.impl.dll")]
    public void AccountExecContactInfoOrdinaryShopperTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      AccountExecContactInfoRequestData request = new AccountExecContactInfoRequestData(_ordinaryShopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      AccountExecContactInfoResponseData response1 = SessionCache.SessionCache.GetProcessRequest<AccountExecContactInfoResponseData>(request, _requestType);
      AccountExecContactInfoResponseData response2 = SessionCache.SessionCache.GetProcessRequest<AccountExecContactInfoResponseData>(request, _requestType);

      Debug.WriteLine(response1.ToXML());
      Assert.IsTrue(response1.IsSuccess);

      string xml = response2.SerializeSessionData();
      Debug.WriteLine("Serialized Response");
      Debug.WriteLine(xml);

      Assert.AreEqual(response1.VipRepInfo, response2.VipRepInfo);
      Assert.IsTrue(response2.IsSuccess);
    }
  }
}
