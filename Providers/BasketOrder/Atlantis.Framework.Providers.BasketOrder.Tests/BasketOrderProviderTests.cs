using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.BasketOrder.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.BasketOrder.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  [DeploymentItem("BrokenOrder.xml")]
  [DeploymentItem("EmptyItemsOrder.xml")]
  [DeploymentItem("EmptyOrder.xml")]
  [DeploymentItem("EmptyOrderDetailOrder.xml")]
  [DeploymentItem("FiveItems_ValidOrder.xml")]
  [DeploymentItem("MissingItemsOrder.xml")]
  [DeploymentItem("MissingOrderDetailOrder.xml")]
  [DeploymentItem("OverlappingItemsOrder.xml")]
  [DeploymentItem("ValidOrder.xml")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.GetBasketPrice.Impl.dll")]
  public class BasketOrderProviderTests
  {
    private const string _shopperId = "";

    private IBasketOrderProvider _basketOrderProvider;
    private IBasketOrderProvider BasketOrderProvider
    {
      get { return _basketOrderProvider; }
      set { _basketOrderProvider = value; }
    }

    public BasketOrderProviderTests()
    {
      
    }

    private IBasketOrderProvider NewBasketOrderProvider()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.mysite.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      var container = new MockProviderContainer();

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IBasketOrderProvider, BasketOrderProvider>();

      //container.Resolve<IShopperContext>().SetLoggedInShopper(shopperId);

      return container.Resolve<IBasketOrderProvider>();
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    private string BrokenOrderXml
    {
      get
      {
        return File.ReadAllText("BrokenOrder.xml");
        //return XDocument.Load("BrokenOrder.xml").ToString();
      }
    }

    private string EmptyItemsOrderXml
    {
      get { return XDocument.Load("EmptyItemsOrder.xml").ToString(); }
    }

    private string EmptyOrderXml
    {
      get { return XDocument.Load("EmptyOrder.xml").ToString(); }
    }

    private string EmptyOrderDetailOrderXml
    {
      get { return XDocument.Load("EmptyOrderDetailOrder.xml").ToString(); }
    }

    private string MissingItemsOrderXml
    {
      get { return XDocument.Load("MissingItemsOrder.xml").ToString(); }
    }

    private string MissingOrderDetailOrderXml
    {
      get { return XDocument.Load("MissingOrderDetailOrder.xml").ToString(); }
    }

    private string OverlappingItemsOrderXml
    {
      get { return XDocument.Load("OverlappingItemsOrder.xml").ToString(); }
    }

    private string ValidOrderXml
    {
      get { return XDocument.Load("ValidOrder.xml").ToString(); }
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

    private void CheckNullAsserts(IBasketOrderTrackingData basketOrder)
    {
      CheckNullAsserts(basketOrder as IBasketOrder);

      Assert.IsNotNull(basketOrder.CustomerCity);
      Assert.IsNotNull(basketOrder.CustomerCountry);
      Assert.IsNotNull(basketOrder.CustomerState);
      Assert.IsNotNull(basketOrder.ShippingTotalUsdFormatted);
      Assert.IsNotNull(basketOrder.TaxTotalUsdFormatted);
      Assert.IsNotNull(basketOrder.TotalPriceUsdFormatted);
    }

    private void CheckNullAsserts(IBasketOrder basketOrder)
    {
      Assert.IsNotNull(basketOrder);
      Assert.IsNotNull(basketOrder.OrderId);
      Assert.IsNotNull(basketOrder.ShippingTotal);
      Assert.IsNotNull(basketOrder.TaxTotal);
      Assert.IsNotNull(basketOrder.TotalPrice);
      Assert.IsNotNull(basketOrder.OrderItems);
    }


    private void CheckOrderItemsEmpty(IBasketOrder orderData)
    {
      Assert.AreEqual(orderData.OrderItems.Count(), 0);
    }

    private void CheckOrderItemsNotEmpty(IBasketOrder orderData)
    {
      Assert.AreNotEqual(orderData.OrderItems.Count(), 0);
    }


    // BrokenOrderXml
    // EmptyItemsOrderXml
    // EmptyOrderXml
    // EmptyOrderDetailOrderXml
    // MissingItemsOrderXml
    // MissingOrderDetailOrderXml
    // OverlappingItemsOrderXml
    // ValidOrderXml

    private void CheckValidDataWithItems(string orderXml)
    {
      var basketOrderProvider = NewBasketOrderProvider();
      IBasketOrderTrackingData orderData;
      var success = basketOrderProvider.TryGetBasketOrderTrackingDataFromReceiptXml(out orderData, orderXml);
      Assert.IsTrue(success);

      CheckNullAsserts(orderData);
      CheckOrderItemsNotEmpty(orderData);
    }

    private void CheckValidDataWithoutItems(string orderXml)
    {
      var basketOrderProvider = NewBasketOrderProvider();
      IBasketOrderTrackingData orderData;
      var success = basketOrderProvider.TryGetBasketOrderTrackingDataFromReceiptXml(out orderData, orderXml);
      Assert.IsTrue(success);

      CheckNullAsserts(orderData);
      CheckOrderItemsEmpty(orderData);
    }

    private void CheckInvalidData(string orderXml)
    {
      var basketOrderProvider = NewBasketOrderProvider();
      IBasketOrderTrackingData orderData;
      var success = basketOrderProvider.TryGetBasketOrderTrackingDataFromReceiptXml(out orderData, orderXml);
      Assert.IsFalse(success);

      CheckNullAsserts(orderData);
      CheckOrderItemsEmpty(orderData);
    }

    #region Test Methods

    [TestMethod]
    public void TrackingData_BrokenOrderXml_Test()
    {
      CheckInvalidData(BrokenOrderXml);
    }

    [TestMethod]
    public void TrackingData_EmptyItemsOrderXml_Test()
    {
      CheckValidDataWithoutItems(EmptyItemsOrderXml);
    }

    [TestMethod]
    public void TrackingData_EmptyOrderXml_Test()
    {
      CheckValidDataWithoutItems(EmptyOrderXml);
    }

    [TestMethod]
    public void TrackingData_EmptyOrderDetailOrderXml_Test()
    {
      CheckValidDataWithItems(EmptyOrderDetailOrderXml);
    }

    [TestMethod]
    public void TrackingData_MissingItemsOrderXml_Test()
    {
      CheckValidDataWithoutItems(MissingItemsOrderXml);
    }
    
    [TestMethod]
    public void TrackingData_MissingOrderDetailOrderXml_Test()
    {
      CheckValidDataWithItems(MissingOrderDetailOrderXml);
    }

    [TestMethod]
    public void TrackingData_OverlappingItemsOrderXml_Test()
    {
      CheckValidDataWithItems(OverlappingItemsOrderXml);
    }

    [TestMethod]
    public void TrackingData_ValidOrderXml_Test()
    {
      CheckValidDataWithItems(ValidOrderXml);
    }

    #endregion
  }
}
