﻿using System;
using Atlantis.Framework.FastballGetOffersMsgData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballGetOffersMsgData.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.FastballGetOffersMsgData.Impl.dll")]
  public class FastballGetOffersMsgDataTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetiPhone4BannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        "860427", "http://yuck.com", string.Empty, "TestPathGuid", 7, 1, 40, "iPhone4CrossSellBanner", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetiPhone4NoShopperBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        string.Empty, "http://yuck.com", string.Empty, "TestPathGuid", 7, 1, 40, "iPhone4CrossSellBanner", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetiPhone3BannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        "860427", "http://yuck.com", string.Empty, "TestPathGuid", 7, 1, 41, "iPhone3CrossSellBanner", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetiPhone3NoShopperBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        string.Empty, "http://yuck.com", string.Empty, "TestPathGuid", 7, 1, 41, "iPhone3CrossSellBanner", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAndroidBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        "847235", "http://yuck.com", string.Empty, Guid.NewGuid().ToString(), 7, 1, 42, "androidCrossSellBannerLow", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAndroidNoShopperBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        string.Empty, "http://yuck.com", string.Empty, Guid.NewGuid().ToString(), 7, 1, 42, "androidCrossSellBannerLow", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMobileSalesBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        "847235", "http://yuck.com", string.Empty, Guid.NewGuid().ToString(), 7, 1, 43, "mobileSiteCrossSellBanner", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMobileSalesNoShopperBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        string.Empty, "http://yuck.com", string.Empty, Guid.NewGuid().ToString(), 7, 1, 43, "mobileSiteCrossSellBanner", null, null);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsFalse(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMobileSalesNoISCBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        string.Empty, "http://yuck.com", string.Empty, Guid.NewGuid().ToString(), 7, 1, 43, "mobileSiteCrossSellBanner", null, string.Empty);

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsTrue(response.FastBallAds.Count > 0);
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMobileSalesISCBannersTest()
    {
      FastballGetOffersMsgDataRequestData request = new FastballGetOffersMsgDataRequestData(
        string.Empty, "http://yuck.com", string.Empty, Guid.NewGuid().ToString(), 7, 1, 43, "mobileSiteCrossSellBanner", null, "save10now");

      FastballGetOffersMsgDataResponseData response = (FastballGetOffersMsgDataResponseData)Engine.Engine.ProcessRequest(request, 250);
      Assert.IsTrue(response.FastBallAds.Count == 0);
      Assert.IsTrue(response.IsSuccess);

    }
  }
}
