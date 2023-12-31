﻿using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Support;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Support.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Providers.Support.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.Support.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.Support.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.SupportApi.Impl.dll")]
  public class SupportPhoneTokenTests
  {
    const string TOKEN_FORMAT = "[@T[supportphone:{0}]@T]";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new SupportPhoneTokenHandler());
    }

    private IProviderContainer SetBasicContextAndProviders()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ISupportProvider, SupportProvider>();
      result.RegisterProvider<ISupportContactProvider, SupportContactProvider>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      return result;
    }

    private void TokenSuccess(string xmlTokenData)
    {
      var container = SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void EmptyXml()
    {
      var container = SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(TOKEN_FORMAT, string.Empty);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void BadXml()
    {
      var container = SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(TOKEN_FORMAT, "asdjfklsadjf");
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void TechnicalSupportPhoneSuccess()
    {
      TokenSuccess("<technical />");
    }

    [TestMethod]
    public void HostingExchangeSupportPhoneSuccess()
    {
      TokenSuccess("<hostingexchange />");
    }

    [TestMethod]
    public void BillingSupportPhoneSuccess()
    {
      TokenSuccess("<billing />");
    }

    [TestMethod]
    public void CompanyFaxSupportPhoneSuccess()
    {
      TokenSuccess("<companyfax />");
    }

    [TestMethod]
    public void CompanyMainSupportPhoneSuccess()
    {
      TokenSuccess("<companymain />");
    }

    [TestMethod]
    public void DomainsSupportPhoneSuccess()
    {
      TokenSuccess("<domains />");
    }

    [TestMethod]
    public void PremiumDomainsSupportPhoneSuccess()
    {
      TokenSuccess("<premiumdomains />");
    }

    [TestMethod]
    public void ServerSupportPhoneSuccess()
    {
      TokenSuccess("<server />");
    }
    [TestMethod]
    public void AdSpaceSupportPhoneSuccess()
    {
      TokenSuccess("<adspace />");
    }
    [TestMethod]
    public void SSLSupportPhoneSuccess()
    {
      TokenSuccess("<ssl />");
    }
    [TestMethod]
    public void DesignTeamSupportPhoneSuccess()
    {
      TokenSuccess("<designteam />");
    }
    [TestMethod]
    public void ResellerSalesSupportPhoneSuccess()
    {
      TokenSuccess("<resellersales />");
    }
    [TestMethod]
    public void McafeeSupportPhoneSuccess()
    {
      TokenSuccess("<mcafee />");
    }
  }
}
