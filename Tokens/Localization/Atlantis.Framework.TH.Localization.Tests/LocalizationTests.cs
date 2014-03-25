using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockLocalization;
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

    private IProviderContainer SetContext(string marketId)
    {
      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      result.SetData(MockLocalizationProviderSettings.MarketInfo, new MockMarketInfo(marketId, marketId, "en-US", false));
      return result;
    }

    private string TokenSuccess(string xmlTokenData)
    {
      IProviderContainer container = SetContext("fr-CA");

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);

      return outputText;
    }

    // Negative test
    private string TokenFail(string xmlTokenData)
    {
      IProviderContainer container = SetContext("fr-CA");

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
      Assert.AreEqual("fr", output);
    }

    [TestMethod]
    public void FullLanguage()
    {
      string output = TokenSuccess("<language full=\"true\" />");
      Assert.AreEqual("fr-CA", output);
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
