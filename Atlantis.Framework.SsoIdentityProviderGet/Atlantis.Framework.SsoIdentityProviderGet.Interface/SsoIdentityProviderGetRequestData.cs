using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoIdentityProviderGet.Interface
{
  public class SsoIdentityProviderGetRequestData : RequestData
  {
    #region Properties

    public string IdentityProviderName { get; private set; }

    #endregion Properties

    public SsoIdentityProviderGetRequestData(string shopperId
                                          , string sourceUrl
                                          , string orderId
                                          , string pathway
                                          , int pageCount
                                          , string identityProviderName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(60d);
      IdentityProviderName = identityProviderName;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SsoIdentityProviderGetRequestData");
    }
  }
}