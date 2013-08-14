using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Providers.BasketOrder.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.BasketOrder.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  [DeploymentItem("EmptyItemsOrder.xml")]
  [DeploymentItem("EmptyOrder.xml")]
  [DeploymentItem("EmptyOrderDetailOrder.xml")]
  [DeploymentItem("NineItems_ValidOrder.xml")]
  [DeploymentItem("MissingItemsOrder.xml")]
  [DeploymentItem("MissingOrderDetailOrder.xml")]
  [DeploymentItem("OverlappingItemsOrder.xml")]
  [DeploymentItem("atlantis.config")]
  public class BasketOrderTests
  {
    private const string _shopperId = "";


    public BasketOrderTests()
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

    private string NineItems_ValidOrderXml
    {
      get { return XDocument.Load("NineItems_ValidOrder.xml").ToString(); }
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

    private void CheckNullAsserts(IBasketOrder basketOrder)
    {
      Assert.IsNotNull(basketOrder);
      Assert.IsNotNull(basketOrder.OrderId);
      Assert.IsNotNull(basketOrder.ShippingTotal);
      Assert.IsNotNull(basketOrder.TaxTotal);
      Assert.IsNotNull(basketOrder.TotalPrice);
      Assert.IsNotNull(basketOrder.OrderItems);
    }

    private void CheckItemCount(IBasketOrder basketOrder, int expectedCount)
    {
      Assert.AreEqual(expectedCount, basketOrder.OrderItems.Count());
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
    public void EmptyOrderXmlTest()
    {
      var gaOrderData = new BasketOrder(EmptyOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void EmptyItemsOrderTest()
    {
      var gaOrderData = new BasketOrder(EmptyItemsOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void EmptyOrderDetailOrderTest()
    {
      var gaOrderData = new BasketOrder(EmptyOrderDetailOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    public void MissingItemsOrderTest()
    {
      var gaOrderData = new BasketOrder(MissingItemsOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void MissingOrderDetailOrderTest()
    {
      var gaOrderData = new BasketOrder(MissingOrderDetailOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    public void OverlappingItemsOrderTest()
    {
      var gaOrderData = new BasketOrder(OverlappingItemsOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "A null orderXml was inappropriately allowed.")]
    public void NullOrderXDocumentTest()
    {
      var gaOrderData = new BasketOrder(null as XDocument);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "A null orderXml was inappropriately allowed.")]
    public void NullOrderStringTest()
    {
      var gaOrderData = new BasketOrder(null as string);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsEmpty(gaOrderData);
    }

    [TestMethod]
    public void NineItems_ValidOrderTest()
    {
      var gaOrderData = new BasketOrder(NineItems_ValidOrderXml);
      CheckNullAsserts(gaOrderData);
      CheckOrderItemsNotEmpty(gaOrderData);
      CheckItemCount(gaOrderData, 9);
    }
    
  }
}
