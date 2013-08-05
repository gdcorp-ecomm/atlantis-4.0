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
  public class ProductLineProviderTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();
    private ISiteContext _siteContext;
    private IProductLineProvider _productLineProvider;

    [TestInitialize]
    public void SetUp()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<IProductLineProvider, ProductLineProvider>();
      _siteContext = _container.Resolve<ISiteContext>();
    }

    [TestMethod]
    public void ProductLineNameGoDaddyTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");
      _productLineProvider = _container.Resolve<IProductLineProvider>();

      Assert.AreEqual(_productLineProvider.GetProductLineName("Auctions", 1), "GoDaddy Auctions");
      Assert.AreEqual(_productLineProvider.GetProductLineName("Auctions"), "Domain Auctions");
      Assert.AreEqual(_productLineProvider.GetProductLineName("WebsiteBuilder", 1), "GoDaddy Website Builder");
      Assert.AreEqual(_productLineProvider.GetProductLineName("WebsiteBuilder"), "Website Builder");
      Assert.AreEqual(_productLineProvider.GetProductLineName("BusinessRegistration"), "Business Registration");
      Assert.AreEqual(_productLineProvider.GetProductLineName("DomainBackorders"), "Domain Backorders");
      Assert.AreEqual(_productLineProvider.GetProductLineName("FaxThruEmail"), "Fax Thru Email");
      Assert.AreEqual(_productLineProvider.GetProductLineName("ProtectedRegistration"), "Protected Registration");
      Assert.AreEqual(_productLineProvider.GetProductLineName("HostingConnection"), "Value Applications");
    }

    [TestMethod]
    public void ProductLineNameNonGDTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.bluerazor.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");
      _productLineProvider = _container.Resolve<IProductLineProvider>();

      Assert.AreEqual(_productLineProvider.GetProductLineName("Auctions"), "Domain Auctions");
      Assert.AreEqual(_productLineProvider.GetProductLineName("HostingConnection"), "Value Applications");
      Assert.AreEqual(_productLineProvider.GetProductLineName("BusinessRegistration"), "Business Registration");
      Assert.AreEqual(_productLineProvider.GetProductLineName("DomainBackorders"), "Domain Backorders");
      Assert.AreEqual(_productLineProvider.GetProductLineName("FaxThruEmail"), "Fax Thru Email");
      Assert.AreEqual(_productLineProvider.GetProductLineName("ProtectedRegistration"), "Protected Registration");
      Assert.AreEqual(_productLineProvider.GetProductLineName("WebsiteBuilder"), "Website Builder");

      Assert.AreNotEqual(_productLineProvider.GetProductLineName("Auctions"), "GoDaddy Auctions");
      Assert.AreNotEqual(_productLineProvider.GetProductLineName("HostingConnection"), "GoDaddy Hosting Connection");
    }
  }
}
