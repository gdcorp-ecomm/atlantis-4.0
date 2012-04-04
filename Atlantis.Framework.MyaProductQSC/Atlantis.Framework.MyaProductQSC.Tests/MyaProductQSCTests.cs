using System;
using Atlantis.Framework.MyaProductQSC.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductQSC.Tests
{
  [TestClass]
  public class MyaProductQSCTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MyaProductQSC.Impl.dll")]
    [DeploymentItem("netConnect.dll")]
    public void GetMyaQSCProductsValidShopperDefault()
    {
      MyaProductQSCRequestData requestData = new MyaProductQSCRequestData("857020",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      MyaProductQSCResponseData responseData;

      try
      {
        responseData = (MyaProductQSCResponseData)Engine.Engine.ProcessRequest(requestData, 182);

        Console.WriteLine(string.Format("QSC product count: {0}", responseData.QSCProducts.Count));
        foreach (QSCProduct QSCProduct in responseData.QSCProducts)
        {
          Console.WriteLine("CommonName: {0}, Expiration: {1}, Account Expiration: {2}, IsRenwable: {3}", QSCProduct.CommonName, QSCProduct.AccountExpirationDate.ToLongDateString(), QSCProduct.AccountExpirationDate.ToLongDateString(), QSCProduct.IsRenewable);
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
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MyaProductQSC.Impl.dll")]
    [DeploymentItem("netConnect.dll")]
    public void GetMyaQSCProductsValidShopperGetAll()
    {
      MyaProductQSCRequestData requestData = new MyaProductQSCRequestData("857020",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductQSCResponseData responseData;

      try
      {
        responseData = (MyaProductQSCResponseData)Engine.Engine.ProcessRequest(requestData, 182);

        Console.WriteLine(string.Format("QSC product count: {0}", responseData.QSCProducts.Count));
        foreach (QSCProduct QSCProduct in responseData.QSCProducts)
        {
          Console.WriteLine("CommonName: {0}, Expiration: {1}, Account Expiration: {2}, IsRenwable: {3}", QSCProduct.CommonName, QSCProduct.AccountExpirationDate.ToLongDateString(), QSCProduct.AccountExpirationDate.ToLongDateString(), QSCProduct.IsRenewable);
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
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MyaProductQSC.Impl.dll")]
    [DeploymentItem("netConnect.dll")]
    public void GetMyaQSCProductsValidShopperPage1With5PerPage()
    {
      MyaProductQSCRequestData requestData = new MyaProductQSCRequestData("857020",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = false;
      requestData.PagingInfo.CurrentPage = 1;
      requestData.PagingInfo.RowsPerPage = 5;

      MyaProductQSCResponseData responseData;

      try
      {
        responseData = (MyaProductQSCResponseData)Engine.Engine.ProcessRequest(requestData, 182);

        Console.WriteLine(string.Format("QSC product count: {0}", responseData.QSCProducts.Count));
        foreach (QSCProduct QSCProduct in responseData.QSCProducts)
        {
          Console.WriteLine("CommonName: {0}, Expiration: {1}, Account Expiration: {2}, IsRenwable: {3}", QSCProduct.CommonName, QSCProduct.AccountExpirationDate.ToLongDateString(), QSCProduct.AccountExpirationDate.ToLongDateString(), QSCProduct.IsRenewable);
        }

        Console.WriteLine(string.Format("Current Page: {0}", requestData.PagingInfo.CurrentPage));
        Console.WriteLine(string.Format("Rows per Page: {0}", requestData.PagingInfo.RowsPerPage));
        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.QSCProducts.Count <= 5);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MyaProductQSC.Impl.dll")]
    [DeploymentItem("netConnect.dll")]
    public void GetMyaQSCProductsInvalidShopper()
    {
      MyaProductQSCRequestData requestData = new MyaProductQSCRequestData("2326554512213554",
                                                                                              1,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              0);

      requestData.PagingInfo.ReturnAll = true;

      MyaProductQSCResponseData responseData;

      try
      {
        responseData = (MyaProductQSCResponseData)Engine.Engine.ProcessRequest(requestData, 182);

        Console.WriteLine(string.Format("Total Records: {0}", responseData.PagingResult.TotalNumberOfRecords));
        Console.WriteLine(string.Format("Total Pages: {0}", responseData.PagingResult.TotalNumberOfPages));

        Assert.IsTrue(responseData.QSCProducts.Count == 0);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
