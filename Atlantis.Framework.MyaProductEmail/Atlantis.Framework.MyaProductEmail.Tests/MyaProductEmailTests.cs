using System;
using Atlantis.Framework.MyaProductEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductEmail.Tests
{
  [TestClass]
  public class MyaProductEmailTests
  {
    [TestMethod]
    public void GetMyaEmailProductsValidShopperDefault()
    {
      MyaProductEmailRequestData requestData = new MyaProductEmailRequestData("847235",
                                                                              1,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              0);

      MyaProductEmailResponseData responseData;

      try
      {
        responseData = (MyaProductEmailResponseData)Engine.Engine.ProcessRequest(requestData, 180);

        Console.WriteLine(string.Format("Email product count: {0}", responseData.EmailProducts.Count));
        foreach (EmailProduct emailProduct in responseData.EmailProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", emailProduct.CommonName, emailProduct.IsFree, emailProduct.AccountExpirationDate.ToLongDateString(), emailProduct.IsRenewable);
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
    public void GetMyaEmailProductsValidShopperGetAll()
    {
      MyaProductEmailRequestData requestData = new MyaProductEmailRequestData("847235",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductEmailResponseData responseData;

      try
      {
        responseData = (MyaProductEmailResponseData)Engine.Engine.ProcessRequest(requestData, 180);

        Console.WriteLine(string.Format("Email product count: {0}", responseData.EmailProducts.Count));
        foreach (EmailProduct emailProduct in responseData.EmailProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", emailProduct.CommonName, emailProduct.IsFree, emailProduct.AccountExpirationDate.ToLongDateString(), emailProduct.IsRenewable);
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
    public void GetMyaEmailProductsValidShopperPage1With5PerPage()
    {
      MyaProductEmailRequestData requestData = new MyaProductEmailRequestData("847235",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = false;
      requestData.PagingInfo.CurrentPage = 1;
      requestData.PagingInfo.RowsPerPage = 5;

      MyaProductEmailResponseData responseData;

      try
      {
        responseData = (MyaProductEmailResponseData)Engine.Engine.ProcessRequest(requestData, 180);

        Console.WriteLine(string.Format("Email product count: {0}", responseData.EmailProducts.Count));
        foreach (EmailProduct emailProduct in responseData.EmailProducts)
        {
          Console.WriteLine("CommonName: {0}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", emailProduct.CommonName, emailProduct.IsFree, emailProduct.AccountExpirationDate.ToLongDateString(), emailProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.EmailProducts.Count <= 5);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void GetMyaEmailProductsInvalidShopper()
    {
      MyaProductEmailRequestData requestData = new MyaProductEmailRequestData("2326554512213554",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductEmailResponseData responseData;

      try
      {
        responseData = (MyaProductEmailResponseData)Engine.Engine.ProcessRequest(requestData, 180);

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.EmailProducts.Count == 0);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
