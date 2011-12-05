using System.Diagnostics;
using Atlantis.Framework.DBSGetDomainShopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.DBSGetDomainShopper.Tests
{
  [TestClass]
  public class DBSGetDomainShopperTests
  {
    private const int _resourceId = 409152;
    private const string _shopperId = "842749";
    private const int _engineRequest = 303;

    public DBSGetDomainShopperTests()
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
      DBSGetDomainShopperRequestData _request = new DBSGetDomainShopperRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _resourceId);
      _request.RequestTimeout = TimeSpan.FromSeconds(4);
      DBSGetDomainShopperResponseData _response = (DBSGetDomainShopperResponseData)Engine.Engine.ProcessRequest(_request, _engineRequest);
      string ReturnData = _response.ReturnData;
      Debug.WriteLine("**********************");
      Debug.WriteLine(ReturnData);
      Debug.WriteLine("**********************");
      Debug.WriteLine("SellerShopperId = " + _response.SellerShopperId);
      //Assert.IsTrue(_response.IsSuccess);
    }
  }

}