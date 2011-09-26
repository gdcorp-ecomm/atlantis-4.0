using System;
using Atlantis.Framework.AuctionPlaceBid.Interface;
using Atlantis.Framework.AuctionSearch.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionPlaceBid.Tests
{
  [TestClass]
  public class AuctionPlaceBidTests
  {
    private int _sourceSystem = 26;
    private string _externalIpAddress = "172.16.38.32";
    private string _requestingServerIp = "1.1.0.1";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PlaceBidOnValidAuction()
    {
      string auctionItemId = "4771647";
      string shopperId = "859775";
      string bidAmount = "1000";
      string comments = "Set by API via triplet unit test.";

      RequestorInformation requestorInformation = new RequestorInformation { ExternalIpAddress = _externalIpAddress, RequestingServerIp = _requestingServerIp, RequestingServerName = "AuctionPlaceBidBulkTests", SourceSystemId = _sourceSystem };
      AuctionPlaceBidRequestData request = new AuctionPlaceBidRequestData(requestorInformation, auctionItemId, bidAmount, comments, shopperId, string.Empty, string.Empty, string.Empty, 0);

      var response = (AuctionPlaceBidResponseData) Engine.Engine.ProcessRequest(request, 360);

      Console.WriteLine("Valid Bid: " + response.AuctionBidAttributes.IsBidValid);
      if (!response.AuctionBidAttributes.IsBidValid)
      {
        Console.WriteLine("Error: " + response.AuctionBidAttributes.Error);
      }
      Console.WriteLine("Auction Item: " + response.AuctionBidAttributes.AuctionItemId);


      Assert.IsTrue(response.IsSuccess);
    }
  }
}
