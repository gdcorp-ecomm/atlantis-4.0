using System;
using Atlantis.Framework.ShortUrl.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShortUrl.Tests
{
  [TestClass]
  public class ShortUrlTests
  {
    [TestMethod]
    public void ValidUrlToShortUrl()
    {
      ShortUrlRequestData requestData = new ShortUrlRequestData("http://www.godaddy.com/", string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ShortUrlResponseData responseData = (ShortUrlResponseData)Engine.Engine.ProcessRequest(requestData, 308);
      Assert.IsTrue(responseData.IsSuccess);
      Console.Write("Short Url: {0}, Original Url: {1}", responseData.ShortUrl, responseData.OriginalUrl);
    }

    [TestMethod]
    public void AndroidMarketUrlToShortUrl()
    {
      ShortUrlRequestData requestData = new ShortUrlRequestData("market://details?id=com.godaddy.mobile.android", string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ShortUrlResponseData responseData = (ShortUrlResponseData)Engine.Engine.ProcessRequest(requestData, 308);
      Assert.IsTrue(responseData.IsSuccess);
      Console.Write("Short Url: {0}, Original Url: {1}", responseData.ShortUrl, responseData.OriginalUrl);
    }
  }
}
