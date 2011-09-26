using System;
using System.Collections.Generic;
using Atlantis.Framework.AuctionGetDetailsBulk.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionGetDetailsBulk.Tests
{
  [TestClass]
  public class AuctionGetDetailBulkTests
  {
    private string _shopperId = "859775";
    private string _externalIpAddress = "172.16.38.32";
    private string _requestingServerIp = "172.16.38.32";
    private string _requestingServerName = "S1WSDV-TRIED";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetTwoValidAuctions()
    {
      List<string> auctionsToGet = new List<string>
                                  {
                                    "4771639",
                                    "4771647"
                                  };

      AuctionGetDetailsBulkResponseData responseData = null;
      AuctionGetDetailsBulkRequestData requestData = new AuctionGetDetailsBulkRequestData(_shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        _externalIpAddress,
        _requestingServerIp,
        _requestingServerName,
        auctionsToGet);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionGetDetailsBulkResponseData) Engine.Engine.ProcessRequest(requestData, 372);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.AuctionDetailList.Count == 2);
      Assert.IsTrue(!responseData.DidDetailsHaveErrors);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAuctionsUsingEmptyList()
    {
      List<string> auctionsToGet = new List<string>();

      AuctionGetDetailsBulkResponseData responseData = null;
      AuctionGetDetailsBulkRequestData requestData = new AuctionGetDetailsBulkRequestData(_shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        _externalIpAddress,
        _requestingServerIp,
        _requestingServerName,
        auctionsToGet);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      responseData = (AuctionGetDetailsBulkResponseData)Engine.Engine.ProcessRequest(requestData, 372);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.AuctionDetailList.Count == 0);
      Assert.IsTrue(!responseData.DidDetailsHaveErrors);
    }
  }
}