using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.AuctionSearch.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionSearch.Tests
{
  public class AuctionSearchTests
  {
    [TestClass]
    public class AuctionRecommendationsTests
    {
      private static readonly RequestorInformation _requestorInformation = new RequestorInformation { ExternalIpAddress = "1.2.3.4", RequestingServerIp = "5.6.7.8", RequestingServerName = "testingserver", SourceSystemId = 26 /*Mobile*/, HasAuctionAccount = true };

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGet()
      {
        AuctionSearchResponseData response = null;

        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData request = new AuctionSearchRequestData(requestorInformation, "859775", string.Empty, string.Empty, string.Empty, 0);
        //request.SearchFilters = new SearchFilters { PreDefinedSearchType = PreDefinedSearch.BuyNow };
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);

        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetObsolete()
      {
        AuctionSearchResponseData response = null;
        AuctionSearchRequestData request = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver", PreDefinedSearchType.BuyNow, "850398", string.Empty, string.Empty, string.Empty, 0);
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);
        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetWithSearchPhrase()
      {
        AuctionSearchResponseData response = null;
        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData request = new AuctionSearchRequestData(requestorInformation, "858421", string.Empty, string.Empty, string.Empty, 0);
        request.SearchOptions = new SearchOptions { Keywords = new KeywordFilter(StringFilterType.Contains, new List<string> { "d" }) };
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);

        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetWithSearchPhraseObsolete()
      {
        AuctionSearchResponseData response = null;
        AuctionSearchRequestData request = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver", string.Empty, "d", "858421", string.Empty, string.Empty, string.Empty, 0);
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);
        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetWithSearchPhrasePagedDataObsolete()
      {
        string searchPhrase = "kinu";
        int pageNumber = 1;
        
        AuctionSearchResponseData response = null;
        AuctionSearchRequestData request = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver", string.Empty, searchPhrase, pageNumber, "858421", string.Empty, string.Empty, string.Empty, 0);
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);
        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchForceExceptionObsolete()
      {
        string searchPhrase = "d<}()</>";
        int pageNumber = 1;

        AuctionSearchRequestData requestData = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver",
                                                                        string.Empty, searchPhrase, pageNumber, "858421",
                                                                        string.Empty, string.Empty, string.Empty, 0);
        requestData.RequestTimeout = new TimeSpan(0, 0, 0, 30);

        AuctionSearchResponseData responseData = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(requestData, 358);

        Assert.IsFalse(responseData.IsSuccess);

        PrintOutResults(requestData, responseData);  
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetWithSearchPhrasePagedDataSortedObsolete()
      {
        string searchPhrase = "d";
        int pageNumber = 2;
        AuctionSearchResponseData response = null;

        AuctionSearchRequestData request = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver", string.Empty, searchPhrase, SortColumns.CurrentPrice, pageNumber, "858421", string.Empty, string.Empty, string.Empty, 0);
        request.RequestTimeout = new TimeSpan(0,0,0,30);
        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetWithSingleAuctionIdObsolete()
      {
        List<string> auctions = new List<string>();
        auctions.Add("4771639");
        
        AuctionSearchResponseData response = null;

        AuctionSearchRequestData request = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver", auctions, "858421", string.Empty, string.Empty, string.Empty, 0);
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);
        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchGetWithMultipleAuctionIdsObsolete()
      {
        List<string> auctions = new List<string>();
        auctions.Add("4771639");
        auctions.Add("4771647");
        auctions.Add("4771638");

        AuctionSearchResponseData response = null;

        AuctionSearchRequestData request = new AuctionSearchRequestData("1.2.3.4", "5.6.7.8", "testingserver", auctions, "858421", string.Empty, string.Empty, string.Empty, 0);
        request.RequestTimeout = new TimeSpan(0, 0, 0, 30);
        response = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(request, 358);

        PrintOutResults(request, response);

        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Auctions);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchPatternFilter()
      {
        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData requestData = new AuctionSearchRequestData(requestorInformation, "847235", string.Empty, string.Empty, string.Empty, 0);

        SearchOptions searchOptions = new SearchOptions();

        searchOptions.PatternFilter = new PatternFilter
                                        {
                                          CharacterOne = PatternCharacterType.Consonant,
                                          CharacterTwo = PatternCharacterType.Vowel,
                                          CharacterThree = PatternCharacterType.Consonant,
                                          CharacterFour = PatternCharacterType.Consonant,
                                        };


        requestData.SearchOptions = searchOptions;
        requestData.PagingOptions = new PagingOptions { PageNumber = 1, RowsPerPage = 25, SortColumn = SortColumns.Price, SortOrder = SortOrder.Asc.ToString().ToUpper() };
        requestData.RequestTimeout = TimeSpan.FromSeconds(30);

        AuctionSearchResponseData responseData = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(requestData, 358);

        PrintOutResults(requestData, responseData);

        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Auctions != null && responseData.Auctions.Count > 0);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchAdvanced()
      {
        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData requestData = new AuctionSearchRequestData(requestorInformation, "847235", string.Empty, string.Empty, string.Empty, 0);

        SearchOptions searchOptions = new SearchOptions();

        searchOptions.AuctionTypeList.Add(AuctionType.PublicAuction);

        searchOptions.Bid = new BidFilter(NumericFilterType.LessThan, 1000);

        searchOptions.Character = new CharacterFilter(NumericFilterType.LessThan, 50);

        searchOptions.Price = new PriceFilter { MinPrice = 1, MaxPrice = 10000 };

        searchOptions.TldFilter.Add(TldFilterTypes.DotCom);
        searchOptions.TldFilter.Add(TldFilterTypes.DotCo);
        searchOptions.TldFilter.Add(TldFilterTypes.DotNet);
        searchOptions.TldFilter.Add(TldFilterTypes.DotMe);
        searchOptions.TldFilter.Add(TldFilterTypes.DotBiz);
        searchOptions.TldFilter.Add(TldFilterTypes.DotOrg);
        searchOptions.TldFilter.Add(TldFilterTypes.DotMobi);

        searchOptions.Traffic = new TrafficFilter { MinTraffic = 0, MaxTraffic = 1000 };


        requestData.SearchOptions = searchOptions;
        requestData.PagingOptions = new PagingOptions { PageNumber = 1, RowsPerPage = 25, SortColumn = SortColumns.Price, SortOrder = SortOrder.Asc.ToString().ToUpper() };
        requestData.RequestTimeout = TimeSpan.FromSeconds(30);

        AuctionSearchResponseData responseData = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(requestData, 358);

        PrintOutResults(requestData, responseData);

        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Auctions != null && responseData.Auctions.Count > 0);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchSaveAndUpdateCustomSearch()
      {
        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData requestData = new AuctionSearchRequestData(requestorInformation, "847235", string.Empty, string.Empty, string.Empty, 0);

        SearchOptions searchOptions = new SearchOptions();

        searchOptions.AuctionTypeList.Add(AuctionType.PublicAuction);

        searchOptions.Bid = new BidFilter(NumericFilterType.LessThan, 1000);

        searchOptions.Character = new CharacterFilter(NumericFilterType.LessThan, 50);

        searchOptions.Keywords = new KeywordFilter(StringFilterType.Contains, new List<string> { "test" });

        searchOptions.Price = new PriceFilter { MinPrice = 1, MaxPrice = 10000 };

        searchOptions.TldFilter.Add(TldFilterTypes.DotCom);
        searchOptions.TldFilter.Add(TldFilterTypes.DotCo);
        searchOptions.TldFilter.Add(TldFilterTypes.DotNet);
        searchOptions.TldFilter.Add(TldFilterTypes.DotMe);
        searchOptions.TldFilter.Add(TldFilterTypes.DotBiz);
        searchOptions.TldFilter.Add(TldFilterTypes.DotOrg);
        searchOptions.TldFilter.Add(TldFilterTypes.DotMobi);

        searchOptions.Traffic = new TrafficFilter { MinTraffic = 0, MaxTraffic = 1000 };

        SaveSearchOptions saveSearchOptions = new SaveSearchOptions("Crazy Unit Test Search2", SaveSearchActionType.ExecuteAndInsertUpdate);

        requestData.SearchOptions = searchOptions;
        requestData.PagingOptions = new PagingOptions { PageNumber = 1, RowsPerPage = 25, SortColumn = SortColumns.Price, SortOrder = SortOrder.Asc.ToString().ToUpper() };
        requestData.SaveSearchOptions = saveSearchOptions;
        requestData.RequestTimeout = TimeSpan.FromSeconds(30);

        AuctionSearchResponseData responseData = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(requestData, 358);

        PrintOutResults(requestData, responseData);

        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Auctions != null && responseData.Auctions.Count >= 0);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchExecuteCustomSearch()
      {
        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData requestData = new AuctionSearchRequestData(requestorInformation, "847235", string.Empty, string.Empty, string.Empty, 0);

        SaveSearchOptions saveSearchOptions = new SaveSearchOptions("More Than 1 Bid", SaveSearchActionType.Execute);

        requestData.SaveSearchOptions = saveSearchOptions;

        AuctionSearchResponseData responseData = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(requestData, 358);

        PrintOutResults(requestData, responseData);

        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Auctions != null && responseData.Auctions.Count >= 0);
      }

      [TestMethod]
      [DeploymentItem("atlantis.config")]
      public void AuctionSearchPagingCheck()
      {
        RequestorInformation requestorInformation = _requestorInformation;

        AuctionSearchRequestData requestData = new AuctionSearchRequestData(requestorInformation, "847235", string.Empty, string.Empty, string.Empty, 0);

        SearchOptions searchOptions = new SearchOptions();
        searchOptions.PreDefinedSearchType = PreDefinedSearchType.Featured;

        requestData.SearchOptions = searchOptions;
        requestData.PagingOptions = new PagingOptions { PageNumber = 1, RowsPerPage = 1, SortColumn = SortColumns.Price, SortOrder = SortOrder.Asc.ToString().ToUpper() };
        requestData.RequestTimeout = TimeSpan.FromSeconds(30);

        AuctionSearchResponseData responseData = (AuctionSearchResponseData)Engine.Engine.ProcessRequest(requestData, 358);

        PrintOutResults(requestData, responseData);

        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Auctions != null && responseData.Auctions.Count > 0 && responseData.TotalRecords > responseData.Auctions.Count);
      }

      private static void PrintOutResults(AuctionSearchRequestData requestData, AuctionSearchResponseData responseData)
      {
        Console.WriteLine("Response Xml:");
        Console.WriteLine(responseData.ToXML());
        Console.WriteLine(string.Empty);
        Console.WriteLine("Request Xml:");
        Console.WriteLine(requestData.RequestXml);

        Debug.WriteLine("Response Xml:");
        Debug.WriteLine(responseData.ToXML());
        Debug.WriteLine(string.Empty);
        Debug.WriteLine("Request Xml:");
        Debug.WriteLine(requestData.RequestXml);
      }
    }
  }
}
