using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.HDVDUpdFTPBackupInfo.Impl;
using Atlantis.Framework.HDVDUpdFTPBackupInfo.Interface;


namespace Atlantis.Framework.HDVDUpdFTPBackupInfo.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHDVDUpdFTPBackupInfoTests
  {

    private const string _shopperId = "858421"; 
    private const int _requestType = 999;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HDVDUpdFTPBackupInfoTest_usingNewPassword()
    {
      string accountUid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      string username = "kklinkdevtest";
      string password = "Password2";
      string oldPassword = "Password1";

      HDVDUpdFTPBackupInfoRequestData request = new HDVDUpdFTPBackupInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , accountUid
         , username
         , password
         );

      HDVDUpdFTPBackupInfoResponseData response = (HDVDUpdFTPBackupInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess, response.Response.Message);



      HDVDUpdFTPBackupInfoRequestData resetRequest = new HDVDUpdFTPBackupInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , accountUid
         , username
         , oldPassword
         );

      HDVDUpdFTPBackupInfoResponseData resetResponse = (HDVDUpdFTPBackupInfoResponseData)Engine.Engine.ProcessRequest(resetRequest, _requestType);
      Debug.WriteLine(resetResponse.ToXML());
      Assert.IsTrue(resetResponse.IsSuccess, resetResponse.Response.Message);

    
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HDVDUpdFTPBackupInfoTest_usingExistingPassword()
    {
      string accountUid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      string username = "kklinkdevtest";
      string password = "Password1";

      HDVDUpdFTPBackupInfoRequestData request = new HDVDUpdFTPBackupInfoRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , accountUid
         , username
         , password
         );

      HDVDUpdFTPBackupInfoResponseData response = (HDVDUpdFTPBackupInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      // Cache call
      //HDVDUpdFTPBackupInfoResponseData response = (HDVDUpdFTPBackupInfoResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //

      Debug.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess, response.Response.Message);
    }
  }
}
