using System;
using System.Diagnostics;
using Atlantis.Framework.HDVDSubmitRebootRequest.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HDVDSubmitRebootRequest.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHDVDSubmitRebootTests
  {

    private const string _shopperId = "858421";
    private const int _requestType = 483;
    private const string _accountUid = "d11319d0-4d10-11e1-83a0-0050569575d8";
	
    [TestMethod]
	  [DeploymentItem("atlantis.config")]
    public void CreateValidResponseTest()
    {
      var request = new HDVDSubmitRebootRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 1, _accountUid);
      
      Assert.IsNotNull(request, "request was invalid: null object");

      if (request.AccountUid.ToString() != _accountUid)
      {
        Assert.Fail("request was invalid: AccountUid not properly set");
      }

    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MakeRequestTest()
    {
      var request = new HDVDSubmitRebootRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 1, _accountUid);

      Assert.IsNotNull(request, "request was invalid: null object");

      if (request.AccountUid.ToString() != _accountUid)
      {
        Assert.Fail("request was invalid: AccountUid not properly set");
      }

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      var  response = Engine.Engine.ProcessRequest(request, _requestType) as HDVDSubmitRebootResponseData;
     
      Assert.IsNotNull(response);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);

    }

  }
}
