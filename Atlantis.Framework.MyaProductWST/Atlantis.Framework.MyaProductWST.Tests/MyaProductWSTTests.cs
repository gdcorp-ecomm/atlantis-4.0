using System;
using Atlantis.Framework.MyaProductWST.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductWST.Tests
{
  [TestClass]
  public class MyaProductWSTTests
  {
    [TestMethod]
    public void GetMyaWSTProductsValidShopperDefault()
    {
      MyaProductWSTRequestData requestData = new MyaProductWSTRequestData("857020",
                                                                          1,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          0);

      MyaProductWSTResponseData responseData;

      try
      {
        responseData = (MyaProductWSTResponseData)Engine.Engine.ProcessRequest(requestData, 181);

        Console.WriteLine(string.Format("WST product count: {0}", responseData.WSTProducts.Count));
        foreach (WSTProduct WSTProduct in responseData.WSTProducts)
        {
          Console.WriteLine("CommonName: {0}, Expiration: {1}, Is Free: {2}, IsRenwable: {3}, Has Maintenance: {4}, Custom Design Pfid: {5}", WSTProduct.CommonName, WSTProduct.AccountExpirationDate.ToLongDateString(), WSTProduct.IsFree, WSTProduct.IsRenewable, WSTProduct.HasMaintenance, WSTProduct.DesignProductId == null ? "None" : WSTProduct.DesignProductId.Value.ToString());
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
    public void GetMyaWSTProductsValidShopperGetAll()
    {
      MyaProductWSTRequestData requestData = new MyaProductWSTRequestData("857020",
                                                                          1,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductWSTResponseData responseData;

      try
      {
        responseData = (MyaProductWSTResponseData)Engine.Engine.ProcessRequest(requestData, 181);

        Console.WriteLine(string.Format("WST product count: {0}", responseData.WSTProducts.Count));
        foreach (WSTProduct WSTProduct in responseData.WSTProducts)
        {
          Console.WriteLine("CommonName: {0}, Expiration: {1}, Is Free: {2}, IsRenwable: {3}, Has Maintenance: {4}, Custom Design Pfid: {5}", WSTProduct.CommonName, WSTProduct.AccountExpirationDate.ToLongDateString(), WSTProduct.IsFree, WSTProduct.IsRenewable, WSTProduct.HasMaintenance, WSTProduct.DesignProductId == null ? "None" : WSTProduct.DesignProductId.Value.ToString());
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
    public void GetMyaWSTProductsValidShopperPage1With5PerPage()
    {
      MyaProductWSTRequestData requestData = new MyaProductWSTRequestData("857020",
                                                                          1,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          0);

      requestData.PagingInfo.ReturnAll = false;
      requestData.PagingInfo.CurrentPage = 1;
      requestData.PagingInfo.RowsPerPage = 5;

      MyaProductWSTResponseData responseData;

      try
      {
        responseData = (MyaProductWSTResponseData)Engine.Engine.ProcessRequest(requestData, 181);

        Console.WriteLine(string.Format("WST product count: {0}", responseData.WSTProducts.Count));
        foreach (WSTProduct WSTProduct in responseData.WSTProducts)
        {
          Console.WriteLine("CommonName: {0}, Expiration: {1}, Is Free: {2}, IsRenwable: {3}, Has Maintenance: {4}, Custom Design Pfid: {5}", WSTProduct.CommonName, WSTProduct.AccountExpirationDate.ToLongDateString(), WSTProduct.IsFree, WSTProduct.IsRenewable, WSTProduct.HasMaintenance, WSTProduct.DesignProductId == null ? "None" : WSTProduct.DesignProductId.Value.ToString());
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.WSTProducts.Count <= 5);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void GetMyaWSTProductsInvalidShopper()
    {
      MyaProductWSTRequestData requestData = new MyaProductWSTRequestData("2326554512213554",
                                                                          1,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          string.Empty,
                                                                          0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductWSTResponseData responseData;

      try
      {
        responseData = (MyaProductWSTResponseData)Engine.Engine.ProcessRequest(requestData, 181);

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.WSTProducts.Count == 0);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
