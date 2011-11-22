using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetAuthenticationGuid.Interface
{
  public class EEMGetAuthenticationGuidRequestData : RequestData
  {
    public int CustomerId { get; private set; }

    public EEMGetAuthenticationGuidRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int customerId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CustomerId = customerId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EEMGetAuthenticationGuidRequestData");     
    }
  }
}
