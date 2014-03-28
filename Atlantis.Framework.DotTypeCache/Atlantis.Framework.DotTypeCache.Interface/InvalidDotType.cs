using System;
using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public class InvalidDotType : IDotTypeInfo
  {
    private static readonly IDotTypeInfo _instance;

    static InvalidDotType()
    {
      _instance = new InvalidDotType();
    }

    public static IDotTypeInfo Instance
    {
      get { return _instance; }
    }

    private ITLDApplicationControl _invalidApplicationControl = new InvalidApplicationControl();

    private InvalidDotType()
    {
    }

    #region IDotTypeInfo Members

    public string DotType
    {
      get { return "INVALID"; }
    }

    public int MinExpiredAuctionRegLength
    {
      get { return 1; }
    }

    public int MaxExpiredAuctionRegLength
    {
      get { return 10; }
    }

    public int MinRegistrationLength
    {
      get { return 1; }
    }

    public int MaxRegistrationLength
    {
      get { return 10; }
    }

    public int MinTransferLength
    {
      get { return 1; }
    }

    public int MaxTransferLength
    {
      get { return 10; }
    }

    public int MinRenewalLength
    {
      get { return 1; }
    }

    public int MaxRenewalLength
    {
      get { return 10; }
    }

    public RegistryLanguage GetLanguageByName(string languageName)
    {
      return null;
    }

    public RegistryLanguage GetLanguageById(int languageId)
    {
      return null;
    }

    public bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength)
    {
      maxValidRenewalLength = -1;
      return false;
    }

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetRegistrationProductId(int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetTransferProductId(int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetRenewalProductId(int registrationLength, int domainCount)
    {
      return 0;
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public bool IsMultiRegistry
    {
      get { return false; }
    }

    public bool IsGtld 
    {
      get { return false; }
    }

    public IEnumerable<RegistryLanguage> RegistryLanguages
    {
      get { return null; }
    }

    public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegLengths(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public string GetRegistryIdByProductId(int productId)
    {
      return string.Empty;
    }

    public ITLDProduct Product
    {
      get { return null; }
    }

    public int TldId
    {
      get { return 0; }
    }

    public ITLDTld Tld
    {
      get { return null; }
    }

    public ITLDApplicationControl ApplicationControl
    {
      get { return _invalidApplicationControl; }
    }

    public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
    {
      return null;
    }

    public ITLDLaunchPhaseGroupCollection GetAllLaunchPhaseGroups(bool activeOnly = true)
    {
      return null;
    }

    public bool IsPreRegPhaseActive
    {
      get { return false; }
    }

    public IList<string> GetTuiFormTypes(LaunchPhases launchPhase)
    {
      return new List<string>();
    }

    public bool RequiresTuiForm(LaunchPhases launchPhase)
    {
      return false;
    }

    public int GetMinPreRegLength(LaunchPhases phase)
    {
      return 0;
    }

    public int GetMaxPreRegLength(LaunchPhases phase)
    {
      return 0;
    }

    public bool HasPhaseApplicationFee(LaunchPhases phase, out string applicationProductType)
    {
      applicationProductType = string.Empty;
      return false;
    }

    public List<int> GetPhaseApplicationProductIdList(LaunchPhases phase)
    {
      return new List<int>();
    }

    public int GetProductId(IDomainProductLookup domainProductLookup)
    {
      return 0;
    }

    public List<int> GetProductIdList(IDomainProductListLookup domainProductListLookup)
    {
      return new List<int>();
    }

    public string GetRegistrationFieldsXml()
    {
      return string.Empty;
    }

    public List<int> GetTrusteeProductId(TLDProductTypes productType) {
        return new List<int>();
    }

    #endregion

    public class InvalidApplicationControl : ITLDApplicationControl
    {
      public string DotTypeDescription
      {
        get { return string.Empty; }
      }

      public string LandingPageUrl
      {
        get { return string.Empty; }
      }

      public bool IsMultiRegistry
      {
        get { return false; }
      }

      public Dictionary<string, ITLDTuiFormGroup> TuiFormGroups
      {
        get { return new Dictionary<string, ITLDTuiFormGroup>(); }
      }
    }
  }
}
