using Atlantis.Framework.MAFirstDataCreateApplication.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MAFirstDataCreateApplication.Tests
{
  [TestClass]
  public class GetMAFirstDataCreateApplicationTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 594;


    public GetMAFirstDataCreateApplicationTests()
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
    public void MAFirstDataCreateApplicationFailTest()
    {
      int merchantAccountId = 35000; //currently DEV has only 788+ rows so anything greater (like 35,000) will fail

      MAFirstDataCreateApplicationRequestData request = new MAFirstDataCreateApplicationRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , merchantAccountId);

      MAFirstDataCreateApplicationResponseData response = (MAFirstDataCreateApplicationResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MAFirstDataCreateApplicationPassTest()
    {
      int merchantAccountId = 788;

      MAFirstDataCreateApplicationRequestData request = new MAFirstDataCreateApplicationRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , merchantAccountId);

      MAFirstDataCreateApplicationResponseData response = (MAFirstDataCreateApplicationResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
