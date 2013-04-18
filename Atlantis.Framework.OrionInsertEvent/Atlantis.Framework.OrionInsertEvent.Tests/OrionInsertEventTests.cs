using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.OrionEvent.Interface;

namespace Atlantis.Framework.OrionEvent.Tests
{
  [TestClass]
  public class GetOrionInsertEventTests
  {
    private const string SHOPPER_ID = "856907";
    private const int REQUEST_TYPE = 680;

    public TestContext TestContext { get; set; }

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
    [DeploymentItem("Atlantis.Framework.OrionEvent.Impl.dll")]
    public void OrionInsertEventTest()
    {
      var request = new OrionInsertEventRequestData(SHOPPER_ID
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "ADD_DNS_OPW"
        , "1"
        , "2131085");

      var response = (OrionInsertEventResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);
      
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
