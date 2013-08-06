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
  public class BrandProviderTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();
    private ISiteContext _siteContext ;
    private IBrandProvider _brandProvider;

    [TestInitialize]
    public void SetUp()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<IBrandProvider, BrandProvider>();
      _siteContext = _container.Resolve<ISiteContext>();
    }

    [TestMethod]
    public void GoDaddyCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");
      _brandProvider = _container.Resolve<IBrandProvider>();

      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "GoDaddy");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "GoDaddy.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "GoDaddy, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY), "The GoDaddy Group");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_TWITTER), "@Godaddy");

    }

    [TestMethod]
    public void WildWestCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.wildwestdomains.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1387");
      _brandProvider = _container.Resolve<IBrandProvider>();

      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_SHORT), "Wild West");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "Wild West Domains");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "WildWestDomains.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "Wild West Domains, LLC");
    }

    [TestMethod]
    public void BlueRazorCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");
      _brandProvider = _container.Resolve<IBrandProvider>();

      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "Blue Razor");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "BlueRazor.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "Blue Razor Domains, LLC");
    }

    [TestMethod]
    public void PlCompanyName()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1592");
      _brandProvider = _container.Resolve<IBrandProvider>();
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "Domains Priced Right");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "Domains Priced Right");
      Assert.AreEqual(_brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "Domains Priced Right");
    }

    [TestMethod]
    public void ProductLineNameGoDaddyTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");
      _brandProvider = _container.Resolve<IBrandProvider>();

      Assert.AreEqual(_brandProvider.GetProductLineName("Auctions", 1), "GoDaddy Auctions");
      Assert.AreEqual(_brandProvider.GetProductLineName("Auctions"), "Domain Auctions");
      Assert.AreEqual(_brandProvider.GetProductLineName("WebsiteBuilder", 1), "GoDaddy Website Builder");
      Assert.AreEqual(_brandProvider.GetProductLineName("WebsiteBuilder"), "Website Builder");
      Assert.AreEqual(_brandProvider.GetProductLineName("BusinessRegistration"), "Business Registration");
      Assert.AreEqual(_brandProvider.GetProductLineName("DomainBackorders"), "Domain Backorders");
      Assert.AreEqual(_brandProvider.GetProductLineName("FaxThruEmail"), "Fax Thru Email");
      Assert.AreEqual(_brandProvider.GetProductLineName("ProtectedRegistration"), "Protected Registration");
      Assert.AreEqual(_brandProvider.GetProductLineName("HostingConnection"), "Value Applications");
    }

    [TestMethod]
    public void ProductLineNameNonGDTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");
      _brandProvider = _container.Resolve<IBrandProvider>();

      Assert.AreEqual(_brandProvider.GetProductLineName("Auctions"), "Domain Auctions");
      Assert.AreEqual(_brandProvider.GetProductLineName("HostingConnection"), "Value Applications");
      Assert.AreEqual(_brandProvider.GetProductLineName("BusinessRegistration"), "Business Registration");
      Assert.AreEqual(_brandProvider.GetProductLineName("DomainBackorders"), "Domain Backorders");
      Assert.AreEqual(_brandProvider.GetProductLineName("FaxThruEmail"), "Fax Thru Email");
      Assert.AreEqual(_brandProvider.GetProductLineName("ProtectedRegistration"), "Protected Registration");
      Assert.AreEqual(_brandProvider.GetProductLineName("WebsiteBuilder"), "Website Builder");

      Assert.AreNotEqual(_brandProvider.GetProductLineName("Auctions"), "GoDaddy Auctions");
      Assert.AreNotEqual(_brandProvider.GetProductLineName("HostingConnection"), "GoDaddy Hosting Connection");
    }
  }
}
