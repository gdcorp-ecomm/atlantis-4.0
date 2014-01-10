using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using Atlantis.Framework.Providers.SsoAuth.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class ShopperContextProviderTests
  {
    private void SetWebEmptyContext(NameValueCollection mockCookies = null)
    {
      var mockRequest = new MockHttpRequest("http://www.godaddy.com/");

      if (mockCookies != null)
      {
        mockRequest.MockCookies(mockCookies);
      }

      MockHttpContext.SetFromWorkerRequest(mockRequest);
    }

    private IProviderContainer SetProviders(int privateLabelId)
    {
      MockProviderContainer container = new MockProviderContainer();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, ShopperContextProvider>();

      return container;
    }

    [TestMethod]
    public void EmptyShopperBasic()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual(string.Empty, shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
      Assert.AreEqual(0, shopperContext.ShopperPriceType);
      Assert.AreEqual(string.Empty, AtlantisExceptionWebState.ShopperId);
    }

    [TestMethod]
    public void SetNewShopperBasic()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);
      container.RegisterProvider<IDebugContext, MockDebugContext>();

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
      Assert.AreNotEqual(0, HttpContext.Current.Response.Cookies.Count);
      Assert.AreEqual("832652", AtlantisExceptionWebState.ShopperId);

      var debug = container.Resolve<IDebugContext>();
      var foundShopperId = false;

      foreach (var keyValuePair in debug.GetDebugTrackingData())
      {
        if ("832652".Equals(keyValuePair.Value))
        {
          foundShopperId = true;
          break;
        }
      }

      Assert.IsTrue(foundShopperId);
    }

    [TestMethod]
    public void SetNewShopperWithAuthToken()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);
      container.RegisterProvider<IAuthenticationProvider, MockAuthenticationProvider>();
      MockAuthenticationProvider.ClearHistory();
      Assert.IsFalse(MockAuthenticationProvider.DeauthenticateWasCalled);

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");

      Assert.IsTrue(MockAuthenticationProvider.DeauthenticateWasCalled);
    }

    [TestMethod]
    public void ClearShopperBasic()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
      Assert.AreEqual("832652", shopperContext.ShopperId);

      shopperContext.ClearShopper();
      Assert.AreEqual(string.Empty, shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
      Assert.AreNotEqual(0, HttpContext.Current.Response.Cookies.Count);
    }

    [TestMethod]
    public void ClearShopperWithAuthToken()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);
      container.RegisterProvider<IAuthenticationProvider, MockAuthenticationProvider>();

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
      Assert.AreEqual("832652", shopperContext.ShopperId);

      MockAuthenticationProvider.ClearHistory();
      Assert.IsFalse(MockAuthenticationProvider.DeauthenticateWasCalled);

      shopperContext.ClearShopper();
      Assert.IsTrue(MockAuthenticationProvider.DeauthenticateWasCalled);
    }

    [TestMethod]
    public void PublicShopperBasic()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void AuthTokenShopperBasic()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);
      container.RegisterProvider<IAuthTokenProvider, MockAuthTokenProvider>();
      container.RegisterProvider<IAuthenticationProvider, MockAuthenticationProvider>();
      MockAuthTokenProvider.IsMockTokenvalid = true;

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("bogusshopperid", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Authenticated, shopperContext.ShopperStatus);
      Assert.AreEqual(1, HttpContext.Current.Response.Cookies.Count);
      Assert.AreNotEqual(0, HttpContext.Current.Session.Keys.Count);
    }

    [TestMethod]
    public void NullSession()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      HttpContext.Current.Items.Remove("AspSession");
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.PartiallyTrusted, shopperContext.ShopperStatus);

      shopperContext.SetLoggedInShopper("832652");
      Assert.AreEqual(0, shopperContext.ShopperPriceType);
    }

    [TestMethod]
    public void NullSessionWithAuthToken()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      HttpContext.Current.Items.Remove("AspSession");
      var container = SetProviders(1);
      container.RegisterProvider<IAuthTokenProvider, MockAuthTokenProvider>();
      container.RegisterProvider<IAuthenticationProvider, MockAuthenticationProvider>();

      var shopperContext = container.Resolve<IShopperContext>();
      MockAuthenticationProvider.ClearHistory();

      shopperContext.SetLoggedInShopper("832652");
      Assert.IsFalse(MockAuthenticationProvider.DeauthenticateWasCalled);
    }

    [TestMethod]
    public void PublicShopperNotEncrypted()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = "832652";

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual(string.Empty, shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PublicShopperNotEncryptedEnvironmentTest()
    {
      var cookies = new NameValueCollection();
      cookies["testShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);
      container.SetData(MockSiteContextSettings.ServerLocation, ServerLocationType.Test);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PublicShopperNotEncryptedEnvironmentOte()
    {
      var cookies = new NameValueCollection();
      cookies["oteShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);
      container.SetData(MockSiteContextSettings.ServerLocation, ServerLocationType.Ote);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PublicShopperNotEncryptedWithValidMemAuth()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = "832652";
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual(string.Empty, shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PartiallyTrustedShopperBasic()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.PartiallyTrusted, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PartiallyTrustedShopperMemAuthNoMatch()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("833447", 1);

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PartiallyTrustedShopperNotEncrypted()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = "832652|" + DateTime.Now + "|1";

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PartiallyTrustedCookieButNoPublicCookie()
    {
      var cookies = new NameValueCollection();
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual(string.Empty, shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void ShopperPriceType()
    {
      ConfigElement shopperPriceTypeConfigItem;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.ShopperPriceType, out shopperPriceTypeConfigItem);
      shopperPriceTypeConfigItem.ResetStats();

      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      int shopperPriceTypeFirstCall = shopperContext.ShopperPriceType;
      int shopperPriceTypeSecondCall = shopperContext.ShopperPriceType;

      Assert.AreEqual(shopperPriceTypeFirstCall, shopperPriceTypeSecondCall);
      Assert.AreNotEqual(0, HttpContext.Current.Session.Keys.Count);
      Assert.AreEqual(1, shopperPriceTypeConfigItem.Stats.Succeeded);
    }

    [TestMethod]
    public void ShopperPriceTypeShopperAfterShopperSwitch()
    {
      ConfigElement shopperPriceTypeConfigItem;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.ShopperPriceType, out shopperPriceTypeConfigItem);
      shopperPriceTypeConfigItem.ResetStats();

      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      int shopperPriceTypeFirstCall = shopperContext.ShopperPriceType;

      shopperContext.SetNewShopper("833347");
      int shopperPriceTypeSecondCall = shopperContext.ShopperPriceType;

      Assert.AreNotEqual(0, HttpContext.Current.Session.Keys.Count);
      Assert.AreEqual(2, shopperPriceTypeConfigItem.Stats.Succeeded);
    }

    [TestMethod]
    public void ShopperPriceTypeException()
    {
      int shopperPriceTypeRequestType = ShopperProviderEngineRequests.ShopperPriceType;
      ShopperProviderEngineRequests.ShopperPriceType = ExceptionRequest.RequestType;

      try
      {
        ConfigElement shopperPriceTypeConfigItem;
        Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.ShopperPriceType, out shopperPriceTypeConfigItem);
        shopperPriceTypeConfigItem.ResetStats();

        var cookies = new NameValueCollection();
        cookies["devShopperId1"] = CreateShopperCoookieData("832652");

        SetWebEmptyContext(cookies);
        var container = SetProviders(1);

        var shopperContext = container.Resolve<IShopperContext>();
        int shopperPriceTypeFirstCall = shopperContext.ShopperPriceType;
        int shopperPriceTypeSecondCall = shopperContext.ShopperPriceType;

        Assert.AreEqual(shopperPriceTypeFirstCall, shopperPriceTypeSecondCall);
        Assert.AreNotEqual(0, HttpContext.Current.Session.Keys.Count);
        Assert.AreEqual(1, shopperPriceTypeConfigItem.Stats.Failed);
        Assert.AreEqual(0, shopperPriceTypeConfigItem.Stats.Succeeded);
      }
      finally
      {
        ShopperProviderEngineRequests.ShopperPriceType = shopperPriceTypeRequestType;
      }
    }

    [TestMethod]
    public void ShopperPriceTypeFromSession()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      HttpContext.Current.Session["ShopperContextProvider.ShopperPriceType.ShopperId"] = "832652";
      HttpContext.Current.Session["ShopperContextProvider.ShopperPriceType.Data"] = 8;
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(8, shopperContext.ShopperPriceType);
    }

    [TestMethod]
    public void ShopperPriceTypeFromSessionNullData()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      HttpContext.Current.Session["ShopperContextProvider.ShopperPriceType.ShopperId"] = "832652";
      HttpContext.Current.Session["ShopperContextProvider.ShopperPriceType.Data"] = null;
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(0, shopperContext.ShopperPriceType);
    }

    [TestMethod]
    public void ShopperPriceTypeFromSessionBadData()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");

      SetWebEmptyContext(cookies);
      HttpContext.Current.Session["ShopperContextProvider.ShopperPriceType.ShopperId"] = "832652";
      HttpContext.Current.Session["ShopperContextProvider.ShopperPriceType.Data"] = "This is not an int";
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(0, shopperContext.ShopperPriceType);
    }

    [TestMethod]
    public void SetShopperLoggedInWithCookieOverride()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopperWithCookieOverride("832652");
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Authenticated, shopperContext.ShopperStatus);
      Assert.AreEqual(2, HttpContext.Current.Response.Cookies.Count);
      Assert.AreNotEqual(0, HttpContext.Current.Session.Keys.Count);
    }

    [TestMethod]
    public void SetShopperLoggedInWithCookieOverrideWithAuthToken()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);
      container.RegisterProvider<IAuthTokenProvider, MockAuthTokenProvider>();
      container.RegisterProvider<IAuthenticationProvider, MockAuthenticationProvider>();
      MockAuthenticationProvider.ClearHistory();

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopperWithCookieOverride("832652");
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Authenticated, shopperContext.ShopperStatus);
      Assert.AreEqual(2, HttpContext.Current.Response.Cookies.Count);
      Assert.AreNotEqual(0, HttpContext.Current.Session.Keys.Count);
      Assert.IsTrue(MockAuthenticationProvider.DeauthenticateWasCalled);
    }

    [TestMethod]
    public void ShopperAlreadyFullyAuthenticated()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      HttpContext.Current.Session["SecShopperId"] = "832652";
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Authenticated, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PartiallyTrustedShopperLoggedIn()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.PartiallyTrusted, shopperContext.ShopperStatus);

      shopperContext.SetLoggedInShopper("832652");
      Assert.AreEqual(ShopperStatusType.Authenticated, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void PartiallyTrustedShopperAttemptToLogInIncorrectShopper()
    {
      var cookies = new NameValueCollection();
      cookies["devShopperId1"] = CreateShopperCoookieData("832652");
      cookies["devMemAuthId1"] = CreateMemAuthCookieData("832652", 1);

      SetWebEmptyContext(cookies);
      var container = SetProviders(1);

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.PartiallyTrusted, shopperContext.ShopperStatus);

      shopperContext.SetLoggedInShopper("837777");
      Assert.AreEqual(ShopperStatusType.PartiallyTrusted, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void ManagerShopper()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);
      container.RegisterProvider<IManagerContext, MockManagerContext>();
      container.SetData(MockManagerContextSettings.IsManager, true);
      container.SetData(MockManagerContextSettings.PrivateLabelId, 1);
      container.SetData(MockManagerContextSettings.ShopperId, "832652");

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual("832652", shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Manager, shopperContext.ShopperStatus);
    }

    [TestMethod]
    public void DetermineShopperError()
    {
      SetWebEmptyContext();
      var container = SetProviders(1);
      container.RegisterProvider<IManagerContext, MockManagerContextException>();

      var shopperContext = container.Resolve<IShopperContext>();
      Assert.AreEqual(string.Empty, shopperContext.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperContext.ShopperStatus);
    }


    private static string CreateMemAuthCookieData(string shopperId, int privateLabelId)
    {
      string delimitedData = shopperId + "|" + DateTime.Now + "|" + privateLabelId;
      string memAuthData;

      using (var cookieHelper = CookieEncryption.CreateDisposable())
      {
        memAuthData = cookieHelper.EncryptCookieValue(delimitedData);
      }

      return memAuthData;
    }

    private static string CreateShopperCoookieData(string shopperId)
    {
      string encryptedShopperId;
      using (var cookieEncryption = CookieEncryption.CreateDisposable())
      {
        encryptedShopperId = cookieEncryption.EncryptCookieValue(shopperId);
      }
      return encryptedShopperId;
    }
  }
}
