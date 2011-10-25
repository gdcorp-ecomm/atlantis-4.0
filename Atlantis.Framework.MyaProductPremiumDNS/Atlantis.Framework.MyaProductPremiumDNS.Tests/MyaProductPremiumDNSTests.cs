using System;
using System.Diagnostics;
using Atlantis.Framework.MyaProductPremiumDNS.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductPremiumDNS.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetMyaProductPremiumDNSTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 276;

    public GetMyaProductPremiumDNSTests()
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
    public void GetMyaPremiumDNSProductsValidShopperDefault()
    {
      MyaProductPremiumDNSRequestData requestData = new MyaProductPremiumDNSRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1);

      MyaProductPremiumDNSResponseData responseData;
      try
      {
        responseData = (MyaProductPremiumDNSResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Premium DNS product count: {0}", responseData.PremiumDNSProducts.Count));
        foreach (PremiumDNSProduct premiumDNSProduct in responseData.PremiumDNSProducts)
        {
          premiumDNSProduct.CommonName = "Premium DNS";
          Debug.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenewable: {3}", premiumDNSProduct.CommonName, premiumDNSProduct.IsFree, premiumDNSProduct.AccountExpirationDate.ToLongDateString(), premiumDNSProduct.IsRenewable);
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
    public void GetMyaPremiumDNSProductsValidShopperGetAll()
    {
      MyaProductPremiumDNSRequestData requestData = new MyaProductPremiumDNSRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1); 

      requestData.PagingInfo.ReturnAll = true;

      MyaProductPremiumDNSResponseData responseData;

      try
      {
        responseData = (MyaProductPremiumDNSResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Premium DNS product count: {0}", responseData.PremiumDNSProducts.Count));
        foreach (PremiumDNSProduct premiumDNSProduct in responseData.PremiumDNSProducts)
        {
          Debug.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", premiumDNSProduct.CommonName, premiumDNSProduct.IsFree, premiumDNSProduct.AccountExpirationDate.ToLongDateString(), premiumDNSProduct.IsRenewable);
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
    public void GetMyaPremiumDNSProductsValidShopperPage1With5PerPage()
    {
      MyaProductPremiumDNSRequestData requestData = new MyaProductPremiumDNSRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1); 

      requestData.PagingInfo.ReturnAll = false;
      requestData.PagingInfo.CurrentPage = 1;
      requestData.PagingInfo.RowsPerPage = 5;

      MyaProductPremiumDNSResponseData responseData;

      try
      {
        responseData = (MyaProductPremiumDNSResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Console.WriteLine(string.Format("Premium DNS product count: {0}", responseData.PremiumDNSProducts.Count));
        foreach (PremiumDNSProduct premiumDNSProduct in responseData.PremiumDNSProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", premiumDNSProduct.CommonName, premiumDNSProduct.IsFree, premiumDNSProduct.AccountExpirationDate.ToLongDateString(), premiumDNSProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.PremiumDNSProducts.Count <= 5);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaPremiumDNSProductsInvalidShopper()
    {
      MyaProductPremiumDNSRequestData requestData = new MyaProductPremiumDNSRequestData("2326554512213554"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1); 

      requestData.PagingInfo.ReturnAll = true;

      MyaProductPremiumDNSResponseData responseData;

      try
      {
        responseData = (MyaProductPremiumDNSResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.PremiumDNSProducts.Count == 0);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }



  }
}
