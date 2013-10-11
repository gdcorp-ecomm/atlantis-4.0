﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainContactFields.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public abstract class StaticDotType : IDotTypeInfo
  {
    private static int _domainContactFieldsRequest = 651;
    private static int _languagesRequest = 655;
    private static int _validDotTypesRequest = 667;

    private StaticDotTypeTiers _registerProductIds;
    private StaticDotTypeTiers _transferProductIds;
    private StaticDotTypeTiers _renewalProductIds;
    private StaticDotTypeTiers _preregistrationProductIds;
    private StaticDotTypeTiers _expiredAuctionRegProductIds;

    private StaticProduct _staticProduct;
    private StaticTld _staticTld;
    private StaticApplicationControl _staticApplicationControl;

    private bool _isMultiRegistry = false;
    public bool IsMultiRegistry
    {
      get { return _isMultiRegistry; }
      protected set { _isMultiRegistry = value; }
    }

    private bool _isGtld = false;
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
      return (DomainContactFieldsResponseData)DataCache.DataCache.GetProcessRequest(request, _domainContactFieldsRequest);
    }

    private TLDLanguageResponseData LoadLanguagesData()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, TldId);
      return (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, _languagesRequest);
    }

    private ValidDotTypesResponseData LoadValidDotTypes()
    {
      var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, _validDotTypesRequest);
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
      return this.GetPreRegProductId(phase, registrationLength, domainCount);
    }

    public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
    {
      return this.GetRegistrationProductId(registrationLength, domainCount);
    }

    public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
    {
      return this.GetTransferProductId(registrationLength, domainCount);
    }

    public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
    {
      return this.GetRenewalProductId(registrationLength, domainCount);
    }

    public int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount)
    {
      return this.GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidPreRegLengths(phase, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidRegistrationProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidTransferProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidRenewalProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
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

    public Dictionary<string, ITLDLaunchPhasePeriod> GetAllLaunchPhases()
    {
      return new Dictionary<string, ITLDLaunchPhasePeriod>();
    }

    public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
    {
      return null;
    }

    public bool IsLivePhase(LaunchPhases phase)
    {
      return true;
    }

    public bool HasPreRegPhases
    {
      get { return false; }
    }

    public IList<string> GetTuiFormTypes(LaunchPhases launchPhase)
    {
      return new List<string>();
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
            result = this.GetRegistrationProductId(domainProductLookup.RegistryId.ToString(), domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          else
          {
            result = this.GetRegistrationProductId(domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          break;
        case TLDProductTypes.Transfer:
          if (domainProductLookup.RegistryId != null)
          {
            result = this.GetTransferProductId(domainProductLookup.RegistryId.ToString(), domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          else
          {
            result = this.GetTransferProductId(domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          break;
        case TLDProductTypes.Renewal:
          if (domainProductLookup.RegistryId != null)
          {
            result = this.GetRenewalProductId(domainProductLookup.RegistryId.ToString(), domainProductLookup.Years, domainProductLookup.DomainCount);
          }
          else
          {
            result = this.GetRenewalProductId(domainProductLookup.Years, domainProductLookup.DomainCount);
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
            result = this.GetValidRegistrationProductIdList(domainProductListLookup.RegistryId.ToString(), domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          else
          {
            result = this.GetValidRegistrationProductIdList(domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          break;
        case TLDProductTypes.Transfer:
          if (domainProductListLookup.RegistryId != null)
          {
            result = this.GetValidTransferProductIdList(domainProductListLookup.RegistryId.ToString(), domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          else
          {
            result = this.GetValidTransferProductIdList(domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          break;
        case TLDProductTypes.Renewal:
          if (domainProductListLookup.RegistryId != null)
          {
            result = this.GetValidRenewalProductIdList(domainProductListLookup.RegistryId.ToString(), domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          else
          {
            result = this.GetValidRenewalProductIdList(domainProductListLookup.DomainCount, domainProductListLookup.Years);
          }
          break;
      }
      return result;
    }

    public bool HasLeafPage
    {
      get { return false; }
    }

    public string GetRegistrationFieldsXml()
    {
      var domainContactFieldsData = LoadDomainContactFieldsData();
      return domainContactFieldsData.DomainContactFields;
    }

    #endregion
  }
}
