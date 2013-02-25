﻿using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.DomainContactFields.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public class TLDMLDotTypeInfo : IDotTypeInfo
  {
    private const string _DEFAULTPREREGTYPE = "GA";

    private string _tld;
    private Lazy<ProductIdListResponseData> _productIdList;
    private Lazy<RegDotTypeRegistryResponseData> _dotTypeRegistryData;
    private Lazy<TLDMLByNameResponseData> _tldml;
    private Lazy<int> _tldId;
    private Lazy<DomainContactFieldsResponseData> _domainContactFieldsData;
    private Lazy<TLDLanguageResponseData> _languagesData;

    internal static TLDMLDotTypeInfo FromDotType(string dotType)
    {
      return new TLDMLDotTypeInfo(dotType);
    }

    private TLDMLDotTypeInfo(string tld)
    {
      _tld = tld;
      _tldId = new Lazy<int>(() => { return LoadTldId(); });
      _productIdList = new Lazy<ProductIdListResponseData>(() => { return LoadProductIds(); });
      _dotTypeRegistryData = new Lazy<RegDotTypeRegistryResponseData>(() => { return LoadDotTypeRegistryData(); });
      _tldml = new Lazy<TLDMLByNameResponseData>(() => { return LoadTLDML(); });
      _domainContactFieldsData = new Lazy<DomainContactFieldsResponseData>(LoadDomainContactFieldsData);
      _languagesData = new Lazy<TLDLanguageResponseData>(LoadLanguagesData);

      ITLDProduct product = _tldml.Value.Product; // Preload the TLDML
    }

    private int LoadTldId()
    {
      const string tldIdColumnName = "tldid";
      int result = 0;

      var tldData = DataCache.DataCache.GetExtendedTLDData(_tld);
      Dictionary<string, string> tldInfo;
      if (tldData.TryGetValue(_tld, out tldInfo))
      {
        string tldIdString;
        if (tldInfo.TryGetValue(tldIdColumnName, out tldIdString))
        {
          int tldId;
          if (int.TryParse(tldIdString, out tldId))
          {
            result = tldId;
          }
        }
      }

      return result;
    }

    private TLDMLByNameResponseData LoadTLDML()
    {
      TLDMLByNameRequestData request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
      return (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.TLDMLByName);
    }

    private RegDotTypeRegistryResponseData LoadDotTypeRegistryData()
    {
      RegDotTypeRegistryRequestData request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
      return (RegDotTypeRegistryResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.Registry);
    }

    private ProductIdListResponseData LoadProductIds()
    {
      ProductIdListRequestData request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
      return (ProductIdListResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductIdList);
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

    public int MinPreRegLength
    {
      get { return _tldml.Value.Product.PreregistrationYears(_DEFAULTPREREGTYPE).Min; }
    }

    public int MaxPreRegLength
    {
      get { return _tldml.Value.Product.PreregistrationYears(_DEFAULTPREREGTYPE).Max; }
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

    public int MaxRenewalMonthsOut
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsMultiRegistry
    {
      // TODO: how to tell multiregistry from TLDML?
      get { return StaticDotTypes.IsDotTypeMultiRegistry(_tld); }
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
              maxRenewalDate = DateTime.Now.Date.AddMonths(Tld.RenewProhibitedPeriodForExpiration*-1);
              break;
            case "day":
              maxRenewalDate = DateTime.Now.Date.AddDays(Tld.RenewProhibitedPeriodForExpiration*-1);
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
        int? renewalMonthsBeforeExpiration = TLDRenewal.GetRenewalMonthsBeforeExpiration(DotType);
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

    public int GetPreRegProductId(int registrationLength, int domainCount)
    {
      // TODO: handle prereg subtype
      return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.PreRegister);
    }

    public int GetPreRegProductId(string registryId, int registrationLength, int domainCount)
    {
      // TODO: handle prereg subtype
      return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.PreRegister);
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

      if (productType == DotTypeProductTypes.Registration)
      {
        registryId = _dotTypeRegistryData.Value.RegistrationAPI.Id;
      }
      else if (productType == DotTypeProductTypes.Transfer)
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
        tiers = _productIdList.Value.GetProductTiersForRegistry(registryId, productType);
      }

      if (tiers == null)
      {
        tiers = _productIdList.Value.GetDefaultProductTiers(productType);
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

    public List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.PreRegister, _tldml.Value.Product.PreregistrationYears(_DEFAULTPREREGTYPE), domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidProductIds(DotTypeProductTypes.PreRegister, _tldml.Value.Product.PreregistrationYears(_DEFAULTPREREGTYPE), registryId, domainCount, registrationLengths);
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

    public List<int> GetValidPreRegLengths(int domainCount, params int[] registrationLengths)
    {
      return InternalGetValidYears(DotTypeProductTypes.PreRegister, _tldml.Value.Product.PreregistrationYears(_DEFAULTPREREGTYPE), domainCount, registrationLengths);
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

      if (productType == DotTypeProductTypes.Registration)
      {
        registryId = _dotTypeRegistryData.Value.RegistrationAPI.Id;
      }
      else if (productType == DotTypeProductTypes.Transfer)
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
        tiers = _productIdList.Value.GetProductTiersForRegistry(registryId, productType);
      }

      if (tiers == null)
      {
        tiers = _productIdList.Value.GetDefaultProductTiers(productType);
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
      if (_productIdList.Value.TryGetProductByProductId(productId, out product))
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

    public string GetRegistrationFieldsXml()
    {
      return _domainContactFieldsData.Value.DomainContactFields;
    }
  }
}
