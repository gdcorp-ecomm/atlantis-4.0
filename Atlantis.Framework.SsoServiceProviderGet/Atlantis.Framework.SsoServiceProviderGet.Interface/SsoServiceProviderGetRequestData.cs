using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoServiceProviderGet.Interface
{
  public class SsoServiceProviderGetRequestData : RequestData
  {
    #region Properties

    public string ServiceProviderName { get; private set; }

    #endregion Properties

    public SsoServiceProviderGetRequestData(string shopperId
                                          , string sourceUrl
                                          , string orderId
                                          , string pathway
                                          , int pageCount
                                          , string serviceProviderName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(60d);
      ServiceProviderName = serviceProviderName;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SsoServiceProviderGetRequestData");
    }
  }
}
