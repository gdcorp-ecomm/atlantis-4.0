using System;
using Atlantis.Framework.AuctionConfirmBuyNow.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Tests
{
  [TestClass]
  public class AuctionConfirmBuyNowTests
  {
    private const int _SOURCE_SYSTEM_ID = 26;
    private const string _MBLE_BUY_NOW_ITC = "mbl_buynow_auction";
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ConfirmBuyNowWithBuyNowItem()
    {
      const string auctionItemId = "4771476";
      const string shopperId = "859775";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowRequestData request = new AuctionConfirmBuyNowRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, auctionItemId, comments, _SOURCE_SYSTEM_ID, string.Empty, _MBLE_BUY_NOW_ITC);

      var response = (AuctionConfirmBuyNowResponseData) Engine.Engine.ProcessRequest(request, 364);

      Console.WriteLine("Valid Bid: " + response.IsConfirmBuyNowValid);

      if (!response.IsConfirmBuyNowValid)
      {
        Console.WriteLine("Error: " + response.ErrorMessage);
      }
      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ConfirmBuyNowFailsBecauseItemIsNotBuyNow()
    {
      const string auctionItemId = "4771647";
      const string shopperId = "859775";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowRequestData request = new AuctionConfirmBuyNowRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, auctionItemId, comments, _SOURCE_SYSTEM_ID, string.Empty, _MBLE_BUY_NOW_ITC);

      var response = (AuctionConfirmBuyNowResponseData)Engine.Engine.ProcessRequest(request, 364);

      Console.WriteLine("Valid Bid: " + response.IsConfirmBuyNowValid);

      if (!response.IsConfirmBuyNowValid)
      {
        Console.WriteLine("Error: " + response.ErrorMessage);
      }
      Console.WriteLine(response.ToXML());

      //Assert.IsTrue(response.ErrorMessage.ToLower() == "this is not a buynow item");
      Assert.IsFalse(response.IsConfirmBuyNowValid);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
