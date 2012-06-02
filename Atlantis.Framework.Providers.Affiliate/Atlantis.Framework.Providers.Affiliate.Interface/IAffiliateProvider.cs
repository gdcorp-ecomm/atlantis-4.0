using System;

namespace Atlantis.Framework.Providers.Affiliate.Interface
{
  public interface IAffiliateProvider
  {
    int? UpdatedAffiliateMetaDataRequest { get; set; }

    bool ProcessAffiliateSourceCode(string isc, out string affiliateType, out DateTime affiliateStartDate);
    bool IsValidAffiliate(string affiliateType);
  }
}
