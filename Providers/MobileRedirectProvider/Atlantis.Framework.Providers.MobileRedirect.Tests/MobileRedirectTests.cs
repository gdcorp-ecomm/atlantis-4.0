using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.MobileRedirect.Interface;
using Atlantis.Framework.Providers.UserAgentDetection;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.MobileRedirect.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.UserAgentEx.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
  public class MobileRedirectTests
  {
    private const string IPHONE_USER_AGENT = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
    private const string CHROME_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36";
    private const string GOOGLE_BOT_USER_AGENT = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

    public static IProviderContainer ProviderContainer
    {
      get
      {
        IProviderContainer providerContainer = new MockProviderContainer();
        providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
        providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
        providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        providerContainer.RegisterProvider<ILinkProvider, LinkProvider>();
        providerContainer.RegisterProvider<IUserAgentDetectionProvider, UserAgentDetectionProvider>();
        providerContainer.RegisterProvider<IMobileRedirectProvider, MobileRedirectProvider>();

        return providerContainer;
      }
    }

    private XDocument _userAgentXml;
    private XDocument UserAgentXml
    {
      get
      {
        string resourcePath = "Atlantis.Framework.Providers.MobileRedirect.Tests.useragents.xml";
        Assembly assembly = Assembly.GetExecutingAssembly();
        using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
        {
          _userAgentXml = XDocument.Load(textReader);
        }

        return _userAgentXml;
      }
    }

    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

    [TestMethod]
    public void IphoneUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();
      string redirectUrl = mobileRedirectProvider.GetRedirectUrl("atlantis.homepage", null);

      WriteOutput("Redirect Url: " + redirectUrl);
      Assert.IsTrue(redirectRequired, "Redirect should have been required");
      Assert.IsTrue(Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute), "Redirect url is malformed");
    }

    [TestMethod]
    public void IphoneUserAgentWithExtraQueryParamsTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home?isc=abc123");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();
      string redirectUrl = mobileRedirectProvider.GetRedirectUrl("atlantis.homepage", new NameValueCollection { { "hello", "world" } });

      WriteOutput("Redirect Url: " + redirectUrl);
      Assert.IsTrue(redirectRequired, "Redirect should have been required");
      Assert.IsTrue(Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute), "Redirect url is malformed");

      Uri redirectUri = new Uri(redirectUrl, UriKind.Absolute);
      Assert.IsTrue(redirectUri.Query.Contains("mrf=%2fhome%3fisc%3dabc123"));
      Assert.IsTrue(redirectUri.Query.Contains("hello=world"));
    }

    [TestMethod]
    public void IphoneUserAgentWithPublisherHashTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home?isc=abc123");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      mockHttpRequest.MockCookies(new NameValueCollection { { "publisherHash", "asdifjkaslfjfjsajkl" }, { "IapExp", DateTime.Today.AddDays(1).ToShortDateString() } });
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();
      string redirectUrl = mobileRedirectProvider.GetRedirectUrl("atlantis.homepage", null);

      WriteOutput("Redirect Url: " + redirectUrl);
      Assert.IsTrue(redirectRequired, "Redirect should have been required");
      Assert.IsTrue(Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute), "Redirect url is malformed");
    }

    [TestMethod]
    public void IphoneUserAgentWithRedirectCookieTrue()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      mockHttpRequest.MockCookies(new NameValueCollection { { "mobile.redirect.browser", "1" } });
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();
      string redirectUrl = mobileRedirectProvider.GetRedirectUrl("atlantis.homepage", null);

      WriteOutput("Redirect Url: " + redirectUrl);
      Assert.IsTrue(redirectRequired, "Redirect should have been required");
      Assert.IsTrue(Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute), "Redirect url is malformed");     
    }

    [TestMethod]
    public void IphoneUserAgentWithRedirectCookieFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      mockHttpRequest.MockCookies(new NameValueCollection { { "mobile.redirect.browser", "0" } });
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void IphoneUserAgentFullSiteTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home?iphoneview=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void IphoneUserAgentWithRedirectCookieTrueFullSiteTrue()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home?iphoneview=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      mockHttpRequest.MockCookies(new NameValueCollection { { "mobile.redirect.browser", "1" } });
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void ChromeUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(CHROME_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void GoogleBotUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(GOOGLE_BOT_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void NullUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(null);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void EmptyUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(string.Empty);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

      bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();

      Assert.IsFalse(redirectRequired, "Redirect should NOT have been required");
    }

    [TestMethod]
    public void UserAgentXmlTest()
    {
      IEnumerable<XElement> userAgentElements = UserAgentXml.Element("UserAgents").Elements("UserAgent");

      foreach (XElement userAgentElement in userAgentElements)
      {
        string type = userAgentElement.Attribute("type").Value;
        string userAgent = userAgentElement.Value;

        MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
        mockHttpRequest.MockUserAgent(userAgent);
        MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

        IMobileRedirectProvider mobileRedirectProvider = ProviderContainer.Resolve<IMobileRedirectProvider>();

        
        bool redirectRequired = mobileRedirectProvider.IsRedirectRequired();
        string redirectUrl = mobileRedirectProvider.GetRedirectUrl("atlantis.homepage", null);

        switch (type.ToLowerInvariant())
        {
          case "mobile":
          case "legacybrowser":
            Assert.IsTrue(redirectRequired, "Redirect should have been required. User Agent: " + userAgent);
            Assert.IsTrue(Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute), "Redirect url is malformed");
            break;
          default:
            Assert.IsFalse(redirectRequired, "Redirect should have NOT been required. User Agent: " + userAgent);
            Assert.IsTrue(redirectUrl == string.Empty);
            break;
        }
      }
    }
  }
}
