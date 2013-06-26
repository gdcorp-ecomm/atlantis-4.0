using System;
using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public class InvalidDotType : IDotTypeInfo
  {
    static readonly IDotTypeInfo _instance;

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

    public int MaxRenewalMonthsOut
    {
      get { return 120; }
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

    public int GetPreRegProductId(PreRegPhases preRegPhase, int registrationLength, int domainCount)
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

    public List<int> GetValidPreRegProductIdList(PreRegPhases preRegPhase, int domainCount, params int[] registrationLengths)
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

    public bool HasExpiredAuctionRegIds
    {
      get { return false; }
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

    public List<int> GetValidPreRegLengths(PreRegPhases preRegPhase, int domainCount, params int[] registrationLengths)
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

    public int GetPreRegProductId(PreRegPhases preRegPhase, string registryId, int registrationLength, int domainCount)
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

    public List<int> GetValidPreRegProductIdList(PreRegPhases preRegPhase, string registryId, int domainCount, params int[] registrationLengths)
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

    public List<int> GetValidExpiredAuctionRegLengths(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegLengths(PreRegPhases preRegPhase, string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRegistrationLengths(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidTransferLengths(string registryId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRenewalLengths(string registryId, int domainCount, params int[] registrationLengths)
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

    public Dictionary<string, ITLDLaunchPhase> GetActiveClientRequestPhases()
    {
      return new Dictionary<string, ITLDLaunchPhase>();
    }

    public ITLDLaunchPhase GetLaunchPhase(PreRegPhases preRegPhase)
    {
      return null;
    }

    public int GetMinPreRegLength(PreRegPhases preRegPhase)
    {
      return 0;
    }

    public int GetMaxPreRegLength(PreRegPhases preRegPhase)
    {
      return 0;
    }

    public bool HasPreRegApplicationFee(PreRegPhases preRegPhase)
    {
      return false;
    }

    public int GetPreRegApplicationProductId(PreRegPhases preRegPhase)
    {
      return 0;
    }

    public string GetRegistrationFieldsXml()
    {
      return string.Empty;
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
    }
  }
}
