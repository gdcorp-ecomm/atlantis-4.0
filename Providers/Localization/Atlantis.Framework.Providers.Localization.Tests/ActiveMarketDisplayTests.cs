using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Localization.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class ActiveMarketDisplayTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();

    private IActiveMarketDisplayProvider ActiveMarketDisplayProvider()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IActiveMarketDisplayProvider, ActiveMarketDisplayProvider>();
      _container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      return _container.Resolve<IActiveMarketDisplayProvider>();
    }

    [TestMethod]
    public void ActiveMarketDisplaySuccess()
    {
      IActiveMarketDisplayProvider provider = ActiveMarketDisplayProvider();
      IList<IActiveMarketDisplay> activeMarketDisplays = provider.GetActiveMarketDisplay();
      Assert.AreEqual(true, activeMarketDisplays != null);
      Assert.AreEqual(true, activeMarketDisplays.Count > 0);
      Assert.AreEqual(true, activeMarketDisplays[0].CountryName != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].CountrySiteId != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].Language != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].MarketDescription != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].MarketId != string.Empty);
    }
  }
}
