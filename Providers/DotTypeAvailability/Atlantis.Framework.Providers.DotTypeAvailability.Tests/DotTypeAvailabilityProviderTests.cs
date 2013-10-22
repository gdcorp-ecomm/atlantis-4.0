using Atlantis.Framework.Providers.DotTypeAvailability.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DotTypeAvailability.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  public class DotTypeAvailabilityProviderTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();

    private IDotTypeAvailabilityProvider NewDotTypeAvailabilityProvider()
    {
      _container.RegisterProvider<IDotTypeAvailabilityProvider, DotTypeAvailabilityProvider>();

      return _container.Resolve<IDotTypeAvailabilityProvider>();
    }

    [TestMethod]
    public void DotTypeHasLeafPage()
    {
      IDotTypeAvailabilityProvider provider = NewDotTypeAvailabilityProvider();

      bool hasLeafPage = provider.HasLeafPage("watch.borg");
      Assert.AreEqual(true, hasLeafPage);
    }

    [TestMethod]
    public void DotTypeHasLeafPageFail()
    {
      IDotTypeAvailabilityProvider provider = NewDotTypeAvailabilityProvider();

      bool hasLeafPage = provider.HasLeafPage("jsaldfjksadlf");
      Assert.AreEqual(false, hasLeafPage);
    }
  }
}
