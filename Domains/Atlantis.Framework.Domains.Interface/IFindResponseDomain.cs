using System;
using System.Collections.Generic;
using Atlantis.Framework.DataCacheGeneric.Interface;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Domains.Interface
{
  public interface IFindResponseDomain
  {
    bool IsBadWord { get; }
    bool HasDashes { get; }
    bool HasNumbers { get; }
    bool IsAvailable { get; }
    bool IsIdn { get; }
    bool IsBackOrderAvailable { get; }
    bool IsTypo { get; }
    bool IsDomainUsingSynonym { get; }
    bool IsInternalTransfer { get; }
    bool IsPremium { get; }
    bool IsAuction { get; }
    bool IsCloseOutAuction { get; }
    bool IsValid { get; }
    bool HasLeafPage { get; }
    bool InPreRegPhase { get; }

    //Adding new error message for blocked domain names
    int InvalidReasonCode { get; }

    DateTime AuctionEndTimeStamp { get; } 
    DateTime LastUpdateTimeStamp { get; }
    DateTime WhoIsExpiration { get; }

    int DatabasePercentileRank { get; }
    int LengthOfSld { get; }
    int NumberOfKeywordsInDomain { get; }
    int Price { get; }
    int VendorId { get; }
    int VendorCost { get; }
    int? VendorTier { get; }
    int? InternalTier { get; }

    double CommissionPercentage { get; }

    string AuctionId { get; }
    string AuctionType { get; }
    string AuctionTypeId { get; }
    string Language { get; }
    string CurrencyType { get; }
    string AvailCheckTypePerformed { get; }
    string DomainSearchDataBase { get; }
    string IdnScript { get; }
    string IdnScriptId { get; }

    Dictionary<string, string> CartAttributes { get; }
    IDomain Domain { get; }
    IEnumerable<LaunchPhaseItem> LaunchPhaseItems { get; }
  }
}
