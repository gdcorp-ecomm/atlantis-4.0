using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using System.Collections.Specialized;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using System.Web;

namespace Atlantis.Framework.Providers.Links.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class LinkProviderTests
  {
    public LinkProviderTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ILinkProvider, LinkProvider>();
    }

    private ILinkProvider NewLinkProvider(int privateLabelId, string shopperId)
    {
      HttpContext.Current.Items.Clear();
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      ((TestContexts)siteContext).SetContextInfo(privateLabelId, shopperId);
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)shopperContext).SetContextInfo(privateLabelId, shopperId);
      IManagerContext managerContext = HttpProviderContainer.Instance.Resolve<IManagerContext>();
      ((TestContexts)managerContext).SetContextInfo(privateLabelId, shopperId);

      ILinkProvider linkProvider = HttpProviderContainer.Instance.Resolve<ILinkProvider>();
      return linkProvider;
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void IsDebugInternal()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      ILinkProvider links = NewLinkProvider(1, "832652");

      Assert.IsTrue(links.IsDebugInternal());

      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.dev.intranet.gdg/default.aspx", string.Empty);
      Assert.IsFalse(links.IsDebugInternal());
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void ProgId()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);

      ILinkProvider links = NewLinkProvider(1724, "");
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));

      links = NewLinkProvider(1, "");
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));

      links = NewLinkProvider(2, "");
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));

      links = NewLinkProvider(1387, "");
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));

      LinkProviderCommonParameters.HandleProgId = false;
      links = NewLinkProvider(1724, "");
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      LinkProviderCommonParameters.HandleProgId = true;
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void FullUrlNoTilda()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);

      ILinkProvider links = NewLinkProvider(1724, "");

      string url = links.GetFullUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void ISC()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", "isc=blue");

      ILinkProvider links = NewLinkProvider(2, "");

      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("isc=blue"));

      url = links.GetRelativeUrl("/test.aspx", "isc", "green");
      Assert.IsTrue(url.Contains("isc=green"));

      LinkProviderCommonParameters.HandleISC = false;
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("isc="));

      LinkProviderCommonParameters.HandleISC = true;
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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void CommonParametersDelegate()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);

      LinkProviderCommonParameters.AddCommonParameters += new LinkProviderCommonParameters.AddCommonParametersHandler(HandleCommonParmeters);
      ILinkProvider links = NewLinkProvider(2, "");

      _handleActive = true;
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("extra=true"));

    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void XSSParameters()
    {
      MockHttpContext.SetMockHttpContext("ssl-certificates.aspx", "http://www.godaddy.com/ssl-certificates.aspx", "ci=8979&%3C/script%3E%3Cscript%3Ealert%28666%29%3C/script%3E=123");
      ILinkProvider links = NewLinkProvider(1, string.Empty);
      _handleActive = true;
      string url = links.GetRelativeUrl("/test.aspx", HttpContext.Current.Request.QueryString);
      Assert.IsFalse(url.Contains('<'));
    }

  }
}
