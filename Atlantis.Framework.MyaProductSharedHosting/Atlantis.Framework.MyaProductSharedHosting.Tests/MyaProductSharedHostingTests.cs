using System;
using Atlantis.Framework.MyaProductSharedHosting.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductSharedHosting.Tests
{
  [TestClass]
  public class MyaProductSharedHostingTests
  {
    [TestMethod]
    public void GetMyaSharedHostingProductsValidShopperDefault()
    {
      MyaProductSharedHostingRequestData requestData = new MyaProductSharedHostingRequestData("822497",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      MyaProductSharedHostingResponseData responseData;

      try
      {
        responseData = (MyaProductSharedHostingResponseData)Engine.Engine.ProcessRequest(requestData, 179);

        Console.WriteLine(string.Format("Shared hosting product count: {0}", responseData.SharedHostingProducts.Count));
        foreach (SharedHostingProduct sharedHostingProduct in responseData.SharedHostingProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", sharedHostingProduct.CommonName, sharedHostingProduct.IsFree, sharedHostingProduct.AccountExpirationDate.ToLongDateString(), sharedHostingProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void GetMyaSharedHostingProductsValidShopperGetAll()
    {
      MyaProductSharedHostingRequestData requestData = new MyaProductSharedHostingRequestData("847235",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductSharedHostingResponseData responseData;

      try
      {
        responseData = (MyaProductSharedHostingResponseData)Engine.Engine.ProcessRequest(requestData, 179);

        Console.WriteLine(string.Format("Shared hosting product count: {0}", responseData.SharedHostingProducts.Count));
        foreach (SharedHostingProduct sharedHostingProduct in responseData.SharedHostingProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", sharedHostingProduct.CommonName, sharedHostingProduct.IsFree, sharedHostingProduct.AccountExpirationDate.ToLongDateString(), sharedHostingProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void GetMyaSharedHostingProductsValidShopperPage1With5PerPage()
    {
      MyaProductSharedHostingRequestData requestData = new MyaProductSharedHostingRequestData("847235",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = false;
      requestData.PagingInfo.CurrentPage = 1;
      requestData.PagingInfo.RowsPerPage = 5;

      MyaProductSharedHostingResponseData responseData;

      try
      {
        responseData = (MyaProductSharedHostingResponseData)Engine.Engine.ProcessRequest(requestData, 179);

        Console.WriteLine(string.Format("Shared hosting product count: {0}", responseData.SharedHostingProducts.Count));
        foreach (SharedHostingProduct sharedHostingProduct in responseData.SharedHostingProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", sharedHostingProduct.CommonName, sharedHostingProduct.IsFree, sharedHostingProduct.AccountExpirationDate.ToLongDateString(), sharedHostingProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.SharedHostingProducts.Count <= 5);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void GetMyaSharedHostingProductsInvalidShopper()
    {
      MyaProductSharedHostingRequestData requestData = new MyaProductSharedHostingRequestData("2326554512213554",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductSharedHostingResponseData responseData;

      try
      {
        responseData = (MyaProductSharedHostingResponseData)Engine.Engine.ProcessRequest(requestData, 179);

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.SharedHostingProducts.Count == 0);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
