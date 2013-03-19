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

    #region IsSuccess == True Tests
    [TestMethod]
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncNoAddOnsTest()
    {
      var targetDate = DateTime.Parse("2/21/2013");
      var billDate1 = DateTime.Parse("2010-08-15 00:00:00.000");
      var billDate2 = DateTime.Parse("2010-05-15 00:00:00.000");
      var bsp1 = new BillingSyncProduct(1, 383377, "hosting", "a97bbfcd-48e3-11df-b65b-005056956427", billDate1, "monthly", 10055, 1, "blah");
      var bsp2 = new BillingSyncProduct(1, 383378, "hosting", "b020a244-48e3-11df-b65b-005056956427", billDate2, "monthly", 10055, 1, "blah");
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
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncNumberOfPeriodsGreaterThanOneTest()
    {
      var targetDate = DateTime.Parse("2/21/2013");
      var billDate1 = DateTime.Parse("2010-08-15 00:00:00.000");
      var billDate2 = DateTime.Parse("2010-05-15 00:00:00.000");
      var billDate3 = DateTime.Parse("2013-06-07 00:00:00.000");
      var bsp1 = new BillingSyncProduct(1, 383377, "hosting", "a97bbfcd-48e3-11df-b65b-005056956427", billDate1, "monthly", 10055, 1, "blah");
      var bsp2 = new BillingSyncProduct(1, 383378, "hosting", "b020a244-48e3-11df-b65b-005056956427", billDate2, "monthly", 10055, 1, "blah");
      var bsp3 = new BillingSyncProduct(1, 399173, "hosting", "bd2bfcf7-725c-11df-9145-005056956427", billDate3, "annual", 52014, 3, "blah");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp1, bsp2, bsp3 };

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

      var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
    }
    [TestMethod]
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncWithAddOnsTest()
    {
      var targetDate = DateTime.Parse("2/21/2013");
      var billDate1 = DateTime.Parse("2010-08-15 00:00:00.000");
      var billDate2 = DateTime.Parse("2010-05-15 00:00:00.000");
      var billDate3 = DateTime.Parse("2011-01-29 00:00:00.000");
      var bsp1 = new BillingSyncProduct(1, 383377, "hosting", "a97bbfcd-48e3-11df-b65b-005056956427", billDate1, "monthly", 10055, 1, "blah");
      var bsp2 = new BillingSyncProduct(1, 383378, "hosting", "b020a244-48e3-11df-b65b-005056956427", billDate2, "monthly", 10055, 1, "blah");
      var bsp3 = new BillingSyncProduct(1, 381156, "dedhost", "d423f0f3-0d15-11df-a185-005056956427", billDate3, "monthly", 11211, 1, "blah");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp1, bsp2, bsp3 };

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

      var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncInvalidOneOfTwoProductsRecurringTypeTest()
    {
      var dt = DateTime.Parse("2/8/2013");
      var billDate = DateTime.Parse("2013-06-07 00:00:00.000");
      var bsp = new BillingSyncProduct(1, 1, "blah", "abceian-anad0ah", DateTime.MinValue, "onetime", 11111, 1, "blah");
      var bsp2 = new BillingSyncProduct(1, 399173, "hosting", "bd2bfcf7-725c-11df-9145-005056956427", billDate, "annual", 52014, 3, "blah");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp, bsp2 };

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

      var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(request.BillingSyncProducts.Count.Equals(1));
      Assert.IsTrue(request.BillingSyncErrors.Count > 0);
      Assert.IsTrue(request.BillingSyncErrors[0].ErrorType == BillingSyncErrorData.BillingSyncErrorType.RenewalNotAllowedByRecurringType);
    }

    [TestMethod]
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncCashParkHybridTest()
    {
      var syncDate = DateTime.Parse("2/26/2013");
      var billDate = DateTime.Parse("2009-12-25 00:00:00.000");
      var bsp = new BillingSyncProduct(1, 361888, "cashparkHy", "c94be490-91f5-479c-b2d3-6b8e34013131", billDate, "annual", 16875, 1, "BESTTUCKER.COM");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp };

      var request = new EcommBillingSyncRequestData(SHOPPER_ID
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , billingSyncProducts
        , syncDate
        , "mya_web_billingsync"
        , "USD"
        , "127.0.0.1"
        , 1);

      var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(request.BillingSyncProducts.Count.Equals(1));
      Assert.IsTrue(request.BillingSyncErrors.Count.Equals(0));
    }

    [TestMethod]
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncSemiAnnualProduct()
    {
      var targetDate = DateTime.Parse("3/27/2013");
      var billDate1 = DateTime.Parse("2013-09-19 00:00:00.000");
      var bsp1 = new BillingSyncProduct(1, 1037810, "eem", "1c93236f-90e1-11e2-b132-0050569575d8", billDate1, "semiannual", 101177, 1, "New Account");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp1 };

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

      var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
    }
    #endregion 

    #region IsSuccess == False Tests
    [TestMethod]
    public void EcommBillingSyncInvalidDateTest()
    {
      var dt = DateTime.MaxValue;
      var bsp = new BillingSyncProduct(1, 1, "blah", "abceian-anad0ah", DateTime.MinValue, "annual", 11111, 1, "blah");
      var billingSyncProducts = new List<BillingSyncProduct> {bsp};

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

      try
      {
        var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);
      }
      catch (Exception)
      {
        Assert.IsTrue(request.RequestContainsInvalidSyncDate);
        Assert.IsTrue(request.BillingSyncErrors.Count > 0);
        Assert.IsTrue(request.BillingSyncErrors[0].ErrorType == BillingSyncErrorData.BillingSyncErrorType.InvalidSyncDate);
      }
    }

    [TestMethod]
    public void EcommBillingSyncInvalidProductRenewalByFlagTest()
    {
      var dt = DateTime.Parse("2/8/2013");
      var bsp = new BillingSyncProduct(0, 1, "blah", "abceian-anad0ah", DateTime.MinValue, "annual", 11111, 1, "blah");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp };

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

      try
      {
        var response = (EcommBillingSyncResponseData) Engine.Engine.ProcessRequest(request, REQUEST_TYPE);
      }
      catch
      {
        Assert.IsTrue(request.BillingSyncProducts.Count.Equals(0));
        Assert.IsTrue(request.BillingSyncErrors.Count > 0);
        Assert.IsTrue(request.BillingSyncErrors[0].ErrorType == BillingSyncErrorData.BillingSyncErrorType.RenewalNotAllowedByFlag);
      }
    }

    [TestMethod]
    public void EcommBillingSyncInvalidAllProductsRecurringTypeTest()
    {
      var dt = DateTime.Parse("2/8/2013");
      var bsp = new BillingSyncProduct(1, 1, "blah", "abceian-anad0ah", DateTime.MinValue, "onetime", 11111, 1, "blah");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp };

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

      try
      {
        var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);
      }
      catch (Exception)
      {
        Assert.IsTrue(request.BillingSyncProducts.Count.Equals(0));
        Assert.IsTrue(request.BillingSyncErrors.Count > 0);
        Assert.IsTrue(request.BillingSyncErrors[0].ErrorType == BillingSyncErrorData.BillingSyncErrorType.RenewalNotAllowedByRecurringType);
      }
    }
    #endregion

    #region PL Shopper Tests
    [TestMethod]
    [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
    [DeploymentItem("atlantis.framework.additem.impl.dll")]
    public void EcommBillingSyncResellerUnlimitedCalendar()
    {
      var targetDate = DateTime.Parse("3/27/2013");
      var billDate1 = DateTime.Parse("2015-03-19 00:00:00.000");
      var bsp1 = new BillingSyncProduct(1, 1037802, "calendar", "5710bcfb-90d2-11e2-b132-0050569575d8", billDate1, "annual", 10832, 2, "New Account");
      var billingSyncProducts = new List<BillingSyncProduct> { bsp1 };

      var request = new EcommBillingSyncRequestData("859675"  
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , billingSyncProducts
        , targetDate
        , "mya_web_billingsync"
        , "USD"
        , "127.0.0.1"
        , 440151);

      var response = (EcommBillingSyncResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.IsSuccess);
    }
    #endregion
  }
}
