using System;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.SupportApi.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Support.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class SupportTokenTests
  {
    private TestContext _TestContextInstance;
    private const int REQUEST_TYPE = 812;

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
      string key = TestHelpers.SUPPORT_TOKEN_KEY;
      string data = string.Format("<{0} cityid=\"1\" />", SupportType.TechnicalSupportHours);
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      var target = new SupportToken(key, data, fullTokenString);
      Assert.IsNotNull(target);
      Assert.IsInstanceOfType(target, typeof(XmlToken));
      Assert.AreEqual(key, target.TokenKey);
      Assert.AreEqual(data, target.RawTokenData);
      Assert.AreEqual(fullTokenString, target.FullTokenString);
      Assert.AreEqual("1", target.CityId);
    }

    [TestMethod]
    public void RenderTypeTest()
    {
      string key = TestHelpers.SUPPORT_TOKEN_KEY;
      string data = string.Format("<{0} cityid=\"1\" />", SupportType.TechnicalSupportHours);
      string fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      var target = new SupportToken(key, data, fullTokenString);
      Assert.IsNotNull(target);
      Assert.AreEqual(SupportType.TechnicalSupportHours, target.RenderType);

      data = string.Format("<{0} cityid=\"1\" />", SupportType.TechnicalSupportHours);
      fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      target = new SupportToken(key, data, fullTokenString);
      Assert.IsNotNull(target);
      Assert.AreEqual(SupportType.TechnicalSupportHours, target.RenderType);

      data = string.Format("<{0} cityid=\"1\" />", SupportType.CompanyMainPhone);
      fullTokenString = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, data);
      target = new SupportToken(key, data, fullTokenString);
      Assert.IsNotNull(target);
      Assert.AreEqual(SupportType.CompanyMainPhone, target.RenderType);
    }

    [TestMethod]
    public void TokenSuccessIntegrationTest()
    {
      //var responseData = SupportContactsResponseData.FromMarketSupportContacts(TestHelpers.GetTestContacts());
      //EngineRequestMocking.RegisterOverride(REQUEST_TYPE, responseData);

      var container = TestHelpers.SetBasicContextAndProviders(1);
      TokenManager.RegisterTokenHandler(new SupportTokenHandler());

      string outputText;

      string xmlTokenData = string.Format("<{0} />", SupportType.TechnicalSupportHours);
      string token = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      TestContext.WriteLine(outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      TestContext.WriteLine(outputText);
      Assert.AreNotEqual(string.Empty, outputText);
      var expected = "24/7";
      Assert.AreEqual(expected, outputText);

      xmlTokenData = string.Format("<{0} cityid=\"PHX\" />", SupportType.TechnicalSupportHours);
      token = string.Format(TestHelpers.SUPPORT_TOKEN_FORMAT, xmlTokenData);
      result = TokenManager.ReplaceTokens(token, container, out outputText);
      TestContext.WriteLine(outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      TestContext.WriteLine(outputText);
      Assert.AreNotEqual(string.Empty, outputText);
      Assert.AreEqual(expected, outputText);
    }
  }
}
