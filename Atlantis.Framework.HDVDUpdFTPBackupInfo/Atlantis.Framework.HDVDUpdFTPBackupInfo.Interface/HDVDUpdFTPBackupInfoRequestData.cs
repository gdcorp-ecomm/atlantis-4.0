using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDUpdFTPBackupInfo.Interface
{
  public class HDVDUpdFTPBackupInfoRequestData : RequestData
  {
    public HDVDUpdFTPBackupInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string accountUid, string username, string password) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      Username = username;
      Password = password;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HDVDUpdFTPBackupInfoRequestData");     
    }



    public string AccountUid { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
  }
}
