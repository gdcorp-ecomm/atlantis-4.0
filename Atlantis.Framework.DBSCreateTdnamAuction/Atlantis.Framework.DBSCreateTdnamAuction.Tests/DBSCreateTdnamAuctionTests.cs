using System.Diagnostics;
using Atlantis.Framework.DBSCreateTdnamAuction.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DBSCreateTdnamAuction.Tests
{
  [TestClass]
  public class DBSCreateTdnamAuctionTests
  {
   
    private const string _sellerShopperId = "862071";
    private const int _resourceId = 410069;
    private const int _sellerOfferPriceUSD = 10001;
    
    private const string _shopperId = "862071";
    private const int _engineRequest = 304;

    public DBSCreateTdnamAuctionTests()
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
      DBSCreateTdnamAuctionRequestData _request = new DBSCreateTdnamAuctionRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _sellerShopperId, _resourceId, _sellerOfferPriceUSD);
      DBSCreateTdnamAuctionResponseData _response = (DBSCreateTdnamAuctionResponseData)Engine.Engine.ProcessRequest(_request, _engineRequest);
      string ReturnData = _response.ReturnData;
      Debug.WriteLine("**********************");
      Debug.WriteLine(ReturnData);
      Debug.WriteLine("**********************");
      Debug.WriteLine("Error = " + _response.Error);
      Debug.WriteLine("IsSuccess = " + _response.IsSuccess);
      Assert.IsTrue(_response.IsSuccess);
    }
  }

}