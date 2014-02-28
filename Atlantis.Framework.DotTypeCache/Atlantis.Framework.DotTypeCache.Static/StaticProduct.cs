using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class StaticProduct : ITLDProduct
  {
    private readonly StaticDotType _staticDotType;
    public StaticProduct(StaticDotType staticDotType)
    {
      _staticDotType = staticDotType;
      _registrationYears = new StaticValidYearsSet(_staticDotType.MinRegistrationLength, _staticDotType.MaxRegistrationLength);
      _transferYears = new StaticValidYearsSet(_staticDotType.MinTransferLength, _staticDotType.MaxTransferLength);
      _renewalYears = new StaticValidYearsSet(_staticDotType.MinRenewalLength, _staticDotType.MaxRenewalLength);
      _expiredAuctionsYears = new StaticValidYearsSet(_staticDotType.MinExpiredAuctionRegLength, _staticDotType.MaxExpiredAuctionRegLength);
      _preregistrationYears = new StaticValidYearsSet(_staticDotType.MinPreRegLength, _staticDotType.MaxPreRegLength);
    }

    private readonly StaticValidYearsSet _registrationYears;
    public ITLDValidYearsSet RegistrationYears 
    {
      get
      {
        return _registrationYears;
      }
    }

    private readonly StaticValidYearsSet _transferYears;
    public ITLDValidYearsSet TransferYears
    {
      get
      {
        return _transferYears;
      }
    }

    private readonly StaticValidYearsSet _renewalYears;
    public ITLDValidYearsSet RenewalYears
    {
      get
      {
        return _renewalYears;
      }
    }

    private readonly StaticValidYearsSet _expiredAuctionsYears;
    public ITLDValidYearsSet ExpiredAuctionsYears
    {
      get
      {
        return _expiredAuctionsYears;
      }
    }

    private readonly StaticValidYearsSet _preregistrationYears;
    public ITLDValidYearsSet PreregistrationYears(string type)
    {
      return _preregistrationYears;
    }

    public bool HasPhaseApplicationFee(string phaseCode, out string applicationProductType)
    {
      applicationProductType = string.Empty;

      return false;
    }

    public List<int> GetPhaseApplicationProductIdList(string applicationProductType)
    {
      return new List<int>();
    }

    public ITLDTrustee Trustee
    {
      get { return new StaticTrustee(); }
    }

    public ITLDRegistryPremiumDomains RegistryPremiumDomains
    {
      get { return null; }
    }
  }
}
