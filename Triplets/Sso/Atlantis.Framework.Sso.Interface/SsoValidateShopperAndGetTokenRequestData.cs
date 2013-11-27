using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoValidateShopperAndGetTokenRequestData : RequestData
  {
    public string Username { get; private set; }  
    public string Password { get; private set; }
    public int PrivateLabelId { get; private set; }
    public string ClientIp { get; private set; }
    public string UserAgent { get; private set; }

    public SsoValidateShopperAndGetTokenRequestData(string username, string password, int privateLabelId, string clientIp, string userAgent = "")
    {
      Username = username;
      Password = password;
      PrivateLabelId = privateLabelId;
      ClientIp = clientIp;
      RequestTimeout = new TimeSpan(0, 0, 5);
      UserAgent = userAgent;
    }
  }
}
