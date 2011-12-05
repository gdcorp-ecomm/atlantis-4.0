using System.Diagnostics;
using Atlantis.Framework.DBSSellerInterested.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DBSSellerInterested.Tests
{
  [TestClass]
  public class DBSSellerInterestedTests
  {
    private const int _resourceId = 410069;
    private const int _claimId = 719;
    private const string _managerUserId = "-1";
    
    private const string _shopperId = "842749";
    private const int _engineRequest = 301;

    public DBSSellerInterestedTests()
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
      DBSSellerInterestedRequestData _request = new DBSSellerInterestedRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _resourceId, _claimId, _managerUserId);
      DBSSellerInterestedResponseData _response = (DBSSellerInterestedResponseData)Engine.Engine.ProcessRequest(_request, _engineRequest);
      string ReturnData = _response.ReturnData;
      Debug.WriteLine("**********************");
      Debug.WriteLine(ReturnData);
      Debug.WriteLine("**********************");
      Debug.WriteLine(_response.IsSuccess);
      Assert.IsTrue(_response.IsSuccess);
    }
  }

}