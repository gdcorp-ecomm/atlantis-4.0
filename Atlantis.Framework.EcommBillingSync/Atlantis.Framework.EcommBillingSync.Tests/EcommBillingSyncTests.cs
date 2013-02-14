using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommBillingSync.Interface;

namespace Atlantis.Framework.EcommBillingSync.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("atlantis.framework.ecommbillingsync.impl.dll")]
  public class GetEcommBillingSyncTests
  {
    private const string SHOPPER_ID = "856907";
    private const int REQUEST_TYPE = 647;
	
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
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncNoAddOnsTest()
    {
      var targetDate = DateTime.Parse("2/21/2013");
      var billDate1 = DateTime.Parse("2010-08-15 00:00:00.000");
      var billDate2 = DateTime.Parse("2010-05-15 00:00:00.000");
      var bsp1 = new BillingSyncProduct(1, 383377, "hosting", "a97bbfcd-48e3-11df-b65b-005056956427", billDate1, "monthly", 10055, 1);
      var bsp2 = new BillingSyncProduct(1, 383378, "hosting", "b020a244-48e3-11df-b65b-005056956427", billDate2, "monthly", 10055, 1);
      var billingSyncProducts = new List<BillingSyncProduct> { bsp1, bsp2 };

      var request = new EcommBillingSyncRequestData(SHOPPER_ID
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , billingSyncProducts
        , targetDate
        , "mya_web_billingsync"
        , "USD"
        , "127.0.0.1"
        , 1);

      var response = (EcommBillingSyncResponseData) Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void EcommBillingSyncInvalidDateTest()
    {
      var dt = DateTime.MaxValue;
      var bsp = new BillingSyncProduct(1, 1, "blah", "abceian-anad0ah", DateTime.MinValue, "annual", 11111, 1);
      var billingSyncProducts = new List<BillingSyncProduct> {bsp};

      try
      {
        var request = new EcommBillingSyncRequestData(SHOPPER_ID
          , string.Empty
          , string.Empty
          , string.Empty
          , 0
          , billingSyncProducts
          , dt
          , "mya_web_billingsync"
          , "USD"
          , "127.0.0.1"
          , 1);
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex.Message.Equals("Synchronization date must be between 1 & 28 inclusive"));
      }
    }

    [TestMethod]
    public void EcommBillingSyncInvalidBillingSyncProductTest()
    {
      var dt = DateTime.Parse("2/8/2013");
      var bsp = new BillingSyncProduct(1, 1, "blah", "abceian-anad0ah", DateTime.MinValue, "annual", 11111, 1);
      var billingSyncProducts = new List<BillingSyncProduct> { bsp };

      try
      {
        var request = new EcommBillingSyncRequestData(SHOPPER_ID
          , string.Empty
          , string.Empty
          , string.Empty
          , 0
          , billingSyncProducts
          , dt
          , "mya_web_billingsync"
          , "USD"
          , "127.0.0.1"
          , 1);
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex.Message.Contains("Sychronization list includes product ineligible for renewal"));
      }
    }
  }
}
