using System;
using System.Collections.Generic;

using Atlantis.Framework.BonsaiPlanFeatures.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiPlanFeatures.Interface
{
  public class BonsaiPlanFeaturesRequestData : RequestData
  {
    public int UnifiedProductId { get; private set; }
    public string ProductNamespace { get; private set; }
    public bool IsFree { get; private set; }
    public List<UnifiedProductIdOverride> Overrides { get; private set; }

    public BonsaiPlanFeaturesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int unifiedProductId, string productNamespace, bool isFree, List<UnifiedProductIdOverride> unifiedProductIdOverrides)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedProductId = unifiedProductId;
      ProductNamespace = productNamespace;
      IsFree = isFree;
      Overrides = unifiedProductIdOverrides;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in BonsaiPlanFeaturesRequestData");  
    }
  }
}
