using Atlantis.Framework.MessagingShowUpdateEmail.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MessagingShowUpdateEmail.Tests
{
  [TestClass]
  public class GetMessagingShowUpdateEmailTests
  {

    private const string _shopperId = "";

    public GetMessagingShowUpdateEmailTests()
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
    public void MessagingShowUpdateEmailTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      string email = "xyz@godaddy.com";
      MessagingShowUpdateEmailRequestData request = new MessagingShowUpdateEmailRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , email
         , 1);

      MessagingShowUpdateEmailResponseData response = SessionCache.SessionCache.GetProcessRequest<MessagingShowUpdateEmailResponseData>(request, 147);
      MessagingShowUpdateEmailResponseData response2 = SessionCache.SessionCache.GetProcessRequest<MessagingShowUpdateEmailResponseData>(request, 147);

      Assert.IsTrue(response.IsSuccess && response.ShowUpdateEmailInfo == false && response.ToXML()== response2.ToXML());
    }
  }
}
