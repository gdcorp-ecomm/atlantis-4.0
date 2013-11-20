using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Providers.Language.Interface;
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

    [TestInitialize]
    public void SetUp()
    {      
    }

    private IBrandProvider NewBrandProvider(int privateLabelId, bool isInternal = false)
    {
      var container = new MockProviderContainer();

      if (isInternal)
      {
        container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      }

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IBrandProvider, BrandProvider>();
      container.RegisterProvider<ILanguageProvider, MockLanguageProvider>();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      return container.Resolve<IBrandProvider>();
    }

    [TestMethod]
    public void GoDaddyCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      //_container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");
      //brandProvider = _container.Resolve<IBrandProvider>();

      var brandProvider = NewBrandProvider(1);

      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "GoDaddy");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "GoDaddy.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "GoDaddy, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY), "The GoDaddy Group");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_TWITTER), "@Godaddy");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "GoDaddy, Go Daddy, godadddy.com, godaddy, go daddy");
    }

    [TestMethod]
    public void WildWestCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.wildwestdomains.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(1387);

      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_SHORT), "Wild West");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "Wild West Domains");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "WildWestDomains.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "Wild West Domains, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "Wild West Domains, wildwestdomains.com, wildwestdomains, wild west, wildwest");
    }    

    [TestMethod]
    public void BlueRazorCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(2);

      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "Blue Razor");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "BlueRazor.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "Blue Razor Domains, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "Blue Razor.com, Blue Razor Domains, bluerazor.com, blue razor");
    }      
    
    [TestMethod]
    public void PlCompanyName()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(1592);

      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "Domains By Proxy");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "DomainsByProxy.com");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "Domains By Proxy, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "GoDaddy Operating Company, LLC");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "Domains Priced Right");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "Domains Priced Right");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "Domains Priced Right");
      Assert.AreEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "Domains Priced Right");
    }

    [TestMethod]
    public void ProductLineNameTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(1);

      Assert.AreEqual(brandProvider.GetProductLineName("Auctions"), "auctionsXXX");
    }


    [TestMethod]
    public void InvalidGoDaddyCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(1);

      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "xxx Domains By Proxy");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "xxx DomainsByProxy.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "xxx Domains By Proxy, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "xxx GoDaddy Operating Company, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "xxx GoDaddy");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "xxx GoDaddy.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "xxx GoDaddy, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_PARENT_COMPANY), "xxx The GoDaddy Group");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_TWITTER), "xxx @Godaddy");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "xxx GoDaddy, Go Daddy, godadddy.com, godaddy, go daddy");
    }
    
    [TestMethod]
    public void InvalidWildWestCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.wildwestdomains.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(1387);

      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "xxx Domains By Proxy");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "xxx DomainsByProxy.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "xxx Domains By Proxy, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "xxx GoDaddy Operating Company, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_SHORT), "xxx Wild West");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "xxx Wild West Domains");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "xxx WildWestDomains.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "xxx Wild West Domains, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "xxx Wild West Domains, wildwestdomains.com, wildwestdomains, wild west, wildwest");
    }

    [TestMethod]
    public void InvalidBlueRazorCompanyNames()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(2);

      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "xxx Domains By Proxy");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "xxx DomainsByProxy.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "xxx Domains By Proxy, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "xxx GoDaddy Operating Company, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "xxx Blue Razor");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "xxx BlueRazor.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "xxx Blue Razor Domains, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "xxx Blue Razor.com, Blue Razor Domains, bluerazor.com, blue razor");
    }

    [TestMethod]
    public void InvalidPlCompanyName()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.securepaynet.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var brandProvider = NewBrandProvider(1592);

      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP), "xxx Domains By Proxy");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_DOT_COM), "xxx DomainsByProxy.com");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_LEGAL), "xxx Domains By Proxy, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL), "xxx GoDaddy Operating Company, LLC");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY), "xxx Domains Priced Right");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_DOT_COM), "xxx Domains Priced Right");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_COMPANY_LEGAL), "xxx Domains Priced Right");
      Assert.AreNotEqual(brandProvider.GetCompanyName(BrandKeyConstants.NAME_KEYWORDS), "xxx Domains Priced Right");
    }
  }
}
