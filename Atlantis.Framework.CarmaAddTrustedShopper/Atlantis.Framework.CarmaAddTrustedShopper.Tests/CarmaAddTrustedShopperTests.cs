using Atlantis.Framework.CarmaAddTrustedShopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CarmaAddTrustedShopper.Tests
{
  [TestClass]
  public class GetCarmaAddTrustedShopperTests
  {
    private const string _shopperId = "839409";
    private const string _primaryShopperId = "856907";
    private const int _requestType = 418;


    public GetCarmaAddTrustedShopperTests()
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
    public void CarmaAddTrustedShopperTest()
    {
      CarmaAddTrustedShopperRequestData request = new CarmaAddTrustedShopperRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , _primaryShopperId);

      CarmaAddTrustedShopperResponseData response = (CarmaAddTrustedShopperResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
