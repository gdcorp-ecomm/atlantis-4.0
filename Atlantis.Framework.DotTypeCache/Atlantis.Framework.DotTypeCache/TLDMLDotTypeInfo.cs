using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.DomainContactFields.Interface;
using Atlantis.Framework.DotTypeAvailability.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes;

namespace Atlantis.Framework.DotTypeCache
{
  public class TLDMLDotTypeInfo : IDotTypeInfo
  {
    private const string _DEFAULTPREREGTYPE = "GA";
    private const string _REGULARREGISTRATIONPHASECODE = "REGREG";

    private string _tld;
    private Lazy<ProductIdListResponseData> _productIdList;
    private Lazy<RegDotTypeRegistryResponseData> _dotTypeRegistryData;
    private Lazy<TLDMLByNameResponseData> _tldml;
    private Lazy<int> _tldId;
    private readonly Lazy<DomainContactFieldsResponseData> _domainContactFieldsData;
    private readonly Lazy<TLDLanguageResponseData> _languagesData;
    private readonly Lazy<ValidDotTypesResponseData> _validDotTypes;
    private readonly Lazy<ISiteContext> _siteContext;
    private Lazy<TLDPhaseDateListResponseData> _tldPhaseDateListData;
    private Lazy<DotTypeAvailabilityResponseData> _tldAvailabilityData;

    internal static TLDMLDotTypeInfo FromDotType(string dotType, IProviderContainer container)
    {
      return new TLDMLDotTypeInfo(dotType, container);
    }

    private TLDMLDotTypeInfo(string tld, IProviderContainer container)
    {
      _tld = tld;
      _tldId = new Lazy<int>(LoadTldId);
      _productIdList = new Lazy<ProductIdListResponseData>(LoadProductIds);
      _dotTypeRegistryData = new Lazy<RegDotTypeRegistryResponseData>(LoadDotTypeRegistryData);
      _tldml = new Lazy<TLDMLByNameResponseData>(LoadTLDML);
      _domainContactFieldsData = new Lazy<DomainContactFieldsResponseData>(LoadDomainContactFieldsData);
      _languagesData = new Lazy<TLDLanguageResponseData>(LoadLanguagesData);
      _validDotTypes = new Lazy<ValidDotTypesResponseData>(LoadValidDotTypes);
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _tldPhaseDateListData = new Lazy<TLDPhaseDateListResponseData>(LoadTldPhaseDateList);
      _tldAvailabilityData = new Lazy<DotTypeAvailabilityResponseData>(LoadTldAvailabilityData);

      ITLDProduct product = _tldml.Value.Product; // Preload the TLDML
    }

    private int LoadTldId()
    {
      int tldId;
      _validDotTypes.Value.TryGetTldId(_tld, out tldId);

      return tldId;
    }

    private TLDMLByNameResponseData LoadTLDML()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
      return (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.TLDMLByName);
    }

    private RegDotTypeRegistryResponseData LoadDotTypeRegistryData()
    {
      try
      {
        var request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
        return (RegDotTypeRegistryResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.Registry);
      }
      catch (Exception)
      {
        return null;
      }
    }

    private ProductIdListResponseData LoadProductIds()
    {
      try
      {
        var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
        return (ProductIdListResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductIdList);
      }
      catch (Exception)
      {
        return null;
      }
    }

    private DomainContactFieldsResponseData LoadDomainContactFieldsData()
    {
      var request = new DomainContactFieldsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
      return (DomainContactFieldsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.DomainContactFields);
    }

