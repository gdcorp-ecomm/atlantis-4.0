using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeInfo
  {
    string DotType { get; }
    void RefreshIfNeeded();
    int MinExpiredAuctionRegLength { get; }
    int MaxExpiredAuctionRegLength { get; }
    int MinPreRegLength { get; }
    int MaxPreRegLength { get; }
    int MinRegistrationLength { get; }
    int MaxRegistrationLength { get; }
    int MinTransferLength { get; }
    int MaxTransferLength { get; }
    int MinRenewalLength { get; }
    int MaxRenewalLength { get; }
    bool HasExpiredAuctionRegIds { get; }
    bool HasPreRegIds { get; }
    bool HasRegistrationIds { get; }
    bool HasTransferIds { get; }
    bool HasRenewalIds { get; }
    bool IsMultiRegistrar { get; set; }
    Dictionary<string, string> AdditionalInfo { get; set; }

    bool HasExpiredAuctionRegIdsForRegistrarId(string registrarId);
    bool HasRegistrationIdsForRegistrarId(string registrarId);
    bool HasTransferIdsForRegistrarId(string registrarId);
    bool HasRenewalIdsForRegistrarId(string registrarId);

    int GetExpiredAuctionRegProductId(int registrationLength, int domainCount);
    int GetExpiredAuctionRegProductId(string registrarId, int registrationLength, int domainCount);
    int GetPreRegProductId(int registrationLength, int domainCount);
    int GetPreRegProductId(string registrarId, int registrationLength, int domainCount);
    int GetRegistrationProductId(int registrationLength, int domainCount);
    int GetRegistrationProductId(string registrarId, int registrationLength, int domainCount);
    int GetTransferProductId(int registrationLength, int domainCount);
    int GetTransferProductId(string registrarId, int registrationLength, int domainCount);
    int GetRenewalProductId(int registrationLength, int domainCount);
    int GetRenewalProductId(string registrarId, int registrationLength, int domainCount);

    List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidExpiredAuctionRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegProductIdList(string registrarId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(string registrarId, int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(string registrarId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(string registrarId, int domainCount, params int[] registrationLengths);

    List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths);
  }

  public interface IMultiRegDotTypeInfo
  {
    string GetRegistrarIdByPfid(int pfid);
  }
}
