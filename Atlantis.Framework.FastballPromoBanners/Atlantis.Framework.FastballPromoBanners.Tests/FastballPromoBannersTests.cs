﻿using System;
using Atlantis.Framework.FastballPromoBanners.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballPromoBanners.Tests
{
  [TestClass]
  public class FastballPromoBannersTests
  {
    [TestInitialize]
    public void InitializeFakeContext()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersNoShopper()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileISCBanner", "gdbbcart15", "USD", string.Empty, 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.FastballPromoBanners.Count > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersValidShopper()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileISCBanner", "gdbbcart15", "USD", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      //FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);


      FastballPromoBannersResponseData responseData = SessionCache.SessionCache.GetProcessRequest<FastballPromoBannersResponseData>(requestData, 331);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.FastballPromoBanners.Count > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersInValidPromo()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileISCBanner", "sfsdfsafafafssfs", "USD", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersResponseData responseData = SessionCache.SessionCache.GetProcessRequest<FastballPromoBannersResponseData>(requestData, 331);

      //FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);

      Assert.IsFalse(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersExpiredPromo()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileISCBanner", "gdx1239n", "USD", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersResponseData responseData = SessionCache.SessionCache.GetProcessRequest<FastballPromoBannersResponseData>(requestData, 331);

      //FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);

      Assert.IsFalse(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CompareMultipuleRequestsMD5Hash()
    {

      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileSiteCrossSellBanner", "SAVE25NOW4", "USD", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersRequestData requestData2 = new FastballPromoBannersRequestData(43, "mobileSiteCrossSellBanner", "SAVE25NOW42", "USD", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);



      Assert.AreNotEqual(requestData2.GetCacheMD5(), requestData.GetCacheMD5());
    }

  }
}
