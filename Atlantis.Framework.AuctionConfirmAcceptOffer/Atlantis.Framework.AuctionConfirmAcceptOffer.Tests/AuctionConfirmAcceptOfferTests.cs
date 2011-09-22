using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.AuctionConfirmAcceptOffer.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionConfirmAcceptOffer.Tests
{
  [TestClass]
  public class AuctionConfirmAcceptOfferTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AcceptOfferOnClosedItem()
    {
      string auctionItemId = "4771713"; // closed item
      string bidId = "216363";
      string buyingShopperId = "857365";

      AuctionConfirmAcceptOfferResponseData responseData = null;
      AuctionConfirmAcceptOfferRequestData request = new AuctionConfirmAcceptOfferRequestData(buyingShopperId,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              1,
                                                                                              auctionItemId,
                                                                                              bidId);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionConfirmAcceptOfferResponseData) Engine.Engine.ProcessRequest(request, 377);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsFalse(responseData.IsValid);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AcceptOfferOnItem()
    {
      string auctionItemId = "4771719"; 
      string bidId = "216375";
      string buyingShopperId = "859775";

      AuctionConfirmAcceptOfferResponseData responseData = null;
      AuctionConfirmAcceptOfferRequestData request = new AuctionConfirmAcceptOfferRequestData(buyingShopperId,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              string.Empty,
                                                                                              1,
                                                                                              auctionItemId,
                                                                                              bidId);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionConfirmAcceptOfferResponseData)Engine.Engine.ProcessRequest(request, 377);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsFalse(responseData.IsValid);

    }
  }
}
