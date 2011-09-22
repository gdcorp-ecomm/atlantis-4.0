using System.Collections.Generic;

namespace Atlantis.Framework.Auction.Interface
{
  public class AuctionStatusCodeDictionary
  {
    private static readonly IDictionary<int, AuctionStatusCode> _auctionStatusCodes = new Dictionary<int, AuctionStatusCode>()
    {
       {  1, new AuctionStatusCode {StatusCodeId = 1, StatusType = "ACTIVE", StatusDescription = "Active Member", StatusGroup = "MEMBERS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  2, new AuctionStatusCode {StatusCodeId = 2, StatusType = "BIDDER", StatusDescription = "Bidder offer", StatusGroup = "BIDS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  3, new AuctionStatusCode {StatusCodeId = 3, StatusType = "SELLER", StatusDescription = "Seller offer", StatusGroup = "BIDS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  4, new AuctionStatusCode {StatusCodeId = 4, StatusType = "OPEN", StatusDescription = "Auction is open", StatusGroup = "ITEMS", IsListable=true, IsMemberArea = true, AllowDuplicateListings = false}},
{  5, new AuctionStatusCode {StatusCodeId = 5, StatusType = "CLOSED", StatusDescription = "Auction is closed", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = true}},
{  6, new AuctionStatusCode {StatusCodeId = 6, StatusType = "SOLD", StatusDescription = "Bidder has purchased item", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = true}},
{  7, new AuctionStatusCode {StatusCodeId = 7, StatusType = "PENDING ITEM PAYMENT", StatusDescription = "Item listing is pending payment", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  8, new AuctionStatusCode {StatusCodeId = 8, StatusType = "DELETED", StatusDescription = "Item was built but removed", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = true}},
{  9, new AuctionStatusCode {StatusCodeId = 9, StatusType = "EDIT ITEM", StatusDescription = "Edit of item incomplete", StatusGroup = "ITEMS", IsListable=true, IsMemberArea = true, AllowDuplicateListings = false}},
{  10, new AuctionStatusCode {StatusCodeId = 10, StatusType = "CANCELED", StatusDescription = "Item listing canceled", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = true}},
{  11, new AuctionStatusCode {StatusCodeId = 11, StatusType = "PENDING FEATURE PAYMENT", StatusDescription = "Pending feature addon payment", StatusGroup = "ITEMS", IsListable=true, IsMemberArea = true, AllowDuplicateListings = false}},
{  14, new AuctionStatusCode {StatusCodeId = 14, StatusType = "BANNED", StatusDescription = "Member has been banned", StatusGroup = "MEMBERS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  15, new AuctionStatusCode {StatusCodeId = 15, StatusType = "ITEM PAYMENT FAILED", StatusDescription = "Payment of item failed", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  16, new AuctionStatusCode {StatusCodeId = 16, StatusType = "FEATURE PAYMENT FAILED", StatusDescription = "Payment of addon feature failed", StatusGroup = "EDIT", IsListable=true, IsMemberArea = true, AllowDuplicateListings = false}},
{  17, new AuctionStatusCode {StatusCodeId = 17, StatusType = "PENDING BIDDER PAYMENT", StatusDescription = "Pending bidder purchase", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  18, new AuctionStatusCode {StatusCodeId = 18, StatusType = "BIDDER PAYMENT FAILED", StatusDescription = "Bidder purchase failed", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  19, new AuctionStatusCode {StatusCodeId = 19, StatusType = "COMPLETED", StatusDescription = "Item was sold,purchased,transferred", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = true}},
{  20, new AuctionStatusCode {StatusCodeId = 20, StatusType = "BID CANCELED", StatusDescription = "Customer service bid canceled", StatusGroup = "BIDS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  21, new AuctionStatusCode {StatusCodeId = 21, StatusType = "ITEM BUILD", StatusDescription = "Item build incomplete", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = true}},
{  22, new AuctionStatusCode {StatusCodeId = 22, StatusType = "RELIST", StatusDescription = "Item relisted", StatusGroup = "ITEMS", IsListable=true, IsMemberArea = true, AllowDuplicateListings = false}},
{  23, new AuctionStatusCode {StatusCodeId = 23, StatusType = "PENDING APPROVAL", StatusDescription = "Item is pending GoDaddy approval", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  24, new AuctionStatusCode {StatusCodeId = 24, StatusType = "LISTING REJECTED", StatusDescription = "Listing rejected by GoDaddy", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  25, new AuctionStatusCode {StatusCodeId = 25, StatusType = "CANCEL PAID", StatusDescription = "Listing was cancelled after it was paid", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = true}},
{  26, new AuctionStatusCode {StatusCodeId = 26, StatusType = "DUPLICATE LISTING", StatusDescription = "Duplicate Listing", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  27, new AuctionStatusCode {StatusCodeId = 27, StatusType = "BLOCKED", StatusDescription = "Member blocked from bidding/selling", StatusGroup = "MEMBERS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  28, new AuctionStatusCode {StatusCodeId = 28, StatusType = "NEVER BLOCK", StatusDescription = "Member never blocked from bidding", StatusGroup = "MEMBERS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  29, new AuctionStatusCode {StatusCodeId = 29, StatusType = "INACTIVE", StatusDescription = "Inactive Member", StatusGroup = "MEMBERS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  30, new AuctionStatusCode {StatusCodeId = 30, StatusType = "TO BE CANCELLED", StatusDescription = "Item cancelled by the bulk load process", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  31, new AuctionStatusCode {StatusCodeId = 31, StatusType = "Miscellaneous", StatusDescription = "Miscellaneous", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = true, AllowDuplicateListings = false}},
{  32, new AuctionStatusCode {StatusCodeId = 32, StatusType = "ITEM ON HOLD", StatusDescription = "Closeout item on hold", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  33, new AuctionStatusCode {StatusCodeId = 33, StatusType = "HOLD", StatusDescription = "HOLD", StatusGroup = "AUCTION", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  34, new AuctionStatusCode {StatusCodeId = 34, StatusType = "Escrow - Cancelled", StatusDescription = "Escrow.com (0) - Transaction Cancelled", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  35, new AuctionStatusCode {StatusCodeId = 35, StatusType = "Escrow - Parties Accepted", StatusDescription = "Escrow.com (15) - Both Parties Accepted", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  36, new AuctionStatusCode {StatusCodeId = 36, StatusType = "Escrow - Payment Type", StatusDescription = "Escrow.com (20) - Buyer selected a payment type", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  37, new AuctionStatusCode {StatusCodeId = 37, StatusType = "Escrow - Funds Secured", StatusDescription = "Escrow.com (25) - Funds secured, seller transfer name", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  38, new AuctionStatusCode {StatusCodeId = 38, StatusType = "Escrow - Name Transferred", StatusDescription = "Escrow.com (30) - Seller transferred name.  Buyer asked to confirm.", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  39, new AuctionStatusCode {StatusCodeId = 39, StatusType = "Escrow - Buyer has Name", StatusDescription = "Escrow.com (35) - Buyer confirms they have name.", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  40, new AuctionStatusCode {StatusCodeId = 40, StatusType = "Escrow - Buyer accepts Name", StatusDescription = "Escrow.com (40) - Buyer accepts name.", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  41, new AuctionStatusCode {StatusCodeId = 41, StatusType = "Escrow - Buyer / Seller Dispute over name", StatusDescription = "Escrow.com (45-70) - Buyer / Seller dispute name.", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  42, new AuctionStatusCode {StatusCodeId = 42, StatusType = "Escrow - Pending Cancellation", StatusDescription = "Escrow.com (75) - Cancellation is pending.", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  43, new AuctionStatusCode {StatusCodeId = 43, StatusType = "Escrow - Complete", StatusDescription = "Escrow.com (80) - Transacton complete, buyer has name and seller has funds.", StatusGroup = "ITEMS", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  44, new AuctionStatusCode {StatusCodeId = 44, StatusType = "Approval - Auto Approved", StatusDescription = "Approval - Auto Approved", StatusGroup = "APPROVAL", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  45, new AuctionStatusCode {StatusCodeId = 45, StatusType = "Approval - Manually Approved", StatusDescription = "Approval - Manually Approved", StatusGroup = "APPROVAL", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  46, new AuctionStatusCode {StatusCodeId = 46, StatusType = "Approval - Pending Admin Response", StatusDescription = "Approval - Pending Admin Response", StatusGroup = "APPROVAL", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  47, new AuctionStatusCode {StatusCodeId = 47, StatusType = "Approval - Pending Auto Approval", StatusDescription = "Approval - Pending Auto Approval", StatusGroup = "APPROVAL", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  48, new AuctionStatusCode {StatusCodeId = 48, StatusType = "Approval - Pending Manual Approval", StatusDescription = "Approval - Pending Manual Approval", StatusGroup = "APPROVAL", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  49, new AuctionStatusCode {StatusCodeId = 49, StatusType = "Auction Status Backorder close", StatusDescription = "Auction Status Backorder close", StatusGroup = "Auction", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  50, new AuctionStatusCode {StatusCodeId = 50, StatusType = "OK to Pay", StatusDescription = "Default status - ready to move to 'Send to GAP after waiting period'", StatusGroup = "GAP", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  51, new AuctionStatusCode {StatusCodeId = 51, StatusType = "Do not Pay", StatusDescription = "Do not send to GAP - Terminal status", StatusGroup = "GAP", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  52, new AuctionStatusCode {StatusCodeId = 52, StatusType = "Pay Now", StatusDescription = "Skip payment rules and pay now", StatusGroup = "GAP", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  53, new AuctionStatusCode {StatusCodeId = 53, StatusType = "Pay On Hold", StatusDescription = "Manual override. Wait to Pay", StatusGroup = "GAP", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  54, new AuctionStatusCode {StatusCodeId = 54, StatusType = "Send to GAP", StatusDescription = "Ready to be processed or ready to send to GAP", StatusGroup = "GAP", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}},
{  55, new AuctionStatusCode {StatusCodeId = 55, StatusType = "Sent to GAP", StatusDescription = "Was processed and has been sent to GAP", StatusGroup = "GAP", IsListable=false, IsMemberArea = false, AllowDuplicateListings = false}}

    };

    public static IDictionary<int, AuctionStatusCode> AuctionStatusCodes { get { return _auctionStatusCodes; } }
  }
}
