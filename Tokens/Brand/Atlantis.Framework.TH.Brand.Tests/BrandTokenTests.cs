using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Language;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Localization;

namespace Atlantis.Framework.TH.Brand.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Brand.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Brand.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.Brand.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.Brand.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Interface.dll")]
  public class BrandTokenTests
  {
    private const string _companyTokenFormat = "[@T[companyname:{0}]@T]";
    private const string _productLineCompanyTokenFormat = "[@T[productline:{0}]@T]";

    private MockProviderContainer _container = new MockProviderContainer();

    [TestInitialize]
    public void SetUp()
    {
      TokenManager.RegisterTokenHandler(new CompanyTokenHandler());
      TokenManager.RegisterTokenHandler(new ProductLineTokenHandler());
    }

    private MockProviderContainer NewTHBrand(int privateLabelId, bool isInternal = false)
    {
      var container = new MockProviderContainer();

      if (isInternal)
      {
        container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      }

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IBrandProvider, BrandProvider>();
      container.RegisterProvider<ILanguageProvider, LanguageProvider>();
      container.RegisterProvider<ILocalizationProvider, LocalizationProvider>();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      container.Resolve<ISiteContext>();

      return container;
    }

    private string GetTokenString(string token)
    {
      string outputText;
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, _container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      return outputText;
    }


    [TestMethod]
    public void GoDaddyCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      //_container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");
      _container = NewTHBrand(1);

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP)), "Domains By Proxy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_DOT_COM)), "DomainsByProxy.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_LEGAL)), "Domains By Proxy, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL)), "GoDaddy Operating Company, LLC");

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_SHORT)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY)), "GoDaddy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_DOT_COM)), "GoDaddy.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_LEGAL)), "GoDaddy, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY)), "The GoDaddy Group");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_TWITTER)), "@Godaddy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_KEYWORDS)), "GoDaddy, Go Daddy, godadddy.com, godaddy, go daddy");
    }

    [TestMethod]
    public void BlueRazorCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container = NewTHBrand(2);

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP)), "Domains By Proxy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_DOT_COM)), "DomainsByProxy.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_LEGAL)), "Domains By Proxy, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL)), "GoDaddy Operating Company, LLC");

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_SHORT)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY)), "Blue Razor");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_DOT_COM)), "BlueRazor.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_LEGAL)), "Blue Razor Domains, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_TWITTER)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_KEYWORDS)), "Blue Razor.com, Blue Razor Domains, bluerazor.com, blue razor");
    }

    [TestMethod]
    public void WildWestCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.wildwestdomains.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container = NewTHBrand(1387);

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP)), "Domains By Proxy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_DOT_COM)), "DomainsByProxy.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_LEGAL)), "Domains By Proxy, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL)), "GoDaddy Operating Company, LLC");

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_SHORT)), "Wild West");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY)), "Wild West Domains");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_DOT_COM)), "WildWestDomains.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_LEGAL)), "Wild West Domains, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_TWITTER)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_KEYWORDS)), "Wild West Domains, wildwestdomains.com, wildwestdomains, wild west, wildwest");
    }

    [TestMethod]
    public void PlCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container = NewTHBrand(1592);

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP)), "Domains By Proxy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_DOT_COM)), "DomainsByProxy.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_LEGAL)), "Domains By Proxy, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL)), "GoDaddy Operating Company, LLC");

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_DOT_COM)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_LEGAL)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_KEYWORDS)), "Domains Priced Right");
    }

    [TestMethod]
    public void ProductLineGDTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container = NewTHBrand(1);

      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<Auctions contextid=\"GD\" />")), "GoDaddy Auctions");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<Auctions contextid=\"Invalid\"/>")), "Domain Auctions");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<WebsiteBuilder contextid=\"1\"/>")), "GoDaddy Website Builder");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<WebsiteBuilder />")), "Website Builder");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<BusinessRegistration />")), "Business Registration");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<DomainBackorders />")), "Domain Backorders");

    }

    [TestMethod]
    public void ProductLineNonGDTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container = NewTHBrand(1592);

      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<Auctions contextid=\"GD\" />")), "Domain Auctions");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<BusinessRegistration />")), "Business Registration");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<DomainBackorders />")), "Domain Backorders");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "<WebsiteBuilder />")), "Website Builder");
    }
  }
}
