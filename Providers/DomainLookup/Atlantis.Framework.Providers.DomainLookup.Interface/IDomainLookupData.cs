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
      int PdDomainId { get; }
      string DomainName { get; }
      int ClientId { get; }
      string Client { get; }
      string AdultClient { get; }
      int Channel { get; }
      int Pkid { get; }
      string PkidTemplateId { get; }
      int AdultStatusMap { get; }
      string TemplateId { get; }
      int SubCategoryTypeId { get; }
      string PatKeywords { get; }
      int InvalidWhoIsStatus { get; }
      int HasClicks { get; }
      int HasProdRev { get; }
      DateTime RecordUpdated { get; }
      string CashParkingTemplateId { get; }
      bool CashParkingActiveFlag { get; }
      int CashParkingAdultMode { get; }
      string CashParkingKeywords { get; }
      int CashParkingThemeId { get; }
      int CashParkingTemplateTypeCode { get; }
      int CashParkingSubCategoryTypeId { get; }
      int CashParkingTrpListingStatusCode { get; }
      int CashParkingClientId { get; }
      string CashParkingClient { get; }
      string CashParkingAdultClient { get; }
      int CashParkingChannel { get; }
      bool CashParkingPopundersEnabled { get; }
      bool CashParkingZeroclickEnabled { get; }
      int CashParkingInquireLinkEnabled { get; }
      string CustomListingText { get; }
      string CustomListingLink { get; }
      string DRID { get; }
    }
}
