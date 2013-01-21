using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeInfo
  {
    string DotType { get; }
    int MinExpiredAuctionRegLength { get; }
    int MaxExpiredAuctionRegLength { get; }
    int MinPreRegistrationLength(string preRegistrationType);
    int MaxPreRegistrationLength(string preRegistrationType);
    int MinRegistrationLength { get; }
    int MaxRegistrationLength { get; }
    int MinTransferLength { get; }
    int MaxTransferLength { get; }
    int MinRenewalLength { get; }
    int MaxRenewalLength { get; }
    int MaxRenewalMonthsOut { get; }
    bool IsMultiRegistry { get; }

    int GetExpiredAuctionRegProductId(int registrationLength, int domainCount);
    int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount);
    int GetPreRegistrationProductId(int registrationLength, int domainCount, string preRegistrationType);
    int GetPreRegistrationProductId(string registryId, int registrationLength, int domainCount, string preRegistrationType);
    int GetRegistrationProductId(int registrationLength, int domainCount);
    int GetRegistrationProductId(string registryId, int registrationLength, int domainCount);
    int GetTransferProductId(int registrationLength, int domainCount);
    int GetTransferProductId(string registryId, int registrationLength, int domainCount);
    int GetRenewalProductId(int registrationLength, int domainCount);
    int GetRenewalProductId(string registryId, int registrationLength, int domainCount);

    List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegistrationProductIdList(int domainCount, string preRegistrationType, params int[] registrationLengths);
    List<int> GetValidPreRegistrationProductIdList(string registryId, int domainCount, string preRegistrationType, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths);

    List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegistrationLengths(int domainCount, string preRegistrationType, params int[] registrationLengths);
    List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths);
    
    string GetRegistrationFieldsXml();

    string GetRegistryIdByProductId(int productId);
  }
}
