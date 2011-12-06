using System;
using System.Data;
using System.Diagnostics;
using Atlantis.Framework.ResellerCalculator.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ResellerCalculator.Tests
{
  [TestClass]
  public class ResellerCalculatorTests
  {
    private const string _shopperId = "842749";
    //private const string _xmlDoc = "<COMMISSIONCALCULATOR privateLabelResellerTypeID=\"2\"><PRODUCT pf_id=\"250001\" list_price=\"\" quantity=\"\" /><PRODUCT pf_id=\"270058\" list_price=\"\" quantity=\"\" /><PRODUCT pf_id=\"271865\" list_price=\"\" quantity=\"\" /><PRODUCT pf_id=\"272302\" list_price=\"\" quantity=\"\" /><PRODUCT pf_id=\"273604\" list_price=\"\" quantity=\"\" /><PRODUCT pf_id=\"277001\" list_price=\"\" quantity=\"\" /></COMMISSIONCALCULATOR>";
    private const string _xmlDoc = "<COMMISSIONCALCULATOR privateLabelResellerTypeID=\"2\"><PRODUCT pf_id=\"250001\" list_price=\"999999\" quantity=\"9999999\" /><PRODUCT pf_id=\"270058\" list_price=\"0\" quantity=\"0\" /><PRODUCT pf_id=\"271865\" list_price=\"0\" quantity=\"0\" /><PRODUCT pf_id=\"7501\" list_price=\"0\" quantity=\"0\" /><PRODUCT pf_id=\"273604\" list_price=\"0\" quantity=\"0\" /><PRODUCT pf_id=\"277001\" list_price=\"\" quantity=\"\" /></COMMISSIONCALCULATOR>";
    private const int _ResellerCalculatorRequest = 250;

    public ResellerCalculatorTests()
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

      ResellerCalculatorRequestData _request = new ResellerCalculatorRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _xmlDoc);
      _request.RequestTimeout = TimeSpan.FromSeconds(10);
      ResellerCalculatorResponseData _response = (ResellerCalculatorResponseData)Engine.Engine.ProcessRequest(_request, _ResellerCalculatorRequest);
      DataTable ds = _response.ResultTable;

      if (ds != null)
      {
        Debug.WriteLine("rows returned = " + ds.Rows.Count);
        Debug.WriteLine(string.Empty);
        foreach (DataRow v in ds.Rows)
        {
          foreach (DataColumn c in ds.Columns)
          {
            Debug.WriteLine(c + " = " + v[c]);
          }
          Debug.WriteLine(string.Empty);
        }
      }
      Assert.IsTrue(_response.IsSuccess);
    }
  }

}
