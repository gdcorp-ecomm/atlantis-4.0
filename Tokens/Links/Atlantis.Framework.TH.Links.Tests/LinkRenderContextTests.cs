using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Links.Tests
{
  [TestClass]
  public class LinkRenderContextTests
  {
    private const string GoDaddyDevNonSecureSiteUrl = "http://www.dev-godaddy.com?ci=1";
    private const string GoDaddyDevSecureSiteUrl = "https://www.dev-godaddy.com?ci=1";

    private const string GoDaddyTestNonSecureSiteUrl = "http://www.test-godaddy.com?ci=1";
    private const string GoDaddyTestSecureSiteUrl = "https://www.test-godaddy.com?ci=1";

    private const string GoDaddyNonSecureSiteUrl = "http://www.godaddy.com?ci=1";
    private const string GoDaddySecureSiteUrl = "https://www.godaddy.com?ci=1";

    private const string QASHOWNONMIN = "&qaminify=true";
    private const string QASHOWMIN = "&qaminify=false";
    private const string TOKENFORMAT = "[@T[{0}:{1}]@T]";
    private const string KEY = "link";

    public TestContext TestContext { get; set; }

    private IProviderContainer SetBasicContextAndProviders(string siteUrl, ServerLocationType serverLocationType = ServerLocationType.Dev, bool isInternal = true)
    {
      var request = new MockHttpRequest(siteUrl);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpContext.Current.Items[MockSiteContextSettings.IsRequestInternal] = isInternal;

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IManagerContext, MockManagerContext>();
      result.RegisterProvider<ILinkProvider, LinkProvider>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      result.SetData(MockSiteContextSettings.ServerLocation, serverLocationType);

      return result;
    }

    public IProviderContainer TestProvider { get; set; }

    [TestMethod]
    public void CssFilesInDev()
    {
      const string data = "<css path=\"shared/css/1/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyDevNonSecureSiteUrl);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.CssRoot + "shared/css/1/styles_20130904.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void CssFilesInTest()
    {
      const string data = "<css path=\"shared/css/1/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestNonSecureSiteUrl, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.CssRoot + "shared/css/1/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void CssFilesInProd()
    {
      const string data = "<css path=\"shared/css/1/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl, ServerLocationType.Prod);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.CssRoot + "shared/css/1/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void CssOverridePath()
    {
      const string data = "<css path=\"shared/css/1/styles_20130904.min.css\" namemode='explicit' />";

      var container = SetBasicContextAndProviders(GoDaddyDevSecureSiteUrl);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.CssRoot + "shared/css/1/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void JavascriptFilesInDev()
    {
      const string data = "<js path=\"fos/domains/search-new/js/cds_search_20140129.js\" />";

      var container = SetBasicContextAndProviders(GoDaddyDevNonSecureSiteUrl);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "fos/domains/search-new/js/cds_search_20140129.js";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);
      
      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void JavascriptFilesInTest()
    {
      const string data = "<js path=\"fos/domains/search-new/js/cds_search_20140129.js\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "fos/domains/search-new/js/cds_search_20140129.min.js";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void JavascriptFilesInProd()
    {
      const string data = "<js path=\"fos/domains/search-new/js/cds_search_20140129.js\" />";

      var container = SetBasicContextAndProviders(GoDaddySecureSiteUrl, ServerLocationType.Prod);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "fos/domains/search-new/js/cds_search_20140129.min.js";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void JavascriptOverridePath()
    {
      const string data = "<js path=\"fos/domains/search-new/js/cds_search_20140129.js\" namemode='explicit' />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "fos/domains/search-new/js/cds_search_20140129.js";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void JavascriptQueryStringFalseTest()
    {
      const string data = "<js path=\"fos/domains/search-new/js/cds_search_20140129.js\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWMIN, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "fos/domains/search-new/js/cds_search_20140129.js";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void JavascriptFilesInOTE()
    {
      const string data = "<js path=\"fos/domains/search-new/js/cds_search_20140129.js\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl, ServerLocationType.Ote);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "fos/domains/search-new/js/cds_search_20140129.min.js";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void CssFilesInUnknown()
    {
      const string data = "<css path=\"shared/css/1/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl, ServerLocationType.Undetermined);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "shared/css/1/styles_20130904.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void CssQueryStringTrueTest()
    {
      const string data = "<css path=\"shared/css/1/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "shared/css/1/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void ContextIdTest()
    {
      const string data = "<css path=\"shared/css/{contextid}/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "shared/css/1/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void RegionSiteTest()
    {
      const string data = "<css path=\"shared/css/{regionsite}/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "shared/css/www/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void MarketIdTest()
    {
      const string data = "<css path=\"shared/css/{marketid}/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "shared/css/en-us/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      privates.Invoke("CreateMinifiedPath", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void TestShowMinFile()
    {
      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);

      const bool expected = true;
      var actual =  privates.Invoke("ShowMinFile", string.Empty);

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestRenderToken()
    {
      const string data = "<css path=\"shared/css/{marketid}/styles_20130904.css\" />";

      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var links = container.Resolve<ILinkProvider>();
      var expected = links.JavascriptRoot + "shared/css/en-us/styles_20130904.min.css";
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      var fullTokenString = string.Format(TOKENFORMAT, KEY, data);
      var token = new LinkToken(KEY, data, fullTokenString);

      var actual = privates.Invoke("RenderToken", token);

      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void BreakRenderToken()
    {
      var container = SetBasicContextAndProviders(GoDaddyTestSecureSiteUrl + QASHOWNONMIN, ServerLocationType.Test);
      var target = new LinkRenderContext(container);
      var privates = new PrivateObject(target);
      LinkToken token = null;
      const bool expected = false;
      var actual = privates.Invoke("RenderToken", token);

      Assert.AreEqual(expected, actual);
    }
  }
}

