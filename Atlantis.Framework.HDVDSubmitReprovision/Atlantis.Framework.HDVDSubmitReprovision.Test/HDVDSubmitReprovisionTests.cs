using System.Diagnostics;
using Atlantis.Framework.HDVDSubmitReprovision.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HDVDSubmitReprovision.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHDVDSubmitReprovisionTests
  {

    private const string _shopperId = "858421";
    private const int _requestType = 489;
    private const string accountUid = "d11319d0-4d10-11e1-83a0-0050569575d8";
    private const string serverName = "TESTSERVER02";
    private const string username = "kklinkdevtest";
    private const string password = "Password1";
    private const string osVersion = "CentOS 5";


    [TestMethod]
	  [DeploymentItem("atlantis.config")]
    public void HDVDSubmitReprovisionTest()
    {
      
      HDVDSubmitReprovisionRequestData request = new HDVDSubmitReprovisionRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0,
        accountUid,
        serverName, 
        username, 
        password,
        osVersion
        );

      HDVDSubmitReprovisionResponseData response = (HDVDSubmitReprovisionResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
	    Assert.IsTrue(response.IsSuccess);
      Debug.WriteLine(response.ToXML());

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
