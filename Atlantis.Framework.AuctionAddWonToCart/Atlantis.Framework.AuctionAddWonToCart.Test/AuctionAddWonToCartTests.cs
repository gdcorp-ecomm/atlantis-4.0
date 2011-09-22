using System;
using System.Collections.Generic;
using Atlantis.Framework.AuctionAddWonToCart.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionAddWonToCart.Test {
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class AuctionAddWonToCartTests
  {
    private int engineRequestId = 391;

    private string _shopperId = "858421";
    private const string _ITC = "Mobile_Auction_ITC";
    private const string _ISC = "Mobile_Auction_ISC";

     /* The following test method is commented out because the auction in question is no longer valid. 
      * To verify, update the auction id to use a valid auction that the specified shopper
      * has won and then un-comment and run. */

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //public void AuctionAddWonToCart()
    //{

    //  AuctionAddWonToCartResponseData responseData = null;
      
    //  // Update the shopperId for the account you want to test:
    //  _shopperId = "858421";

    //  // Make sure to update the shopperId and auctionId to valid values.  The shopperId in this case is the AuctionMemberId NOT the GD ShopperId.  Also, make
    //  // sure that the auctionId is valid and is a WON auction in the shopper's account
    //  var auctions = new List<string>() { "4771730", "123123443" };
      
    //  var requestData = new AuctionAddWonToCartRequestData(
    //                        auctions,
    //                        _ISC,
    //                        _ITC,
    //                        _shopperId,
    //                        string.Empty,
    //                        string.Empty,
    //                        string.Empty,
    //                        1
    //                        ) { RequestTimeout = TimeSpan.FromSeconds(30) };

    //  responseData = (AuctionAddWonToCartResponseData)Engine.Engine.ProcessRequest(requestData, engineRequestId);

    //  Assert.IsTrue(responseData.IsSuccess);

    //}

    // This test method will always pass and will at least verify that the API is responding.
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuctionAddWonToCartFails()
    {

      AuctionAddWonToCartResponseData responseData = null;
      var auctions = new List<string>() { "4771730", "123123443" };

      var requestData = new AuctionAddWonToCartRequestData(
                            auctions,
                            _ISC,
                            _ITC,
                            _shopperId,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            1
                            ) { RequestTimeout = TimeSpan.FromSeconds(30) };

      responseData = (AuctionAddWonToCartResponseData)Engine.Engine.ProcessRequest(requestData, engineRequestId);

      Assert.IsFalse(responseData.IsSuccess);

    }
  }
}
