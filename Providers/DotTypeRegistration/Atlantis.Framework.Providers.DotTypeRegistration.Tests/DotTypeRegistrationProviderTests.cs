using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainContactValidation;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;
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
  [DeploymentItem("Atlantis.Framework.DomainContactValidation.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainsTrustee.Impl.dll")]
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
          _providerContainer.RegisterProvider<IDomainContactValidationProvider, DomainContactValidationProvider>();
        }

        return _providerContainer;
      }
    }

    private IDomainContactValidationProvider _domainContactProvider;
    private IDomainContactValidationProvider DomainContactProvider
    {
      get
      {
        if (_domainContactProvider == null)
        {
          _domainContactProvider = ProviderContainer.Resolve<IDomainContactValidationProvider>();
        }

        return _domainContactProvider;
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
    public void DotTypeFormsSchemaSuccessForDK()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "domain1.n.borg" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "dk", "MOBILE", "GA");

      var tlds = new List<string> { "DK" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.TrySetContact(DomainContactType.Registrant, registrantContact);

      var adminContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Admin", "badmin@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.TrySetContact(DomainContactType.Administrative, adminContact);
      lookup.DomainContactGroup = contactGroup;

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

    [TestMethod]
    public void DotTypeClaimsExist()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      const string domain = "validateandt9st.lrclaim";
      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "mobile", "lr");

      bool isSuccess = provider.DotTypeClaimsExist(lookup, domain);
      Assert.AreEqual(true, isSuccess);
    }

    [TestMethod]
    public void DotTypeClaimsNotExist()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      const string domain = "jhkjshkdfsdtrr.lrclaim";
      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "mobile", "lr");

      bool isSuccess = provider.DotTypeClaimsExist(lookup, domain);
      Assert.AreEqual(false, isSuccess);
    }

  }
}
