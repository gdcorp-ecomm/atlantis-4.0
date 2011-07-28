using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthAuthorize.Interface
{
  public class AuthAuthorizeRequestData : RequestData
  {
    public AuthAuthorizeRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string loginName, string password, int privateLabelId, string ipAddress )
      : base( shopperId, sourceUrl, orderId, pathway, pageCount )
    {
      LoginName = loginName;
      Password = password;
      PrivateLabelId = privateLabelId;
      IpAddress = ipAddress;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public string LoginName { get; private set; }
    
    public string Password { get; private set; }
    
    public int PrivateLabelId { get; private set; }

    public string IpAddress { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException( "Auth Authorize is not a cacheable request." );
    }
  }
}
