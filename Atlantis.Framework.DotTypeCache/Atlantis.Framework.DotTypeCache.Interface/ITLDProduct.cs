using System;
using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDProduct
  {
    ITLDValidYearsSet RegistrationYears { get; }

    ITLDValidYearsSet TransferYears { get; }

    ITLDValidYearsSet RenewalYears { get; }

    ITLDValidYearsSet ExpiredAuctionsYears { get; }

    ITLDValidYearsSet PreregistrationYears(string type);

    bool HasPhaseApplicationFee(string phaseCode, out string applicationProductType);

    List<int> GetPhaseApplicationProductIdList(string applicationProductType);

    ITLDTrustee Trustee { get; }

    ITLDRegistryPremiumDomains RegistryPremiumDomains { get; }
  }
}
