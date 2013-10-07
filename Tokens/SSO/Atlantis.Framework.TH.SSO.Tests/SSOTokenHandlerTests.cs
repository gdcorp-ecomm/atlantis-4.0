using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Testing.MockHttpContext;
using System.Collections.Generic;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.BasePages.Providers;

namespace Atlantis.Framework.TH.SSO.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.Localization.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TH.SSO.dll")]
  public class SSOTokenHandlerTests
  {
    [TestInitialize]
    public void SSOTokenHandlerTests_InitializeTests()
    {
      TestContext = new TimerTestContext(TestContext);

      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILinkProvider, LinkProvider>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      result.RegisterProvider<IDebugContext, DebugProvider>();

      this.ProviderContainer = result;

      TokenManager.RegisterTokenHandler(new SSOTokenHandler());
    }

    public IProviderContainer ProviderContainer { get; set; }
    
    public TestContext TestContext
    {
      get;
      set;
    }

    [TestMethod]
    public void SSOTokenHandler_EvaluateTokenSuccessIntegrationTest()
    {
      string tokenData = "spkey";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      string resultText = string.Empty;
      string timerName = String.Format("Replacing Token: {0}", tokenString);
      TestContext.BeginTimer(timerName);
      TokenEvaluationResult actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.EndTimer(timerName);
      TestContext.WriteLine("Result: {0}", resultText);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(resultText));
      Assert.AreEqual("GDSWNET-130122134218001", resultText);

      tokenData = "spgroupname";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      resultText = string.Empty;
      timerName = String.Format("Replacing Token: {0}", tokenString);
      TestContext.BeginTimer(timerName);
      actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.EndTimer(timerName);
      TestContext.WriteLine("Result: {0}", resultText);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(resultText));
      Assert.AreEqual("GDSWNET", resultText);

      tokenData = "logouturl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      resultText = string.Empty;
      timerName = String.Format("Replacing Token: {0}", tokenString);
      TestContext.BeginTimer(timerName);
      actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.EndTimer(timerName);
      TestContext.WriteLine("Result: {0}", resultText);
      
      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(resultText));
      Assert.IsTrue(resultText.Contains("logout.aspx"));
      Assert.IsTrue(resultText.Contains("godaddy-com"));

      tokenData = "loginurl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      resultText = string.Empty;
      timerName = String.Format("Replacing Token: {0}", tokenString);
      TestContext.BeginTimer(timerName);
      actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.EndTimer(timerName);
      TestContext.WriteLine("Result: {0}", resultText);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(resultText));
      Assert.IsTrue(resultText.Contains("login.aspx"));
      Assert.IsTrue(resultText.Contains("godaddy-com"));
    }

    [TestMethod]
    public void SSOTokenHandler_ConstructorTest()
    {
      SSOTokenHandler target = new SSOTokenHandler();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    public void SSOTokenHandler_TokenKeyTest()
    {
      SSOTokenHandler target = new SSOTokenHandler();
      Assert.IsNotNull(target);

      string expected = "sso";
      Assert.AreEqual(expected, target.TokenKey);
    }

    [TestMethod]
    public void SSOTokenHandler_EvaluateTokensSuccessTest()
    {
      SSOTokenHandler target = new SSOTokenHandler();
      Assert.IsNotNull(target);

      string tokenData = "spkey";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      IToken token = new SimpleToken("sso", tokenData, tokenString);
      IEnumerable<IToken> tokens = new List<IToken>() { token };
      TokenEvaluationResult actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("GDSWNET-130122134218001", token.TokenResult);

      tokenData = "spgroupname";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      token = new SimpleToken("sso", tokenData, tokenString);
      tokens = new List<IToken>() { token };
      actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("GDSWNET", token.TokenResult);

      tokenData = "logouturl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      token = new SimpleToken("sso", tokenData, tokenString);
      tokens = new List<IToken>() { token };
      actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("logout.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("godaddy-com"));

      tokenData = "loginurl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      token = new SimpleToken("sso", tokenData, tokenString);
      tokens = new List<IToken>() { token };
      actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("login.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("godaddy-com"));

    }

    [TestMethod]
    public void SSOTokenHandler_EvaluateTokensFailTest()
    {
      SSOTokenHandler target = new SSOTokenHandler();
      Assert.IsNotNull(target);

      string tokenData = "x";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      IToken token = new SimpleToken("sso", tokenData, tokenString);
      IEnumerable<IToken> tokens = new List<IToken>() { token };
      TokenEvaluationResult actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Errors, actual);
    }

    [TestMethod]
    public void SSOTokenHandler_EvaluateTokensFailIntegrationTest()
    {
      SSOTokenHandler target = new SSOTokenHandler();
      Assert.IsNotNull(target);

      string tokenData = "x";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokenData);
      string resultText = string.Empty;
      TokenEvaluationResult actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.WriteLine(resultText);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Errors, actual);
    }
  }
}
