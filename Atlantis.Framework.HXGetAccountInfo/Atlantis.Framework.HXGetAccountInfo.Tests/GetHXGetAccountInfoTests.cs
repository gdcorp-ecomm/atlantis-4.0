using System;
using Atlantis.Framework.HXGetAccountInfo.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HXGetAccountInfo.Tests
{
  [TestClass]
  public class GetHXGetAccountInfoTests
  {
    private const string _shopperId = "";
    private const string _accountUid = "239724cf-d1a7-11df-8839-005056952fd6"; // valid for test env
    //private const string _accountUid = "48a6b224-0af5-11e1-9ffb-0050569575d8"; // valid for dev env
    private const int _requestType = 278;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HXGetAccountInfoRequestTest()
    {
      HXGetAccountInfoRequestData request = new HXGetAccountInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _accountUid);

      request.RequestTimeout = new TimeSpan(0, 0, 25);

      HXGetAccountInfoResponseData response = (HXGetAccountInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    [DeploymentItem("atlantis.config")]
    public void HXGetAccountInfoRequestTest_Bad_AccountUid()
    {
      string badAccountUid = "10000000-0000-0000-0000-000000000000";
      HXGetAccountInfoRequestData request = new HXGetAccountInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , badAccountUid);

      HXGetAccountInfoResponseData response = (HXGetAccountInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void HXGetAccountInfoRequestDataTest_Null_AccountUid()
    {
         HXGetAccountInfoRequestData request = new HXGetAccountInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , null);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void HXGetAccountInfoRequestDataTest_EmptyString_AccountUid()
    {
      HXGetAccountInfoRequestData request = new HXGetAccountInfoRequestData(_shopperId
      , string.Empty
      , string.Empty
      , string.Empty
      , 0
      , string.Empty);
    }
  }
}
