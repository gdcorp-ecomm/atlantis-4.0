﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeInfo
  {
    string DotType { get; }
    int MinExpiredAuctionRegLength { get; }
    int MaxExpiredAuctionRegLength { get; }
    int MinRegistrationLength { get; }
    int MaxRegistrationLength { get; }
    int MinTransferLength { get; }
    int MaxTransferLength { get; }
    int MinRenewalLength { get; }
    int MaxRenewalLength { get; }
    bool IsMultiRegistry { get; }
    bool IsGtld { get; }

    IEnumerable<RegistryLanguage> RegistryLanguages { get; }
    RegistryLanguage GetLanguageByName(string languageName);
    RegistryLanguage GetLanguageById(int languageId);
    bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength);

    int GetExpiredAuctionRegProductId(int registrationLength, int domainCount);
    int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount);
    int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount);
    int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount);
    int GetRegistrationProductId(int registrationLength, int domainCount);
    int GetRegistrationProductId(string registryId, int registrationLength, int domainCount);
    int GetTransferProductId(int registrationLength, int domainCount);
    int GetTransferProductId(string registryId, int registrationLength, int domainCount);
    int GetRenewalProductId(int registrationLength, int domainCount);
    int GetRenewalProductId(string registryId, int registrationLength, int domainCount);

    List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths);

    List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidPreRegLengths(LaunchPhases phase, int domainCount, params int[] registrationLengths);
    List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths);
    List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths);

    string GetRegistrationFieldsXml();

    string GetRegistryIdByProductId(int productId);

    ITLDProduct Product { get; }
    int TldId { get; }
    ITLDTld Tld { get; }
    ITLDApplicationControl ApplicationControl { get; }

    Dictionary<string, ITLDLaunchPhase> GetAllLaunchPhases(string periodType = "");
    ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase);
    bool IsLivePhase(LaunchPhases phase);
    bool HasPreRegPhases { get; }

    int GetMinPreRegLength(LaunchPhases phase);
    int GetMaxPreRegLength(LaunchPhases phase);
    bool HasPreRegApplicationFee(LaunchPhases phase);
    int GetPreRegApplicationProductId(LaunchPhases phase);
  }
}
