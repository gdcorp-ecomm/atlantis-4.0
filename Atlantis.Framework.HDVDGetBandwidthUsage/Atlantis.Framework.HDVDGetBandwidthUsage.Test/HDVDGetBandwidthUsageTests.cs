using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.HDVDGetBandwidthUsage.Impl;
using Atlantis.Framework.HDVDGetBandwidthUsage.Interface;


namespace Atlantis.Framework.HDVDGetBandwidthUsage.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHDVDGetBandwidthUsageTests
  {

    private const string _shopperId = "858421";
    private const int _requestType = 999;
    private string _accountUid = "d11319d0-4d10-11e1-83a0-0050569575d8";


    [TestMethod]
	[DeploymentItem("atlantis.config")]
    public void HDVDGetBandwidthUsageTest()
    {
     HDVDGetBandwidthUsageRequestData request = new HDVDGetBandwidthUsageRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 
        , _accountUid
        );

      HDVDGetBandwidthUsageResponseData response = (HDVDGetBandwidthUsageResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
	    Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
