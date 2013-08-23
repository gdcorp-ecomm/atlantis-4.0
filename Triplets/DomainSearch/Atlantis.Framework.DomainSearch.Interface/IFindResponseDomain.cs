using System;
using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;

namespace Atlantis.Framework.DomainSearch.Interface
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

    DateTime AuctionEndTimeStamp { get; }
    DateTime LastUpdateTimeStamp { get; }
    DateTime WhoIsExpiration { get; }

    int DatabasePercentileRank { get; }
    int LengthOfSld { get; }
    int NumberOfKeywordsInDomain { get; }
    double CommissionPercentage { get; }
    int Price { get; }

    int VendorId { get; }
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
  }
}
