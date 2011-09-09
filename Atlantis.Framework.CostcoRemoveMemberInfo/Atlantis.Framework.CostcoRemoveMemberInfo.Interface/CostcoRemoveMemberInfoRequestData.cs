using System;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.CostcoRemoveMemberInfo.Interface
{
  public class CostcoRemoveMemberInfoRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    public CostcoRemoveMemberInfoRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, _defaultRequestTimeout)
    { 
    }

    public CostcoRemoveMemberInfoRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = requestTimeout;    
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("This data is not cacheable");
    }

  }
}
