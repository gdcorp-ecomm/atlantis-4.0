using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.AuctionGetAreaBySectionXml.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionGetAreaBySectionXml.Tests
{
  [TestClass]
  public class AuctionGetMemberAreaBySectionXmlTests {
    private string _shopperId = "858421";
    private int engineRequestId = 376;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAllSections() {
      
      AuctionGetAreaBySectionXmlResponseData responseData = null;
      var areasToGet = new List<string>() {MemberArea.Sold, MemberArea.ApprovalPending, MemberArea.Bidding, MemberArea.Invited, MemberArea.Lost, MemberArea.NotSold, MemberArea.Selling, MemberArea.Watching, MemberArea.Won};

      var requestData = new AuctionGetAreaBySectionXmlRequestData(
                                                                  false,
                                                                  areasToGet, 
                                                                  null, 
                                                                  null,
                                                                  1,
                                                                  15,
                                                                  SortColumns.AuctionEndTime,
                                                                  SortOrder.Asc, 
                                                                  _shopperId,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  1
                                                                  ) 
                                                                  {RequestTimeout = TimeSpan.FromSeconds(30)};

      responseData = (AuctionGetAreaBySectionXmlResponseData)Engine.Engine.ProcessRequest(requestData, engineRequestId);

      Assert.IsTrue(responseData.Results.Count == 9); //represents the 9 requested sections

      Assert.IsTrue(responseData.IsSuccess);
     
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetSingleSection() {

      AuctionGetAreaBySectionXmlResponseData responseData = null;
      var areasToGet = new List<string>() { MemberArea.Watching };

      var requestData = new AuctionGetAreaBySectionXmlRequestData(
                                                                  false,
                                                                  areasToGet,
                                                                  null,
                                                                  null,
                                                                  1,
                                                                  15,
                                                                  SortColumns.AuctionEndTime,
                                                                  SortOrder.Asc,
                                                                  _shopperId,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  1
                                                                  ) { RequestTimeout = TimeSpan.FromSeconds(30) };

      responseData = (AuctionGetAreaBySectionXmlResponseData)Engine.Engine.ProcessRequest(requestData, engineRequestId);

      Assert.IsTrue(responseData.Results.Count == 1); //represents the 1 requested sections

      foreach (AuctionMemberSection section in responseData.Results)
      {
        foreach (Auction.Interface.Auction auction in section.Auctions)
        {
          Debug.WriteLine("{0} - {1} - {2}", auction.DomainName, auction.IsBuyNow, auction.TypeId);
        }
      }

      Assert.IsTrue(responseData.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAllSectionsWithBidHistory() {

      AuctionGetAreaBySectionXmlResponseData responseData = null;
      var areasToGet = new List<string>() { MemberArea.Sold, MemberArea.ApprovalPending, MemberArea.Bidding, MemberArea.Invited, MemberArea.Lost, MemberArea.NotSold, MemberArea.Selling, MemberArea.Watching, MemberArea.Won };

      var requestData = new AuctionGetAreaBySectionXmlRequestData(
                                                                  true,
                                                                  areasToGet,
                                                                  null,
                                                                  null,
                                                                  1,
                                                                  15,
                                                                  SortColumns.AuctionEndTime,
                                                                  SortOrder.Asc,
                                                                  _shopperId,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  1
                                                                  ) { RequestTimeout = TimeSpan.FromSeconds(30) };

      responseData = (AuctionGetAreaBySectionXmlResponseData)Engine.Engine.ProcessRequest(requestData, engineRequestId);

      Assert.IsTrue(responseData.Results.Count == 9); //represents the 9 requested sections

      Assert.IsTrue(responseData.IsSuccess);

    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetSectionsForceError() {

      AuctionGetAreaBySectionXmlResponseData responseData = null;
      List<string> areasToGet = null;

      var requestData = new AuctionGetAreaBySectionXmlRequestData(
                                                                  false,
                                                                  areasToGet,
                                                                  null,
                                                                  null,
                                                                  1,
                                                                  15,
                                                                  SortColumns.AuctionEndTime,
                                                                  SortOrder.Asc,
                                                                  _shopperId,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  1
                                                                  ) { RequestTimeout = TimeSpan.FromSeconds(30) };

      responseData = (AuctionGetAreaBySectionXmlResponseData)Engine.Engine.ProcessRequest(requestData, engineRequestId);


      Assert.IsTrue(responseData.Results.Count == 0); //represents the 9 requested sections
      Assert.IsFalse(responseData.IsSuccess);

    }
  }
}
