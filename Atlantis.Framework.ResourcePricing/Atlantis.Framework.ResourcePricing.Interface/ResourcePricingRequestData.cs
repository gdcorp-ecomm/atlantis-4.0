using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.ResourcePricing.Interface
{
  public class ResourcePricingRequestData : RequestData
  {
    public string ResourceId { get; private set; }
    public string ResourceType { get; private set; }
    public string IdType { get; private set; } // billing or orion
    public ICurrencyInfo TransactionalCurrencyType { get; private set; }
    public int[] AdditionalUnifiedProductIds { get; private set; }

    public ResourcePricingRequestData(string shopperId
                                    , string sourceUrl
                                    , string orderId
                                    , string pathway
                                    , int pageCount
                                    , string resourceId
                                    , string resourceType
                                    , string idType
                                    , ICurrencyInfo transactionalCurrencyType)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(60d);
      ResourceId = resourceId;
      ResourceType = resourceType;
      IdType = idType;
      TransactionalCurrencyType = transactionalCurrencyType;
    }

    public ResourcePricingRequestData(string shopperId
                                    , string sourceUrl
                                    , string orderId
                                    , string pathway
                                    , int pageCount
                                    , string resourceId
                                    , string resourceType
                                    , string idType
                                    , ICurrencyInfo transactionalCurrencyType
                                    , int[] addlUnifiedProductIdList)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, resourceId, resourceType, idType, transactionalCurrencyType)
    {
      AdditionalUnifiedProductIds = addlUnifiedProductIdList;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ResourcePricingRequestData");
    }
  }
}
