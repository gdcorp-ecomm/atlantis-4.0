using System;
using Atlantis.Framework.FastballPromoBanners.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballPromoBanners.Tests
{
  [TestClass]
  public class FastballPromoBannersTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersNoShopper()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileSiteCrossSellBanner", "SAVE25NOW4", string.Empty, 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.FastballPromoBanners.Count > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersValidShopper()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileSiteCrossSellBanner", "SAVE25NOW4", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);

      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsTrue(responseData.FastballPromoBanners.Count > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPromoBannersInValidPromo()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileISCBanner", "sfsdfsafafafssfs", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersResponseData responseData = (FastballPromoBannersResponseData)Engine.Engine.ProcessRequest(requestData, 331);

      Assert.IsFalse(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CompareMultipuleRequestsMD5Hash()
    {
      FastballPromoBannersRequestData requestData = new FastballPromoBannersRequestData(43, "mobileSiteCrossSellBanner", "SAVE25NOW4", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      FastballPromoBannersRequestData requestData2 = new FastballPromoBannersRequestData(43, "mobileSiteCrossSellBanner", "SAVE25NOW42", "847235", 1, string.Empty, string.Empty, Guid.NewGuid().ToString(), 1);
      requestData.RequestTimeout = TimeSpan.FromSeconds(30);


      Assert.AreNotEqual(requestData2.GetCacheMD5(), requestData.GetCacheMD5());
    }

  }
}
