using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommPrunedActivationData.Impl;
using Atlantis.Framework.EcommPrunedActivationData.Interface;


namespace Atlantis.Framework.EcommPrunedActivationData.Tests
{
  [TestClass]
  public class GetEcommPrunedActivationDataTests
  {
    //private const string _shopperId = "853392";
    //private const string _orderId = "1464901";
    //private const string _orderId = "1466042";

    //private const string _shopperId = "4321";
    //private const string _orderId = "12345678";

    private const string _shopperId = "855307";
    private const string _orderId = "1466738";

    private const int _requestType = 530;
	
	
    public GetEcommPrunedActivationDataTests()
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
    [DeploymentItem("atlantis.framework.ecommprunedactivationdata.impl.dll")]
    public void No_Brackets_On_ERID()
    {
      EcommPrunedActivationDataRequestData request = new EcommPrunedActivationDataRequestData(_shopperId
        , string.Empty
        , _orderId
        , string.Empty
        , 0);

      EcommPrunedActivationDataResponseData response = (EcommPrunedActivationDataResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsTrue(response.IsSuccess && response.FreeProducts.Count > 0, "Service call did not work or did not return any data.");

      foreach (ProductInfo currentProduct in response.FreeProducts)
      {
        Assert.IsTrue(!currentProduct.ExternalResourceID.StartsWith("{") && !currentProduct.ExternalResourceID.EndsWith("}"), "Brackets were found on either end of ERID");
      }
    }

    [TestMethod]
	  [DeploymentItem("atlantis.config")]
    [DeploymentItem("atlantis.framework.ecommprunedactivationdata.impl.dll")]
    public void EcommPrunedActivationDataTest()
    {
      EcommPrunedActivationDataRequestData request = new EcommPrunedActivationDataRequestData(_shopperId
        , string.Empty
        , _orderId
        , string.Empty
        , 0 );

      EcommPrunedActivationDataResponseData response = (EcommPrunedActivationDataResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess && response.FreeProducts.Count > 0, "Service call did not work or did not return any data.");
      foreach (ProductInfo currentProduct in response.FreeProducts)
      {
        foreach (ActivatedProducts currentFree in currentProduct.ActivatedProducts)
        {
          if (currentFree.ProductType == ProductInfo.PRODUCT_TYPE_EMAIL)
          {
            System.Diagnostics.Debug.WriteLine(currentFree.Email);
          }
        }
      }
    }
  }
}
