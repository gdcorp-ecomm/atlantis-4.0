using System;
using System.Data;
using System.Diagnostics;
using Atlantis.Framework.ResellerTopAcctByDate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ResellerTopAcctByDate.Tests
{
  [TestClass]
  public class ResellerTopAcctByDateTests
  {
    private const string _shopperId = "842749";
    private DateTime _startDate = Convert.ToDateTime("6/1/2010");
    private DateTime _endDate = Convert.ToDateTime("8/31/2010");
    private const int _numRows = 10;
    private const int _ResellerTopAcctByDateRequest = 249;

    public ResellerTopAcctByDateTests()
    {
    }

    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void WebServiceResponseTest()
    {
      ResellerTopAcctByDateRequestData _request = new ResellerTopAcctByDateRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _startDate, _endDate, _numRows);
      ResellerTopAcctByDateResponseData _response = (ResellerTopAcctByDateResponseData)Engine.Engine.ProcessRequest(_request, _ResellerTopAcctByDateRequest);
      DataTable ds = _response.ResellerList;
      _response = (ResellerTopAcctByDateResponseData)DataCache.DataCache.GetProcessRequest(_request, _ResellerTopAcctByDateRequest);
      ds = _response.ResellerList;

      int ff = ds.Rows.Count;
      Debug.WriteLine(ff);

      foreach (DataRow x in ds.Rows)
      {
        Debug.WriteLine("pl_id = " + x["pl_id"]);
        Debug.WriteLine("totalRevenue = " + x["totalRevenue"]);
        Debug.WriteLine("totalCommission = " + x["totalCommission"]);
      }
      Assert.IsTrue(_response.IsSuccess);
    }
  }

}
