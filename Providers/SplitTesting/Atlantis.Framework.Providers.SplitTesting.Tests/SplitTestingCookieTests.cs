using Atlantis.Framework.DataCache;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting.Tests.Mocks;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class SplitTestingCookieTests
  {
    [TestMethod]
    public void ReadsCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = string.Format(CalculateCookieName(privateLabelId));
      var cookieKey = "123-1";
      var cookieValue = "14";
      var cookies = new NameValueCollection();
      cookies.Add(cookieName, cookieKey + "=" + cookieValue);
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockCookies(cookies);

      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookie(container);

      var result = sut.CookieValues;
      Assert.IsTrue(result.ContainsKey(cookieKey));
      Assert.AreEqual(cookieValue, result[cookieKey]);
    }

    [TestMethod]
    public void ReadsOldCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = string.Format("SplitTesting{0}_{1}", privateLabelId, shopperId);
      var cookieKey = "123-1";
      var cookieValue = "14";
      var cookies = new NameValueCollection();
      cookies.Add(cookieName, cookieKey + "=" + cookieValue);
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockCookies(cookies);

      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookie(container);

      var result = sut.CookieValues;
      Assert.IsTrue(result.ContainsKey(cookieKey));
      Assert.AreEqual(cookieValue, result[cookieKey]);
    }

    [TestMethod]
    public void ReadsCookie_BothNewAndOld()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var cookies = new NameValueCollection();
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      // new cookie
      var cookieName = string.Format(CalculateCookieName(privateLabelId));
      var cookieKey = "123-1";
      var cookieValue = "14";
      cookies.Add(cookieName, cookieKey + "=" + cookieValue);
      // old cookie
      var cookieNameOld = string.Format("SplitTesting{0}_{1}", privateLabelId, shopperId);
      var cookieKeyOld = "124-6";
      var cookieValueOld = "3556";
      cookies.Add(cookieNameOld, cookieKeyOld + "=" + cookieValueOld);

      mockHttpRequest.MockCookies(cookies);

      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookie(container);

      var result = sut.CookieValues;
      Assert.IsTrue(result.ContainsKey(cookieKey));
      Assert.AreEqual(cookieValue, result[cookieKey]);
      Assert.IsTrue(result.ContainsKey(cookieKeyOld));
      Assert.AreEqual(cookieValueOld, result[cookieKeyOld]);
    }

    [TestMethod]
    public void ReadsCookie_NoException_ReturnsEmptyCollection_WhenCookieMissing()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;

      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookie(container);

      var result = sut.CookieValues;
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SetsCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = CalculateCookieName(privateLabelId);
      var cookieKey = "123-1";
      var cookieValue = "14";

      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);
      DataCacheEngineRequests.GetAppSetting = MockEngineRequests.AppSettingRequest_24hours;

      var sut = new SplitTestingCookie(container);
      sut.CookieValues = new Dictionary<string, string>() {{cookieKey, cookieValue}};

      var cookie = HttpContext.Current.Response.Cookies[cookieName];
      Assert.IsNotNull(cookie);
      Assert.AreEqual(cookieValue, cookie.Values[cookieKey]);

      Assert.IsNotNull(cookie.Expires);
      var expectedExpires = Convert.ToDouble(DateTime.Now.AddHours(24).Ticks);
      var actualExprires = Convert.ToDouble(cookie.Expires.Ticks);
      const long allowableDelta_1Minute = TimeSpan.TicksPerSecond*60;

      Assert.AreEqual(expectedExpires, actualExprires, allowableDelta_1Minute);
    }

    private static string CalculateCookieName(int privateLabelId)
    {
      var cookieName = string.Format("SplitTesting{0}", privateLabelId);
      return cookieName;
    }

    private static MockProviderContainer CreateProviderContainer(string shopperId, int privateLabelId)
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      return container;
    }
  }
}