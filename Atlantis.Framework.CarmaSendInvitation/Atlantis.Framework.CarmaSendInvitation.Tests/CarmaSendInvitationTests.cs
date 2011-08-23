using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.CarmaSendInvitation.Impl;
using Atlantis.Framework.CarmaSendInvitation.Interface;

namespace Atlantis.Framework.CarmaSendInvitation.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetCarmaSendInvitationTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 410;


    public GetCarmaSendInvitationTests()
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
    public void CarmaSendInvitationTest()
    {
      CarmaSendInvitationRequestData request = new CarmaSendInvitationRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "ksearle@godaddy.com"
         , "Homer"
         , "Simpson");

      CarmaSendInvitationResponseData response = (CarmaSendInvitationResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
