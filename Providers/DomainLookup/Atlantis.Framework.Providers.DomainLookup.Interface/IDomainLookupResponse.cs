using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.DomainLookup.Interface
{
    public interface IDomainLookupResponse
    {
        int PrivateLabelID { get; }
        string Shopperid { get; }
        bool IsSmartDomain { get; }
        bool IsMobilized { get; }
        bool IsActive { get; }
        DateTime ExpirationDate { get; }
        int DomainID { get; }
        int TldID { get; }
        int IsProxied { get; }
        int Status { get; }
        bool IsPremiumDomain { get; }
        int PremiumVendorMinPrice { get; }
        int PremiumVendorMaxPrice { get; }
        int PremiumVendorRecPrice { get; }
        int PremiumUserListPrice { get; }
        bool IsAuction { get; }
        decimal AuctionPrice { get; }
        int AuctionTypeID { get; }
        DateTime AuctionEndTime { get; }
        DateTime XfrAwayDate { get; }
        int XfrAwayDateUpdateReason { get; }
        bool is60dayLock { get; }
        DateTime CreateDate { get; }
        bool HasSuspectTerms { get; }
    }
}
