using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.CarmaGetTrustedInvitation.Impl;
using Atlantis.Framework.CarmaGetTrustedInvitation.Interface;


namespace Atlantis.Framework.CarmaGetTrustedInvitation.Tests
{
  [TestClass]
  public class GetCarmaGetTrustedInvitationTests
  {
    private const string _shopperId = "839409";
    private const string guid = "F2D550FD-E2DD-4EDA-8648-BBED1861C229";
    private const int _requestType = 419;

    public GetCarmaGetTrustedInvitationTests()
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
    public void CarmaGetTrustedInvitationTest()
    {
      CarmaGetTrustedInvitationRequestData request = new CarmaGetTrustedInvitationRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , guid);

      CarmaGetTrustedInvitationResponseData response = (CarmaGetTrustedInvitationResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
