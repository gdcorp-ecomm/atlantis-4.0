using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using Atlantis.Framework.Providers.Links.Tests.Mocks;

namespace Atlantis.Framework.Providers.Links.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class LinkProviderTests
  {
    private ILinkProvider NewLinkProvider(string url, int privateLabelId, string shopperId, bool isInternal = false, bool isManager = false, IPAddress userHostAddress = null)
    {
      MockHttpRequest request = new MockHttpRequest(url);
      if (userHostAddress != null)
      {
        request.MockRemoteAddress(userHostAddress);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      HttpContext.Current.Items[MockSiteContextSettings.IsRequestInternal] = isInternal;
      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;

      if (isManager)
      {
        HttpContext.Current.Items[MockManagerContextSettings.IsManager] = isManager;
        HttpContext.Current.Items[MockManagerContextSettings.PrivateLabelId] = privateLabelId;
        HttpContext.Current.Items[MockManagerContextSettings.ShopperId] = shopperId;
      }

      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ILinkProvider, LinkProvider>();

      if (!isManager)
      {
        IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        shopperContext.SetNewShopper(shopperId);
      }

      ILinkProvider linkProvider = HttpProviderContainer.Instance.Resolve<ILinkProvider>();
      return linkProvider;
    }

    private ILocalizationProvider LocalizationProvider
    {
      get { return HttpProviderContainer.Instance.Resolve<ILocalizationProvider>(); }
    }

    [TestMethod]
    public void LowerCaseForSEOFullUrlIgnored()
    {
      ILinkProvider links = NewLinkProvider("http://WWW.GODADDY.COM/default.aspx", 1, "832652");
      LinkProvider.LowerCaseRelativeUrlsForSEO = true;

      string mixedCaseLink = links.GetUrl("SITEROOT", "TEST.AsPx");
      Assert.AreNotEqual(mixedCaseLink.ToLowerInvariant(), mixedCaseLink);

      LocalizationProvider.RewrittenUrlLanguage = "es";
      mixedCaseLink = links.GetUrl("SITEROOT", "TEST.AsPx");
      Assert.IsFalse(mixedCaseLink.ToLowerInvariant().Contains("/es/"));
    }

    [TestMethod]
    public void LowerCaseForSEO()
    {
      ILinkProvider links = NewLinkProvider("http://WWW.GODADDY.COM/default.aspx", 1, "832652");
      LinkProvider.LowerCaseRelativeUrlsForSEO = false;

      string mixedCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreNotEqual(mixedCaseLink.ToLowerInvariant(), mixedCaseLink);
      Assert.IsFalse(mixedCaseLink.ToLowerInvariant().Contains("/es/"));

      LinkProvider.LowerCaseRelativeUrlsForSEO = true;
      string lowerCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreEqual(lowerCaseLink.ToLowerInvariant(), lowerCaseLink);
      Assert.IsFalse(lowerCaseLink.ToLowerInvariant().Contains("/es/"));

      string mixedCaseQuery = links.GetRelativeUrl("/test.aspx", "ISC", "Blue");
      Assert.IsTrue(mixedCaseQuery.Contains("ISC=Blue"));
      Assert.IsFalse(mixedCaseQuery.ToLowerInvariant().Contains("/es/"));

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      LocalizationProvider.RewrittenUrlLanguage = "es";

      mixedCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreNotEqual(mixedCaseLink.ToLowerInvariant(), mixedCaseLink);
      Assert.IsTrue(mixedCaseLink.ToLowerInvariant().Contains("/es/"));

      LinkProvider.LowerCaseRelativeUrlsForSEO = true;
      lowerCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreEqual(lowerCaseLink.ToLowerInvariant(), lowerCaseLink);
      Assert.IsTrue(lowerCaseLink.ToLowerInvariant().Contains("/es/"));

      mixedCaseQuery = links.GetRelativeUrl("/test.aspx", "ISC", "Blue");
      Assert.IsTrue(mixedCaseQuery.Contains("ISC=Blue"));
      Assert.IsTrue(mixedCaseQuery.ToLowerInvariant().Contains("/es/"));
    }


    [TestMethod]
    public void IsDebugInternal()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1, "832652", true);
      Assert.IsTrue(links.IsDebugInternal());

      links = NewLinkProvider("http://siteadmin.dev.intranet.gdg/default.aspx", 1, "832652", true);
      Assert.IsFalse(links.IsDebugInternal());
    }

    [TestMethod]
    public void ProgId()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1724, string.Empty);
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1, string.Empty);
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 2, string.Empty);
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1387, string.Empty);
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));

      LinkProviderCommonParameters.HandleProgId = false;
      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1724, string.Empty);
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      LinkProviderCommonParameters.HandleProgId = true;

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1724, string.Empty);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1, string.Empty);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 2, string.Empty);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));

      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1387, string.Empty);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));

      LinkProviderCommonParameters.HandleProgId = false;
      links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1724, string.Empty);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      LinkProviderCommonParameters.HandleProgId = true;
      Assert.IsTrue(url.Contains("/es/"));
    }

    [TestMethod]
    public void FullUrlNoTilda()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1724, string.Empty);
      string url = links.GetFullUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));

      LocalizationProvider.RewrittenUrlLanguage = "es";

      url = links.GetFullUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
    }

    [TestMethod]
    public void ISC()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?isc=blue", 2, string.Empty);

      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("isc=blue"));

      url = links.GetRelativeUrl("/test.aspx", "isc", "green");
      Assert.IsTrue(url.Contains("isc=green"));

      LinkProviderCommonParameters.HandleISC = false;
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("isc="));

      LinkProviderCommonParameters.HandleISC = true;

      LocalizationProvider.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("isc=blue"));
      Assert.IsTrue(url.Contains("/es/"));

      url = links.GetRelativeUrl("/test.aspx", "isc", "green");
      Assert.IsTrue(url.Contains("isc=green"));
      Assert.IsTrue(url.Contains("/es/"));

      LinkProviderCommonParameters.HandleISC = false;
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("isc="));
      Assert.IsTrue(url.Contains("/es/"));
    }

    private static bool _handleActive = false;
    public static void HandleCommonParmeters(ISiteContext siteContext, NameValueCollection queryMap)
    {
      if (_handleActive)
      {
        queryMap["extra"] = "true";
      }
      _handleActive = false;
    }

    [TestMethod]
    public void CommonParametersDelegate()
    {
      LinkProviderCommonParameters.AddCommonParameters += new LinkProviderCommonParameters.AddCommonParametersHandler(HandleCommonParmeters);
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 2, string.Empty);

      _handleActive = true;
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("extra=true"));

      LocalizationProvider.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("/es/"));
    }

    [TestMethod]
    public void XSSParameters()
    {
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/ssl-certificates.aspx&ci=8979&%3C/script%3E%3Cscript%3Ealert%28666%29%3C/script%3E=123", 1, string.Empty);
      _handleActive = true;
      string url = links.GetRelativeUrl("/test.aspx", HttpContext.Current.Request.QueryString);
      Assert.IsFalse(url.Contains('<'));

      LocalizationProvider.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx", HttpContext.Current.Request.QueryString);
      Assert.IsFalse(url.Contains('<'));
      Assert.IsTrue(url.Contains("/es/"));
    }

    [TestMethod]
    public void DoubleSlashURL()
    {
      string homePage = "http://www.godaddy.com/default.aspx";
      ILinkProvider links = NewLinkProvider(homePage, 1, "832652");

      string doubleLink = links.GetUrl("SITEROOT", "/default.aspx");
      Assert.AreEqual(doubleLink.ToLowerInvariant(), homePage.ToLowerInvariant());

      LocalizationProvider.RewrittenUrlLanguage = "es";

      doubleLink = links.GetUrl("SITEROOT", "/default.aspx");
      Assert.AreEqual(doubleLink.ToLowerInvariant(), homePage.ToLowerInvariant());
      Assert.IsFalse(doubleLink.Contains("/es/"));
    }

    void ValidateUseC3ImageUrlsIsOn()
    {
      string useC3ImageUrls = DataCache.DataCache.GetAppSetting("LINKPROVIDER.USEC3IMAGEURLS");
      Assert.IsTrue("true".Equals(useC3ImageUrls, StringComparison.OrdinalIgnoreCase), "This test cannot complete successfully if LINKPROVIDER.USEC3IMAGEURLS is not 'true'");
    }

    [TestMethod]
    public void ImageRoot()
    {
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALIMAGEURL"];
      string managerUrl = links["EXTERNALIMAGEURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.ImageRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.ImageRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

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
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALCSSURL"];
      string managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.CssRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALCSSURL"];
      managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      c3url = links.CssRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void JavascriptRoot()
    {
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALJSURL"];
      string managerUrl = links["EXTERNALJSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.JavascriptRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.JavascriptRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALJSURL"];
      managerUrl = links["EXTERNALJSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.JavascriptRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.JavascriptRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void LargeImagesRoot()
    {
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALBIGIMAGE1URL"];
      string managerUrl = links["EXTERNALBIGIMAGE1URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.LargeImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.LargeImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALBIGIMAGE1URL"];
      managerUrl = links["EXTERNALBIGIMAGE1URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.LargeImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.LargeImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void PresentationCentralRoot()
    {
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALBIGIMAGE2URL"];
      string managerUrl = links["EXTERNALBIGIMAGE2URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.PresentationCentralImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      string c3url = links.PresentationCentralImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALBIGIMAGE2URL"];
      managerUrl = links["EXTERNALBIGIMAGE2URL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.PresentationCentralImagesRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.PresentationCentralImagesRoot;

      ValidateUseC3ImageUrlsIsOn();
      Assert.IsTrue(c3url.Contains(managerUrl));
    }

    [TestMethod]
    public void CSSRootIndiaCSR()
    {
      ILinkProvider links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);

      string nonManagerUrl = links["EXTERNALCSSURL"];
      string managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      string url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      IPAddress indiaP3VMwareAddress = IPAddress.Parse("172.29.33.45");
      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true, indiaP3VMwareAddress);
      string c3url = links.CssRoot;
      Assert.AreNotEqual(url, c3url);

      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true);
      LocalizationProvider.RewrittenUrlLanguage = "es";

      nonManagerUrl = links["EXTERNALCSSURL"];
      managerUrl = links["EXTERNALCSSURL.C3"];

      Assert.AreNotEqual(nonManagerUrl, managerUrl);

      url = links.CssRoot;
      Assert.IsTrue(url.Contains(nonManagerUrl));

      indiaP3VMwareAddress = IPAddress.Parse("172.29.33.45");
      links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, string.Empty, true, true, indiaP3VMwareAddress);
      LocalizationProvider.RewrittenUrlLanguage = "es";
      c3url = links.CssRoot;
      Assert.AreNotEqual(url, c3url);
    }


  }
}
