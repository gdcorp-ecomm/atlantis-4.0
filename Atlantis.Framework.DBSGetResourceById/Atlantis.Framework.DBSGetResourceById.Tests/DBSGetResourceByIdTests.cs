using System;
using System.Data;
using System.Diagnostics;
using Atlantis.Framework.DBSGetResourceById.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DBSGetResourceById.Tests
{
  [TestClass]
  public class DBSGetResourceByIdTests
  {
    private const string _shopperId = "822497";
    private const int _resourceId = 406914;
    private const int _DBSGetResourceByIdRequest = 296;

    public DBSGetResourceByIdTests()
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
      
      DBSGetResourceByIdRequestData _request = new DBSGetResourceByIdRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _resourceId);
      _request.RequestTimeout = TimeSpan.FromSeconds(10);
      DBSGetResourceByIdResponseData _response = (DBSGetResourceByIdResponseData)Engine.Engine.ProcessRequest(_request, _DBSGetResourceByIdRequest);
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
