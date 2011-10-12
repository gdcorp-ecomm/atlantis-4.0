using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Xml;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.RegGetDotTypeProductIdList.Interface;
using Atlantis.Framework.RegGetDotTypeRegistrar.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public class MultiRegDotTypeInfo : IDotTypeInfo, IMultiRegDotTypeInfo
  {
    #region Properties
    private const string _MISSING_ID_ERROR 
      = "Missing ProductId for registryapiid: {0}; registrationLength: {1}; domainCount: {2}";
    private DotTypeProductIds _registerProductIds;
    private DotTypeProductIds _transferProductIds;
    private DotTypeProductIds _renewalProductIds;
    private DotTypeProductIds _preregProductIds;
    private DotTypeProductIds _expiredAuctionRegProductIds;
    private const int _maxRegArrayLenght = 10;
    private const int _maxTrnfArrayLenght = 9;
    private IDotTypeInfo _dotTypeInfo;
    private ISiteContext _siteContext;
    private IShopperContext _shopperContext;
    private Dictionary<DotTypeProductIdTypes, string> _activeRegistrarByRegType;
    internal Dictionary<DotTypeProductIdTypes, string> ActiveRegistrarByRegType
    {
      get
      {
        if (_activeRegistrarByRegType == null)
        {
          _activeRegistrarByRegType = GetActiveRegistrarByRegType();
        }

        return _activeRegistrarByRegType;
      }
    }
    private Dictionary<string, Dictionary<DotTypeProductIdTypes, DotTypeProductIds>> _multiRegDotTypeProductIds
      = new Dictionary<string, Dictionary<DotTypeProductIdTypes, DotTypeProductIds>>();

    private Dictionary<int, string> _multiRegRegistrarLookupByPfid
      = new Dictionary<int,string>();

    #endregion Properties

    #region Constractors

    private MultiRegDotTypeInfo(string dotType)
    {
      this._siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      this._shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      this._dotTypeInfo = DotTypeCache.GetStaticDotTypeInfo(dotType);

      try
      {
        LoadMultiRegDotTypeInfo(dotType);
        LoadActiveRegistrarDotTypeInfo();
      }
      catch (Exception ex)
      {
        LogException("Atlantis.Framework.DotTypeCache.MultiRegDotTypeInfo", ex.Message, ex.Source);
        Debug.WriteLine("Error creating MultiRegDotTypeInfo for ." + dotType);
        throw;
      }
    }

    public static MultiRegDotTypeInfo GetMultiRegDotTypeInfo(string dotType)
    {
      return new MultiRegDotTypeInfo(dotType);
    }

    #endregion Constractors

    #region Private Methods

    private bool HasIdsForRegistrarIdHelper(string registrarId, DotTypeProductIdTypes dotTypeProductIdTypes, string callingFunction)
    {
      bool result = false;

      if (IsValidRegistrarId(registrarId))
      {
        DotTypeProductIds productIds;
        if (this._multiRegDotTypeProductIds[registrarId].TryGetValue(dotTypeProductIdTypes, out productIds))
        {
          if (productIds != null)
          {
            result = true;
          }
          else
          {
            LogException(callingFunction, "Missing data for registryapiid: " + registrarId, string.Empty);
          }
        }
      }

      return result;
    }

    private void LoadActiveRegistrarDotTypeInfo()
    {
      foreach (KeyValuePair<DotTypeProductIdTypes, string> activeRegistrar in ActiveRegistrarByRegType)
      {
        Dictionary<DotTypeProductIdTypes, DotTypeProductIds> dotTypeProductIds;

        if (this._multiRegDotTypeProductIds.TryGetValue(activeRegistrar.Value, out dotTypeProductIds))
        {
          foreach (KeyValuePair<DotTypeProductIdTypes, DotTypeProductIds> pair in dotTypeProductIds)
          {
            if (activeRegistrar.Key == pair.Key)
            {
              switch (pair.Key)
              {
                case DotTypeProductIdTypes.Register:
                  this._registerProductIds = pair.Value;
                  break;
                case DotTypeProductIdTypes.Transfer:
                  this._transferProductIds = pair.Value;
                  break;
                case DotTypeProductIdTypes.PreRegister:
                  this._preregProductIds = pair.Value;
                  break;
                case DotTypeProductIdTypes.Renewal:
                  this._renewalProductIds = pair.Value;
                  break;
                case DotTypeProductIdTypes.ExpiredAuctionReg:
                  this._expiredAuctionRegProductIds = pair.Value;
                  break;
              }
            }
          }
        }
      }
    }

    private void LoadMultiRegDotTypeInfo(string dotType)
    {
      const string productNodeName = "product";
      const string durationAttrName = "duration";
      const string quantityAttrName = "quantity";
      const string typeAttrName = "type";
      const string productIdAttrName = "id";
      const string registrationApiIdAttrName = "registrationapiid";
      string currentElementName = null;
      string responseXml = GetDotTypeProductIdListXml(dotType);

      using (XmlReader reader = XmlReader.Create(new StringReader(responseXml)))
      {
        while (reader.Read())
        {
          switch (reader.NodeType)
          {
            case XmlNodeType.Element:
              currentElementName = reader.Name;

              if (currentElementName.Equals(productNodeName, StringComparison.InvariantCultureIgnoreCase))
              {
                int duration = 0, quantity = 0, productId = 0;
                string type = string.Empty, registrationApiId = string.Empty;

                DotTypeProductIdTypes validType;

                if (reader.MoveToAttribute(durationAttrName))
                {
                  if (!int.TryParse(reader.Value, out duration))
                  {
                    duration = 0;
                  }
                }

                if (reader.MoveToAttribute(quantityAttrName))
                {
                  if (!int.TryParse(reader.Value, out quantity))
                  {
                    quantity = 0;
                  }
                }

                if (reader.MoveToAttribute(productIdAttrName))
                {
                  if (!int.TryParse(reader.Value, out productId))
                  {
                    productId = 0;
                  }
                }

                if (reader.MoveToAttribute(registrationApiIdAttrName))
                {
                  registrationApiId = reader.Value;
                }

                if (reader.MoveToAttribute(typeAttrName))
                {
                  type = reader.Value;
                }

                if (GetValidDotTypeProductIdType(type, out validType)
                  && (duration > 0) && (quantity > 0) && (productId > 0))
                {
                  AddDotTypeTier(validType, registrationApiId, productId, duration, quantity);
                }
              }
              break;
            default:
              break;
          }
        }
      }
    }

    private void AddDotTypeTier(DotTypeProductIdTypes type, string registrationApiId,
      int productId, int registrationLength, int domainCount)
    {
      Dictionary<DotTypeProductIdTypes, DotTypeProductIds> dotTypeProductIds;
      int arrayLength = GetProductIdsArrayLength(type);
      int durationIndex = registrationLength - 1;
      int[] productIds = new int[arrayLength];
      DotTypeProductIds tiers;

      if (this._multiRegDotTypeProductIds.TryGetValue(registrationApiId, out dotTypeProductIds))
      {
        productIds[durationIndex] = productId;
        if (!dotTypeProductIds.TryGetValue(type, out tiers))
        {
          tiers = new DotTypeProductIds(type, new DotTypeTier[] { new DotTypeTier(domainCount, productIds) });
        }
        else
        {
          DotTypeTier tier = tiers.GetTier(domainCount);

          if (tier == null || tier.MinDomains != domainCount)
          {
            tier = new DotTypeTier(domainCount, productIds);
            tiers.AddDotTypeTier(tier);
          }
          else
          {
            tier.AddProductId(registrationLength, productId);
          }
        }
      }
      else
      {
        dotTypeProductIds = new Dictionary<DotTypeProductIdTypes, DotTypeProductIds>();
        productIds[durationIndex] = productId;
        tiers = new DotTypeProductIds(type, new DotTypeTier[] { new DotTypeTier(domainCount, productIds) });
      }
      foreach (int tempProductId in productIds)
      {
        if (!_multiRegRegistrarLookupByPfid.ContainsKey(tempProductId))
        {
          this._multiRegRegistrarLookupByPfid.Add(tempProductId, registrationApiId);
        }
      }

      dotTypeProductIds[type] = tiers;
      this._multiRegDotTypeProductIds[registrationApiId] = dotTypeProductIds;
    }

    private string GetDotTypeProductIdListXml(string dotType)
    {
      string xml = string.Empty;
      RegGetDotTypeProductIdListRequestData request
        = new RegGetDotTypeProductIdListRequestData(this._shopperContext.ShopperId,
        HttpContext.Current.Request.Url.ToString(), string.Empty, this._siteContext.Pathway,
        this._siteContext.PageCount, dotType);
      request.RequestTimeout = TimeSpan.FromMilliseconds(6000);
      RegGetDotTypeProductIdListResponseData response
        = (RegGetDotTypeProductIdListResponseData)Engine.Engine.ProcessRequest(request,
        DotTypeEngineRequests.GetDotTypeProductIdListRequest);

      if (response.IsValid)
      {
        xml = response.ToXML();
      }

      return xml;
    }

    private Dictionary<DotTypeProductIdTypes, string> GetActiveRegistrarByRegType()
    {
      Dictionary<DotTypeProductIdTypes, string> dict = new Dictionary<DotTypeProductIdTypes, string>();
      string currentElementName = null;
      string xml = GetDotTypeRegistrarXml(this._dotTypeInfo.DotType);
      const string apiinfoNodeName = "apiinfo";
      const string registryapiidAttrName = "registryapiid";
      const string typeAttrName = "type";

      if (!string.IsNullOrEmpty(xml))
      {
        using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
        {
          while (reader.Read())
          {
            switch (reader.NodeType)
            {
              case XmlNodeType.Element:
                currentElementName = reader.Name;
                if (currentElementName.Equals(apiinfoNodeName, StringComparison.InvariantCultureIgnoreCase))
                {
                  string registryapiid = string.Empty, type = string.Empty;
                  DotTypeProductIdTypes validType;

                  if (reader.MoveToAttribute(registryapiidAttrName))
                  {
                    registryapiid = reader.Value;
                  }

                  if (reader.MoveToAttribute(typeAttrName))
                  {
                    type = reader.Value;
                  }

                  if (GetValidDotTypeProductIdType(type, out validType))
                  {
                    dict[validType] = registryapiid;
                  }
                }
                break;
              default:
                break;
            }
          }
        }
      }

      return dict;
    }

    private string GetDotTypeRegistrarXml(string dotType)
    {
      string xml = string.Empty;

      try
      {
        RegGetDotTypeRegistrarRequestData request 
          = new RegGetDotTypeRegistrarRequestData(this._shopperContext.ShopperId,
          HttpContext.Current.Request.Url.ToString(), string.Empty, this._siteContext.Pathway,
          this._siteContext.PageCount, dotType);
        request.RequestTimeout = TimeSpan.FromMilliseconds(6000);
        RegGetDotTypeRegistrarResponseData response
          = (RegGetDotTypeRegistrarResponseData)Engine.Engine.ProcessRequest(request, 
          DotTypeEngineRequests.RegGetDotTypeRegistrarRequest);

        if (response.IsValid)
        {
          xml = response.ToXML();
        }
      }
      catch (Exception ex)
      {
        LogException("MultiRegDotTypeInfo.GetDotTypeRegistrarXml", ex.Message, ex.Source);
      }

      return xml;
    }

    private bool GetValidDotTypeProductIdType(string value, out DotTypeProductIdTypes type)
    {
      bool isValidType = false;
      type = DotTypeProductIdTypes.Register;

      if (value.Equals("registration", StringComparison.InvariantCultureIgnoreCase))
      {
        type = DotTypeProductIdTypes.Register;
        isValidType = true;
      }
      else if (value.Equals(Enum.GetName(typeof(DotTypeProductIdTypes), 1), StringComparison.InvariantCultureIgnoreCase))
      {
        type = DotTypeProductIdTypes.Transfer;
        isValidType = true;
      }
      else if (value.Equals(Enum.GetName(typeof(DotTypeProductIdTypes), 2), StringComparison.InvariantCultureIgnoreCase))
      {
        type = DotTypeProductIdTypes.Renewal;
        isValidType = true;
      }
      else if (value.Equals(Enum.GetName(typeof(DotTypeProductIdTypes), 3), StringComparison.InvariantCultureIgnoreCase))
      {
        type = DotTypeProductIdTypes.PreRegister;
        isValidType = true;
      }
      else if (value.Equals(Enum.GetName(typeof(DotTypeProductIdTypes), 4), StringComparison.InvariantCultureIgnoreCase))
      {
        type = DotTypeProductIdTypes.ExpiredAuctionReg;
        isValidType = true;
      }

      return isValidType;
    }

    private int GetProductIdsArrayLength(DotTypeProductIdTypes regType)
    {
      if (regType == DotTypeProductIdTypes.Transfer)
      {
        return _maxTrnfArrayLenght;
      }
      else
      {
        return _maxRegArrayLenght;
      }
    }

    private List<int> GetValidProductIdList(DotTypeProductIds productIds, int minLength, int maxLength,
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

    private List<int> GetValidProductIdList(string registrarId, DotTypeProductIds productIds, int minLength, int maxLength,
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

    private bool IsValidRegistrarId(string registrarId)
    {
      return this._multiRegDotTypeProductIds.ContainsKey(registrarId);
    }

    private void LogException(string sourceFunction, string message, string source)
    {
      AtlantisException aex = new AtlantisException(sourceFunction, HttpContext.Current.Request.Url.ToString(),
      "0", message, source, this._shopperContext.ShopperId, string.Empty,
      HttpContext.Current.Request.UserHostAddress, this._siteContext.Pathway, this._siteContext.PageCount);
      Engine.Engine.LogAtlantisException(aex);
    }

    #endregion Private Methods

    #region IDotTypeInfo Members

    public string DotType
    {
      get { return this._dotTypeInfo.DotType; }
    }

    public void RefreshIfNeeded()
    {
      this._dotTypeInfo.RefreshIfNeeded();
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

    public bool HasExpiredAuctionRegIds
    {
      get { return this._dotTypeInfo.HasExpiredAuctionRegIds; }
    }

    public bool HasPreRegIds
    {
      get { return this._dotTypeInfo.HasPreRegIds; }
    }

    public bool HasRegistrationIds
    {
      get { return this._dotTypeInfo.HasRegistrationIds; }
    }

    public bool HasTransferIds
    {
      get { return this._dotTypeInfo.HasTransferIds; }
    }

    public bool HasRenewalIds
    {
      get { return this._dotTypeInfo.HasRenewalIds; }
    }

    private bool _isMultiRegistrar = true;
    public bool IsMultiRegistrar
    {
      get { return _isMultiRegistrar; }
      set { _isMultiRegistrar = value; }
    }

    public Dictionary<string, string> AdditionalInfo
    {
      get { return this._dotTypeInfo.AdditionalInfo; }
      set { this._dotTypeInfo.AdditionalInfo = value; }
    }

    public int GetPreRegProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (this._preregProductIds == null)
      {
        result = this._dotTypeInfo.GetPreRegProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinPreRegLength)
        && (registrationLength <= MaxPreRegLength))
      {
        result = this._preregProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (this._expiredAuctionRegProductIds == null)
      {
        result = this._dotTypeInfo.GetExpiredAuctionRegProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinExpiredAuctionRegLength)
        && (registrationLength <= MaxExpiredAuctionRegLength))
      {
        result = this._expiredAuctionRegProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetRegistrationProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (this._registerProductIds == null)
      {
        result = this._dotTypeInfo.GetRegistrationProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinRegistrationLength)
        && (registrationLength <= MaxRegistrationLength))
      {
        result = this._registerProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetTransferProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (this._transferProductIds == null)
      {
        result = this._dotTypeInfo.GetTransferProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinTransferLength)
        && (registrationLength <= MaxTransferLength))
      {
        result = this._transferProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public int GetRenewalProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if (this._renewalProductIds == null)
      {
        result = this._dotTypeInfo.GetRenewalProductId(registrationLength, domainCount);
      }
      else if ((registrationLength >= MinRenewalLength)
        && (registrationLength <= MaxRenewalLength))
      {
        result = this._renewalProductIds.GetProductId(registrationLength, domainCount);
      }

      return result;
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._expiredAuctionRegProductIds == null)
      {
        return this._dotTypeInfo.GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._expiredAuctionRegProductIds, MinExpiredAuctionRegLength, MaxExpiredAuctionRegLength,
          domainCount, registrationLengths);
      }
    }

    public List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._preregProductIds == null)
      {
        return this._dotTypeInfo.GetValidPreRegProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._preregProductIds, MinPreRegLength, MaxPreRegLength,
          domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._registerProductIds == null)
      {
        return this._dotTypeInfo.GetValidRegistrationProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._registerProductIds, MinRegistrationLength, MaxRegistrationLength,
          domainCount, registrationLengths);
      }
    }

    public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._transferProductIds == null)
      {
        return this._dotTypeInfo.GetValidTransferProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._transferProductIds, MinTransferLength, MaxTransferLength,
          domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
    {
      if (this._renewalProductIds == null)
      {
        return this._dotTypeInfo.GetValidRenewalProductIdList(domainCount, registrationLengths);
      }
      else
      {
        return GetValidProductIdList(this._renewalProductIds, MinRenewalLength, MaxRenewalLength,
          domainCount, registrationLengths);
      }
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

    public bool HasExpiredAuctionRegIdsForRegistrarId(string registrarId)
    {
      return HasIdsForRegistrarIdHelper(registrarId, DotTypeProductIdTypes.ExpiredAuctionReg, "MultiRegDotTypeInfo.HasExpiredAuctionRegIdsForRegistrarId");
    }

    public bool HasPreRegIdsForRegistrarId(string registrarId)
    {
      return HasIdsForRegistrarIdHelper(registrarId, DotTypeProductIdTypes.PreRegister, "MultiRegDotTypeInfo.HasPreRegIdsForRegistrarId");
    }

    public bool HasRegistrationIdsForRegistrarId(string registrarId)
    {
      return HasIdsForRegistrarIdHelper(registrarId, DotTypeProductIdTypes.Register, "MultiRegDotTypeInfo.HasRegistrationIdsForRegistrarId");
    }

    public bool HasTransferIdsForRegistrarId(string registrarId)
    {
      return HasIdsForRegistrarIdHelper(registrarId, DotTypeProductIdTypes.Transfer, "MultiRegDotTypeInfo.HasTransferIdsForRegistrarId");
    }

    public bool HasRenewalIdsForRegistrarId(string registrarId)
    {
      return HasIdsForRegistrarIdHelper(registrarId, DotTypeProductIdTypes.Renewal, "MultiRegDotTypeInfo.HasRenewalIdsForRegistrarId");
    }

    public string GetRegistrarIdByPfid(int pfid)
    {
      if (this._multiRegRegistrarLookupByPfid.ContainsKey(pfid))
      {
        return _multiRegRegistrarLookupByPfid[pfid];
      }
      return String.Empty;
    }
    public int GetExpiredAuctionRegProductId(string registrarId, int registrationLength, int domainCount)
    {
      int productId = -1;

      if (HasExpiredAuctionRegIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.ExpiredAuctionReg];

        if ((registrationLength >= MinExpiredAuctionRegLength)
        && (registrationLength <= MaxExpiredAuctionRegLength))
        {
          productId = productIds.GetProductId(registrationLength, domainCount);
        }
      }

      if (productId < 0)
      {
        productId = this.GetExpiredAuctionRegProductId(registrationLength, domainCount);
        LogException("MultiRegDotTypeInfo.GetExpiredAuctionRegProductId",
              string.Format(_MISSING_ID_ERROR, registrarId, registrationLength, domainCount), string.Empty);
      }

      return productId;
    }

    public int GetPreRegProductId(string registrarId, int registrationLength, int domainCount)
    {
      int productId = -1;

      if (HasPreRegIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.PreRegister];

        if ((registrationLength >= MinPreRegLength)
        && (registrationLength <= MaxPreRegLength))
        {
          productId = productIds.GetProductId(registrationLength, domainCount);
        }
      }

      if(productId < 0)
      {
        productId = this.GetPreRegProductId(registrationLength, domainCount);
        LogException("MultiRegDotTypeInfo.GetPreRegProductId",
              string.Format(_MISSING_ID_ERROR, registrarId, registrationLength, domainCount), string.Empty);
      }

      return productId;
    }

    public int GetRegistrationProductId(string registrarId, int registrationLength, int domainCount)
    {
      int productId = -1;

      if (HasRegistrationIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.Register];

        if ((registrationLength >= MinRegistrationLength)
        && (registrationLength <= MaxRegistrationLength))
        {
          productId = productIds.GetProductId(registrationLength, domainCount);
        }
      }

      if (productId < 0)
      {
        productId = this.GetRegistrationProductId(registrationLength, domainCount);
        LogException("MultiRegDotTypeInfo.GetRegistrationProductId",
              string.Format(_MISSING_ID_ERROR, registrarId, registrationLength, domainCount), string.Empty);
      }

      return productId;
    }

    public int GetTransferProductId(string registrarId, int registrationLength, int domainCount)
    {
      int productId = -1;

      if (HasTransferIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.Transfer];

        if ((registrationLength >= MinTransferLength)
        && (registrationLength <= MaxTransferLength))
        {
          productId = productIds.GetProductId(registrationLength, domainCount);
        }
      }

      if (productId < 0)
      {
        productId = this.GetTransferProductId(registrationLength, domainCount);
        LogException("MultiRegDotTypeInfo.GetTransferProductId",
              string.Format(_MISSING_ID_ERROR, registrarId, registrationLength, domainCount), string.Empty);
      }

      return productId;
    }

    public int GetRenewalProductId(string registrarId, int registrationLength, int domainCount)
    {
      int productId = -1;

      if (HasRenewalIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.Renewal];

        if ((registrationLength >= MinRenewalLength)
        && (registrationLength <= MaxRenewalLength))
        {
          productId = productIds.GetProductId(registrationLength, domainCount);
        }
      }

      if (productId < 0)
      {
        productId = this.GetRenewalProductId(registrationLength, domainCount);
        LogException("MultiRegDotTypeInfo.GetRegistrationProductId",
              string.Format(_MISSING_ID_ERROR, registrarId, registrationLength, domainCount), string.Empty);
      }

      return productId;
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      if (HasExpiredAuctionRegIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.ExpiredAuctionReg];
        return GetValidProductIdList(productIds, MinExpiredAuctionRegLength, MaxExpiredAuctionRegLength,
          domainCount, registrationLengths);
      }
      else
      {
        return GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidPreRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      if (HasPreRegIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.PreRegister];
        return GetValidProductIdList(productIds, MinPreRegLength, MaxPreRegLength,
          domainCount, registrationLengths);
      }
      else
      {
        return GetValidPreRegProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRegistrationProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      if (HasRegistrationIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.Register];
        return GetValidProductIdList(productIds, MinRegistrationLength, MaxRegistrationLength,
          domainCount, registrationLengths);
      }
      else
      {
        return GetValidRegistrationProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidTransferProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      if (HasTransferIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.Transfer];
        return GetValidProductIdList(productIds, MinTransferLength, MaxTransferLength,
          domainCount, registrationLengths);
      }
      else
      {
        return GetValidTransferProductIdList(domainCount, registrationLengths);
      }
    }

    public List<int> GetValidRenewalProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      if (HasRenewalIdsForRegistrarId(registrarId))
      {
        DotTypeProductIds productIds = this._multiRegDotTypeProductIds[registrarId][DotTypeProductIdTypes.Renewal];
        return GetValidProductIdList(productIds, MinRenewalLength, MaxRenewalLength,
          domainCount, registrationLengths);
      }
      else
      {
        return GetValidRenewalProductIdList(domainCount, registrationLengths);
      }
    }

    public List<string> GetRegistrarIds()
    {
      var registrarIds = new List<string>();
      foreach(string key in _multiRegDotTypeProductIds.Keys)
      {
          registrarIds.Add(key);
      }
      return registrarIds;
    }

    #endregion
  }
}
