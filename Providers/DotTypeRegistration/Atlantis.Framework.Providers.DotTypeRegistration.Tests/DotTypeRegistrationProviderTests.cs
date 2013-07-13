using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
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
    public void DotTypeFormsSchemaSuccess()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();
      IDictionary<string, IList<IFormField>> formFieldsByDomain;
      string[] domains = { "domain1.shop", "domain2.shop" };
      bool isSuccess = provider.GetDotTypeFormSchemas(1640, "MOBILE", "GA", "EN", domains, out formFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, formFieldsByDomain.Count > 0);
    }

    [TestMethod]
    public void DotTypeFormsSchemaFailure()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();
      IDictionary<string, IList<IFormField>> formFieldsByDomain;
      string[] domains = { "domain1.shop", "domain2.shop" };
      bool isSuccess = provider.GetDotTypeFormSchemas(-1, "name of placement", "GA", "EN", domains, out formFieldsByDomain);
      Assert.AreEqual(false, isSuccess);
      Assert.AreEqual(true, formFieldsByDomain.Count == 0);
    }

    [TestMethod]
    public void DotTypeFormsSuccess()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();
      string dotTypeFormsHtml;
      bool isSuccess = provider.GetDotTypeForms(1640, "MOBILE", "GA", "EN", out dotTypeFormsHtml);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(dotTypeFormsHtml));
    }

  }
}
