using System;

namespace Atlantis.Framework.Providers.DomainLookup.Interface
{
    public interface IDomainLookupData
    {
      int PrivateLabelId { get; }
      string Shopperid { get; }
      bool IsSmartDomain { get; }
      bool IsMobilized { get; }
      bool IsActive { get; }
      DateTime ExpirationDate { get; }
      int DomainId { get; }
      int TldId { get; }
      int IsProxied { get; }
      int Status { get; }
      bool IsPremiumDomain { get; }
      int PremiumVendorMinPrice { get; }
      int PremiumVendorMaxPrice { get; }
      int PremiumVendorRecPrice { get; }
      int PremiumUserListPrice { get; }
      bool IsAuction { get; }
      decimal AuctionPrice { get; }
      int AuctionTypeId { get; }
      DateTime AuctionEndTime { get; }
      DateTime XfrAwayDate { get; }
      int XfrAwayDateUpdateReason { get; }
      bool Is60DayLock { get; }
      DateTime CreateDate { get; }
      bool HasSuspectTerms { get; }
    }
}
