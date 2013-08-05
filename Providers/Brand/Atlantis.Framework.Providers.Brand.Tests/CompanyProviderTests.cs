using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Brand.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Brand.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Brand.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Interface.dll")]
  public class CompanyProviderTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();
    private ISiteContext _siteContext ;
    private ICompanyProvider _companyProvider;

    [TestInitialize]
    public void SetUp()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<ICompanyProvider, CompanyProvider>();
      _siteContext = _container.Resolve<ISiteContext>();
    }

    [TestMethod]
    public void GoDaddyCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");
      _companyProvider = _container.Resolve<ICompanyProvider>();

      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY), "GoDaddy");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_DOT_COM), "GoDaddy.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_LEGAL), "GoDaddy, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY), "The GoDaddy Group");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_TWITTER), "@Godaddy");

    }

    [TestMethod]
    public void WildWestCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.wildwestdomains.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1387");
      _companyProvider = _container.Resolve<ICompanyProvider>();

      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_SHORT), "Wild West");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY), "Wild West Domains");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_DOT_COM), "WildWestDomains.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_LEGAL), "Wild West Domains, LLC");
    }

    [TestMethod]
    public void BlueRazorCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");
      _companyProvider = _container.Resolve<ICompanyProvider>();

      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY), "Blue Razor");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_DOT_COM), "BlueRazor.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_LEGAL), "Blue Razor Domains, LLC");
    }

    [TestMethod]
    public void PlCompanyName()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1592");
      _companyProvider = _container.Resolve<ICompanyProvider>();
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY), "Domains Priced Right");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_DOT_COM), "Domains Priced Right");
      Assert.AreEqual(_companyProvider.GetCompanyPropertyValue(BrandKeyConstants.NAME_COMPANY_LEGAL), "Domains Priced Right");
    }
  }
}
