using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public class InvalidDotType : IDotTypeInfo
  {
    public InvalidDotType()
    {
    }

    #region IDotTypeInfo Members

    public string DotType
    {
      get { return "INVALID"; }
    }

    public void RefreshIfNeeded()
    { }

    public int MinExpiredAuctionRegLength
    {
      get { return 1; }
    }

    public int MaxExpiredAuctionRegLength
    {
      get { return 10; }
    }

    public int MinPreRegLength
    {
      get { return 1; }
    }

    public int MaxPreRegLength
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

    public Dictionary<string, string> AdditionalInfo
    {
      get { return new Dictionary<string, string>(); }
      set { }
    }

    public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetPreRegProductId(int registrationLength, int domainCount)
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

    public List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths)
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

    public bool HasPreRegIds
    {
      get { return false; }
    }

    public bool HasRegistrationIds
    {
      get { return false; }
    }

    public bool HasTransferIds
    {
      get { return false; }
    }

    public bool HasRenewalIds
    {
      get { return false; }
    }

    private bool _isMultiRegistrar = false;
    public bool IsMultiRegistrar
    {
      get { return _isMultiRegistrar; }
      set { _isMultiRegistrar = false; }
    }

    public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegLengths(int domainCount, params int[] registrationLengths)
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

    public bool HasExpiredAuctionRegIdsForRegistrarId(string registrarId)
    {
      return false;
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

    public int GetExpiredAuctionRegProductId(string registrarId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetPreRegProductId(string registrarId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetRegistrationProductId(string registrarId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetTransferProductId(string registrarId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public int GetRenewalProductId(string registrarId, int registrationLength, int domainCount)
    {
      return 0;
    }

    public List<int> GetValidExpiredAuctionRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRegistrationProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidTransferProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRenewalProductIdList(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidExpiredAuctionRegLengths(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidPreRegLengths(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRegistrationLengths(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidTransferLengths(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    public List<int> GetValidRenewalLengths(string registrarId, int domainCount, params int[] registrationLengths)
    {
      return new List<int>();
    }

    #endregion
  }
}
