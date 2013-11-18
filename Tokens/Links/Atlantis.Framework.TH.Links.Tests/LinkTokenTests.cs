using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Links.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TH.Links.dll")]
  public class LinkTokenTests
  {
    private const string TokenFormat = "[@T[link:{0}]@T]";

    private const string GoDaddyDebugNonSecureSiteUrl = "http://www.debug.godaddy-com.ide/default.aspx?ci=1";
    private const string GoDaddyDebugSecureSiteUrl = "https://www.debug.godaddy-com.ide/default.aspx?ci=1";
    private const string GoDaddyDebugNonSecureSiteUrlWithIsc = "http://www.debug.godaddy-com.ide/default.aspx?ci=1&isc=123456";

    private const string GoDaddyNonSecureSiteUrl = "http://www.godaddy.com/default.aspx?ci=1";
    private const string GoDaddySecureSiteUrl = "https://www.godaddy.com/default.aspx?ci=1";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new LinkTokenHandler());
    }

    private IProviderContainer SetBasicContextAndProviders(string siteUrl, string privateLabelId = "", bool isInternal = true)
    {
      MockHttpRequest request = new MockHttpRequest(siteUrl);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpContext.Current.Items[MockSiteContextSettings.IsRequestInternal] = isInternal;
      if (!string.IsNullOrEmpty(privateLabelId))
      {
        HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      }
      
      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IManagerContext, MockManagerContext>();
      result.RegisterProvider<ILinkProvider, LinkProvider>();
      
      return result;
    }

    private string TokenSuccess(string xmlTokenData, string siteUrl, string privateLabelId = "", bool isInternal = true)
    {
      var container = SetBasicContextAndProviders(siteUrl, privateLabelId, isInternal);

      string outputText;

      string token = string.Format(TokenFormat, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);

      return outputText;
    }

    [TestMethod]
    public void InvalidRenderTypeReturnsFalse()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);

      string outputText;

      string token = string.Format(TokenFormat, "<invalid/>");
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void EmptyRenderType()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);

      string outputText;

      string token = string.Format(TokenFormat, string.Empty);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void InvalidTokenType()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);

      string outputText;

      string token = string.Format(TokenFormat, "name");
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreNotEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void CurrentImageRoot()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.ImageRoot;
      string actualUrl = this.TokenSuccess("<IMAGEROOT/>", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void CurrentScriptCssRoot()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.CssRoot;
      string actualUrl = this.TokenSuccess("<CSSROOT/>", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void CurrentJavascriptRoot()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.JavascriptRoot;
      string actualUrl = this.TokenSuccess("<JAVASCRIPTROOT/>", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void DefaultRelativeLinkTokenReturnsHostWithProperSchemeNonSecure()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = "http://www.godaddy.com/";
      string actualUrl = this.TokenSuccess("<relative />", GoDaddyNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void DefaultRelativeLinkTokenReturnsHostWithProperSchemeSecure()
    {
      var container = SetBasicContextAndProviders(GoDaddySecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = "https://www.godaddy.com:80/";
      string actualUrl = this.TokenSuccess("<relative />", GoDaddySecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void DefaultRelativeLinkTokenWithParamsReturnsCorrectLink()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = "http://www.debug.godaddy-com.ide/?ci=1";
      string actualUrl = this.TokenSuccess("<relative><param name=\"ci\" value=\"1\" /></relative>", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void RelativeLinkTokenUsingCommonParametersReturnsCorrectLink()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrlWithIsc);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = "http://www.debug.godaddy-com.ide/?isc=123456";
      string actualUrl = this.TokenSuccess("<relative parammode=\"common\" />", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void RelativeLinkTokenUsingExplicitParametersReturnsCorrectLink()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrlWithIsc);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = "http://www.debug.godaddy-com.ide/";
      string actualUrl = this.TokenSuccess("<relative parammode=\"explicit\" />", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void RelativeLinkTokenWithJustPath()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetRelativeUrl("/hosting/web-hosting.aspx");
      string actualUrl = this.TokenSuccess("<relative path=\"/hosting/web-hosting.aspx\" />", GoDaddyDebugNonSecureSiteUrl);
      
      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void NonSecureRelativeLinkTokenWithPathAndCommonParams()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrlWithIsc);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetRelativeUrl("/hosting/web-hosting.aspx", QueryParamMode.CommonParameters);
      string actualUrl = this.TokenSuccess("<relative path=\"/hosting/web-hosting.aspx\" secure=\"false\" parammode=\"common\" />", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl,  actualUrl);
    }

    [TestMethod]
    public void NonSecureRelativeLinkTokenWithPathAndCommonParamsAndParamOverride()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      NameValueCollection testParams = new NameValueCollection();
      testParams["ci"] = "123456";

      string expectedUrl = links.GetRelativeUrl("/hosting/web-hosting.aspx", QueryParamMode.CommonParameters, testParams);
      string actualUrl = this.TokenSuccess("<relative path=\"/hosting/web-hosting.aspx\" secure=\"false\"><param name=\"ci\" value=\"123456\" /></relative>", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void NonSecureExternalLinkToken()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", string.Empty);
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" />", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void NonSecureExternalLinkTokenSiteUrl()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("siteurl", string.Empty);
      string actualUrl = this.TokenSuccess("<external linktype=\"siteurl\" />", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void NonSecureExternalLinkTokenWithCommonParams()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", string.Empty, QueryParamMode.CommonParameters, "isc", "123456");
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" parammode=\"common\" />", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void SecureExternalLinkTokenWithRelativePath()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", "/cart/basket.aspx" , QueryParamMode.CommonParameters, true);
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" path=\"/cart/basket.aspx\" secure=\"true\"/>", GoDaddyDebugNonSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void SecureExternalLinkTokenWithRelativePathAndParamOverride()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrlWithIsc);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", "/cart/basket.aspx", QueryParamMode.CommonParameters, true, "isc", "654321");
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" path=\"/cart/basket.aspx\" secure=\"true\"><param name=\"isc\" value=\"654321\" /></external>", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void ExternalLinkTokenWithExplicitModeDropsQueryString()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrlWithIsc);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", "/cart/basket.aspx", QueryParamMode.ExplicitParameters, false);
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" parammode=\"explicit\" path=\"/cart/basket.aspx\" secure=\"false\"/>", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void ExternalLinkTokenWithNoSecureAttribute()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugNonSecureSiteUrlWithIsc);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", "/cart/basket.aspx");
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" path=\"/cart/basket.aspx\" />", GoDaddyDebugNonSecureSiteUrlWithIsc);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }

    [TestMethod]
    public void SecureExternalLinkTokenWithNoSecureAttribute()
    {
      var container = SetBasicContextAndProviders(GoDaddyDebugSecureSiteUrl);
      ILinkProvider links = container.Resolve<ILinkProvider>();

      string expectedUrl = links.GetUrl("CARTURL", "/cart/basket.aspx");
      string actualUrl = this.TokenSuccess("<external linktype=\"CARTURL\" path=\"/cart/basket.aspx\" />", GoDaddyDebugSecureSiteUrl);

      Debug.WriteLine(string.Format("Expected: {0}", expectedUrl));
      Debug.WriteLine(string.Format("  Actual: {0}", actualUrl));

      Assert.AreEqual(expectedUrl, actualUrl);
    }
    [TestMethod]
    public void InvalidHAHAToken()
    {
      var container = SetBasicContextAndProviders(GoDaddyNonSecureSiteUrl);

      string outputText;

      string token = string.Format(TokenFormat, "<relative path='/hosting/website-builder.aspx' secure='false' parammode='explicit'>Haha");
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreEqual(string.Empty, outputText);
    }
  }
}
