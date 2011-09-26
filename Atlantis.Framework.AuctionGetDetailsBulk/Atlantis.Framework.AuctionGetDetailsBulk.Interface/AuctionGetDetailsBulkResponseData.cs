using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Auction.Interface;

namespace Atlantis.Framework.AuctionGetDetailsBulk.Interface
{
  public class AuctionGetDetailsBulkResponseData : IResponseData
  {
    #region Members

    private AtlantisException _atlEx;
    private string _auctionDetailBulkResponseXml;

    #endregion


    #region Properties

    public bool IsSuccess { get; private set; }

    public bool IsValidGetAuctionDetailBulkRequest { get; private set; }

    public string ErrorMessage { get; private set; }

    public List<AuctionDetailResponse> AuctionDetailList { get; private set; }

    public bool DidDetailsHaveErrors { get; private set; }
    #endregion


    #region Constructors

    public AuctionGetDetailsBulkResponseData(string responseXml)
    {
      if (!string.IsNullOrEmpty(responseXml))
      {
        _auctionDetailBulkResponseXml = responseXml;
        List<AuctionDetailResponse> auctionDetailList = null;
        if (ParseAuctionDetailResponse(responseXml, out auctionDetailList))
        {
          AuctionDetailList = auctionDetailList;
          IsSuccess = true;
        }
      }
    }

    public AuctionGetDetailsBulkResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
        "AuctionGetDetailsBulkResponseData",
        ex.Message,
        oRequestData.ToXML());
    }

    #endregion


    #region Methods

    private bool ParseAuctionDetailResponse(string responseXml, out List<AuctionDetailResponse> auctionDetailList)
    {
      bool success = false;
      auctionDetailList = new List<AuctionDetailResponse>();

      XDocument xDoc = XDocument.Parse(responseXml);

      if (xDoc.Root != null)
      {
        XElement bulkAuctionDetailsResponseElement = xDoc.Element("BulkAuctionDetailsResponse");

        if (bulkAuctionDetailsResponseElement != null && !string.IsNullOrEmpty(bulkAuctionDetailsResponseElement.Attribute("Valid").Value))
        {
          IsValidGetAuctionDetailBulkRequest = string.Compare(bulkAuctionDetailsResponseElement.Attribute("Valid").Value, "true", true) == 0;

          if (!IsValidGetAuctionDetailBulkRequest)
          {
            ErrorMessage = bulkAuctionDetailsResponseElement.Attribute("Error").Value;
          }
          else
          {
            List<XElement> detailResponses = xDoc.Descendants("GetAuctionDetailsRsp").ToList();

            if (detailResponses != null)
            {
              AuctionDetailResponse aDetailResponse = null;

              foreach (XElement item in detailResponses)
              {
                bool isValid = (item.Attribute("Valid") != null ? string.Compare(item.Attribute("Valid").Value, "true", true) == 0 : false);
                string errorMessage = (item.Attribute("Error") != null ? item.Attribute("Error").Value : string.Empty);

                aDetailResponse = new AuctionDetailResponse(isValid, errorMessage);

                if (isValid)
                {
                  List<AuctionDetail> myAuctionDetails = (item.Descendants("MemberItem").Select(dList => new AuctionDetail()
                    {
                      AuctionItemId = (dList.Attribute("AuctionId") != null ? dList.Attribute("AuctionId").Value : string.Empty),
                      MemberId = (dList.Attribute("MemberId") != null ? dList.Attribute("MemberId").Value : string.Empty),
                      ShopperId = (dList.Attribute("ShopperId") != null ? dList.Attribute("ShopperId").Value : string.Empty),
                      AuctionTypeId = (dList.Attribute("AuctionTypeId") != null ? dList.Attribute("AuctionTypeId").Value : string.Empty),
                      StatusCodeId = (dList.Attribute("StatusCodeId") != null ? dList.Attribute("StatusCodeId").Value : string.Empty),
                      DomainExtensionId = (dList.Attribute("DomainExtensionId") != null ? dList.Attribute("DomainExtensionId").Value : string.Empty),
                      DomainName = (dList.Attribute("DomainName") != null ? dList.Attribute("DomainName").Value : string.Empty),
                      FullDomainName = (dList.Attribute("FullDomainName") != null ? dList.Attribute("FullDomainName").Value : string.Empty),
                      ItemDescription = (dList.Attribute("ItemDescription") != null ? dList.Attribute("ItemDescription").Value : string.Empty),
                      CurrentPrice = (dList.Attribute("CurrentPrice") != null ? dList.Attribute("CurrentPrice").Value : string.Empty),
                      StartingBidAmount = (dList.Attribute("StartingBidAmount") != null ? dList.Attribute("StartingBidAmount").Value : string.Empty),
                      BidIncrementAmount = (dList.Attribute("BidIncrementAmount") != null ? dList.Attribute("BidIncrementAmount").Value : string.Empty),
                      BuyItNowAmount = (dList.Attribute("BuyItNowAmount") != null ? dList.Attribute("BuyItNowAmount").Value : string.Empty),
                      ReservedPriceAmount = (dList.Attribute("ReservedPriceAmount") != null ? dList.Attribute("ReservedPriceAmount").Value : string.Empty),
                      TimeZone = (dList.Attribute("TimeZone") != null ? dList.Attribute("TimeZone").Value : string.Empty),
                      AuctionListDate = (dList.Attribute("AuctionListDate") != null ? dList.Attribute("AuctionListDate").Value : string.Empty),
                      AuctionStartTime = (dList.Attribute("AuctionStartTime") != null ? dList.Attribute("AuctionStartTime").Value : string.Empty),
                      AuctionEndTime = (dList.Attribute("AuctionEndTime") != null ? dList.Attribute("AuctionEndTime").Value : string.Empty),
                      IsWebsiteIncluded = (dList.Attribute("WebsiteIncludedFlag") != null ? string.Compare(dList.Attribute("WebsiteIncludedFlag").Value, "true", true) == 0 : false),
                      EmailToFriend = (dList.Attribute("EmailToFriendFlag") != null ? string.Compare(dList.Attribute("EmailToFriendFlag").Value, "true", true) == 0 : false),
                      IsFeatureListing = (dList.Attribute("FeatureListingFlag") != null ? string.Compare(dList.Attribute("FeatureListingFlag").Value, "true", true) == 0 : false),
                      IsCategoryFeatureListing = (dList.Attribute("CategoryFeatureListingFlag") != null ? string.Compare(dList.Attribute("CategoryFeatureListingFlag").Value, "true", true) == 0 : false),
                      IsSubCategoryFeatureListing = (dList.Attribute("SubCategoryFeatureListingFlag") != null ? string.Compare(dList.Attribute("SubCategoryFeatureListingFlag").Value, "true", true) == 0 : false),
                      IsBuyItNow = (dList.Attribute("BuyItNowFlag") != null ? string.Compare(dList.Attribute("BuyItNowFlag").Value, "true", true) == 0 : false),
                      IsAddlCategoryListing = (dList.Attribute("AddlCategoryListingFlag") != null ? string.Compare(dList.Attribute("AddlCategoryListingFlag").Value, "true", true) == 0 : false),
                      HasReservedPrice = (dList.Attribute("ReservedPriceFlag") != null ? string.Compare(dList.Attribute("ReservedPriceFlag").Value, "true", true) == 0 : false),
                      IsAdultListing = (dList.Attribute("AdultListingFlag") != null ? string.Compare(dList.Attribute("AdultListingFlag").Value, "true", true) == 0 : false),
                      HasBidAccepted = (dList.Attribute("BidAcceptedFlag") != null ? string.Compare(dList.Attribute("BidAcceptedFlag").Value, "true", true) == 0 : false),
                      BidAcceptedDate = (dList.Attribute("BidAcceptedDate") != null ? dList.Attribute("BidAcceptedDate").Value : string.Empty),
                      BillingResourceId = (dList.Attribute("BillingResourceId") != null ? dList.Attribute("BillingResourceId").Value : string.Empty),
                      HighestBidderId = (dList.Attribute("HighestBidderId") != null ? dList.Attribute("HighestBidderId").Value : string.Empty),
                      HighestBidderShopperId = (dList.Attribute("HighestBidderShopperId") != null ? dList.Attribute("HighestBidderShopperId").Value : string.Empty),
                      FulfillmentDataId = (dList.Attribute("FulfillmentDataId") != null ? dList.Attribute("FulfillmentDataId").Value : string.Empty),
                      FulfillmentShopperId = (dList.Attribute("FulfillmentShopperId") != null ? dList.Attribute("FulfillmentShopperId").Value : string.Empty),
                      PayPalAddress = (dList.Attribute("PayPalAddress") != null ? dList.Attribute("PayPalAddress").Value : string.Empty),
                      Bids = (dList.Attribute("Bids") != null ? dList.Attribute("Bids").Value : string.Empty),
                      BackorderTypeId = (dList.Attribute("BackorderTypeId") != null ? dList.Attribute("BackorderTypeId").Value : string.Empty),
                      BackorderMemberId = (dList.Attribute("BackorderMemberId") != null ? dList.Attribute("BackorderMemberId").Value : string.Empty),
                      BackorderShopperId = (dList.Attribute("BackorderShopperId") != null ? dList.Attribute("BackorderShopperId").Value : string.Empty),
                      DomainId = (dList.Attribute("DomainId") != null ? dList.Attribute("DomainId").Value : string.Empty),
                      Rank = (dList.Attribute("Rank") != null ? dList.Attribute("Rank").Value : string.Empty),
                      AppraisalId = (dList.Attribute("AppraisalId") != null ? dList.Attribute("AppraisalId").Value : string.Empty),
                      HasTraffic = (dList.Attribute("HasTraffic") != null ? dList.Attribute("HasTraffic").Value : string.Empty),
                      Last14DaysTraffic = (dList.Attribute("Last14DaysTraffic") != null ? dList.Attribute("Last14DaysTraffic").Value : string.Empty),
                      OnSalePercent = (dList.Attribute("OnSalePercent") != null ? dList.Attribute("OnSalePercent").Value : string.Empty),
                      PrivateCode = (dList.Attribute("PrivateCode") != null ? dList.Attribute("PrivateCode").Value : string.Empty),
                      HasAuthenticationEmailSent = (dList.Attribute("AuthenticationEmailSent") != null ? string.Compare(dList.Attribute("AuthenticationEmailSent").Value, "true", true) == 0 : false),
                      EasyPushStatusCode = (dList.Attribute("EasyPushStatusCode") != null ? dList.Attribute("EasyPushStatusCode").Value : string.Empty),
                      LastBidOfferDtm = (dList.Attribute("LastBidOfferDtm") != null ? dList.Attribute("LastBidOfferDtm").Value : string.Empty),
                      BackorderId = (dList.Attribute("BackorderId") != null ? dList.Attribute("BackorderId").Value : string.Empty),
                      SourceId = (dList.Attribute("SourceId") != null ? dList.Attribute("SourceId").Value : string.Empty),
                      EscrowId = (dList.Attribute("EscrowId") != null ? dList.Attribute("EscrowId").Value : string.Empty),
                      VendorId = (dList.Attribute("VendorId") != null ? dList.Attribute("VendorId").Value : string.Empty),
                      ValuationPrice = (dList.Attribute("ValuationPrice") != null ? dList.Attribute("ValuationPrice").Value : string.Empty),
                      IsHide = (dList.Attribute("Hide") != null ? string.Compare(dList.Attribute("Hide").Value, "true", true) == 0 : false),
                      Tld = (dList.Attribute("TLD") != null ? dList.Attribute("TLD").Value : string.Empty),
                      AuctionModel = (dList.Attribute("AuctionModel") != null ? dList.Attribute("AuctionModel").Value : string.Empty),
                      SaleType = (dList.Attribute("SaleType") != null ? dList.Attribute("SaleType").Value : string.Empty),

                      Categories = (dList.Attribute("Categories") != null ? dList.Attribute("Categories").Value.Split(",".ToCharArray(), StringSplitOptions.None).ToList() : new List<string>()),

                      ReserveMet = (dList.Attribute("ReserveMet") != null ? dList.Attribute("ReserveMet").Value : string.Empty),
                      ViewCount = (dList.Attribute("ViewCount") != null ? dList.Attribute("ViewCount").Value : string.Empty),

                      MemberHasWatch = (dList.Attribute("WatchingItem") != null ? dList.Attribute("WatchingItem").Value : string.Empty )
                    }).ToList());

                  if (myAuctionDetails != null && myAuctionDetails.Count > 0)
                  {
                    aDetailResponse.AuctionDetail = myAuctionDetails[0];
                  }
                }
                auctionDetailList.Add(aDetailResponse);
                
                if (!aDetailResponse.IsDetailValid)
                {
                  DidDetailsHaveErrors = true;
                }
              }
            }
            success = true;
          }
        }
      }
      return success;
    }

    #endregion


    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionDetailBulkResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
