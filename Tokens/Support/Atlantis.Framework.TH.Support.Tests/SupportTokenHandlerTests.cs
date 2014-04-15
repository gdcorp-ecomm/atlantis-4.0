using System;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Support;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Localization;
using System.Linq;

namespace Atlantis.Framework.TH.Support.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class SupportTokenHandlerTests
  {
    private TestContext _TestContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return _TestContextInstance;
      }
      set
      {
        _TestContextInstance = value;
      }
    }

    [TestMethod]
    public void ConstructorTest()
    {
      var target = new SupportTokenHandler();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    public void GetKeyTest()
    {
      var expected = "support";
      var target = new SupportTokenHandler();
      Assert.IsNotNull(target);
      var actual = target.TokenKey;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreateTokenTest()
    {
      var target = new SupportTokenHandler();
      Assert.IsNotNull(target);
      string tokenData = String.Empty;
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, tokenData);
      var actual = target.CreateToken(tokenData, fullTokenString);
      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(SupportToken));
      Assert.AreEqual(tokenData, actual.RawTokenData);
      Assert.AreEqual(fullTokenString, actual.FullTokenString);
    }

    [TestMethod]
    public void EvaluateTokensSuccessTest()
    {
      var target = new SupportTokenHandler();
      Assert.IsNotNull(target);

      IProviderContainer container = TestHelpers.SetBasicContextAndProviders(1);
      string data = string.Format("<{0} />", SupportType.TechnicalSupportHours);
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      IEnumerable<IToken> tokens = Enumerable.Repeat(new SupportToken(TestHelpers.SUPPORT_TOKEN_KEY, data, fullTokenString), 1);
      var actual = target.EvaluateTokens(tokens, container);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      foreach (var item in tokens)
      {
        TestContext.WriteLine(item.TokenResult);
      }

    }

    [TestMethod]
    public void EvaluateTokensFailureTest()
    {
      var target = new SupportTokenHandler();
      Assert.IsNotNull(target);

      string data = String.Empty;
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      IProviderContainer container = TestHelpers.SetBasicContextAndProviders(1);
      IEnumerable<IToken> tokens = Enumerable.Repeat(new SupportToken(TestHelpers.SUPPORT_TOKEN_KEY, data, fullTokenString), 1);
      var actual = target.EvaluateTokens(tokens, container);

      Assert.AreEqual(TokenEvaluationResult.Errors, actual);
    }
  }
}

