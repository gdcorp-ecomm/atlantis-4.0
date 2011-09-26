using System;
using System.Collections.Generic;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.AuctionPlaceBidBulk.Interface;
using Atlantis.Framework.AuctionSearch.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionPlaceBidBulk.Tests
{
  [TestClass]
  public class AuctionPlaceBidBulkTests
  {
    private int _sourceSystem = 26;
    private string _shopperId = "859775";
    private string _externalIpAddress = "172.16.38.32";
    private string _requestingServerIp = "1.1.0.1";
    
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SubmitThreeValidBids()
    {
      List<AuctionPlaceBidItem> myBids = new List<AuctionPlaceBidItem>
                                           {
                                             new AuctionPlaceBidItem("4771639", _shopperId, "1000", string.Empty),
                                             new AuctionPlaceBidItem("4771647", _shopperId, "1000", string.Empty),
                                             new AuctionPlaceBidItem("4771638", _shopperId, "1000", string.Empty),
                                           };
      
      AuctionPlaceBidBulkResponseData responseData = null;

      RequestorInformation requestorInformation = new RequestorInformation { ExternalIpAddress = _externalIpAddress, RequestingServerIp = _requestingServerIp, RequestingServerName = "AuctionPlaceBidBulkTests", SourceSystemId = _sourceSystem };
      AuctionPlaceBidBulkRequestData requestData = new AuctionPlaceBidBulkRequestData(requestorInformation,
                                                                                      myBids,
                                                                                      _shopperId, 
                                                                                      string.Empty,
                                                                                      String.Empty,
                                                                                      string.Empty,
                                                                                      1);

      responseData = (AuctionPlaceBidBulkResponseData) Engine.Engine.ProcessRequest(requestData, 369);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.IsValidPlaceBidBulkRequest);
      Assert.IsTrue(responseData.DidBidsHaveErrors);
    }
  }
}
