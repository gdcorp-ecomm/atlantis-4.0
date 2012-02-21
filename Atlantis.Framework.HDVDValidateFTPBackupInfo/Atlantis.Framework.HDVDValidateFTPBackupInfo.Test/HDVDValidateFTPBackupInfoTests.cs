using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.HDVDValidateFTPBackupInfo.Impl;
using Atlantis.Framework.HDVDValidateFTPBackupInfo.Interface;


namespace Atlantis.Framework.HDVDValidateFTPBackupInfo.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHDVDValidateFTPBackupInfoTests
  {

    private const string _shopperId = "858421";
    private const int _requestType = 999;
    private string _accountUid = "d11319d0-4d10-11e1-83a0-0050569575d8";
    private string _username = "kklinkdevtest";
    private string _password = "Password1";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HDVDValidateFTPBackupInfoTest()
    {
      HDVDValidateFTPBackupInfoRequestData request = new HDVDValidateFTPBackupInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         ,_accountUid
         ,_username
         ,_password);

      HDVDValidateFTPBackupInfoResponseData response = (HDVDValidateFTPBackupInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
