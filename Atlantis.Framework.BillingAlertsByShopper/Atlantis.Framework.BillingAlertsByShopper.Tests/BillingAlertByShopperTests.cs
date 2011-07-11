using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.BillingAlertsByShopper.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BillingAlertsByShopper.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetBillingAlertsByShopperTests
  {

    private const string _shopperId = "832652";
    private const int _requestType = 99;
	
    public GetBillingAlertsByShopperTests()
    {
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
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
    public void BillingAlertsByShopperTest()
    {
      List<int> productList = new List<int>();
      productList.Add(1);
      productList.Add(24);
      productList.Add(25);
      productList.Add(23);
      productList.Add(15);
      productList.Add(28);
      productList.Add(54);
      productList.Add(59);
      productList.Add(21);
      productList.Add(38);
      productList.Add(49);
      productList.Add(13);
      productList.Add(22);
      productList.Add(26);
      productList.Add(4);
      productList.Add(66);
      productList.Add(74);

      BillingAlertsByShopperRequestData request = new BillingAlertsByShopperRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , productList);
      request.TotalAlertsThreshold = 0;

      BillingAlertsByShopperResponseData response = (BillingAlertsByShopperResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
      foreach(BillingAlert alert in response.BillingAlerts)
      {
        Debug.WriteLine("**********************");
        Debug.WriteLine(string.Format("ProductGroup :: {0}", alert.ProductGroupId));
        Debug.WriteLine(string.Format("BillingResourceID :: {0}", alert.BillingFailureResourceId));
        Debug.WriteLine(string.Format("SetupResourceID :: {0}", alert.SetupResourceId));
        Debug.WriteLine(string.Format("ExpiringResourceID :: {0}", alert.ExpiringResourceId ));     
      }
    
      Assert.IsTrue(response.IsSuccess);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BillingAlertsByShopperSerializeTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      List<int> productList = new List<int>();
      productList.Add(1);
      productList.Add(24);
      productList.Add(25);
      productList.Add(23);
      productList.Add(15);
      productList.Add(28);
      productList.Add(54);
      productList.Add(59);
      productList.Add(21);
      productList.Add(38);
      productList.Add(49);
      productList.Add(13);
      productList.Add(22);
      productList.Add(26);
      productList.Add(4);
      productList.Add(66);
      productList.Add(74);

      BillingAlertsByShopperRequestData request = new BillingAlertsByShopperRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , productList);
      request.TotalAlertsThreshold = 2;

      BillingAlertsByShopperResponseData response = SessionCache.SessionCache.GetProcessRequest<BillingAlertsByShopperResponseData>(request, _requestType);

      BillingAlertsByShopperResponseData serializedResponse = SessionCache.SessionCache.GetProcessRequest<BillingAlertsByShopperResponseData>(request, _requestType);

      Assert.AreEqual(response.BillingAlerts[0].ProductGroupId, serializedResponse.BillingAlerts[0].ProductGroupId);
      Assert.AreEqual(response.BillingAlerts[0].BillingFailureResourceId, serializedResponse.BillingAlerts[0].BillingFailureResourceId);
      Assert.AreEqual(response.BillingAlerts[0].SetupResourceId, serializedResponse.BillingAlerts[0].SetupResourceId);
      Assert.AreEqual(response.BillingAlerts[0].ExpiringResourceId, serializedResponse.BillingAlerts[0].ExpiringResourceId);

      Assert.IsTrue(serializedResponse.IsSuccess);
    }
  }
}
