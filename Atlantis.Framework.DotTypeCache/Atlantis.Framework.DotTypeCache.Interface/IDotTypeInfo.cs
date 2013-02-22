﻿using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeInfo
  {
    string DotType { get; }
    int MinExpiredAuctionRegLength { get; }
    int MaxExpiredAuctionRegLength { get; }
    int MinPreRegLength{ get; }
    int MaxPreRegLength{ get; }
    int MinRegistrationLength { get; }
    int MaxRegistrationLength { get; }
    int MinTransferLength { get; }
    int MaxTransferLength { get; }
    int MinRenewalLength { get; }
    int MaxRenewalLength { get; }
    int MaxRenewalMonthsOut { get; }
    bool IsMultiRegistry { get; }

    IEnumerable<RegistryLanguage> RegistryLanguages { get; }
    RegistryLanguage GetLanguageByName(string languageName);
    RegistryLanguage GetLanguageById(int languageId);
    
    int GetExpiredAuctionRegProductId(int registrationLength, int domainCount);
    int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount);
    int GetPreRegProductId(int registrationLength, int domainCount);
    int GetPreRegProductId(string registryId, int registrationLength, int domainCount);
    int GetRegistrationProductId(int registrationLength, int domainCount);
    int GetRegistrationProductId(string registryId, int registrationLength, int domainCount);
    int GetTransferProductId(int registrationLength, int domainCount);
    int GetTransferProductId(string registryId, int registrationLength, int domainCount);
    int GetRenewalProductId(int registrationLength, int domainCount);
    int GetRenewalProductId(string registryId, int registrationLength, int domainCount);

    List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths);

    List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths);
    
    string GetRegistrationFieldsXml();

    string GetRegistryIdByProductId(int productId);

    ITLDProduct Product { get; }
    int TldId { get; }
    ITLDTld Tld { get; }
  }
}
