using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Localization.Tests
{
  [TestClass]
  public class LocalizationTests
  {

    private const string _TOKEN_FORMAT = "[@T[localization:{0}]@T]";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new LocalizationTokenHandler());
    }

    private IProviderContainer SetContext(string url)
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx?ci=1", "ci=1");

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
      return result;
    }

    private string TokenSuccess(string xmlTokenData)
    {
      IProviderContainer container = SetContext("http://www.godaddy.com/");

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);

      return outputText;
    }

    // Negative test
    private string TokenFail(string xmlTokenData)
    {
      IProviderContainer container = SetContext("http://www.godaddy.com/");

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);

      return outputText;
    }

    [TestMethod]
    public void ShortLanguage()
    {
      string output = TokenSuccess("<language />");
      Assert.IsNotNull(output);
    }

    [TestMethod]
    public void FullLanguage()
    {
      string output = TokenSuccess("<language full=\"true\" />");
      Assert.IsNotNull(output);
    }

    // Negative tests
    [TestMethod]
    public void InvalidRootElement()
    {
      string output = TokenFail("<foo usdamount=\"1000\" />");
      Assert.AreEqual(string.Empty, output, false);
    }

    [TestMethod]
    public void InvalidXml()
    {
      string output = TokenFail("usdamount=\"1000\"");
      Assert.AreEqual(string.Empty, output, false);
    }
  }
}
