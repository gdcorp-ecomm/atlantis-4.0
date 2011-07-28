using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthVerify.Interface
{
  public class AuthVerifyRequestData : RequestData
  {
    public AuthVerifyRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string loginName, string password, int privateLabelId, string ipAddress )
      : base( shopperId, sourceUrl, orderId, pathway, pageCount )
    {
      LoginName = loginName;
      Password = password;
      PrivateLabelId = privateLabelId;
      IpAddress = ipAddress;
    }

    public string LoginName { get; private set; }
    
    public string Password { get; private set; }
    
    public int PrivateLabelId { get; private set; }

    public string IpAddress { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException( "Auth Verify is not a cacheable request." );
    }
  }
}
