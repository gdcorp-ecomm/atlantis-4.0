using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitReprovision.Interface
{
  public class HDVDSubmitReprovisionRequestData : RequestData
  {
    public HDVDSubmitReprovisionRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string accountUid, string serverName, string username, string password, string osVersion) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      ServerName = serverName;
      Username = username;
      Password = password;
      OSVersion = osVersion;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HDVDSubmitReprovisionRequestData");     
    }

    public string AccountUid { get; set; }
    public string ServerName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string OSVersion { get; set; }
  }
}
