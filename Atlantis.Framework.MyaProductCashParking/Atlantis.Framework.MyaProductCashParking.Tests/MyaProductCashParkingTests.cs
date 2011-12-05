using System;
using System.Diagnostics;
using Atlantis.Framework.MyaProductCashParking.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductCashParking.Tests
{
  [TestClass]
  public class GetMyaProductCashParkingTests
  {
    private const string _shopperId = "859147";
    private const int _requestType = 314;

    public GetMyaProductCashParkingTests()
    { }

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
    public void GetMyaCashParkingProductsValidShopperDefault()
    {
      MyaProductCashParkingRequestData requestData = new MyaProductCashParkingRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1);

      MyaProductCashParkingResponseData responseData;
      try
      {
        responseData = (MyaProductCashParkingResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Cash Parking product count: {0}", responseData.CashParkingProducts.Count));
        foreach (CashParkingProduct cashParkingProduct in responseData.CashParkingProducts)
        {
          cashParkingProduct.CommonName = "Cash Parking";
          Debug.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenewable: {3}", cashParkingProduct.CommonName, cashParkingProduct.IsFree, cashParkingProduct.AccountExpirationDate.ToLongDateString(), cashParkingProduct.IsRenewable);
        }

        Debug.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Debug.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Debug.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Debug.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaCashParkingProductsValidShopperGetAll()
    {
      MyaProductCashParkingRequestData requestData = new MyaProductCashParkingRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductCashParkingResponseData responseData;

      try
      {
        responseData = (MyaProductCashParkingResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Cash Parking product count: {0}", responseData.CashParkingProducts.Count));
        foreach (CashParkingProduct cashParkingProduct in responseData.CashParkingProducts)
        {
          Debug.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", cashParkingProduct.CommonName, cashParkingProduct.IsFree, cashParkingProduct.AccountExpirationDate.ToLongDateString(), cashParkingProduct.IsRenewable);
        }

        Debug.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Debug.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaCashParkingProductsValidShopperPage1With5PerPage()
    {
      MyaProductCashParkingRequestData requestData = new MyaProductCashParkingRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1);

      requestData.PagingInfo.ReturnAll = false;
      requestData.PagingInfo.CurrentPage = 1;
      requestData.PagingInfo.RowsPerPage = 5;

      MyaProductCashParkingResponseData responseData;

      try
      {
        responseData = (MyaProductCashParkingResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Console.WriteLine(string.Format("Cash Parking product count: {0}", responseData.CashParkingProducts.Count));
        foreach (CashParkingProduct cashParkingProduct in responseData.CashParkingProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", cashParkingProduct.CommonName, cashParkingProduct.IsFree, cashParkingProduct.AccountExpirationDate.ToLongDateString(), cashParkingProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.CashParkingProducts.Count <= 5);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaCashParkingProductsInvalidShopper()
    {
      MyaProductCashParkingRequestData requestData = new MyaProductCashParkingRequestData("2326554512213554"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductCashParkingResponseData responseData;

      try
      {
        responseData = (MyaProductCashParkingResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.CashParkingProducts.Count == 0);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
