using System;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.AuctionGetBidHistory.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionGetBidHistory.Tests
{
  [TestClass]
  public class AuctionGetBidHistoryTests
  {
    private string _shopperId = "858421";
    private string _externalIpAddress = "172.16.38.32";
    private string _requestingServerIp = "172.16.38.32";
    private string _requestingServerName = "S1WSDV-TRIED";

    private string _auctionId = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetBidHistoryForValidAuctionWithMemberArea()
    {
      _auctionId = "4761763";
                                                  
      AuctionGetBidHistoryResponseData responseData = null;
      AuctionGetBidHistoryRequestData requestData = new AuctionGetBidHistoryRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 1, _externalIpAddress, _requestingServerIp, _requestingServerName, _auctionId, true);

      responseData = (AuctionGetBidHistoryResponseData) Engine.Engine.ProcessRequest(requestData, 366);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.IsValidBidHistoryRequest);
      Assert.IsTrue(responseData.HasBids);
      
      Console.WriteLine(String.Format("Details for Auction #{0}",requestData.AuctionItemId));
      Console.WriteLine("Bid Count: " + responseData.BidDetails.Count);
      Console.WriteLine();

      int bidCount = 1;

      foreach (AuctionBidDetail bid in responseData.BidDetails)
      {
        Console.WriteLine(string.Format("Bid #: {0}", bidCount++));
        Console.WriteLine(string.Format("Bid Id: {0}",bid.Id));
        Console.WriteLine(string.Format("Time Zone: {0}", bid.TimeZone));
        Console.WriteLine(string.Format("Bid Start: {0}", bid.StartDate));
        Console.WriteLine(string.Format("Bid End: {0}", bid.EndDate));
        Console.WriteLine(string.Format("Masked Bidder Id: {0}", bid.MaskedBidderId));
        Console.WriteLine(string.Format("Bid Amount: {0}", bid.BidAmount));

        Console.WriteLine(string.Format("Seller Id: {0}", bid.SellerMemberId));
        Console.WriteLine(string.Format("Bidder Id: {0}", bid.BidderMemberId));
        Console.WriteLine(string.Format("Counter Offer Id: {0}", bid.CounterOfferMemberId));
        Console.WriteLine(string.Format("Is Counter Offer: {0}", bid.IsCounterOffer));
        Console.WriteLine(string.Format("Display Accept: {0}", bid.DisplayAccept));
        Console.WriteLine(string.Format("Display Counter Offer: {0}", bid.DisplayCounter));
        Console.WriteLine();
      }     
      Console.WriteLine(responseData.ToXML());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetBidHistoryForValidAuctionWithoutMemberArea()
    {
      _auctionId = "4761763";

      AuctionGetBidHistoryResponseData responseData = null;
      AuctionGetBidHistoryRequestData requestData = new AuctionGetBidHistoryRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 1, _externalIpAddress, _requestingServerIp, _requestingServerName, _auctionId, false);

      responseData = (AuctionGetBidHistoryResponseData)Engine.Engine.ProcessRequest(requestData, 366);

      Assert.IsTrue(responseData.IsSuccess);
      //Assert.IsTrue(responseData.IsValidBidHistoryRequest);
      //Assert.IsTrue(responseData.HasBids);

      if (responseData.HasBids)
      {
        Console.WriteLine(String.Format("Details for Auction #{0}", requestData.AuctionItemId));
        Console.WriteLine("Bid Count: " + responseData.BidDetails.Count);
        Console.WriteLine();

        int bidCount = 1;

        foreach (AuctionBidDetail bid in responseData.BidDetails)
        {
          Console.WriteLine(string.Format("Bid #: {0}", bidCount++));
          Console.WriteLine(string.Format("Bid Id: {0}", bid.Id));
          Console.WriteLine(string.Format("Time Zone: {0}", bid.TimeZone));
          Console.WriteLine(string.Format("Bid Start: {0}", bid.StartDate));
          Console.WriteLine(string.Format("Bid End: {0}", bid.EndDate));
          Console.WriteLine(string.Format("Masked Bidder Id: {0}", bid.MaskedBidderId));
          Console.WriteLine(string.Format("Bid Amount: {0}", bid.BidAmount));

          Console.WriteLine(string.Format("Seller Id: {0}", bid.SellerMemberId));
          Console.WriteLine(string.Format("Bidder Id: {0}", bid.BidderMemberId));
          Console.WriteLine(string.Format("Counter Offer Id: {0}", bid.CounterOfferMemberId));
          Console.WriteLine(string.Format("Is Counter Offer: {0}", bid.IsCounterOffer));
          Console.WriteLine(string.Format("Display Accept: {0}", bid.DisplayAccept));
          Console.WriteLine(string.Format("Display Counter Offer: {0}", bid.DisplayCounter));
          Console.WriteLine();
        }
      }

      Console.WriteLine(responseData.ToXML());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetBidHistoryForInValidAuction()
    {
      _auctionId = "4771639999";

      AuctionGetBidHistoryResponseData responseData = null;
      AuctionGetBidHistoryRequestData requestData = new AuctionGetBidHistoryRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 1, _externalIpAddress, _requestingServerIp, _requestingServerName, _auctionId, true);

      responseData = (AuctionGetBidHistoryResponseData)Engine.Engine.ProcessRequest(requestData, 366);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsFalse(responseData.IsValidBidHistoryRequest);
      Assert.IsFalse(responseData.HasBids);

      Console.WriteLine(String.Format("Error Information Details for Auction #{0}", requestData.AuctionItemId));
      Console.WriteLine(string.Format("Error Code: {0}", responseData.ErrorCode));
      Console.WriteLine(string.Format("Raw Error Message: {0}", responseData.RawErrorMessage));
      Console.WriteLine(string.Format("Friendly Error Message: {0}", responseData.FriendlyErrorMessage));
      Console.WriteLine("");

      Console.WriteLine(responseData.ToXML());



    }
  }
}
