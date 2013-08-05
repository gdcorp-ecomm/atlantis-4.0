using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    private const string _companyTokenFormat = "[@T[company:{0}]@T]";
    private const string _productLineCompanyTokenFormat = "[@T[productline:{0}]@T]";

    readonly MockProviderContainer _container = new MockProviderContainer();
    private ISiteContext _siteContext;

    [TestInitialize]
    public void SetUp()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<ICompanyProvider, CompanyProvider>();
      _container.RegisterProvider<IProductLineProvider,ProductLineProvider>();
      _siteContext = _container.Resolve<ISiteContext>();

      TokenManager.RegisterTokenHandler(new CompanyTokenHandler());
      TokenManager.RegisterTokenHandler(new ProductLineTokenHandler());
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
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

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
    }

    [TestMethod]
    public void BlueRazorCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");


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
    }

    [TestMethod]
    public void WildWestCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.wildwestdomains.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1387");

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
    }

    [TestMethod]
    public void PlCompanyTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1592");

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP)), "Domains By Proxy");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_DOT_COM)), "DomainsByProxy.com");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_LEGAL)), "Domains By Proxy, LLC");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL)), "GoDaddy Operating Company, LLC");

      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_SHORT)), String.Empty);
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_DOT_COM)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_LEGAL)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY)), "Domains Priced Right");
      Assert.AreEqual(GetTokenString(String.Format(_companyTokenFormat, BrandKeyConstants.NAME_COMPANY_TWITTER)), String.Empty);
    }

    [TestMethod]
    public void ProductLineGDTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "Auctions")), "GoDaddy Auctions");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "BusinessRegistration")), "Business Registration");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "DomainBackorders")), "Domain Backorders");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "WebsiteBuilder")), "GoDaddy Website Builder");
    }

    [TestMethod]
    public void ProductLineNonGDTokenTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1592");

      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "Auctions")), "Domain Auctions");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "BusinessRegistration")), "Business Registration");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "DomainBackorders")), "Domain Backorders");
      Assert.AreEqual(GetTokenString(String.Format(_productLineCompanyTokenFormat, "WebsiteBuilder")), "Website Builder");
    }
  }
}
