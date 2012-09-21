using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourcePricing.Interface
{
  public class ResourcePricingRequestData : RequestData
  {
    public string ResourceID { get; private set; }
    public string ResourceType { get; private set; }
    public string IDType { get; private set; }
    public string Currency { get; private set; }
    public string AddlUnifiedProductIDList { get; private set; }

    public ResourcePricingRequestData(string shopperId
                                    , string sourceUrl
                                    , string orderId
                                    , string pathway
                                    , int pageCount
                                    , string resourceId
                                    , string resourceType
                                    , string idType
                                    , string currency
                                    , string addlUnifiedProductIdList)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(60d);
      ResourceID = resourceId;
      ResourceType = resourceType;
      IDType = idType;
      Currency = currency;
      AddlUnifiedProductIDList = addlUnifiedProductIdList;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ResourcePricingRequestData");
    }
  }
}
