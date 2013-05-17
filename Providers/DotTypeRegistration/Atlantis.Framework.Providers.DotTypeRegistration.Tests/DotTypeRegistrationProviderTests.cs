using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeForms.Impl.dll")]
  public class DotTypeRegistrationProviderTests
  {
    private IDotTypeRegistrationProvider NewDotTypeRegistrationProvider()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IDotTypeRegistrationProvider, DotTypeRegistrationProvider>();

      return container.Resolve<IDotTypeRegistrationProvider>();
    }

    [TestMethod]
    public void DotTypeFormsSuccess()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();
      IDotTypeFormsSchema dotTypeFormsSchema;
      bool isSuccess = provider.GetDotTypeFormsSchema(12345, "name of placement", out dotTypeFormsSchema);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormsSchema.FormCollection.Count > 0);
    }

    public void DotTypeFormsFailure()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();
      IDotTypeFormsSchema dotTypeFormsSchema;
      bool isSuccess = provider.GetDotTypeFormsSchema(-1, "name of placement", out dotTypeFormsSchema);
      Assert.AreEqual(false, isSuccess);
      Assert.AreEqual(true, dotTypeFormsSchema == null);
    }
  }
}
