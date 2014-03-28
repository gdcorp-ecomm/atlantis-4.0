using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.DomainContactFields.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public abstract class StaticDotType : IDotTypeInfo
  {
    private const int DomainContactFieldsRequest = 651;
    private const int LanguagesRequest = 655;
    private const int ValidDotTypesRequest = 667;

    private static readonly ITLDLaunchPhaseGroup _gaLaunchPhaseGroup = TldLaunchPhaseGroup.CreateGroup(TldLaunchPhase.GeneralAvailabilityActive());
    private static readonly IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> _gaLaunchPhaseDictionary = new Dictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> { { LaunchPhaseGroupTypes.GeneralAvailability, _gaLaunchPhaseGroup } };
    private static readonly ITLDLaunchPhaseGroupCollection _gaTldLaunchPhaseCollection = TldLaunchPhaseGroupCollection.CreateCollection(_gaLaunchPhaseDictionary);

    private readonly StaticDotTypeTiers _registerProductIds;
    private readonly StaticDotTypeTiers _transferProductIds;
    private readonly StaticDotTypeTiers _renewalProductIds;
    private readonly StaticDotTypeTiers _preregistrationProductIds;
    private readonly StaticDotTypeTiers _expiredAuctionRegProductIds;

    private readonly StaticProduct _staticProduct;
    private readonly StaticTld _staticTld;
    private readonly StaticApplicationControl _staticApplicationControl;

    private bool _isMultiRegistry;
    public bool IsMultiRegistry
    {
      get { return _isMultiRegistry; }
      protected set { _isMultiRegistry = value; }
    }

    private bool _isGtld;
    public bool IsGtld
    {
      get { return _isGtld; }
      protected set { _isGtld = value; }
    }

    public virtual int MinPreRegLength
    {
      get { return 1; }
    }

    public virtual int MaxPreRegLength
    {
      get { return 1; }
    }

    public List<int> GetTrusteeProductId(TLDProductTypes productType)
    {
        return new List<int>();
    }

    public virtual int MinRegistrationLength
    {
      get { return 1; }
    }

    public virtual int MaxRegistrationLength
    {
      get { return 10; }
    }

    public virtual int MinTransferLength
    {
      get { return 1; }
    }

    public virtual int MaxTransferLength
    {
      get { return 9; }
    }

    public virtual int MinRenewalLength
    {
      get { return 1; }
    }

    public virtual int MaxRenewalLength
    {
      get { return 10; }
    }

    protected virtual int MaxRenewalMonthsOut
    {
      get { return 120; }
    }

    public virtual int MinExpiredAuctionRegLength
    {
      get { return 1; }
    }

    public virtual int MaxExpiredAuctionRegLength
    {
      get { return 10; }
    }

    public StaticDotType()
    {
      _registerProductIds = InitializeRegistrationProductIds();
      _transferProductIds = InitializeTransferProductIds();
      _renewalProductIds = InitializeRenewalProductIds();
      _preregistrationProductIds = InitializePreRegistrationProductIds();
      _expiredAuctionRegProductIds = InitializeExpiredAuctionRegProductIds();
      _staticProduct = new StaticProduct(this);
      _staticTld = new StaticTld(this);
      _staticApplicationControl = new StaticApplicationControl(this);
    }

    private DomainContactFieldsResponseData LoadDomainContactFieldsData()
    {
      var request = new DomainContactFieldsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DotType);
      return (DomainContactFieldsResponseData)DataCache.DataCache.GetProcessRequest(request, DomainContactFieldsRequest);
    }

    private TLDLanguageResponseData LoadLanguagesData()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, TldId);
      return (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, LanguagesRequest);
    }

    private ValidDotTypesResponseData LoadValidDotTypes()
    {
      var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, ValidDotTypesRequest);
    }

    protected abstract StaticDotTypeTiers InitializeRegistrationProductIds();
    protected abstract StaticDotTypeTiers InitializeTransferProductIds();
    protected abstract StaticDotTypeTiers InitializeRenewalProductIds();

    protected virtual StaticDotTypeTiers InitializePreRegistrationProductIds()
    {
      return null;
    }

    protected virtual StaticDotTypeTiers InitializeExpiredAuctionRegProductIds()
    {
      return null;
    }

    #region IDotTypeInfo Members

    public abstract string DotType { get; }

    public IEnumerable<RegistryLanguage> RegistryLanguages
    {
      get
      {
        var languagesData = LoadLanguagesData();
        return languagesData.RegistryLanguages;
      }
    }

    public RegistryLanguage GetLanguageByName(string languageName)
    {
      var languagesData = LoadLanguagesData();
      return languagesData.GetLanguageDataByName(languageName);
    }

    public RegistryLanguage GetLanguageById(int languageId)
    {
      var languagesData = LoadLanguagesData();
      return languagesData.GetLanguageDataById(languageId);
    }

    public bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength)
    {
      maxValidRenewalLength = -1;
      bool canRenew = false;

      DateTime origExpirationDate = currentExpirationDate;

      for (int i = MaxRenewalLength; i >= MinRenewalLength; i--)
      {
        var d = origExpirationDate;
        DateTime newRenewalDate = d.AddYears(i);
        DateTime maxRenewalDate = DateTime.Now.Date.AddMonths(MaxRenewalMonthsOut);
        if (newRenewalDate <= maxRenewalDate)
        {
          maxValidRenewalLength = i;
          break;
        }
      }

      if (maxValidRenewalLength > 0)
      {
        int? renewalMonthsBeforeExpiration = TLDRenewal.GetRenewalMonthsBeforeExpiration(DotType);
        if (renewalMonthsBeforeExpiration.HasValue)
        {
          DateTime renewalEligibilityDate = origExpirationDate.AddMonths(renewalMonthsBeforeExpiration.Value * -1);

          if (DateTime.Now.Date >= renewalEligibilityDate)
          {
            canRenew = true;
          }
        }
        else
        {
          canRenew = true;
        }
      }

      return canRenew;
    }

    private List<int> GetValidProductIdList(StaticDotTypeTiers productIds, int minLength, int maxLength,
      int domainCount, params int[] registrationLengths)
    {
      List<int> result = new List<int>(registrationLengths.Length);

      if (productIds != null)
      {
        foreach (int registrationLength in registrationLengths)
        {
          if ((registrationLength >= minLength) &&
            (registrationLength <= maxLength))
          {
            int productId = productIds.GetProductId(registrationLength, domainCount);

            if (productId > 0)
            {
              result.Add(productId);
            }
          }
        }
      }

      return result;
    }

    private List<int> GetValidLengths(StaticDotTypeTiers productIds, int minLength, int maxLength,
      int domainCount, params int[] registrationLengths)
    {
      List<int> result = new List<int>(registrationLengths.Length);
      if (productIds != null)
      {
        foreach (int registrationLength in registrationLengths)
        {
          if ((registrationLength >= minLength) &&
            (registrationLength <= maxLength))
          {
            if (productIds.IsLengthValid(registrationLength, domainCount))
            {
              result.Add(registrationLength);
            }
          }
        }
      }

      return result;
    }

    public int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount)
    {
      int result = 0;

      if ((_preregistrationProductIds != null)
        && (registrationLength >= MinPreRegLength)
        && (registrationLength <= MaxPreRegLength))
      {
        result = _preregistrationProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetRegistrationProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if ((_registerProductIds != null)
        && (registrationLength >= MinRegistrationLength)
        && (registrationLength <= MaxRegistrationLength))
      {
        result = _registerProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetTransferProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if ((_transferProductIds != null)
        && (registrationLength >= MinTransferLength)
        && (registrationLength <= MaxTransferLength))
      {
        result = _transferProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetRenewalProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if ((_renewalProductIds != null)
        && (registrationLength >= MinRenewalLength)
        && (registrationLength <= MaxRenewalLength))
      {
        result = _renewalProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if ((_expiredAuctionRegProductIds != null)
        && (registrationLength >= MinExpiredAuctionRegLength)
        && (registrationLength <= MaxExpiredAuctionRegLength))
      {
        result = _expiredAuctionRegProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
    {
      return GetValidProductIdList(_registerProductIds, MinRegistrationLength, MaxRegistrationLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths)
    {
      return GetValidLengths(_registerProductIds, MinRegistrationLength, MaxRegistrationLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
    {
      return GetValidProductIdList(_transferProductIds, MinTransferLength, MaxTransferLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths)
    {
      return GetValidLengths(_transferProductIds, MinTransferLength, MaxTransferLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
    {
      return GetValidProductIdList(_renewalProductIds, MinRenewalLength, MaxRenewalLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths)
    {
      return GetValidLengths(_renewalProductIds, MinRenewalLength, MaxRenewalLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return GetValidProductIdList(_preregistrationProductIds, MinPreRegLength, MaxPreRegLength, domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegLengths(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return GetValidLengths(_preregistrationProductIds, MinPreRegLength, MaxPreRegLength, domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return GetValidProductIdList(_expiredAuctionRegProductIds, MinExpiredAuctionRegLength, MaxExpiredAuctionRegLength,
        domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
    {
      return GetValidLengths(_expiredAuctionRegProductIds, MinExpiredAuctionRegLength, MaxExpiredAuctionRegLength,
        domainCount, registrationLengths);
    }

    public int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount)
    {
      return GetPreRegProductId(phase, registrationLength, domainCount);
    }

    public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
    {
      return GetRegistrationProductId(registrationLength, domainCount);
    }

    public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
    {
      return GetTransferProductId(registrationLength, domainCount);
    }

    public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
    {
      return GetRenewalProductId(registrationLength, domainCount);
    }

    public int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount)
    {
      return GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths)
    {
      return GetValidPreRegLengths(phase, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return GetValidRegistrationProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return GetValidTransferProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return GetValidRenewalProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
    }

    public string GetRegistryIdByProductId(int productId)
    {
      return string.Empty;
    }

    public ITLDProduct Product
    {
      get { return _staticProduct; }
    }

    public int TldId
    {
      get
      {
        int tldId;

        var validDotTypes = LoadValidDotTypes();
        validDotTypes.TryGetTldId(DotType, out tldId);

        return tldId;
      }
    }

    public ITLDTld Tld
    {
      get { return _staticTld; }
    }

    public ITLDApplicationControl ApplicationControl
    {
      get { return _staticApplicationControl; }
    }

    public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
    {
      ITLDLaunchPhase result = null;

      if (phase == LaunchPhases.GeneralAvailability)
      {
        result = TldLaunchPhase.GeneralAvailabilityActive();
      }

      return result;
    }

    public ITLDLaunchPhaseGroupCollection GetAllLaunchPhaseGroups(bool activeOnly = true)
    {
      return _gaTldLaunchPhaseCollection;
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
      return MinPreRegLength;
    }

    public int GetMaxPreRegLength(LaunchPhases phase)
    {
      return MaxPreRegLength;
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
      int result = 0;
      switch (domainProductLookup.ProductType)
      {
        case TLDProductTypes.Registration:
          if (domainProductLookup.RegistryId != null)
          {
            result = GetRegistrationProductId(domainProductLookup.RegistryId.ToString(), domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          else
          {
            result = GetRegistrationProductId(domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          break;
        case TLDProductTypes.Transfer:
          if (domainProductLookup.RegistryId != null)
          {
            result = GetTransferProductId(domainProductLookup.RegistryId.ToString(), domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          else
          {
            result = GetTransferProductId(domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          break;
        case TLDProductTypes.Renewal:
          if (domainProductLookup.RegistryId != null)
          {
            result = GetRenewalProductId(domainProductLookup.RegistryId.ToString(), domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          else
          {
            result = GetRenewalProductId(domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          break;
      }
      return result;
    }

    public List<int> GetProductIdList(IDomainProductListLookup domainProductListLookup)
    {
      var result = new List<int>(8);
      switch (domainProductListLookup.ProductType)
      {
        case TLDProductTypes.Registration:
          if (domainProductListLookup.RegistryId != null)
          {
            result = GetValidRegistrationProductIdList(domainProductListLookup.RegistryId.ToString(), domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          else
          {
            result = GetValidRegistrationProductIdList(domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          break;
        case TLDProductTypes.Transfer:
          if (domainProductListLookup.RegistryId != null)
          {
            result = GetValidTransferProductIdList(domainProductListLookup.RegistryId.ToString(), domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          else
          {
            result = GetValidTransferProductIdList(domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          break;
        case TLDProductTypes.Renewal:
          if (domainProductListLookup.RegistryId != null)
          {
            result = GetValidRenewalProductIdList(domainProductListLookup.RegistryId.ToString(), domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          else
          {
            result = GetValidRenewalProductIdList(domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          break;
      }
      return result;
    }

    public string GetRegistrationFieldsXml()
    {
      var domainContactFieldsData = LoadDomainContactFieldsData();
      return domainContactFieldsData.DomainContactFields;
    }

    #endregion
  }
}
