using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SmartSpaceRenewalBasketXml.Interface
{
  public class SmartSpaceRenewalBasketXmlRequestData : RequestData
  {
    public enum BillingResourceIdType
    {
      Domain,
      SmartSpace
    }

    public int BillingResourceId { get; private set; }

    public BillingResourceIdType ResourceIdType { get; set; }

    public string Sld { get; private set; }

    public string Tld { get; private set; }

    public int SmartSpaceRenewalProductId { get; private set; }

    public int Duration { get; private set; }
    
    public SmartSpaceRenewalBasketXmlRequestData(int billingResourceId,
                                                 BillingResourceIdType billingResourceIdType,
                                                 string domainSld,
                                                 string domainTld,
                                                 int smartSpaceRenewalProductId, 
                                                 int duration,
                                                 string shopperID, 
                                                 string sourceUrl, 
                                                 string orderID, 
                                                 string pathway,
                                                 int pageCount) : base(shopperID, sourceUrl, orderID, pathway, pageCount)
    {
      BillingResourceId = billingResourceId;
      ResourceIdType = billingResourceIdType;
      Sld = domainSld;
      Tld = domainTld;
      SmartSpaceRenewalProductId = smartSpaceRenewalProductId;
      Duration = duration;
      RequestTimeout = new TimeSpan(0, 0, 10);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("SmartSpaceRenewalBasketXmlRequestData is not a cacheable request.");
    }
  }
}
