using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Domains.Tests
{
  [TestClass]
  [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.Static.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  public class DomainsTokenHandlerTests
  {
    public IProviderContainer ProviderContainer
    {
      get;
      set;
    }

    public TestContext TestContext
    {
      get;
      set;
    }

    [TestMethod]
    public void DomainsTokenHandler_ConstructorTest()
    {
      var target = new DomainsTokenHandler();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    public void DomainsTokenHandler_CreateTokenTest()
    {
      var target = new DomainsTokenHandler();
      Assert.IsNotNull(target);

      const string tokenData = "<icanntlds />";
      const string tokenKey = "domains";
      string tokenString = String.Format("[@T[{1}:{0}]@T]", tokenData, tokenKey);
      IToken actual = target.CreateToken(tokenData, tokenString);

      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(XmlToken));
      XmlToken cast = actual as XmlToken;
      Assert.AreEqual(tokenKey, cast.TokenKey);
      Assert.AreEqual(tokenString, cast.FullTokenString);
    }

    [TestMethod]
    public void DomainsTokenHandler_EvaluateTokensICANNFailIntegrationTest()
    {
      const string tokenData = "x";
      string tokenString = string.Format("[@T[domains:{0}]@T]", tokenData);
      string resultText = string.Empty;
      TokenEvaluationResult actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.WriteLine(resultText);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Errors, actual);
    }

    [TestMethod]
    public void DomainsTokenHandler_EvaluateTokensICANNFailTest()
    {
      var target = new DomainsTokenHandler();
      Assert.IsNotNull(target);

      const string tokenData = "<x />";
      string tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);
      IToken token = target.CreateToken(tokenData, tokenString);
      IEnumerable<IToken> tokens = Enumerable.Repeat(token, 1);
      var actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Errors, actual);
    }

    [TestMethod]
    public void DomainsTokenHandler_EvaluateTokensICANNSuccessIntegrationTest()
    {
      const string tokenData = "<icanntlds />";
      string tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);

      string resultText = string.Empty;
      string timerName = String.Format("Replacing Token: {0}", tokenString);
      TestContext.BeginTimer(timerName);
      var actual = TokenManager.ReplaceTokens(tokenString, ProviderContainer, out resultText);
      TestContext.EndTimer(timerName);
      TestContext.WriteLine("Result: {0}", resultText);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsFalse(string.IsNullOrEmpty(resultText));
    }

    [TestMethod]
    public void DomainsTokenHandler_EvaluateTokensICANNSuccessTest()
    {
      var target = new DomainsTokenHandler();
      Assert.IsNotNull(target);

      string tokenData = "<icanntlds />";
      string tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);
      IToken token = new GrammaticalDelimiterToken("domains", tokenData, tokenString);
      IEnumerable<IToken> tokens = Enumerable.Repeat(token, 1);
      var actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      const string delimiter = "*xyz*";
      tokenData = String.Format("<icanntlds delimiter=\"{0}\" />", delimiter);
      tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);
      token = new GrammaticalDelimiterToken("domains", tokenData, tokenString);
      tokens = Enumerable.Repeat(token, 1);
      actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsTrue(token.TokenResult.Contains(delimiter));

      tokenData = String.Format("<icanntlds grammatical=\"{0}\" />", delimiter);
      tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);
      token = new GrammaticalDelimiterToken("domains", tokenData, tokenString);
      tokens = Enumerable.Repeat(token, 1);
      actual = target.EvaluateTokens(tokens, ProviderContainer);
      TestContext.WriteLine(token.TokenResult);

      Assert.IsNotNull(actual);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);
      Assert.IsTrue(token.TokenResult.Contains(delimiter));
    }

    [TestInitialize]
    public void DomainsTokenHandler_InitializeTests()
    {
      TestContext = new TimerTestContext(TestContext);

      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IDebugContext, MockDebugProvider>();
      result.RegisterProvider<IDotTypeProvider, DotTypeProvider>();

      ProviderContainer = result;

      TokenManager.RegisterTokenHandler(new DomainsTokenHandler());
    }

    [TestMethod]
    public void DomainsTokenHandler_TokenKeyTest()
    {
      var target = new DomainsTokenHandler();
      Assert.IsNotNull(target);

      const string expected = "domains";
      string actual = target.TokenKey;
      Assert.AreEqual(expected, actual);
    }
  }
}
