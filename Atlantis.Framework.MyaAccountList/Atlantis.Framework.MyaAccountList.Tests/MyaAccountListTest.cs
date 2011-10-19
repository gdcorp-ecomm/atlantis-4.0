using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MyaAccountList.Interface;
using System.Diagnostics;

namespace Atlantis.Framework.MyaAccountList.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MyaAccountListTest
  {
    public MyaAccountListTest()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetFreeAccountsOnly()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData("856907", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp", returnFreeListOnly: 1);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.ReportDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetDefault5AccountListPage1()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData("856907", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp");

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetDefault5AccountListPage2()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData("856907", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp", currentPage: 2);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetAccountsExpiringIn5Days()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData("856907", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp", daysTillExpiration: 5);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetAllAccounts()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData("856907", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp", returnAllFlag: 1);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
