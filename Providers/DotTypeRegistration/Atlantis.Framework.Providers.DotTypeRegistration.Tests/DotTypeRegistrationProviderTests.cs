using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeForms.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeClaims.Impl.dll")]
  public class DotTypeRegistrationProviderTests
  {
    private MockProviderContainer NewDotTypeRegistrationProvider()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IDotTypeRegistrationProvider, DotTypeRegistrationProvider>();
      container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      return container;
    }

    [TestMethod]
    public void DotTypeFormsSchemaSuccess()
    {
      var container = NewDotTypeRegistrationProvider();
      container.Resolve<ILocalizationProvider>();

      IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain;
      string[] domains = { "domain1.shop", "claim1.example" };

      var provider = container.Resolve<IDotTypeRegistrationProvider>();
      bool isSuccess = provider.GetDotTypeFormSchemas(string.Empty, 1640, "MOBILE", "GA", domains, out formFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, formFieldsByDomain.Count > 0);
    }

    [TestMethod]
    public void DotTypeFormsSchemaFailure()
    {
      var container = NewDotTypeRegistrationProvider();
      var provider = container.Resolve<IDotTypeRegistrationProvider>();
      container.Resolve<ILocalizationProvider>();

      IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain;
      string[] domains = { "domain1.shop", "domain2.shop" };
      bool isSuccess = provider.GetDotTypeFormSchemas(string.Empty, -1, "name of placement", "GA", domains, out formFieldsByDomain);
      Assert.AreEqual(false, isSuccess);
      Assert.AreEqual(true, formFieldsByDomain.Count == 0);
    }

    [TestMethod]
    public void DotTypeFormsSuccess()
    {
      var container = NewDotTypeRegistrationProvider();
      var provider = container.Resolve<IDotTypeRegistrationProvider>();
      container.Resolve<ILocalizationProvider>();
      
      string dotTypeFormsHtml;
      bool isSuccess = provider.GetDotTypeForms("dpp", 1640, "FOS", "GA", out dotTypeFormsHtml);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(dotTypeFormsHtml));
    }

  }
}
