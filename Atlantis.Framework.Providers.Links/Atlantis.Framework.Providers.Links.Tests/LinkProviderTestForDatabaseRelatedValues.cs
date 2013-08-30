using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using Atlantis.Framework.Providers.Links.Tests.Mocks;
using afe = Atlantis.Framework.Engine;

namespace Atlantis.Framework.Providers.Links.Tests
{
  /// <summary>
  /// This class tests using GdDataCache and the backend database
  /// Only use this class if you NEED to verify database values, otherwise use the other test
  /// classes which moc the database values and do not depend on external dependencies like
  /// GdDataCache and backend databases to run.
  /// </summary>
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class LinkProviderTestForDatabaseRelatedValues
  {
    public const string afeConfig = "atlantis.config";

    private MockProviderContainer FrameworkContainer;

    [ClassInitialize]
    public static void Init(TestContext c)
    {
      afe.Engine.ReloadConfig(afeConfig);

      ReloadCache();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      FrameworkContainer = null;
    }

    [TestInitialize]
    public void TestInit()
    {
      FrameworkContainer = new MockProviderContainer();

      FrameworkContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      FrameworkContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      FrameworkContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      FrameworkContainer.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      FrameworkContainer.RegisterProvider<ILinkProvider, LinkProvider>();
    }

    private static void ReloadCache()
    {
      foreach (var config in Engine.Engine.GetConfigElements())
      {
        DataCache.DataCache.ClearCachedData(config.RequestType);
      }
    }

    private ILinkProvider NewLinkProvider(string url, int privateLabelId, string shopperId, bool isInternal = false, bool isManager = false, IPAddress userHostAddress = null)
    {
      var request = new MockHttpRequest(url);
      if (userHostAddress != null)
      {
        request.MockRemoteAddress(userHostAddress);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      FrameworkContainer.SetMockSetting(MockSiteContextSettings.IsRequestInternal, isInternal);
      FrameworkContainer.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (isManager)
      {
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.IsManager, isManager);
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.PrivateLabelId, privateLabelId);
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.ShopperId, shopperId);
      }
      else
      {
        var shopperContext = FrameworkContainer.Resolve<IShopperContext>();
        shopperContext.SetNewShopper(shopperId);
      }

      var linkProvider = FrameworkContainer.Resolve<ILinkProvider>();
      return linkProvider;
    }

    private MockLocalizationProvider LocalizationProvider
    {
      get { return FrameworkContainer.Resolve<ILocalizationProvider>() as MockLocalizationProvider; }
    }

    [TestMethod]
    public void DoubleSlashURL()
    {
      string homePage = "http://www.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");

      string url = links.GetUrl("SITEROOT", "/default.aspx");
      Assert.AreEqual(url.ToLowerInvariant(), homePage.ToLowerInvariant());
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      LocalizationProvider.RewrittenUrlLanguage = "es";

      url = links.GetUrl("SITEROOT", "/default.aspx");
      Assert.AreEqual(url.ToLowerInvariant(), homePage.ToLowerInvariant());
      Assert.IsFalse(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);
    }

    void ValidateUseC3ImageUrlsIsOn()
    {
      string useC3ImageUrls = DataCache.DataCache.GetAppSetting("LINKPROVIDER.USEC3IMAGEURLS");
      Assert.IsTrue("true".Equals(useC3ImageUrls, StringComparison.OrdinalIgnoreCase), "This test cannot complete successfully if LINKPROVIDER.USEC3IMAGEURLS is not 'true'");
    }

    [TestMethod]
    public void ImageRoot()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALIMAGEURL"];
      string managerUrl = links["EXTERNALIMAGEURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.ImageRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.ImageRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALIMAGEURL"];
      Assert.IsFalse(nonManagerUrl.Contains("/es/"));
      managerUrl = links["EXTERNALIMAGEURL.C3"];
      Assert.IsFalse(managerUrl.Contains("/es/"));

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.ImageRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));
      Assert.IsFalse(url.Contains("/es/"));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.ImageRoot;
      Assert.IsFalse(url.Contains("/es/"));

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void CSSRoot()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALCSSURL"];
      string managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.CssRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALCSSURL"];
      managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      c3url = links.CssRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void JavascriptRoot()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALJSURL"];
      string managerUrl = links["EXTERNALJSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.JavascriptRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.JavascriptRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALJSURL"];
      managerUrl = links["EXTERNALJSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.JavascriptRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.JavascriptRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void LargeImagesRoot()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALBIGIMAGE1URL"];
      string managerUrl = links["EXTERNALBIGIMAGE1URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.LargeImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.LargeImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALBIGIMAGE1URL"];
      managerUrl = links["EXTERNALBIGIMAGE1URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.LargeImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.LargeImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void PresentationCentralRoot()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALBIGIMAGE2URL"];
      string managerUrl = links["EXTERNALBIGIMAGE2URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.PresentationCentralImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.PresentationCentralImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALBIGIMAGE2URL"];
      managerUrl = links["EXTERNALBIGIMAGE2URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.PresentationCentralImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.PresentationCentralImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void CSSRootIndiaCSR()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALCSSURL"];
      string managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      IPAddress indiaP3VMwareAddress = IPAddress.Parse("172.29.33.45");
      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true, indiaP3VMwareAddress);
      string c3url = links.CssRoot;
      Assert.AreNotEqual(url, c3url);

      TestCleanup();
      TestInit();

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALCSSURL"];
      managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      TestCleanup();
      TestInit();

      indiaP3VMwareAddress = IPAddress.Parse("172.29.33.45");
      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true, indiaP3VMwareAddress);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.CssRoot;
      Assert.AreNotEqual(url, c3url);
    }

    [TestMethod]
    public void VideoRoot()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string url = links.VideoMeRoot;
      Assert.IsTrue(url.Contains("video"));

      url = links.VideoRoot;
      Assert.IsTrue(url.Contains("video")); 
    }

  }
}
