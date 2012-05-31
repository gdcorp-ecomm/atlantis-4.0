using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoProviderGroupGet.Interface
{
  public class SsoProviderGroupGetRequestData : RequestData
  {
    #region Properties

    public string ServiceProviderGroupName { get; private set; }

    #endregion Properties

    public SsoProviderGroupGetRequestData(string shopperId
                                          , string sourceUrl
                                          , string orderId
                                          , string pathway
                                          , int pageCount
                                          , string serviceProviderGroupName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(60d);
      ServiceProviderGroupName = serviceProviderGroupName;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SsoProviderGroupGetRequestData");
    }
  }
}