using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public class MultiRegDotTypeInfo : IDotTypeInfo
  {
    private static readonly ITLDLaunchPhaseGroup _gaLaunchPhaseGroup = TldLaunchPhaseGroup.CreateGroup(TldLaunchPhase.GeneralAvailabilityActive());
    private static readonly IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> _gaLaunchPhaseDictionary = new Dictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> { { LaunchPhaseGroupTypes.GeneralAvailability, _gaLaunchPhaseGroup } };
    private static readonly ITLDLaunchPhaseGroupCollection _gaTldLaunchPhaseCollection = TldLaunchPhaseGroupCollection.CreateCollection(_gaLaunchPhaseDictionary);

    private const string MISSING_ID_ERROR = "Missing ProductId for registryapiid: {0}; registrationLength: {1}; domainCount: {2}";
    private DotTypeProductTiers _registerProducts;
    private DotTypeProductTiers _transferProducts;

    private readonly IDotTypeInfo _dotTypeInfo;

    private readonly RegDotTypeRegistryResponseData _registryData;
    private readonly ProductIdListResponseData _productData;

    private MultiRegDotTypeInfo(string dotType)
    {
      _dotTypeInfo = StaticDotTypes.GetDotType(dotType);

      var registryRequest = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dotType);
      _registryData = (RegDotTypeRegistryResponseData)DataCache.DataCache.GetProcessRequest(registryRequest, DotTypeEngineRequests.Registry);

      var productRequest = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dotType);
      _productData = (ProductIdListResponseData)DataCache.DataCache.GetProcessRequest(productRequest, DotTypeEngineRequests.ProductIdList);

      LoadActiveRegistryDotTypeInfo();
    }

    internal static MultiRegDotTypeInfo GetMultiRegDotTypeInfo(string dotType)
    {
      return new MultiRegDotTypeInfo(dotType);
    }

    private void LoadActiveRegistryDotTypeInfo()
    {
      _registerProducts = _productData.GetProductTiersForRegistry(_registryData.RegistrationAPI.Id, DotTypeProductTypes.Registration);
      _transferProducts = _productData.GetProductTiersForRegistry(_registryData.TransferAPI.Id, DotTypeProductTypes.Transfer);
    }

    private List<int> GetValidProductIdList(DotTypeProductTiers products, int minLength, int maxLength, int domainCount, params int[] registrationLengths)
    {
      List<int> result = new List<int>(registrationLengths.Length);

      if (products != null)
      {
        foreach (int registrationLength in registrationLengths)
        {
          if ((registrationLength >= minLength) && (registrationLength <= maxLength))
          {
            DotTypeProduct product;
            if (products.TryGetProduct(registrationLength, domainCount, out product))
            {
              result.Add(product.ProductId);
            }
          }
        }
      }

      return result;
    }

    public string DotType
    {
      get { return _dotTypeInfo.DotType; }
    }

    public int MinExpiredAuctionRegLength
    {
      get { return _dotTypeInfo.MinExpiredAuctionRegLength; }
    }

    public int MaxExpiredAuctionRegLength
    {
      get { return _dotTypeInfo.MaxExpiredAuctionRegLength; }
    }

    public int MinRegistrationLength
    {
      get { return _dotTypeInfo.MinRegistrationLength; }
    }

    public int MaxRegistrationLength
    {
      get { return _dotTypeInfo.MaxRegistrationLength; }
    }

    public int MinTransferLength
    {
      get { return _dotTypeInfo.MinTransferLength; }
    }

    public int MaxTransferLength
    {
      get { return _dotTypeInfo.MaxTransferLength; }
    }

    public int MinRenewalLength
    {
      get { return _dotTypeInfo.MinRenewalLength; }
    }

    public int MaxRenewalLength
    {
      get { return _dotTypeInfo.MaxRenewalLength; }
    }

    public bool IsMultiRegistry
    {
      get { return true; }
    }
    public bool IsGtld
    {
      get { return  _dotTypeInfo.IsGtld; }
    }

    public IEnumerable<RegistryLanguage> RegistryLanguages
    {
      get { return _dotTypeInfo.RegistryLanguages; }
    }

    public RegistryLanguage GetLanguageByName(string languageName)
    {
      return _dotTypeInfo.GetLanguageByName(languageName);
    }

    public RegistryLanguage GetLanguageById(int languageId)
    {
      return _dotTypeInfo.GetLanguageById(languageId);
    }

    public bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength)
    {
      return _dotTypeInfo.CanRenew(currentExpirationDate, out maxValidRenewalLength);
    }

    public int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount)
    {
      return _dotTypeInfo.GetPreRegProductId(phase, registrationLength, domainCount);
    }

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      return _dotTypeInfo.GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public int GetRegistrationProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (_registerProducts == null)
      {
        result = _dotTypeInfo.GetRegistrationProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinRegistrationLength) && (registrationLength <= MaxRegistrationLength))
      {
        DotTypeProduct product;
        if (_registerProducts.TryGetProduct(registrationLength, domainCount, out product))
        {
          result = product.ProductId;
        }
      }

      return result;
    }

    public int GetTransferProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (_transferProducts == null)
      {
        result = _dotTypeInfo.GetTransferProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinTransferLength) && (registrationLength <= MaxTransferLength))
      {
        DotTypeProduct product;
        if (_transferProducts.TryGetProduct(registrationLength, domainCount, out product))
        {
          result = product.ProductId;
        }
      }

      return result;
    }

    public int GetRenewalProductId(int registrationLength, int domainCount)
    {
      return _dotTypeInfo.GetRenewalProductId(registrationLength, domainCount);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidPreRegProductIdList(phase, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (_registerProducts == null)
      {
        return _dotTypeInfo.GetValidRegistrationProductIdList(domainCount, registrationLengths);
      }
      
      return GetValidProductIdList(_registerProducts, MinRegistrationLength, MaxRegistrationLength, domainCount, registrationLengths);
    }

    public List<int> GetTrusteeProductId(TLDProductTypes productType)
    {
        return new List<int>();
    }

    public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (_transferProducts == null)
      {
        return _dotTypeInfo.GetValidTransferProductIdList(domainCount, registrationLengths);
      }
      
      return GetValidProductIdList(_transferProducts, MinTransferLength, MaxTransferLength, domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidRenewalProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidExpiredAuctionRegLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegLengths(LaunchPhases phase, int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidPreRegLengths(phase, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidRegistrationLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidTransferLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths)
    {
      return _dotTypeInfo.GetValidRenewalLengths(domainCount, registrationLengths);
    }

    private int GetRegistryProductId(string registryId, DotTypeProductTypes productType, int registrationLength, int domainCount, int minRegistrationLength, int maxRegistrationLength)
    {
      int productId = -1;

      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, productType);
      if (productTiers != null)
      {
        if ((registrationLength >= minRegistrationLength) && (registrationLength <= maxRegistrationLength))
        {
          DotTypeProduct product;
          if (productTiers.TryGetProduct(registrationLength, domainCount, out product))
          {
            productId = product.ProductId;
          }
        }
      }

      return productId;
    }

    public int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.ExpiredAuctionReg, registrationLength, domainCount, MinExpiredAuctionRegLength, MaxExpiredAuctionRegLength);
      if (productId < 0)
      {
        productId = GetExpiredAuctionRegProductId(registrationLength, domainCount);
        string message = string.Format(MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetExpiredAuctionRegProductId", message, DotType);
      }

      return productId;
    }

    public int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.PreRegister, registrationLength, domainCount, GetMinPreRegLength(phase), GetMaxPreRegLength(phase));

      if (productId < 0)
      {
        productId = GetPreRegProductId(phase, registrationLength, domainCount);
        string message = string.Format(MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetPreRegProductId", message, DotType);
      }

      return productId;
    }

    public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.Registration, registrationLength, domainCount, MinRegistrationLength, MaxRegistrationLength);

      if (productId < 0)
      {
        productId = GetRegistrationProductId(registrationLength, domainCount);
        string message = string.Format(MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetRegistrationProductId", message, DotType);
      }

      return productId;
    }

    public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.Transfer, registrationLength, domainCount, MinTransferLength, MaxTransferLength);

      if (productId < 0)
      {
        productId = GetTransferProductId(registrationLength, domainCount);
        string message = string.Format(MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetTransferProductId", message, DotType);
      }

      return productId;
    }

    public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.Renewal, registrationLength, domainCount, MinRenewalLength, MaxRenewalLength);

      if (productId < 0)
      {
        productId = GetRenewalProductId(registrationLength, domainCount);
        string message = string.Format(MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetRegistrationProductId", message, DotType);
      }

      return productId;
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.ExpiredAuctionReg);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinExpiredAuctionRegLength, MaxExpiredAuctionRegLength, domainCount, registrationLengths);
      }

      return GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.PreRegister);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, GetMinPreRegLength(phase), GetMaxPreRegLength(phase), domainCount, registrationLengths);
      }
      
      return GetValidPreRegProductIdList(phase, domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.Registration);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinRegistrationLength, MaxRegistrationLength, domainCount, registrationLengths);
      }
      
      return GetValidRegistrationProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.Transfer);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinTransferLength, MaxTransferLength, domainCount, registrationLengths);
      }
      
      return GetValidTransferProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.Renewal);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinRenewalLength, MaxRenewalLength, domainCount, registrationLengths);
      }
      
      return GetValidRenewalProductIdList(domainCount, registrationLengths);
    }

    public string GetRegistryIdByProductId(int productId)
    {
      string result = string.Empty;

      DotTypeProduct product;
      if (_productData.TryGetProductByProductId(productId, out product))
      {
        result = product.RegistryId;
      }
      return result;
    }

    public ITLDProduct Product
    {
      get { return _dotTypeInfo.Product; }
    }

    public int TldId
    {
      get { return _dotTypeInfo.TldId; }
    }

    public ITLDTld Tld
    {
      get { return _dotTypeInfo.Tld; }
    }

    public ITLDApplicationControl ApplicationControl
    {
      get { return _dotTypeInfo.ApplicationControl; }
    }

    public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
    {
      return _dotTypeInfo.GetLaunchPhase(phase);
    }

    public ITLDLaunchPhaseGroupCollection GetAllLaunchPhaseGroups(bool activeOnly = true)
    {
      return _gaTldLaunchPhaseCollection;
    }

    public bool IsPreRegPhaseActive
    {
      get { return _dotTypeInfo.IsPreRegPhaseActive; }
    }

    public IList<string> GetTuiFormTypes(LaunchPhases launchPhase)
    {
      return new List<string>();
    }

    public bool RequiresTuiForm(LaunchPhases launchPhase)
    {
      return _dotTypeInfo.RequiresTuiForm(launchPhase);
    }

    public int GetMinPreRegLength(LaunchPhases phase)
    {
      return _dotTypeInfo.GetMinPreRegLength(phase);
    }

    public int GetMaxPreRegLength(LaunchPhases phase)
    {
      return _dotTypeInfo.GetMaxPreRegLength(phase);
    }

    public bool HasPhaseApplicationFee(LaunchPhases phase, out string applicationProductType)
    {
      return _dotTypeInfo.HasPhaseApplicationFee(phase, out applicationProductType);
    }

    public List<int> GetPhaseApplicationProductIdList(LaunchPhases phase)
    {
      return _dotTypeInfo.GetPhaseApplicationProductIdList(phase);
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
      return _dotTypeInfo.GetRegistrationFieldsXml();
    }

  }
}
