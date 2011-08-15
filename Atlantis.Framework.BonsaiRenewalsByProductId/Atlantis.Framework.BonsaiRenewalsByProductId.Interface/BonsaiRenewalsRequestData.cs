using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Interface
{
  public class BonsaiRenewalsRequestData : RequestData
  {
    public int UnifiedProductId { get; private set; }
    public int PrivateLabelId { get; private set; }

    public BonsaiRenewalsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int unifiedProductId, int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
