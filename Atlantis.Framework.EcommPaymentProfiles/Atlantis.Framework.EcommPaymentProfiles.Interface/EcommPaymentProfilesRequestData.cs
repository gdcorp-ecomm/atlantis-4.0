using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPaymentProfiles.Interface
{
  public class EcommPaymentProfilesRequestData : RequestData
  {
    public int MaxProfileCount { get; set; }
    public EcommPaymentProfilesRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public EcommPaymentProfilesRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int maxProfileCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      MaxProfileCount = maxProfileCount;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommPaymentProfilesRequestData");     
    }
  }
}
