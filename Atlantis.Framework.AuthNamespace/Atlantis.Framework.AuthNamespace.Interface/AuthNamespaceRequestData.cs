using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthNamespace.Interface
{
  public class AuthNamespaceRequestData : RequestData
  {
    public AuthNamespaceRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string @namespace, string key, int privateLabelId )
      : base( shopperId, sourceUrl, orderId, pathway, pageCount )
    {
      Namespace = @namespace;
      Key = key;
      PrivateLabelId = privateLabelId;
    }

    public string  Namespace { get; private set; }

    public string Key { get; private set; }

    public int PrivateLabelId { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException( "Auth Namespace is not a cacheable request." );
    }
  }
}
