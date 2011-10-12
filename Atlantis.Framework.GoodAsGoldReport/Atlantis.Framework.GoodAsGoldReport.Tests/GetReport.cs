using System;
using System.Data;
using Atlantis.Framework.GoodAsGoldReport.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GoodAsGoldReport.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetReport
  {

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetReportTest()
    {
      DateTime sdate = new DateTime(2011,1,1);
      DateTime edate = new DateTime(2012,1,1);
      GoodAsGoldReportRequestData request = new GoodAsGoldReportRequestData("860431", string.Empty, string.Empty, string.Empty, 0, sdate, edate, 5, 2, "displayDate", "asc", "Withdrawal");

      GoodAsGoldReportResponseData response = (GoodAsGoldReportResponseData)Engine.Engine.ProcessRequest(request, 405);

      if (response.IsSuccess)
      {
        DataSet ds = response.ReportDataSet;

        string a = response.ToXML();
      }
      Assert.IsTrue(response.IsSuccess);
    }

  }
}
