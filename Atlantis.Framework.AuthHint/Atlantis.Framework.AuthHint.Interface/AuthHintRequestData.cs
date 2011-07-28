using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthHint.Interface
{
  public class AuthHintRequestData : RequestData
  {
    public AuthHintRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string loginName, int privateLabelId, string street )
      : base( shopperId, sourceUrl, orderId, pathway, pageCount )
    {
      LoginName = loginName;
      PrivateLabelId = privateLabelId;
      Street = street;
    }

    public string LoginName { get; private set; }

    public int PrivateLabelId { get; private set; }
    
    public string Street { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException( "Auth Reset Password is not a cacheable request." );
    }
  }
}
