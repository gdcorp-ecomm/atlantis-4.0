using System.Diagnostics;
using Atlantis.Framework.DBSSellerNotInterested.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DBSSellerNotInterested.Tests
{
  [TestClass]
  public class DBSSellerNotInterestedTests
  {
    private const int _resourceId = 406914;

    private const string _shopperId = "842749";
    private const int _engineRequest = 302;

    public DBSSellerNotInterestedTests()
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
      DBSSellerNotInterestedRequestData _request = new DBSSellerNotInterestedRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _resourceId);
      DBSSellerNotInterestedResponseData _response = (DBSSellerNotInterestedResponseData)Engine.Engine.ProcessRequest(_request, _engineRequest);
      string ReturnData = _response.ReturnData;
      Debug.WriteLine("**********************");
      Debug.WriteLine(ReturnData);
      Debug.WriteLine("**********************");
      Debug.WriteLine(_response.IsSuccess);
      Assert.IsTrue(_response.IsSuccess);
    }
  }

}