using Atlantis.Framework.MAFirstDataCreateMerchant.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MAFirstDataCreateMerchant.Tests
{
  [TestClass]
  public class GetMAFirstDataCreateMerchantTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 595;
  
  
    public GetMAFirstDataCreateMerchantTests()
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
    public void MAFirstDataCreateMerchantPassTest()
    {
      int merchantId = 788;
      long vendorId = 222222222222;
      MAFirstDataCreateMerchantRequestData request = new MAFirstDataCreateMerchantRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , merchantId
        , vendorId);

      MAFirstDataCreateMerchantResponseData response = (MAFirstDataCreateMerchantResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MAFirstDataCreateMerchantFailTest()
    {
      int merchantId = 35000;  //currently DEV has only 788+ rows so anything greater (like 35,000) will fail
      long vendorId = 222222222222;
      MAFirstDataCreateMerchantRequestData request = new MAFirstDataCreateMerchantRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , merchantId
        , vendorId);

      MAFirstDataCreateMerchantResponseData response = (MAFirstDataCreateMerchantResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsFalse(response.IsSuccess);
    }
  }
}
