using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public abstract class DotTypeStaticBase : IDotTypeInfo
  {
    private DotTypeProductIds _registerProductIds;
    private DotTypeProductIds _transferProductIds;
    private DotTypeProductIds _renewalProductIds;
    private DotTypeProductIds _preregProductIds;
    private DotTypeProductIds _expiredAuctionRegProductIds;

    public bool HasExpiredAuctionRegIds
    {
      get { return (_expiredAuctionRegProductIds != null); }
    }

    public bool HasPreRegIds
    {
      get { return (_preregProductIds != null); }
    }

    public bool HasRegistrationIds
    {
      get { return (_registerProductIds != null); }
    }

    public bool HasTransferIds
    {
      get { return (_transferProductIds != null); }
    }

    public bool HasRenewalIds
    {
      get { return (_renewalProductIds != null); }
    }

    private bool _isMultiRegistrar = false;
    public bool IsMultiRegistrar
    {
      get { return _isMultiRegistrar; }
      set { _isMultiRegistrar = value; }
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

    public virtual int MinExpiredAuctionRegLength
    {
      get { return 1; }
    }

    public virtual int MaxExpiredAuctionRegLength
    {
      get { return 10; }
    }

    public DotTypeStaticBase()
    {
      _registerProductIds = InitializeRegistrationProductIds();
      _transferProductIds = InitializeTransferProductIds();
      _renewalProductIds = InitializeRenewalProductIds();
      _preregProductIds = InitializePreRegistrationProductIds();
      _expiredAuctionRegProductIds = InitializeExpiredAuctionRegProductIds();
    }

    protected abstract DotTypeProductIds InitializeRegistrationProductIds();
    protected abstract DotTypeProductIds InitializeTransferProductIds();
    protected abstract DotTypeProductIds InitializeRenewalProductIds();
    protected virtual DotTypeProductIds InitializePreRegistrationProductIds()
    {
      return null;
    }
    protected virtual DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      return null;
    }

    #region IDotTypeInfo Members

    public abstract string DotType { get; }

    public Dictionary<string, string> AdditionalInfo { get; set; }

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

    private List<int> GetValidLengths(DotTypeProductIds productIds, int minLength, int maxLength,
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
    
    public void RefreshIfNeeded()
    {
      return;
    }

    public int GetPreRegProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      if ((_preregProductIds != null) 
        && (registrationLength >= MinPreRegLength) 
        && (registrationLength <= MaxPreRegLength))
      {
        result = _preregProductIds.GetProductId(registrationLength, domainCount);
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

    public List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths)
    {
      return GetValidProductIdList(_preregProductIds, MinPreRegLength, MaxPreRegLength, 
        domainCount, registrationLengths);
    }

    public List<int> GetValidPreRegLengths(int domainCount, params int[] registrationLengths)
    {
      return GetValidLengths(_preregProductIds, MinPreRegLength, MaxPreRegLength, 
        domainCount, registrationLengths);
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

    public bool HasPreRegIdsForRegistrarId(string registrarId)
    {
      return false;
    }

    public bool HasRegistrationIdsForRegistrarId(string registrarId)
    {
      return false;
    }

    public bool HasTransferIdsForRegistrarId(string registrarId)
    {
      return false;
    }

    public bool HasRenewalIdsForRegistrarId(string registrarId)
    {
      return false;
    }

    public bool HasExpiredAuctionRegIdsForRegistrarId(string registrarId)
    {
      return false;
    }

    public int GetPreRegProductId(string registrarId, int registrationLength, int domainCount)
    {
      return this.GetPreRegProductId(registrationLength, domainCount);
    }

    public int GetRegistrationProductId(string registrarId, int registrationLength, int domainCount)
    {
      return this.GetRegistrationProductId(registrationLength, domainCount);
    }

    public int GetTransferProductId(string registrarId, int registrationLength, int domainCount)
    {
      return this.GetTransferProductId(registrationLength, domainCount);
    }

    public int GetRenewalProductId(string registrarId, int registrationLength, int domainCount)
    {
      return this.GetRenewalProductId(registrationLength, domainCount);
    }

    public int GetExpiredAuctionRegProductId(string registrarId, int registrationLength, int domainCount)
    {
      return this.GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public List<int> GetValidPreRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidPreRegProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidRegistrationProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidRegistrationProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidTransferProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidTransferProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidRenewalProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidRenewalProductIdList(domainCount, registrationLengths);
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return this.GetValidExpiredAuctionRegProductIdList(domainCount, registrationLengths);
    }

    #endregion
  }
}
