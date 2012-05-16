using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPrunedActivationData.Interface
{
  public class EcommPrunedActivationDataRequestData : RequestData
  {

    public EcommPrunedActivationDataRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("Do not Implement Caching on Activation Data");     
    }
  }
}
