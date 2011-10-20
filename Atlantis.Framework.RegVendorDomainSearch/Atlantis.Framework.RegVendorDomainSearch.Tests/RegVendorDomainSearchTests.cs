using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.RegVendorDomainSearch.Interface;
using System;
using System.Threading;

namespace Atlantis.Framework.RegVendorDomainSearch.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class RegVendorDomainSearchTests
  {
    public RegVendorDomainSearchTests()
    {
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DPPDomainSearchValidDomain()
    {
      RegVendorDomainSearchRequestData request = new RegVendorDomainSearchRequestData("839627", string.Empty, string.Empty,
        string.Empty, 0, "premium", "1godaddy31.com", "c1wsdv-rphil", "172.18.172.26", 1, "DPP Avail Check", 5, "");
      RegVendorDomainSearchResponseData response = (RegVendorDomainSearchResponseData)Engine.Engine.ProcessRequest(request, 
        323);
      Assert.IsTrue(response._isSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DPPDomainSearchDomainNotOnAuction()
    {
      RegVendorDomainSearchRequestData request = new RegVendorDomainSearchRequestData("839627", string.Empty,
        string.Empty, string.Empty, 0, "premium, auctions", "lunchbucket.com", "c1wsdv-rphil", "172.18.172.26", 
        1, "DPP Avail Check", 5, "");
      RegVendorDomainSearchResponseData response = (RegVendorDomainSearchResponseData)Engine.Engine.ProcessRequest(request, 
        323);
      Assert.IsTrue(response._isSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PremiumsSearchTest()
    {
      RegVendorDomainSearchRequestData request = new RegVendorDomainSearchRequestData("839627", string.Empty, string.Empty,
        string.Empty, 0, "premium, auctions", "mudmud.com", "c1wsdv-rphil", "172.18.172.26", 1, 
        "DoDomainSearch.BeginRegVendorDomainSearch", 50, "COM,NET,ORG,CO");
      request.RequestTimeout = new TimeSpan(0, 0, 10);
      RegVendorDomainSearchResponseData response 
        = (RegVendorDomainSearchResponseData)Engine.Engine.ProcessRequest(request, 323);
      Assert.IsTrue(response.IsSuccess);
    }


    private volatile bool _asyncSearchComplete = false;
    private RegVendorDomainSearchResponseData _asyncResponse = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DPPDomainSearchAsyncTest()
    {
      _asyncResponse = null;
      RegVendorDomainSearchRequestData request = new RegVendorDomainSearchRequestData("839627", string.Empty, string.Empty,
        string.Empty, 0, "premium, auctions", "lunchbox.com", "c1wsdv-rphil", "172.18.172.26", 1, "DPP Avail Check", 5, "");
      request._requestTimeout = TimeSpan.FromMilliseconds(1);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(request, 330, EndDPPDomainSearchAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse._isSuccess);
    }

    private void EndDPPDomainSearchAsyncTest(IAsyncResult result)
    {
      _asyncResponse = Engine.Engine.EndProcessRequest(result) as RegVendorDomainSearchResponseData;
      _asyncSearchComplete = true;
    }
  }
}
