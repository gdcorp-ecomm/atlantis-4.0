using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeForms.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeClaims.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeValidation.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class DotTypeRegistrationProviderTests
  {
    [TestInitialize]
    public void InitializeTests()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          ((MockProviderContainer)_providerContainer).SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IDotTypeRegistrationProvider, DotTypeRegistrationProvider>();
          _providerContainer.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
        }

        return _providerContainer;
      }
    }

    [TestMethod]
    public void DotTypeFormsSchemaSuccess()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "domain1.n.borg", "claim1.example" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "cl", "MOBILE", "GA");
      bool isSuccess = provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormFieldsByDomain != null && dotTypeFormFieldsByDomain.FormFieldsByDomain.Count > 0);
    }

    [TestMethod]
    public void DotTypeFormsSchemaFailure()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "domain1.shop", "domain2.shop" };
      var lookup = DotTypeFormSchemaLookup.Create("dpp", "abcd", "name of placement", "GA");

      bool isSuccess = provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);
      Assert.AreEqual(false, isSuccess);
      Assert.AreEqual(true, dotTypeFormFieldsByDomain == null);
    }

    [TestMethod]
    public void DotTypeFormsSuccess()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();
      
      string dotTypeFormsHtml;
      IDotTypeFormLookup lookup = DotTypeFormLookup.Create("trademark", "j.borg", "FOS", "GA", "abcd.com");

      bool isSuccess = provider.GetDotTypeForms(lookup, out dotTypeFormsHtml);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(dotTypeFormsHtml));
    }

    [TestMethod]
    public void DotTypeFormsSuccess2()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "validateandt9st.lrclaim" };
      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "mobile", "lr");

      bool isSuccess = provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormFieldsByDomain != null && dotTypeFormFieldsByDomain.FormFieldsByDomain.Count > 0);
    }

  }
}
