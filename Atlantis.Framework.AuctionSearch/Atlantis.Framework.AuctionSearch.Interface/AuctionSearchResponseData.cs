using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  public class AuctionSearchResponseData : IResponseData
  {
    #region Static Members 

    private static readonly IEnumerable<string> _auctionTypeList = new Collection<string> {  AuctionType.CloseOut, 
                                                                                            AuctionType.DomainConnect, 
                                                                                            AuctionType.ExpiringAuction, 
                                                                                            AuctionType.OfferCounterOffer, 
                                                                                            AuctionType.PublicAuction, 
                                                                                            AuctionType.PublicBuyNow, 
                                                                                            AuctionType.ValuePriced };

    private static readonly IEnumerable<string> _tldFilterList = new Collection<string> { TldFilterTypes.DotCom,
                                                                                          TldFilterTypes.DotNet,
                                                                                          TldFilterTypes.DotOrg,
                                                                                          TldFilterTypes.DotCo,
                                                                                          TldFilterTypes.DotCc,
                                                                                          TldFilterTypes.DotInfo,
                                                                                          TldFilterTypes.DotName,
                                                                                          TldFilterTypes.DotBiz,
                                                                                          TldFilterTypes.DotTv,
                                                                                          TldFilterTypes.DotUs,
                                                                                          TldFilterTypes.DotWs,
                                                                                          TldFilterTypes.DotMobi,
                                                                                          TldFilterTypes.DotDe,
                                                                                          TldFilterTypes.DotAm,
                                                                                          TldFilterTypes.DotFm,
                                                                                          TldFilterTypes.DotMe,
                                                                                          TldFilterTypes.DotCa,
                                                                                          TldFilterTypes.DotBz,
                                                                                          TldFilterTypes.DotComDotBz,
                                                                                          TldFilterTypes.DotNetDotBz,
                                                                                          TldFilterTypes.DotIt,
                                                                                          TldFilterTypes.DotMx,
                                                                                          TldFilterTypes.DotCoDotUk,
                                                                                          TldFilterTypes.DotEu,
                                                                                          TldFilterTypes.DotAsia,
                                                                                          TldFilterTypes.DotSe,
                                                                                          TldFilterTypes.DotIn };

    #endregion

    private AtlantisException _atlantisEx;
    private string _auctionReponseXml = string.Empty;

    private AuctionSearchRequestData RequestData { get; set; }

    private bool IsSavedSearchRequest
    {
      get { return RequestData.SaveSearchOptions != null && !string.IsNullOrEmpty(RequestData.SaveSearchOptions.SearchName); }
    }

    public bool IsSuccess { get; private set; }

    public List<Auction.Interface.Auction> Auctions { get; private set; }

    public SearchOptions SavedSearchOptions { get; private set; }

    public PagingOptions SavedPagingOptions { get; private set; }

    public int TotalRecords { get; private set; }

    public string FriendyErrorMessage { get; private set; }

    public string RawErrorMessage { get; private set; }

    public string ErrorNumber { get; private set; }

    public AuctionSearchResponseData(AuctionSearchRequestData requestData, string auctionResponseXml)
    {
      _auctionReponseXml = auctionResponseXml;
      RequestData = requestData;

      List<Auction.Interface.Auction> auctionData;
      if (!string.IsNullOrEmpty(auctionResponseXml) && GetAuctionData(auctionResponseXml, out auctionData))
      {
        Auctions = auctionData;
        IsSuccess = true;
      }
      else
      {
        Auctions = new List<Auction.Interface.Auction>(0);
        IsSuccess = false;
      }
    }

    public AuctionSearchResponseData(RequestData requestData, Exception ex)
    {
      _atlantisEx = new AtlantisException(requestData,
                                  "AuctionSearchResponseData",
                                  ex.Message,
                                  requestData.ToXML());
    }

    private bool GetAuctionData(string auctionResponseXml, out List<Auction.Interface.Auction> auctionData)
    {
      XDocument auctionsXml = XDocument.Parse(auctionResponseXml);

      bool success = false;
      auctionData = new List<Auction.Interface.Auction>();

      XElement auctionSearchResponseElement = auctionsXml.Root;

      if (auctionSearchResponseElement != null)
      {
        XAttribute validAttribute = auctionSearchResponseElement.Attribute("Valid");
        if (validAttribute != null && validAttribute.Value == "false")
        {
          XElement rawErrorStringElement;
          if (GetDecendentElement("RawErrorString", auctionSearchResponseElement, out rawErrorStringElement))
          {
            RawErrorMessage = rawErrorStringElement.Value;
          }

          XElement errorDataElement;
          if (GetDecendentElement("ErrorData", auctionSearchResponseElement, out errorDataElement))
          {
            XAttribute errorNumber = errorDataElement.Attribute("errornumber");
            ErrorNumber = GetAttributeValue(errorNumber);
          }

          XElement friendlyErrorStringElement;
          if (GetDecendentElement("FriendlyErrorString", auctionSearchResponseElement, out friendlyErrorStringElement))
          {
            FriendyErrorMessage = friendlyErrorStringElement.Value;
          }
        }
        else
        {
          int parsedTotalRecords;
          XAttribute totalRecordAttribute = auctionSearchResponseElement.Attribute("TotalRecords");
          string totalRecordsValue = GetAttributeValue(totalRecordAttribute);
          
          if(!string.IsNullOrEmpty(totalRecordsValue) && int.TryParse(totalRecordsValue, out parsedTotalRecords))
          {
            TotalRecords = parsedTotalRecords; 
          }

          IEnumerable<XElement> auctionElementList = auctionsXml.Descendants("Auction");

          foreach (XElement auctionElement in auctionElementList)
          {
            XAttribute idAttribute = auctionElement.Attribute("AuctionId");
            XAttribute timeZoneAttribute = auctionElement.Attribute("TimeZone");
            XAttribute endTimeAttribute = auctionElement.Attribute("AuctionEndTime");
            XAttribute typeIdAttribute = auctionElement.Attribute("AuctionTypeID");
            XAttribute domainNameAttribute = auctionElement.Attribute("DomainName");
            XAttribute startPriceAttribute = auctionElement.Attribute("StartPrice");
            XAttribute currentPriceAttribute = auctionElement.Attribute("CurrentPrice");
            XAttribute reservedPriceAmountAttribute = auctionElement.Attribute("ReservedPriceAmount");
            XAttribute isFeaturedAttribute = auctionElement.Attribute("IsFeatured");
            XAttribute isBuyNowAttribute = auctionElement.Attribute("IsBuyNow");
            XAttribute onSalePercentAttribute = auctionElement.Attribute("OnSalePercent");
            XAttribute privateCodeAttribute = auctionElement.Attribute("PrivateCode");
            XAttribute bidsAttribute = auctionElement.Attribute("Bids");
            XAttribute sellerMemberIdAttribute = auctionElement.Attribute("SellerMemberId");
            XAttribute sellerShopperIdAttribute = auctionElement.Attribute("SellerShopperId");
            XAttribute memberHasWatchAttribute = auctionElement.Attribute("MemberHasWatch");
            XAttribute appraisalIdAttribute = auctionElement.Attribute("AppraisalId");
            XAttribute trafficAttribute = auctionElement.Attribute("Traffic");
            XAttribute valuationPriceAttribute = auctionElement.Attribute("ValuationPrice");
            XAttribute buyNowPriceAttribute = auctionElement.Attribute("BuyNowPrice");
            XAttribute highestBidderIdAttribute = auctionElement.Attribute("HighestBidderId");
            XAttribute highestBidderShopperIdAttribute = auctionElement.Attribute("HighestBidderShopperId");
            XAttribute modelAttribute = auctionElement.Attribute("AuctionModel");
            XAttribute minimumBidAttribute = auctionElement.Attribute("MinimumBid");
            XAttribute saleTypeAttribute = auctionElement.Attribute("SaleType");

            var auction = new Auction.Interface.Auction
            {
              Id = GetAttributeValue(idAttribute),
              TimeZone = GetAttributeValue(timeZoneAttribute),
              EndTime = GetAttributeValue(endTimeAttribute),
              TypeId = GetAttributeValue(typeIdAttribute),
              DomainName = GetAttributeValue(domainNameAttribute),
              StartPrice = GetAttributeValue(startPriceAttribute),
              CurrentPrice = GetAttributeValue(currentPriceAttribute),
              ReservedPrice = GetAttributeValue(reservedPriceAmountAttribute),
              IsFeatured = GetAttributeValue(isFeaturedAttribute),
              IsBuyNow = GetAttributeValue(isBuyNowAttribute),
              OnSalePercent = GetAttributeValue(onSalePercentAttribute),
              PrivateCode = GetAttributeValue(privateCodeAttribute),
              Bids = GetAttributeValue(bidsAttribute),
              SellerMemberId = GetAttributeValue(sellerMemberIdAttribute),
              SellerShopperId = GetAttributeValue(sellerShopperIdAttribute),
              MemberHasWatch = GetAttributeValue(memberHasWatchAttribute),
              AppraisalId = GetAttributeValue(appraisalIdAttribute),
              Traffic = GetAttributeValue(trafficAttribute),
              ValuationPrice = GetAttributeValue(valuationPriceAttribute),
              BuyNowPrice = GetAttributeValue(buyNowPriceAttribute),
              HighestBidderId = GetAttributeValue(highestBidderIdAttribute),
              HighestBidderShopperId = GetAttributeValue(highestBidderShopperIdAttribute),
              AuctionModel = GetAttributeValue(modelAttribute),
              MinBid = GetAttributeValue(minimumBidAttribute),
              SaleType = GetAttributeValue(saleTypeAttribute),
            };

            auctionData.Add(auction);
          }

          if (IsSavedSearchRequest)
          {
            PopulateSavedSearchParamters(auctionSearchResponseElement);
          }

          success = true;
        }
      }
      return success;
    }

    private void PopulateSavedSearchParamters(XElement auctionSearchResponseElement)
    {
      XElement savedSearchParametersElement;
      if(GetDecendentElement("SavedSearchParameters", auctionSearchResponseElement, out savedSearchParametersElement))
      {
        SavedSearchOptions = new SearchOptions();

        ParseKeywordsFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseBidsFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseCharactersFilter(savedSearchParametersElement, SavedSearchOptions);
        ParsePriceFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseTrafficFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseDomainAgeFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseDaysEndingInFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseAuctionIdListFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseAuctionTypeListFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseTldListFilter(savedSearchParametersElement, SavedSearchOptions);
        ParsePatternFilter(savedSearchParametersElement, SavedSearchOptions);
        ParseAttributesFilter(savedSearchParametersElement, SavedSearchOptions);

        SavedPagingOptions = new PagingOptions();

        ParsePagingOptions(savedSearchParametersElement, SavedPagingOptions);
      }
    }

    private static void ParseKeywordsFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      XElement keywordsElement;
      if (GetDecendentElement("KEYWORDS", savedSearchParametersElement, out keywordsElement))
      {
        savedSearchOptions.Keywords = new KeywordFilter(keywordsElement);
      }
    }

    private static void ParseBidsFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      XElement bidsElement;
      if (GetDecendentElement("BIDS", savedSearchParametersElement, out bidsElement))
      {
        savedSearchOptions.Bid = new BidFilter(bidsElement);
      }
    }

    private static void ParseCharactersFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      XElement charactersElement;
      if (GetDecendentElement("CHARACTERS", savedSearchParametersElement, out charactersElement))
      {
        savedSearchOptions.Character = new CharacterFilter(charactersElement);
      }
    }

    private static void ParsePriceFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      savedSearchOptions.Price = new PriceFilter();

      int minPrice;
      XAttribute minPriceAttribute = savedSearchParametersElement.Attribute("minprice");
      string minPriceValue = GetAttributeValue(minPriceAttribute);
      if (!string.IsNullOrEmpty(minPriceValue) && int.TryParse(minPriceValue, out minPrice))
      {
        savedSearchOptions.Price.MinPrice = minPrice;
      }

      int maxPrice;
      XAttribute maxPriceAttribute = savedSearchParametersElement.Attribute("maxprice");
      string maxPriceValue = GetAttributeValue(maxPriceAttribute);
      if (!string.IsNullOrEmpty(maxPriceValue) && int.TryParse(maxPriceValue, out maxPrice))
      {
        savedSearchOptions.Price.MaxPrice = maxPrice;
      }
    }

    private static void ParseTrafficFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      savedSearchOptions.Traffic = new TrafficFilter();

      int minTraffic;
      XAttribute minTrafficAttribute = savedSearchParametersElement.Attribute("mintraffic");
      string minTrafficValue = GetAttributeValue(minTrafficAttribute);
      if (!string.IsNullOrEmpty(minTrafficValue) && int.TryParse(minTrafficValue, out minTraffic))
      {
        savedSearchOptions.Traffic.MinTraffic = minTraffic;
      }

      int maxTraffic;
      XAttribute maxTrafficAttribute = savedSearchParametersElement.Attribute("maxtraffic");
      string maxTrafficValue = GetAttributeValue(maxTrafficAttribute);
      if (!string.IsNullOrEmpty(maxTrafficValue) && int.TryParse(maxTrafficValue, out maxTraffic))
      {
        savedSearchOptions.Traffic.MaxTraffic = maxTraffic;
      }
    }

    private static void ParseDomainAgeFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      savedSearchOptions.DomainAge = new DomainAgeFilter();

      int minDomainAge;
      XAttribute minDomainAgeAttribute = savedSearchParametersElement.Attribute("mindomainage");
      string minDomainAgeValue = GetAttributeValue(minDomainAgeAttribute);
      if (!string.IsNullOrEmpty(minDomainAgeValue) && int.TryParse(minDomainAgeValue, out minDomainAge))
      {
        savedSearchOptions.DomainAge.MinDomainAgeYears = minDomainAge;
      }

      int maxDomainAge;
      XAttribute maxDomainAgeAttribute = savedSearchParametersElement.Attribute("maxdomainage");
      string maxDomainAgeValue = GetAttributeValue(maxDomainAgeAttribute);
      if (!string.IsNullOrEmpty(maxDomainAgeValue) && int.TryParse(maxDomainAgeValue, out maxDomainAge))
      {
        savedSearchOptions.DomainAge.MaxDomainAgeYears = maxDomainAge;
      }
    }

    private static void ParseDaysEndingInFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      int daysEndingIn;
      XAttribute daysEndingInAttribute = savedSearchParametersElement.Attribute("dateoffset");
      string daysEndingInValue = GetAttributeValue(daysEndingInAttribute);
      if (!string.IsNullOrEmpty(daysEndingInValue) && int.TryParse(daysEndingInValue, out daysEndingIn))
      {
        savedSearchOptions.DaysEndingIn = daysEndingIn;
      }
    }

    private static void ParseAuctionIdListFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      XAttribute auctionIdsAttribute = savedSearchParametersElement.Attribute("auctionid");
      string auctionIdsValue = GetAttributeValue(auctionIdsAttribute);
      if(!string.IsNullOrEmpty(auctionIdsValue))
      {
        string[] auctionIdsArray = auctionIdsValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
        if(auctionIdsArray.Length > 0)
        {
          savedSearchOptions.AuctionIdList = new List<string>(auctionIdsArray);
        }
      }
    }

    private static void ParseAuctionTypeListFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      savedSearchOptions.AuctionTypeList = new List<string>(_auctionTypeList.Count());

      foreach (string auctionType in _auctionTypeList)
      {
        XAttribute auctionTypeAttribute = savedSearchParametersElement.Attribute(auctionType);
        bool? auctionTypeValue = GetAttributeValueBool(auctionTypeAttribute);
        if (auctionTypeValue != null && auctionTypeValue.Value)
        {
          savedSearchOptions.AuctionTypeList.Add(auctionType);
        }
      }
    }

    private static void ParseTldListFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      savedSearchOptions.TldFilter = new List<string>(_tldFilterList.Count());

      foreach (string tldFilter in _tldFilterList)
      {
        XAttribute tldFilterAttribute = savedSearchParametersElement.Attribute(tldFilter);
        bool? tldFilterTypeValue = GetAttributeValueBool(tldFilterAttribute);
        if (tldFilterTypeValue != null && tldFilterTypeValue.Value)
        {
          savedSearchOptions.TldFilter.Add(tldFilter);
        }
      }
    }

    private static void ParsePatternFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      XAttribute patternAttribute = savedSearchParametersElement.Attribute("pattern");
      string patternValue = GetAttributeValue(patternAttribute);
      if(!string.IsNullOrEmpty(patternValue) && patternValue.Length == 4)
      {
        savedSearchOptions.PatternFilter = new PatternFilter();
        
        char[] patternArray = patternValue.ToCharArray();
        for(int i = 0; i < patternArray.Length; i++)
        {
          switch (i)
          {
            case 0:
              savedSearchOptions.PatternFilter.CharacterOne = GetPatternCharacterType(patternArray[i]);
              break;
            case 1:
              savedSearchOptions.PatternFilter.CharacterTwo = GetPatternCharacterType(patternArray[i]);
              break;
            case 2:
              savedSearchOptions.PatternFilter.CharacterThree = GetPatternCharacterType(patternArray[i]);
              break;
            case 3:
              savedSearchOptions.PatternFilter.CharacterFour = GetPatternCharacterType(patternArray[i]);
              break;
          }
        }
      }
    }

    private static void ParseAttributesFilter(XElement savedSearchParametersElement, SearchOptions savedSearchOptions)
    {
      XAttribute hasDigitsAttribute = savedSearchParametersElement.Attribute("hasdigits");
      bool? hasDigitsValue = GetAttributeValueBool(hasDigitsAttribute);
      if(hasDigitsValue != null)
      {
        savedSearchOptions.HasDigits = hasDigitsValue.Value;
      }

      XAttribute hasDashesAttribute = savedSearchParametersElement.Attribute("hasdashes");
      bool? hasDashesValue = GetAttributeValueBool(hasDashesAttribute);
      if (hasDashesValue != null)
      {
        savedSearchOptions.HasDashes = hasDashesValue.Value;
      }

      XAttribute buyNowAttribute = savedSearchParametersElement.Attribute("buynow");
      bool? buyNowValue = GetAttributeValueBool(buyNowAttribute);
      if (buyNowValue != null)
      {
        savedSearchOptions.BuyNow = buyNowValue.Value;
      }

      XAttribute websiteAttribute = savedSearchParametersElement.Attribute("website");
      bool? websiteValue = GetAttributeValueBool(websiteAttribute);
      if (websiteValue != null)
      {
        savedSearchOptions.WebSite = websiteValue.Value;
      }

      XAttribute featuredAttribute = savedSearchParametersElement.Attribute("featured");
      bool? featuredValue = GetAttributeValueBool(featuredAttribute);
      if (featuredValue != null)
      {
        savedSearchOptions.Featured = featuredValue.Value;
      }

      XAttribute appraisalAttribute = savedSearchParametersElement.Attribute("appraisal");
      bool? appraisalValue = GetAttributeValueBool(appraisalAttribute);
      if (appraisalValue != null)
      {
        savedSearchOptions.Appraisal = appraisalValue.Value;
      }

      XAttribute inviteOnlyAttribute = savedSearchParametersElement.Attribute("inviteonly");
      bool? inviteOnlyValue = GetAttributeValueBool(inviteOnlyAttribute);
      if (inviteOnlyValue != null)
      {
        savedSearchOptions.InviteOnly = inviteOnlyValue.Value;
      }

      XAttribute onSaleAttribute = savedSearchParametersElement.Attribute("onsale");
      bool? onSaleValue = GetAttributeValueBool(onSaleAttribute);
      if (onSaleValue != null)
      {
        savedSearchOptions.OnSale = onSaleValue.Value;
      }

      XAttribute categoryAttribute = savedSearchParametersElement.Attribute("category");
      string categoryValue = GetAttributeValue(categoryAttribute);
      if (!string.IsNullOrEmpty(categoryValue))
      {
        savedSearchOptions.Category = categoryValue;
      }
    }

    private static void ParsePagingOptions(XElement savedSearchParametersElement, PagingOptions savedPagingOptions)
    {
      XAttribute sortColumnAttribute = savedSearchParametersElement.Attribute("sortcol");
      string sortColumnValue = GetAttributeValue(sortColumnAttribute);
      if(!string.IsNullOrEmpty(sortColumnValue))
      {
        savedPagingOptions.SortColumn = sortColumnValue;
      }

      XAttribute sortDirectionAttribute = savedSearchParametersElement.Attribute("sortdirection");
      string sortDirectionValue = GetAttributeValue(sortDirectionAttribute);
      if (!string.IsNullOrEmpty(sortDirectionValue))
      {
        savedPagingOptions.SortOrder = sortDirectionValue;
      }

      XAttribute rowsPerPageAttribute = savedSearchParametersElement.Attribute("rowsperpage");
      string rowsPerPageValue = GetAttributeValue(rowsPerPageAttribute);
      int rowsPerPage;
      if (!string.IsNullOrEmpty(rowsPerPageValue) && int.TryParse(rowsPerPageValue, out rowsPerPage))
      {
        savedPagingOptions.RowsPerPage = rowsPerPage;
      }
    }

    private static PatternCharacterType GetPatternCharacterType(char patternCharacter)
    {
      PatternCharacterType patternCharacterType = PatternCharacterType.Consonant;
      switch (patternCharacter)
      {
        case 'c':
          patternCharacterType = PatternCharacterType.Consonant;
          break;
        case 'v':
          patternCharacterType = PatternCharacterType.Vowel;
          break;
        case 'n':
          patternCharacterType = PatternCharacterType.Number;
          break;
      }

      return patternCharacterType;
    }

    private static bool? GetAttributeValueBool(XAttribute attribute)
    {
      bool? boolValue = null;

      string attributeValue = GetAttributeValue(attribute);
      if (!string.IsNullOrEmpty(attributeValue))
      {
        if (string.Compare(attributeValue, "true", true) == 0)
        {
          boolValue = true;
        }
        else
        {
          boolValue = false;
        }
      }

      return boolValue;
    }

    private static bool GetDecendentElement(string elementName, XElement parentElement, out XElement decendantElement)
    {
      bool success = false;
      decendantElement = null;

      if(parentElement != null)
      {
        IEnumerable<XElement> decendantElementList = parentElement.Descendants(elementName);
        if(decendantElementList.Count() > 0)
        {
          decendantElement = decendantElementList.First();
          success = true;
        }
      }

      return success;
    }

    private static string GetAttributeValue(XAttribute attribute)
    {
      string value = string.Empty;
      if(attribute != null && !string.IsNullOrEmpty(attribute.Value))
      {
        value = attribute.Value;
      }
      return value;
    }

    public string ToXML()
    {
      return _auctionReponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlantisEx;
    }
  }
}
