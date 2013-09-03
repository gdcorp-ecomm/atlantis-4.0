using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.MobileRedirect;
using Atlantis.Framework.Providers.MobileRedirect.Interface;
using Atlantis.Framework.Providers.UserAgentDetection;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Links.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.UserAgentEx.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TH.Links.dll")]
  public class MobileRedirectLinkTokenTests
  {
    private const string TOKEN_FORMAT = "[@T[mobileRedirectLink:{0}]@T]";
    private const string IPHONE_USER_AGENT = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
    private const string CHROME_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36";
    private const string GOOGLE_BOT_USER_AGENT = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

    private IProviderContainer _providerContainer;

    public void Initialize(string userAgent)
    {
      TokenManager.RegisterTokenHandler(new MobileRedirectLinkTokenHandler());

      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx");
      request.MockUserAgent(userAgent);
      MockHttpContext.SetFromWorkerRequest(request);

      _providerContainer = new MockProviderContainer();
      _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      _providerContainer.RegisterProvider<ILinkProvider, LinkProvider>();
      _providerContainer.RegisterProvider<IUserAgentDetectionProvider, UserAgentDetectionProvider>();
      _providerContainer.RegisterProvider<IMobileRedirectProvider, MobileRedirectProvider>();
    }

    [TestMethod]
    public void InvalidDataElement()
    {
      Initialize(IPHONE_USER_AGENT);

      string outputText;

      string token = string.Format(TOKEN_FORMAT, "<invalid/>");
      TokenEvaluationResult tokenEvaluationResult = TokenManager.ReplaceTokens(token, _providerContainer, out outputText);

      Assert.IsTrue(tokenEvaluationResult == TokenEvaluationResult.Errors);
      Assert.IsTrue(outputText == string.Empty);
    }

    [TestMethod]
    public void EmptyDataElement()
    {
      Initialize(IPHONE_USER_AGENT);

      string outputText;

      string token = string.Format(TOKEN_FORMAT, string.Empty);
      TokenEvaluationResult tokenEvaluationResult = TokenManager.ReplaceTokens(token, _providerContainer, out outputText);

      Assert.IsTrue(tokenEvaluationResult == TokenEvaluationResult.Errors);
      Assert.IsTrue(outputText == string.Empty);
    }

    [TestMethod]
    public void DataElementNoAttributes()
    {
      Initialize(IPHONE_USER_AGENT);

      string outputText;

      string token = string.Format(TOKEN_FORMAT, "<data />");
      TokenEvaluationResult tokenEvaluationResult = TokenManager.ReplaceTokens(token, _providerContainer, out outputText);

      Assert.IsTrue(tokenEvaluationResult == TokenEvaluationResult.Errors);
      Assert.IsTrue(outputText == string.Empty);
    }

    [TestMethod]
    public void DataElementValidRedirectKeyIphoneUserAgent()
    {
      Initialize(IPHONE_USER_AGENT);

      string outputText;

      string token = string.Format(TOKEN_FORMAT, "<data redirectKey=\"sales.somekey\" />");
      TokenEvaluationResult tokenEvaluationResult = TokenManager.ReplaceTokens(token, _providerContainer, out outputText);

      Assert.IsTrue(tokenEvaluationResult == TokenEvaluationResult.Success);
      Assert.IsTrue(Uri.IsWellFormedUriString(outputText, UriKind.Absolute));
    }

    [TestMethod]
    public void DataElementValidRedirectKeyChromeUserAgent()
    {
      Initialize(CHROME_USER_AGENT);

      string outputText;

      string token = string.Format(TOKEN_FORMAT, "<data redirectKey=\"sales.somekey\" />");
      TokenEvaluationResult tokenEvaluationResult = TokenManager.ReplaceTokens(token, _providerContainer, out outputText);

      Assert.IsTrue(tokenEvaluationResult == TokenEvaluationResult.Errors);
      Assert.IsTrue(outputText == string.Empty);
    }
  }
}
