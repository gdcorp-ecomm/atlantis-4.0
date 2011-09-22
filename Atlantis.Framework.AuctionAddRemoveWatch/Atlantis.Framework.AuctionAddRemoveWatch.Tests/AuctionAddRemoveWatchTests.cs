using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.AuctionAddRemoveWatch.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionAddRemoveWatch.Tests
{
  [TestClass]
  public class AuctionAddRemoveWatchTests
  {
    private string _shopperId = "859775";
    private string _externalIpAddress = "172.16.38.32";
    private string _requestingServerIp = "172.16.38.32";
    private string _requestingServerName = "S1WSDV-TRIED";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AddAuctionToWatchList()
    {
      string auctionId = "4771632";

      AuctionAddRemoveWatchResponseData responseData = null;
      AuctionAddRemoveWatchRequestData requestData = new AuctionAddRemoveWatchRequestData(_shopperId, 
                                                                                          string.Empty, 
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1,
                                                                                          _externalIpAddress,
                                                                                          _requestingServerIp,
                                                                                          _requestingServerName,
                                                                                          auctionId,
                                                                                          true, string.Empty);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionAddRemoveWatchResponseData)Engine.Engine.ProcessRequest(requestData, 374);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.IsValidAddRemoveWatchRequest);
      Assert.IsTrue(responseData.ShopperId == _shopperId);
      Assert.IsTrue(responseData.AuctionId == auctionId);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void RemoveAuctionFromWatchList()
    {
      string auctionId = "4771632";

      AuctionAddRemoveWatchResponseData responseData = null;
      AuctionAddRemoveWatchRequestData requestData = new AuctionAddRemoveWatchRequestData(_shopperId,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1,
                                                                                          _externalIpAddress,
                                                                                          _requestingServerIp,
                                                                                          _requestingServerName,
                                                                                          auctionId,
                                                                                          false, string.Empty);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionAddRemoveWatchResponseData)Engine.Engine.ProcessRequest(requestData, 374);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.IsValidAddRemoveWatchRequest);
      Assert.IsTrue(responseData.ShopperId == _shopperId);
      Assert.IsTrue(responseData.AuctionId == auctionId);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AddInvalidAuctionToWatchList()
    {
      string auctionId = "zzz";

      AuctionAddRemoveWatchResponseData responseData = null;
      AuctionAddRemoveWatchRequestData requestData = new AuctionAddRemoveWatchRequestData(_shopperId,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1,
                                                                                          _externalIpAddress,
                                                                                          _requestingServerIp,
                                                                                          _requestingServerName,
                                                                                          auctionId,
                                                                                          false, string.Empty);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionAddRemoveWatchResponseData)Engine.Engine.ProcessRequest(requestData, 374);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsFalse(responseData.IsValidAddRemoveWatchRequest);
      Assert.IsTrue(responseData.ShopperId == _shopperId);
      Assert.IsTrue(responseData.AuctionId == "-1");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AddWatchUsingInvalidXml()
    {
      string auctionId = "'/>";

      AuctionAddRemoveWatchResponseData responseData = null;
      AuctionAddRemoveWatchRequestData requestData = new AuctionAddRemoveWatchRequestData(_shopperId,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1,
                                                                                          _externalIpAddress,
                                                                                          _requestingServerIp,
                                                                                          _requestingServerName,
                                                                                          auctionId,
                                                                                          false, string.Empty);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionAddRemoveWatchResponseData)Engine.Engine.ProcessRequest(requestData, 374);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsFalse(responseData.IsValidAddRemoveWatchRequest);
      Assert.IsTrue(responseData.ErrorNumber == "8001");
      Assert.IsTrue(responseData.AuctionId == "-1");
    }

  }
}
