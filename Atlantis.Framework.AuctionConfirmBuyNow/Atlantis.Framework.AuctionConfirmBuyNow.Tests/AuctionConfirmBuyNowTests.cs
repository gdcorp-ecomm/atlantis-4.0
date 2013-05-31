using System;
using System.Net;
using Atlantis.Framework.AuctionConfirmBuyNow.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Tests
{
  [TestClass]
  public class AuctionConfirmBuyNowTests
  {
    private const int _SOURCE_SYSTEM_ID = 30;
    private const string _BUY_NOW_ITC = "dpp_mgr_buynowbulk_auction";

    [TestMethod]
    public void ConfirmBuyNowWithBuyNowItem()
    {
      const string auctionItemId = "5364435";
      const string shopperId = "90343";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowRequestData request = new AuctionConfirmBuyNowRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, auctionItemId, comments, _SOURCE_SYSTEM_ID, string.Empty, _BUY_NOW_ITC);

      var response = (AuctionConfirmBuyNowResponseData)Engine.Engine.ProcessRequest(request, 364);

      Console.WriteLine("Valid Bid: " + response.IsConfirmBuyNowValid);

      if (!response.IsConfirmBuyNowValid)
      {
        Console.WriteLine("Error: " + response.ErrorMessage);
      }
      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void ConfirmBuyNowFailsBecauseItemIsNotBuyNow()
    {
      const string auctionItemId = "4771602";
      const string shopperId = "90343";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowRequestData request = new AuctionConfirmBuyNowRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, auctionItemId, comments, _SOURCE_SYSTEM_ID, string.Empty, _BUY_NOW_ITC);

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

    [TestMethod]
    public void ConfirmBuyNowBulkWithBuyNowItems()
    {
      const string shopperId = "90343";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowBulkRequestData request = new AuctionConfirmBuyNowBulkRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, _SOURCE_SYSTEM_ID, "172.23.44.77", "172.23.44.77", "D1WSDV-NTHOR2");
      request.AddBuyNowDomain(string.Empty, "drpepper.com", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "domaintest20.net", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "dolphintest1.org", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "dgwjuly23.co", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "denveraurora.org", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "dddd.com", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "dccunittest-register-test2-jul122010.com", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "catpurrs.com", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "catest10212010m.ca", shopperId, comments, string.Empty, _BUY_NOW_ITC);

      var response = (AuctionConfirmBuyNowBulkResponseData)Engine.Engine.ProcessRequest(request, 678);

      Console.WriteLine("Success: " + response.IsSuccess);

      if (!response.IsSuccess)
      {
        Console.WriteLine("Error: " + response.ErrorMessage);
      }
      else
      {
        if (response.ConfirmBuyNowValid != null)
        {
          foreach (var item in response.ConfirmBuyNowValid)
          {
            Console.WriteLine("Auction: " + item.Key);
            Console.WriteLine("Valid Bid: " + item.Value);
            if (!item.Value && response.ConfirmBuyNowError.ContainsKey(item.Key))
            {
              Console.WriteLine("Error: " + response.ConfirmBuyNowError[item.Key].ErrorNumber);
              Console.WriteLine("Message: " + response.ConfirmBuyNowError[item.Key].ErrorMessage);
            }
          }
        }
      }

      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void ConfirmBuyNowBulkFailsBecauseItemsAreNotBuyNow()
    {
      const string shopperId = "123717";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowBulkRequestData request = new AuctionConfirmBuyNowBulkRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, _SOURCE_SYSTEM_ID, "172.23.44.77", "172.23.44.77", "D1WSDV-NTHOR2");
      request.AddBuyNowDomain(string.Empty, "boarders.us", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "boatbuilding.us", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "brewerytours.us", shopperId, comments, string.Empty, _BUY_NOW_ITC);

      var response = (AuctionConfirmBuyNowBulkResponseData)Engine.Engine.ProcessRequest(request, 678);

      Console.WriteLine("Success: " + response.IsSuccess);

      if (!response.IsSuccess)
      {
        Console.WriteLine("Error: " + response.ErrorMessage);
      }
      else
      {
        if (response.ConfirmBuyNowValid != null)
        {
          foreach (var item in response.ConfirmBuyNowValid)
          {
            Console.WriteLine("Auction: " + item.Key);
            Console.WriteLine("Valid Bid: " + item.Value);
            if (!item.Value && response.ConfirmBuyNowError.ContainsKey(item.Key))
            {
              Console.WriteLine("Error: " + response.ConfirmBuyNowError[item.Key].ErrorNumber);
              Console.WriteLine("Message: " + response.ConfirmBuyNowError[item.Key].ErrorMessage);
            }
          }
        }
      }

      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void ConfirmBuyNowBulkSomeFailSomeSucceed()
    {
      const string shopperId = "90343";
      const string comments = "Set by API via triplet unit test.";

      AuctionConfirmBuyNowBulkRequestData request = new AuctionConfirmBuyNowBulkRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, _SOURCE_SYSTEM_ID, "172.23.44.77", "172.23.44.77", "D1WSDV-NTHOR2");
      request.AddBuyNowDomain(string.Empty, "newyear299.com", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain(string.Empty, "bullklist208.biz", shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain("102895", string.Empty, shopperId, comments, string.Empty, _BUY_NOW_ITC);
      request.AddBuyNowDomain("102897", "bullklist210.ws", shopperId, comments, string.Empty, _BUY_NOW_ITC);

      var response = (AuctionConfirmBuyNowBulkResponseData)Engine.Engine.ProcessRequest(request, 678);

      Console.WriteLine("Success: " + response.IsSuccess);

      if (!response.IsSuccess)
      {
        Console.WriteLine("Error: " + response.ErrorMessage);
      }
      else
      {
        if (response.ConfirmBuyNowValid != null)
        {
          foreach (var item in response.ConfirmBuyNowValid)
          {
            Console.WriteLine("Auction: " + item.Key);
            Console.WriteLine("Valid Bid: " + item.Value);
            if (!item.Value && response.ConfirmBuyNowError.ContainsKey(item.Key))
            {
              Console.WriteLine("Error: " + response.ConfirmBuyNowError[item.Key].ErrorNumber);
              Console.WriteLine("Message: " + response.ConfirmBuyNowError[item.Key].ErrorMessage);
            }
          }
        }
      }

      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess);
    }
  }
}