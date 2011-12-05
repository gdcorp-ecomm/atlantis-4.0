using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMDowngrade.Interface
{
  public class EEMDowngradeRequestData : RequestData
  {
    public int BillingResourceId { get; private set; }
    public int DowngradeProductId { get; private set; }
    public int PrivateLabelId { get; private set; }
    public string EnteredBy { get; private set; }

    public EEMDowngradeRequestData(string shopperId,
                                   string sourceUrl,
                                   string orderId,
                                   string pathway,
                                   int pageCount,
                                   int billingResourceId,
                                   int downgradeProductId,
                                   int privateLabelId,
                                   string enteredBy)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BillingResourceId = billingResourceId;
      DowngradeProductId = downgradeProductId;
      PrivateLabelId = privateLabelId;
      EnteredBy = enteredBy;
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EEMDowngradeRequestData");
    }
  }
}
