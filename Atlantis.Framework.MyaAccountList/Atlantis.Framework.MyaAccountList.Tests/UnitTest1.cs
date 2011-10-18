using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MyaAccountList.Interface;

namespace Atlantis.Framework.MyaAccountList.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void TestMethod1()
    {
      //MyaAccountListRequestData request = new MyaAccountListRequestData("850774", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp", 5, 2, "asc", null, 0, 0, null);
      MyaAccountListRequestData request = new MyaAccountListRequestData("850774", string.Empty, string.Empty, string.Empty, 0, "mya_accountListGetHosting_sp", returnAllFlag:1);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.ReportDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
      }
      Assert.IsTrue(response.IsSuccess);

    }
  }
}
