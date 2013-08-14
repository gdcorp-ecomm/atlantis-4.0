using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.BasketOrder;
using Atlantis.Framework.Providers.BasketOrder.Interface;
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
  public class BasketOrderTrackingDataTests
  {
    private const string _shopperId = "";


    public BasketOrderTrackingDataTests()
    {

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

    [TestMethod]
    [ExpectedException(typeof(XmlException), "An invalid orderXml was inappropriately allowed.")]
    public void BrokenOrderXmlTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(BrokenOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void EmptyOrderXmlTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(EmptyOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void EmptyItemsOrderTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(EmptyItemsOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void EmptyOrderDetailOrderTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(EmptyOrderDetailOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    public void MissingItemsOrderTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(MissingItemsOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void MissingOrderDetailOrderTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(MissingOrderDetailOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    public void OverlappingItemsOrderTest()
    {
      IBasketOrderTrackingData gaOrderData;
      gaOrderData = new BasketOrderTrackingData(OverlappingItemsOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "A null orderXml was inappropriately allowed.")]
    public void NullOrderStringTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(null as string);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "A null orderXml was inappropriately allowed.")]
    public void NullOrderXDocumentTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(null as XDocument);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void ValidOrderTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(ValidOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }
    
    [TestMethod]
    public void ConstructorTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(XDocument.Parse(ValidOrderXml));
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    public void ToXmlTest()
    {
      IBasketOrderTrackingData gaOrderData = new BasketOrderTrackingData(ValidOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
      var orderXml = gaOrderData.ToXml();
      Assert.IsNotNull(orderXml);
      //Assert.AreEqual(ValidOrderXml, orderXml);
    }
  }
}
