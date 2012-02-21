using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateFTPBackupInfo.Interface
{
  public class HDVDValidateFTPBackupInfoRequestData : RequestData
  {
    public HDVDValidateFTPBackupInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string accountUid, string username, string password) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      Username = username;
      Password = password;
    }

    public string AccountUid { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HDVDValidateFTPBackupInfoRequestData");     
    }


  }
}
