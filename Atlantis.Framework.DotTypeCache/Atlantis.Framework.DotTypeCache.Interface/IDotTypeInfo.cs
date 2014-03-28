using System;
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

    [Obsolete("Use the new method GetProductId instead.")]
    int GetExpiredAuctionRegProductId(int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetRegistrationProductId(int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetRegistrationProductId(string registryId, int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetTransferProductId(int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetTransferProductId(string registryId, int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetRenewalProductId(int registrationLength, int domainCount);
    [Obsolete("Use the new method GetProductId instead.")]
    int GetRenewalProductId(string registryId, int registrationLength, int domainCount);

    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
    List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths);
    [Obsolete("Use the new method GetProductIdList instead.")]
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

    ITLDLaunchPhase GetLaunchPhase(LaunchPhases launchPhase);
    ITLDLaunchPhaseGroupCollection GetAllLaunchPhaseGroups(bool activeOnly = true);
    bool IsPreRegPhaseActive { get; }
    IList<string> GetTuiFormTypes(LaunchPhases launchPhase);
    bool RequiresTuiForm(LaunchPhases launchPhase);

    int GetMinPreRegLength(LaunchPhases phase);
    int GetMaxPreRegLength(LaunchPhases phase);
    bool HasPhaseApplicationFee(LaunchPhases phase, out string applicationProductType);
    List<int> GetPhaseApplicationProductIdList(LaunchPhases phase);

    int GetProductId(IDomainProductLookup domainProductLookup);
    List<int> GetProductIdList(IDomainProductListLookup domainProductListLookup);
    List<int> GetTrusteeProductId(TLDProductTypes productType);

  }
}
