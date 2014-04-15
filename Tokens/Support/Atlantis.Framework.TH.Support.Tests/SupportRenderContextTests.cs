using System;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.SupportApi.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Support.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class SupportRenderContextTests
  {
    private const int REQUEST_TYPE = 812;
    private TestContext _TestContextInstance;

    /// <summary>
    /// Gets or sets the test context which provides
    /// information about and functionality for the current test run.
    /// </summary>
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
      IProviderContainer container = TestHelpers.SetBasicContextAndProviders(1);
      var target = new SupportRenderContext(container);
      Assert.IsNotNull(target);

      var privates = new PrivateObject(target);
      var actual = privates.GetProperty("SupportContactProvider");
      Assert.IsNotNull(actual);
      Assert.AreEqual(container.Resolve<ISupportContactProvider>(), actual);
    }

    [TestMethod]
    public void RenderTokenHoursSuccessTest()
    {
      //var responseData = SupportContactsResponseData.FromMarketSupportContacts(TestHelpers.GetTestContacts());
      //EngineRequestMocking.RegisterOverride(REQUEST_TYPE, responseData);

      IProviderContainer container = TestHelpers.SetBasicContextAndProviders(1);
      var target = new SupportRenderContext(container);
      Assert.IsNotNull(target);

      string key = TestHelpers.SUPPORT_TOKEN_KEY;
      string data = string.Format("<{0} />", SupportType.TechnicalSupportHours);
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      IToken token = new SupportToken(key, data, fullTokenString);

      var actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      TestContext.WriteLine(token.TokenResult);
      var expected = "24/7";
      Assert.AreEqual(expected, token.TokenResult);

      data = string.Format("<{0} cityid=\"PHX\" />", SupportType.TechnicalSupportHours);
      fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      token = new SupportToken(key, data, fullTokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      TestContext.WriteLine(token.TokenResult);
      expected = "24/7";
      Assert.AreEqual(expected, token.TokenResult);
    }

    [TestMethod]
    public void RenderTokenFailureTest()
    {
      IProviderContainer container = TestHelpers.SetBasicContextAndProviders(1);
      var target = new SupportRenderContext(container);
      Assert.IsNotNull(target);

      string key = TestHelpers.SUPPORT_TOKEN_KEY;
      string data = String.Empty;
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      IToken token = new SupportToken(key, data, fullTokenString);

      var actual = target.RenderToken(token);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void RenderTokenInvalidRenderTypeTest()
    {
      IProviderContainer container = TestHelpers.SetBasicContextAndProviders(1);
      var target = new SupportRenderContext(container);
      Assert.IsNotNull(target);

      string key = TestHelpers.SUPPORT_TOKEN_KEY;
      string data = String.Empty;
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      IToken token = new SupportToken(key, data, fullTokenString);

      var actual = target.RenderToken(token);
      Assert.IsFalse(actual);

      Assert.AreEqual("SupportToken contains invalid RenderType", token.TokenError);
    }
  }
}

