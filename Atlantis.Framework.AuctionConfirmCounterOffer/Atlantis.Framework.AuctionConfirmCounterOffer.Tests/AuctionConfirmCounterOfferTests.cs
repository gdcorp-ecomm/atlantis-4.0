using System;
using Atlantis.Framework.AuctionConfirmCounterOffer.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionConfirmCounterOffer.Tests
{
  [TestClass]
  public class AuctionConfirmCounterOfferTests
  {
    private string _sellerId = "840355";
    private string _buyerId = "859775";
    private string _auctionId = "4771714";


    
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PlaceCounterOfferByBuyer()
    {
      string bidId = "216367";
      string bidAmount = "550";
      string comments = "Set Via API Test";

      AuctionConfirmCounterOfferResponseData responseData = null;
      AuctionConfirmCounterOfferRequestData request = new AuctionConfirmCounterOfferRequestData(_buyerId, 
                                                                                                string.Empty,
                                                                                                string.Empty,
                                                                                                string.Empty,1,
                                                                                                _auctionId, 
                                                                                                bidId, 
                                                                                                bidAmount, 
                                                                                                comments);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionConfirmCounterOfferResponseData)Engine.Engine.ProcessRequest(request, 375);

      Assert.IsTrue(responseData.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PlaceCounterOfferOnInvalidBid()
    {
      string bidId = "111";
      string bidAmount = "550";
      string comments = "Set Via API Test";

      AuctionConfirmCounterOfferResponseData responseData = null;
      AuctionConfirmCounterOfferRequestData request = new AuctionConfirmCounterOfferRequestData(_buyerId,
                                                                                                string.Empty,
                                                                                                string.Empty,
                                                                                                string.Empty, 1,
                                                                                                _auctionId,
                                                                                                bidId,
                                                                                                bidAmount,
                                                                                                comments);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionConfirmCounterOfferResponseData)Engine.Engine.ProcessRequest(request, 375);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsFalse(responseData.IsValid);
    }
  }
}