    private TLDLanguageResponseData LoadLanguagesData()
    {
      var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tldId.Value);
      return (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.Languages);
    }

    private ValidDotTypesResponseData LoadValidDotTypes()
    {
      var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ValidDotTypes);
    }

    private TLDPhaseDateListResponseData LoadTldPhaseDateList()
    {
      var request = new TLDPhaseDateListRequestData(_tldId.Value);
      return (TLDPhaseDateListResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.TldPhaseDateList);
    }

    private TLDProductDomainAttributesResponseData LoadProductDomainAttributes(string phaseCode, int privateLabelId, int productTypeId)
    {
      var pLTypeResponse = LoadPrivateLabelType(privateLabelId);

      var request = new TLDProductDomainAttributesRequestData(_tldId.Value, phaseCode, pLTypeResponse.PrivateLabelType, productTypeId);
      return (TLDProductDomainAttributesResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductDomainAttributes);
    }

    private PrivateLabelTypeResponseData LoadPrivateLabelType(int privateLabelId)
    {
      var request = new PrivateLabelTypeRequestData(privateLabelId);
      return (PrivateLabelTypeResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.PrivateLabelType);
    }

    private DotTypeAvailabilityResponseData LoadTldAvailabilityData()
    {
      var request = new DotTypeAvailabilityRequestData();
      return (DotTypeAvailabilityResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.TldAvailabilityRequest);
    }

    public string DotType
    {
      get { return _tld; }
    }

    // Goal... make ITldValidYearSets accessible somehow

    public int MinExpiredAuctionRegLength
    {
      get { return _tldml.Value.Product.ExpiredAuctionsYears.Min; }
    }

    public int MaxExpiredAuctionRegLength
    {
      get { return _tldml.Value.Product.ExpiredAuctionsYears.Max; }
    }

    public int MinRegistrationLength
    {
      get { return _tldml.Value.Product.RegistrationYears.Min; }
    }

    public int MaxRegistrationLength
    {
      get { return _tldml.Value.Product.RegistrationYears.Max; }
    }

    public int MinTransferLength
    {
      get
      {
        return _tldml.Value.Product.TransferYears.Min;
      }
    }

    public int MaxTransferLength
    {
      get
      {
        return _tldml.Value.Product.TransferYears.Max;
      }
    }

    public int MinRenewalLength
    {
      get
      {
        return _tldml.Value.Product.RenewalYears.Min;
      }
    }

    public int MaxRenewalLength
    {
      get
      {
        return _tldml.Value.Product.RegistrationYears.Max;
      }
    }

    public bool IsMultiRegistry
    {
      get
      {
        return _tldml.Value.ApplicationControl.IsMultiRegistry;
      }
    }

    public bool IsGtld 
    {
      get 
      { 
        return _tldml.Value.Tld.IsGtld;
      }
    }


    public IEnumerable<RegistryLanguage> RegistryLanguages
    {
      get { return _languagesData.Value.RegistryLanguages; }
    }

    public RegistryLanguage GetLanguageByName(string languageName)
    {
      return _languagesData.Value.GetLanguageDataByName(languageName);
    }

    public RegistryLanguage GetLanguageById(int languageId)
    {
      return _languagesData.Value.GetLanguageDataById(languageId);
    }

    public bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength)
    {
      maxValidRenewalLength = -1;
      var canRenew = false;

      var origExpirationDate = currentExpirationDate;

      for (var i = MaxRenewalLength; i >= MinRenewalLength; i--)
      {
        var d = origExpirationDate;
        var newRenewalDate = d.AddYears(i);
        var maxRenewalDate = DateTime.Now.AddYears(MaxRenewalLength);

        if (!string.IsNullOrEmpty(Tld.RenewProhibitedPeriodForExpirationUnit))
        {
          switch (Tld.RenewProhibitedPeriodForExpirationUnit)
          {
            case "month":
              maxRenewalDate = DateTime.Now.Date.AddMonths(Tld.RenewProhibitedPeriodForExpiration * -1);
              break;
            case "day":
              maxRenewalDate = DateTime.Now.Date.AddDays(Tld.RenewProhibitedPeriodForExpiration * -1);
              break;
          }
        }

        if (newRenewalDate <= maxRenewalDate)
        {
          maxValidRenewalLength = i;
          break;
        }
      }

      if (maxValidRenewalLength > 0)
      {
        int? renewalMonthsBeforeExpiration = Atlantis.Framework.DotTypeCache.Static.TLDRenewal.GetRenewalMonthsBeforeExpiration(DotType);
        if (renewalMonthsBeforeExpiration.HasValue)
        {
          var renewalEligibilityDate = origExpirationDate.AddMonths(renewalMonthsBeforeExpiration.Value * -1);

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

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.ExpiredAuctionReg);
    }

    public int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount)
    {
      return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.ExpiredAuctionReg);
    }

    public int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount)
    {
      return InternalGetProductId(registrationLength, domainCount, PhaseHelper.GetDotTypeProductTypes(phase));
    }

    public int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount)
    {
      return InternalGetProductId(registryId, registrationLength, domainCount, PhaseHelper.GetDotTypeProductTypes(phase));
    }

    public int GetRegistrationProductId(int registrationLength, int domainCount)
    {
      return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.Registration);
    }

    public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
    {
      return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.Registration);
    }

    public int GetTransferProductId(int registrationLength, int domainCount)
    {
      return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.Transfer);
    }

    public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
    {
      return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.Transfer);
    }

    public int GetRenewalProductId(int registrationLength, int domainCount)
    {
      return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.Renewal);
    }

    public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
    {
      return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.Renewal);
    }

    private int InternalGetProductId(int registrationLength, int domainCount, DotTypeProductTypes productType)
    {
      string registryId = null;

      if (productType == DotTypeProductTypes.Registration && _dotTypeRegistryData.Value != null)
      {
        registryId = _dotTypeRegistryData.Value.RegistrationAPI.Id;
      }
      else if (productType == DotTypeProductTypes.Transfer && _dotTypeRegistryData.Value != null)
      {
        registryId = _dotTypeRegistryData.Value.TransferAPI.Id;
      }

      return InternalGetProductId(registryId, registrationLength, domainCount, productType);
    }

    private int InternalGetProductId(string registryId, int registrationLength, int domainCount, DotTypeProductTypes productType)
    {
      int result = 0;
      DotTypeProductTiers tiers = null;

      if (!string.IsNullOrEmpty(registryId))
      {
        if (_productIdList.Value != null)
        {
          tiers = _productIdList.Value.GetProductTiersForRegistry(registryId, productType);
        }
      }

      if (tiers == null)
      {
        if (_productIdList.Value != null)
        {
          tiers = _productIdList.Value.GetDefaultProductTiers(productType);
        }
      }

      if (tiers != null)
      {
        DotTypeProduct product;

        if (tiers.TryGetProduct(registrationLength, domainCount, out product))
        {
          result = product.ProductId;
        }

      }

      if (result == 0)
      {
        LogMissingProductId(registryId, registrationLength, domainCount, productType);
      }

      return result;
    }

    private void LogMissingProductId(string registryId, int registrationLength, int domainCount, DotTypeProductTypes productType)
    {
      const string _MISSING_ID_ERROR = "Missing ProductId for tld: {0}; productType: {1}; registryid: {2}; registrationLength: {3}; domainCount: {4}";
      string message = string.Format(_MISSING_ID_ERROR, _tld, productType.ToString(), registryId, registrationLength, domainCount);
      Logging.LogException("TLDMLDotTypeInfo.GetProductId", message, _tld);
    }

    // TODO: single into types that can do all of this for us.... obsolete or kill the interface

    public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.ExpiredAuctionReg, _tldml.Value.Product.ExpiredAuctionsYears, domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.ExpiredAuctionReg, _tldml.Value.Product.ExpiredAuctionsYears, registryId, domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(PhaseHelper.GetDotTypeProductTypes(phase), _tldml.Value.Product.PreregistrationYears(PhaseHelper.GetPhaseCode(phase)),
                                        domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(PhaseHelper.GetDotTypeProductTypes(phase), _tldml.Value.Product.PreregistrationYears(PhaseHelper.GetPhaseCode(phase)),
                                        registryId, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.Registration, _tldml.Value.Product.RegistrationYears, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.Registration, _tldml.Value.Product.RegistrationYears, registryId, domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.Transfer, _tldml.Value.Product.TransferYears, domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.Transfer, _tldml.Value.Product.TransferYears, registryId, domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.Renewal, _tldml.Value.Product.RenewalYears, domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.Renewal, _tldml.Value.Product.RenewalYears, registryId, domainCount, registrationLengths);
    }

    private List<int> InternalGetValidProductIds(DotTypeProductTypes productType, ITLDValidYearsSet validYears, string registryId, int domainCount, params int[] requestedYears)
    {
      List<DotTypeProduct> products = InternalGetValidProducts(productType, validYears, registryId, domainCount, requestedYears);
      return products.ConvertAll<int>(product => product.ProductId);
    }

    private List<int> InternalGetValidProductIds(DotTypeProductTypes productType, ITLDValidYearsSet validYears, int domainCount, params int[] requestedYears)
    {
      List<DotTypeProduct> products = InternalGetValidProducts(productType, validYears, domainCount, requestedYears);
      return products.ConvertAll<int>(product => product.ProductId);
    }

    public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidYears(DotTypeProductTypes.ExpiredAuctionReg, _tldml.Value.Product.ExpiredAuctionsYears, domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegLengths(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidYears(PhaseHelper.GetDotTypeProductTypes(phase), _tldml.Value.Product.PreregistrationYears(PhaseHelper.GetPhaseCode(phase)),
                                   domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidYears(DotTypeProductTypes.Registration, _tldml.Value.Product.RegistrationYears, domainCount, registrationLengths);
    }

    public List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidYears(DotTypeProductTypes.Transfer, _tldml.Value.Product.TransferYears, domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidYears(DotTypeProductTypes.Renewal, _tldml.Value.Product.RenewalYears, domainCount, registrationLengths);
    }

    private List<int> InternalGetValidYears(DotTypeProductTypes productType, ITLDValidYearsSet validYears, int domainCount, params int[] requestedYears)
    {
      List<DotTypeProduct> products = InternalGetValidProducts(productType, validYears, domainCount, requestedYears);
      return products.ConvertAll<int>(product => product.Years);
    }

    private List<DotTypeProduct> InternalGetValidProducts(DotTypeProductTypes productType, ITLDValidYearsSet validYears, int domainCount, params int[] requestedYears)
    {
      string registryId = null;

      if (productType == DotTypeProductTypes.Registration && _dotTypeRegistryData.Value != null)
      {
        registryId = _dotTypeRegistryData.Value.RegistrationAPI.Id;
      }
      else if (productType == DotTypeProductTypes.Transfer && _dotTypeRegistryData.Value != null)
      {
        registryId = _dotTypeRegistryData.Value.TransferAPI.Id;
      }

      return InternalGetValidProducts(productType, validYears, registryId, domainCount, requestedYears);
    }

    private List<DotTypeProduct> InternalGetValidProducts(DotTypeProductTypes productType, ITLDValidYearsSet validYears, string registryId, int domainCount, params int[] requestedYears)
    {
      List<DotTypeProduct> result = new List<DotTypeProduct>(10);
      DotTypeProductTiers tiers = null;

      if (!string.IsNullOrEmpty(registryId))
      {
        if (_productIdList.Value != null)
        {
          tiers = _productIdList.Value.GetProductTiersForRegistry(registryId, productType);
        }
      }

      if (tiers == null)
      {
        if (_productIdList.Value != null)
        {
          tiers = _productIdList.Value.GetDefaultProductTiers(productType);
        }
      }

      if (tiers != null)
      {
        foreach (int registrationLength in requestedYears)
        {
          if (validYears.IsValid(registrationLength))
          {
            DotTypeProduct product;
            if ((tiers.TryGetProduct(registrationLength, domainCount, out product)) && (product.IsValid))
            {
              result.Add(product);
            }
          }
        }
      }

      return result;
    }

    public string GetRegistryIdByProductId(int productId)
    {
      string result = string.Empty;

      DotTypeProduct product;
      if (_productIdList.Value != null && _productIdList.Value.TryGetProductByProductId(productId, out product))
      {
        result = product.RegistryId;
      }

      return result;
    }

    public ITLDProduct Product
    {
      get { return _tldml.Value.Product; }
    }

    public int TldId
    {
      get { return _tldId.Value; }
    }

    public ITLDTld Tld
    {
      get { return _tldml.Value.Tld; }
    }

    public ITLDApplicationControl ApplicationControl
    {
      get
      {
        return _tldml.Value.ApplicationControl;
      }
    }

    public Dictionary<string, ITLDLaunchPhasePeriod> GetAllLaunchPhases()
    {
      var launchPhases = new Dictionary<string, ITLDLaunchPhasePeriod>();
      if (_tldPhaseDateListData.Value != null)
      {
        var phases = _tldPhaseDateListData.Value.TldPhaseDates;

        foreach ( var phase in phases)
        {
          if (!phase.PhaseCode.Equals(_REGULARREGISTRATIONPHASECODE))
          {
            var launchPhasePeriod = new TldLaunchPhasePeriod
              {
                StartDate = phase.StartDate,
                StopDate = phase.EndDate
              };

            launchPhases.Add(phase.PhaseCode, launchPhasePeriod);
          }
        }
      }
      return launchPhases;
    }

    public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
    {
      ITLDLaunchPhase launchPhase = null;
      if (_tldml.Value.Phase != null)
      {
        launchPhase = _tldml.Value.Phase.GetLaunchPhase(phase);
      }
      return launchPhase;
    }

    public bool IsLivePhase(LaunchPhases phase)
    {
      bool livePhase = false;
      if (_tldml.Value.Phase != null)
      {
        ITLDLaunchPhase launchPhase = _tldml.Value.Phase.GetLaunchPhase(phase);
        if (launchPhase != null)
        {
          livePhase = launchPhase.IsLive;
        }
      }
      return livePhase;
    }

    public bool HasPreRegPhases
    {
      get
      {
        bool result = false;
        if (_tldml.Value.Phase != null)
        {
          result = _tldml.Value.Phase.HasPreRegPhases;
        }
        return result;
      }
    }

    public IList<string> GetTuiFormTypes(LaunchPhases launchPhase)
    {
      var result = new List<string>(16);

      if (_tldml.Value.ApplicationControl != null)
      {
        bool needsClaimCheck = false;
        if (_tldml.Value.Phase != null)
        {
          needsClaimCheck = _tldml.Value.Phase.GetLaunchPhase(launchPhase).NeedsClaimCheck;
        }

        var phaseCode = PhaseHelper.GetPhaseCode(launchPhase);
        var formGroups = _tldml.Value.ApplicationControl.TuiFormGroups;

        foreach (var tldTuiFormGroup in formGroups)
        {
          var formgroupLaunchPhases = tldTuiFormGroup.Value.FormGrouplaunchPhases;
          foreach (var tldTuiFormGroupLaunchPhase in formgroupLaunchPhases)
          {
            if (tldTuiFormGroupLaunchPhase.Code.Equals(phaseCode))
            {
              if (tldTuiFormGroupLaunchPhase.PeriodType.Equals("claims", StringComparison.OrdinalIgnoreCase))
              {
                if (needsClaimCheck)
                {
                  result.Add(tldTuiFormGroup.Key);
                }
              }
              else
              {
                result.Add(tldTuiFormGroup.Key);
              }
              break;
            }
          }
        }
      }

      return result;
    }

    public int GetMinPreRegLength(LaunchPhases phase)
    {
      return _tldml.Value.Product.PreregistrationYears(PhaseHelper.GetPhaseCode(phase)).Min;
    }

    public int GetMaxPreRegLength(LaunchPhases phase)
    {
      return _tldml.Value.Product.PreregistrationYears(PhaseHelper.GetPhaseCode(phase)).Max;
    }

    public bool HasPhaseApplicationFee(LaunchPhases phase, out string applicationProductType)
    {
      bool result = _tldml.Value.Product.HasPhaseApplicationFee(PhaseHelper.GetPhaseCode(phase), out applicationProductType);

      return result;
    }

    public List<int> GetPhaseApplicationProductIdList(LaunchPhases phase)
    {
      var result = new List<int>();

      string applicationProductType;

      if (HasPhaseApplicationFee(phase, out applicationProductType))
      {
        result = _tldml.Value.Product.GetPhaseApplicationProductIdList(applicationProductType);
      }

      return result;
    }

    public int GetProductId(IDomainProductLookup domainProductLookup)
    {
      var result = 0;

      var productTypeId = InternalGetTldProductTypeId(domainProductLookup.DomainCount, domainProductLookup.ProductType);

      var productTiers = InternalGetProductTiers(domainProductLookup.TldPhaseCode, productTypeId, domainProductLookup.RegistryId, domainProductLookup.PriceTierId);
      if (productTiers != null && !productTiers.ValidTierExist(domainProductLookup.DomainCount))
      {
        productTypeId = InternalGetTldProductTypeId(domainProductLookup.DomainCount, domainProductLookup.ProductType, false);

        productTiers = InternalGetProductTiers(domainProductLookup.TldPhaseCode, productTypeId, domainProductLookup.RegistryId, domainProductLookup.PriceTierId);
      }

      if (productTiers != null)
      {
        TLDProduct product;
        if (productTiers.TryGetProduct(domainProductLookup.Years, domainProductLookup.DomainCount, out product))
        {
          result = product.UnifiedProductId;
        }
      }

      return result;
    }

    public List<int> GetProductIdList(IDomainProductListLookup domainProductListLookup)
    {
      var result = new List<TLDProduct>(8);

      var productTypeId = InternalGetTldProductTypeId(domainProductListLookup.DomainCount, domainProductListLookup.ProductType);
      var productTiers = InternalGetProductTiers(domainProductListLookup.TldPhaseCode, productTypeId, domainProductListLookup.RegistryId, domainProductListLookup.PriceTierId);
      if (productTiers != null && !productTiers.ValidTierExist(domainProductListLookup.DomainCount))
      {
        productTypeId = InternalGetTldProductTypeId(domainProductListLookup.DomainCount, domainProductListLookup.ProductType, false);
        productTiers = InternalGetProductTiers(domainProductListLookup.TldPhaseCode, productTypeId, domainProductListLookup.RegistryId, domainProductListLookup.PriceTierId);
      }

      if (productTiers != null)
      {
        foreach (int year in domainProductListLookup.Years)
        {
          TLDProduct product;
          if ((productTiers.TryGetProduct(year, domainProductListLookup.DomainCount, out product)) && (product.IsValid))
          {
            result.Add(product);
          }
        }
      }

      return result.ConvertAll(product => product.UnifiedProductId); 
    }

    public bool HasLeafPage
    {
      get
      {
        bool result = false;

        if (_tldAvailabilityData.Value != null)
        {
          ITldAvailability tldAvailability;
          if (_tldAvailabilityData.Value.TldAvailabilityList.TryGetValue(_tld, out tldAvailability))
          {
            result = tldAvailability.HasLeafPage;
          }
        }
        return result;
      }
    }

    private static int InternalGetTldProductTypeId(int domainCount, TLDProductTypes productType, bool getBulkBasedOnDomainCount = true)
    {
      var productTypeId = TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_REGISTRATION;

      switch (productType)
      {
        case TLDProductTypes.Registration:
          productTypeId = domainCount > 1 && getBulkBasedOnDomainCount ? 
            TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_REGISTRATION : TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_REGISTRATION;
          break;
        case TLDProductTypes.Transfer:
          productTypeId = domainCount > 1 && getBulkBasedOnDomainCount ? 
            TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_TRANSFER : TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_TRANSFER;
          break;
        case TLDProductTypes.Renewal:
          productTypeId = domainCount > 1 && getBulkBasedOnDomainCount ? 
            TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_RENEWAL : TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_RENEWAL;
          break;
      }

      return productTypeId;
    }

    private TLDProductTiers InternalGetProductTiers(string phaseCode, int productTypeId, int? registryId, int? priceTierId)
    {
      var productDomainAttributesResponse = LoadProductDomainAttributes(phaseCode, _siteContext.Value.PrivateLabelId, productTypeId);

      TLDProductTiers productTiers = null;

      if (productDomainAttributesResponse != null)
      {
        if (registryId == null)
        {
          registryId = 0;
        }

        if (priceTierId == null)
        {
          priceTierId = 0;
        }

        if (registryId > 0 && priceTierId > 0)
        {
          productTiers = productDomainAttributesResponse.GetProductTiersByRegistryAndPriceTier((int)registryId,
                                                                                               (int)priceTierId);
        }
        else if (registryId > 0)
        {
          productTiers = productDomainAttributesResponse.GetProductTiersByRegistry((int)registryId);
        }
        else if (priceTierId > 0)
        {
          productTiers = productDomainAttributesResponse.GetProductTiersByPriceTier((int)priceTierId);
        }

        if (productTiers == null)
        {
          productTiers = productDomainAttributesResponse.GetDefaultProductTiers();
        }
      }

      return productTiers;
    }

    public string GetRegistrationFieldsXml()
    {
      return _domainContactFieldsData.Value.DomainContactFields;
    }
  }
}
