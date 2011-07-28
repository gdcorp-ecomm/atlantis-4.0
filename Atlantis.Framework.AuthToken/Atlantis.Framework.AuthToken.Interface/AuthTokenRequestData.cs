using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthToken.Interface
{
  public class AuthTokenRequestData : RequestData
  {
    public AuthTokenRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId )
      : base( shopperId, sourceUrl, orderId, pathway, pageCount )
    {
      PrivateLabelId = privateLabelId;
    }

    public int PrivateLabelId { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException( "Auth Token is not a cacheable request." );
    }
  }
}
