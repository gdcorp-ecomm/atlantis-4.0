using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoValidateShopperAndGetTokenRequestData : RequestData
  {
    public string Username { get; private set; }  
    public string Password { get; private set; }
    public int PrivateLabelId { get; private set; }

#warning rename to validate
    public SsoValidateShopperAndGetTokenRequestData(string username, string password, int privateLabelId)
    {
      Username = username;
      Password = password;
      PrivateLabelId = privateLabelId;
    }
  }
}
