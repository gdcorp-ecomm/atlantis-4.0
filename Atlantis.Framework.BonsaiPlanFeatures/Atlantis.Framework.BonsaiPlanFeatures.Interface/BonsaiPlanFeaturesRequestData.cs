using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
      if (Overrides != null && Overrides.Count > 0)
        throw new NotImplementedException("BonsaiPlanFeaturesRequestData is not a cacheable request if Overrides is populated.");

      MD5 hashProvider = new MD5CryptoServiceProvider();
      hashProvider.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Join(":", UnifiedProductId.ToString(), ProductNamespace, IsFree ? "1" : "0"));
      byte[] md5Bytes = hashProvider.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0).Replace("-", string.Empty);
      return sValue; 
    }
  }
}
