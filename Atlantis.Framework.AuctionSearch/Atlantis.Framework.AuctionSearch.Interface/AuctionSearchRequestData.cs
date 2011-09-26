using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  public class AuctionSearchRequestData : RequestData
  {
    private bool IsSavedSearchRequest
    {
      get { return SaveSearchOptions != null && !string.IsNullOrEmpty(SaveSearchOptions.SearchName); }
    }

    public string RequestXml
    {
      get { return BuildXmlRequest(); }
    }

    public RequestorInformation RequestorInformation { get; private set; }

    private PagingOptions _pagingOptions = new PagingOptions();
    public PagingOptions PagingOptions
    {
      get { return _pagingOptions; }
      set { _pagingOptions = value; }
    }

    private SearchOptions _searchOptions = new SearchOptions();
    public SearchOptions SearchOptions
    {
      get { return _searchOptions; }
      set { _searchOptions = value; }
    }

    public SaveSearchOptions SaveSearchOptions { get; set; }

    public bool HasAuctionAccount { get; private set; }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);
    

    #region Obsolete Constructors

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount )
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, string.Empty, string.Empty, -1, new List<string>(1),  shopperId, sourceUrl, orderId, pathway, pageCount) 
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string searchPhrase, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, searchPhrase, string.Empty, -1, new List<string>(1), shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, IList<string> auctionIdList, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, string.Empty, string.Empty, string.Empty, -1, auctionIdList, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string searchPhrase, string sortColumn, int pageNumber, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, searchPhrase, sortColumn, pageNumber, new List<string>(1), shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string searchPhrase, IList<string> auctionIdList, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, searchPhrase, string.Empty, -1, auctionIdList, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string searchPhrase, int pageNumber, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, searchPhrase, string.Empty, pageNumber, new List<string>(1), shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, IList<string> auctionId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, string.Empty, string.Empty, -1, auctionId, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation, PagingOptions, and SearchOptions")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string searchPhrase, string sortColumn, IList<string> auctionIdList, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(externalIpAddress, requestingServerIp, requestingServerName, searchType, searchPhrase, sortColumn, -1, auctionIdList, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    [Obsolete("Use new constructor with RequestorInformation and use PagingOptions, SearchOptions, and SavedSearchOptions properties")]
    public AuctionSearchRequestData(string externalIpAddress, string requestingServerIp, string requestingServerName, string searchType, string searchPhrase, string sortColumn, int pageNumber, IList<string> auctionIdList, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : this(new RequestorInformation{ ExternalIpAddress = externalIpAddress, RequestingServerIp = requestingServerIp, RequestingServerName = requestingServerName, HasAuctionAccount = true },
             shopperId,
             sourceUrl,
             orderId,
             pathway,
             pageCount)
    {
      PagingOptions = new PagingOptions { PageNumber = pageNumber, SortColumn = sortColumn };
      SearchOptions = new SearchOptions { PreDefinedSearchType = searchType, Keywords = new KeywordFilter(StringFilterType.Contains, new List<string> { searchPhrase }), AuctionIdList = auctionIdList };
    }

    #endregion

    public AuctionSearchRequestData(RequestorInformation requestorInformation, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestorInformation = requestorInformation;
      RequestTimeout = _requestTimeout;

      // We need to not pass a shopper id to search if they do not have an auctions account
      if(!requestorInformation.HasAuctionAccount)
      {
        ShopperID = string.Empty;
      }
    }

    private string BuildXmlRequest()
    {
      XElement auctionSearchElement = new XElement("AuctionSearch");

      AddRequestorInformation(auctionSearchElement);
      AddSearchFilters(auctionSearchElement);
      AddPagingOptions(auctionSearchElement);
      AddSaveSearchOptions(auctionSearchElement);  

      return auctionSearchElement.ToString(SaveOptions.DisableFormatting);
    }

    private void AddRequestorInformation(XElement rootElement)
    {
      if(RequestorInformation == null)
      {
        throw new ArgumentNullException("RequestorInformation", "Property \"RequestorInformation\" is required and cannot be null.");
      }

      rootElement.SetAttributeValue("ExternalIPAddress", RequestorInformation.ExternalIpAddress);
      rootElement.SetAttributeValue("RequestingServerIP", RequestorInformation.RequestingServerIp);
      rootElement.SetAttributeValue("RequestingServerName", RequestorInformation.RequestingServerName);
      rootElement.SetAttributeValue("SourceSystemId", RequestorInformation.SourceSystemId);
      
      if (!string.IsNullOrEmpty(ShopperID))
      {
        rootElement.SetAttributeValue("shopperId", ShopperID);
      }
    }

    private void AddPagingOptions(XElement rootElement)
    {
      if (PagingOptions.PageNumber > 0)
      {
        rootElement.SetAttributeValue("pagenumber", PagingOptions.PageNumber);
      }

      if (!string.IsNullOrEmpty(PagingOptions.SortColumn))
      {
        rootElement.SetAttributeValue("sortcol", PagingOptions.SortColumn);
      }

      if (!string.IsNullOrEmpty(PagingOptions.SortOrder))
      {
        rootElement.SetAttributeValue("sortdirection", PagingOptions.SortOrder);
      }

      if (PagingOptions.RowsPerPage > 0)
      {
        rootElement.SetAttributeValue("rowsperpage", PagingOptions.RowsPerPage);
      }
    }

    private void AddSearchFilters(XElement rootElement)
    {
      if (!IsSavedSearchRequest && !string.IsNullOrEmpty(SearchOptions.PreDefinedSearchType))
      {
        rootElement.SetAttributeValue("predefinedSearch", SearchOptions.PreDefinedSearchType);
      }

      if (SearchOptions.Keywords != null && SearchOptions.Keywords.KeywordList.Count > 0)
      {
        rootElement.Add(SearchOptions.Keywords.GetElement());
      }

      if (SearchOptions.Bid != null && SearchOptions.Bid.BidCount > -1)
      {
        rootElement.Add(SearchOptions.Bid.GetElement());
      }

      if (SearchOptions.Character != null && SearchOptions.Character.CharacterCount > -1)
      {
        rootElement.Add(SearchOptions.Character.GetElement());
      }

      if(SearchOptions.Price != null)
      {
        if(SearchOptions.Price.MinPrice > 0)
        {
          rootElement.SetAttributeValue("minprice", SearchOptions.Price.MinPrice);
        }

        if(SearchOptions.Price.MaxPrice > 0)
        {
          rootElement.SetAttributeValue("maxprice", SearchOptions.Price.MaxPrice);
        }
      }

      if(SearchOptions.Traffic != null)
      {
        if (SearchOptions.Traffic.MinTraffic > 0)
        {
          rootElement.SetAttributeValue("mintraffic", SearchOptions.Traffic.MinTraffic);
        }

        if (SearchOptions.Traffic.MaxTraffic > 0)
        {
          rootElement.SetAttributeValue("maxtraffic", SearchOptions.Traffic.MaxTraffic);
        }
      }

      if (SearchOptions.DomainAge != null)
      {
        if (SearchOptions.DomainAge.MinDomainAgeYears > -1)
        {
          rootElement.SetAttributeValue("mindomainage", SearchOptions.DomainAge.MinDomainAgeYears);
        }

        if (SearchOptions.DomainAge.MaxDomainAgeYears > -1)
        {
          rootElement.SetAttributeValue("maxdomainage", SearchOptions.DomainAge.MaxDomainAgeYears);
        }
      }

      if(SearchOptions.DaysEndingIn != null && SearchOptions.DaysEndingIn.Value > -1)
      {
        rootElement.SetAttributeValue("dateoffset", SearchOptions.DaysEndingIn);
      }

      if (SearchOptions.AuctionIdList.Count > 0)
      {
        rootElement.SetAttributeValue("auctionid", string.Join(",", SearchOptions.AuctionIdList.ToArray()));
      }

      if(SearchOptions.AuctionTypeList.Count > 0)
      {
        foreach (string auctionType in SearchOptions.AuctionTypeList)
        {
          rootElement.SetAttributeValue(auctionType, true); 
        }
      }

      if(SearchOptions.TldFilter.Count > 0)
      {
        foreach (string tldFilter in SearchOptions.TldFilter)
        {
          rootElement.SetAttributeValue(tldFilter, true); 
        } 
      }

      if(SearchOptions.PatternFilter != null)
      {
        rootElement.SetAttributeValue("pattern", SearchOptions.PatternFilter.ToString());
      }

      if(SearchOptions.HasDigits != null)
      {
        rootElement.SetAttributeValue("hasdigits", SearchOptions.HasDigits.Value);
      }

      if (SearchOptions.HasDashes != null)
      {
        rootElement.SetAttributeValue("hasdashes", SearchOptions.HasDashes.Value);
      }

      if (SearchOptions.BuyNow != null)
      {
        rootElement.SetAttributeValue("buynow", SearchOptions.BuyNow.Value);
      }

      if (SearchOptions.WebSite != null)
      {
        rootElement.SetAttributeValue("website", SearchOptions.WebSite.Value);
      }

      if (SearchOptions.Featured != null)
      {
        rootElement.SetAttributeValue("featured", SearchOptions.Featured.Value);
      }

      if (SearchOptions.Appraisal != null)
      {
        rootElement.SetAttributeValue("appraisal", SearchOptions.Appraisal.Value);
      }

      if (SearchOptions.InviteOnly != null)
      {
        rootElement.SetAttributeValue("inviteonly", SearchOptions.InviteOnly);
      }

      if (SearchOptions.OnSale != null)
      {
        rootElement.SetAttributeValue("onsale", SearchOptions.OnSale);
      }

      if(!string.IsNullOrEmpty(SearchOptions.Category))
      {
        rootElement.SetAttributeValue("category", SearchOptions.Category);
      }
    }

    private void AddSaveSearchOptions(XElement rootElement)
    {
      if(IsSavedSearchRequest)
      {
        XElement saveSearchElement = SaveSearchOptions.GetElement();
        rootElement.Add(saveSearchElement);
      }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionSearch is not a cacheable request.");
    }
  }
}
