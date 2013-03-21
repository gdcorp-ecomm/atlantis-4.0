using System;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public class MultiRegDotTypeInfo : IDotTypeInfo
  {
    private const string _MISSING_ID_ERROR = "Missing ProductId for registryapiid: {0}; registrationLength: {1}; domainCount: {2}";
    private DotTypeProductTiers _registerProducts;
    private DotTypeProductTiers _transferProducts;

    private IDotTypeInfo _dotTypeInfo;

    private RegDotTypeRegistryResponseData _registryData;
    private ProductIdListResponseData _productData;

    private MultiRegDotTypeInfo(string dotType)
    {
      this._dotTypeInfo = StaticDotTypes.GetDotType(dotType);

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
      get { return this._dotTypeInfo.DotType; }
    }

    public int MinExpiredAuctionRegLength
    {
      get { return this._dotTypeInfo.MinExpiredAuctionRegLength; }
    }

    public int MaxExpiredAuctionRegLength
    {
      get { return this._dotTypeInfo.MaxExpiredAuctionRegLength; }
    }

    public int MinPreRegLength
    {
      get { return this._dotTypeInfo.MinPreRegLength; }
    }

    public int MaxPreRegLength
    {
      get { return this._dotTypeInfo.MaxPreRegLength; }
    }

    public int MinRegistrationLength
    {
      get { return this._dotTypeInfo.MinRegistrationLength; }
    }

    public int MaxRegistrationLength
    {
      get { return this._dotTypeInfo.MaxRegistrationLength; }
    }

    public int MinTransferLength
    {
      get { return this._dotTypeInfo.MinTransferLength; }
    }

    public int MaxTransferLength
    {
      get { return this._dotTypeInfo.MaxTransferLength; }
    }

    public int MinRenewalLength
    {
      get { return this._dotTypeInfo.MinRenewalLength; }
    }

    public int MaxRenewalLength
    {
      get { return this._dotTypeInfo.MaxRenewalLength; }
    }

    public bool IsMultiRegistry
    {
      get { return true; }
    }

    public IEnumerable<RegistryLanguage> RegistryLanguages
    {
      get { return this._dotTypeInfo.RegistryLanguages; }
    }

    public RegistryLanguage GetLanguageByName(string languageName)
    {
      return this._dotTypeInfo.GetLanguageByName(languageName);
    }

    public RegistryLanguage GetLanguageById(int languageId)
    {
      return this._dotTypeInfo.GetLanguageById(languageId);
    }

    public bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength)
    {
      return this._dotTypeInfo.CanRenew(currentExpirationDate, out maxValidRenewalLength);
    }

    public int GetPreRegProductId(int registrationLength, int domainCount)
    {
      return this._dotTypeInfo.GetPreRegProductId(registrationLength, domainCount);
    }

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      return this._dotTypeInfo.GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public int GetRegistrationProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (this._registerProducts == null)
      {
        result = this._dotTypeInfo.GetRegistrationProductId(registrationLength, domainCount);
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

      if (this._transferProducts == null)
      {
        result = this._dotTypeInfo.GetTransferProductId(registrationLength, domainCount);
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
      return this._dotTypeInfo.GetRenewalProductId(registrationLength, domainCount);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidPreRegProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._registerProducts == null)
      {
        return this._dotTypeInfo.GetValidRegistrationProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._registerProducts, MinRegistrationLength, MaxRegistrationLength, domainCount, registrationLengths);
      }
    }

    public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._transferProducts == null)
      {
        return this._dotTypeInfo.GetValidTransferProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._transferProducts, MinTransferLength, MaxTransferLength, domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidRenewalProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidExpiredAuctionRegLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegLengths(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidPreRegLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidRegistrationLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidTransferLengths(domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths)
    {
      return this._dotTypeInfo.GetValidRenewalLengths(domainCount, registrationLengths);
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
        productId = this.GetExpiredAuctionRegProductId(registrationLength, domainCount);
        string message = string.Format(_MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetExpiredAuctionRegProductId", message, this.DotType);
      }

      return productId;
    }

    public int GetPreRegProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.PreRegister, registrationLength, domainCount, MinPreRegLength, MaxPreRegLength);

      if (productId < 0)
      {
        productId = this.GetPreRegProductId(registrationLength, domainCount);
        string message = string.Format(_MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetPreRegProductId", message, this.DotType);
      }

      return productId;
    }

    public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.Registration, registrationLength, domainCount, MinRegistrationLength, MaxRegistrationLength);

      if (productId < 0)
      {
        productId = this.GetRegistrationProductId(registrationLength, domainCount);
        string message = string.Format(_MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetRegistrationProductId", message, this.DotType);
      }

      return productId;
    }

    public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.Transfer, registrationLength, domainCount, MinTransferLength, MaxTransferLength);

      if (productId < 0)
      {
        productId = this.GetTransferProductId(registrationLength, domainCount);
        string message = string.Format(_MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetTransferProductId", message, this.DotType);
      }

      return productId;
    }

    public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
    {
      int productId = GetRegistryProductId(registryId, DotTypeProductTypes.Renewal, registrationLength, domainCount, MinRenewalLength, MaxRenewalLength);

      if (productId < 0)
      {
        productId = this.GetRenewalProductId(registrationLength, domainCount);
        string message = string.Format(_MISSING_ID_ERROR, registryId, registrationLength, domainCount);
        Logging.LogException("MultiRegDotTypeInfo.GetRegistrationProductId", message, this.DotType);
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
      else
      {
        return GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidPreRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.PreRegister);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinPreRegLength, MaxPreRegLength, domainCount, registrationLengths);
      }
      else
      {
        return GetValidPreRegProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.Registration);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinRegistrationLength, MaxRegistrationLength, domainCount, registrationLengths);
      }
      else
      {
        return GetValidRegistrationProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.Transfer);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinTransferLength, MaxTransferLength, domainCount, registrationLengths);
      }
      else
      {
        return GetValidTransferProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      DotTypeProductTiers productTiers = _productData.GetProductTiersForRegistry(registryId, DotTypeProductTypes.Renewal);
      if (productTiers != null)
      {
        return GetValidProductIdList(productTiers, MinRenewalLength, MaxRenewalLength, domainCount, registrationLengths);
      }
      else
      {
        return GetValidRenewalProductIdList(domainCount, registrationLengths);
      }
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
      get { return this._dotTypeInfo.Product; }
    }

    public int TldId
    {
      get { return this._dotTypeInfo.TldId; }
    }

    public ITLDTld Tld
    {
      get { return this._dotTypeInfo.Tld; } 
    }

    public ITLDApplicationControl ApplicationControl
    {
      get { return this._dotTypeInfo.ApplicationControl; }
    }

    public bool IsValidPreRegistrationPhase(string type, string subType, out ITLDPreRegistrationPhase preRegistrationPhase)
    {
      return this._dotTypeInfo.IsValidPreRegistrationPhase(type, subType, out preRegistrationPhase);
    }

    public string GetRegistrationFieldsXml()
    {
      return this._dotTypeInfo.GetRegistrationFieldsXml();
    }

  }
}
