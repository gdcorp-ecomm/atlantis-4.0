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
      MockHttpContext.CreateFromWorkerRequest(request);
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          ((MockProviderContainer)_providerContainer).SetData(MockSiteContextSettings.IsRequestInternal, true);

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
      get { return _domainContactProvider ?? (_domainContactProvider = ProviderContainer.Resolve<IDomainContactValidationProvider>()); }
    }

    [TestMethod]
    public void DotTypeFormFieldsByDomain_ToJsonTest()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "asdfaeadgsadf234.lawyer" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "lawyer", "fos", "ga");

      var tlds = new List<string> { "LAWYER" };

      lookup.DomainContactGroup = GetDomainContactGroup(tlds, domains);
      
      provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);

      var formFieldsByDomain = new DotTypeFormFieldsByDomain(dotTypeFormFieldsByDomain.FormFieldsByDomain, dotTypeFormFieldsByDomain.FormItems);
      var json = formFieldsByDomain.ToJson;
      Assert.IsTrue(json.Contains("Radio"));
    }

    [TestMethod]
    public void GetDotTypeFormSchemas_CheckValidationLevel()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "carinaoweijeowiejf.eaplawyer" };
      var tlds = new List<string> { "eaplawyer" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "eaplawyer", "FOS", "GA");
      lookup.DomainContactGroup = GetDomainContactGroup(tlds, domains);
      provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);

      if (dotTypeFormFieldsByDomain != null)
      {
        Assert.AreEqual(dotTypeFormFieldsByDomain.FormItems.ValidationLevel, "domain");
      }
      else
      {
        Assert.Fail();
      }
    }
    
    [TestMethod]
    public void DotTypeFormsSchemaSuccess()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "domain1.n.borg", "claim1.example" };
      var tlds = new List<string> { "n.borg" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "cl", "MOBILE", "GA");
      lookup.DomainContactGroup = GetDomainContactGroup(tlds, domains);
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
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "DK", "(303)-555-1213", "(303)-555-2213");
      contactGroup.TrySetContact(DomainContactType.Registrant, registrantContact);

      var adminContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Admin", "badmin@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "DK", "(303)-555-1213", "(303)-555-2213");
      contactGroup.TrySetContact(DomainContactType.Administrative, adminContact);
      lookup.DomainContactGroup = contactGroup;

      bool isSuccess = provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormFieldsByDomain != null && dotTypeFormFieldsByDomain.FormFieldsByDomain.Count > 0);
    }

    [TestMethod]
    public void DotTypeFormsSchemaSuccessForNyc()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "asdfaeadgsadf234.nyc" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "nyc", "fos", "ga");

      var tlds = new List<string> { "NYC" };
      
      lookup.DomainContactGroup = GetDomainContactGroup(tlds, domains);

      bool isSuccess = provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormFieldsByDomain != null && dotTypeFormFieldsByDomain.FormFieldsByDomain.Count > 0);
    }

    [TestMethod]
    public void DotTypeFormsSchemaSuccessForLawyer()
    {
      IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain;
      string[] domains = { "asdfaeadgsadf234.lawyer" };

      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      var lookup = DotTypeFormSchemaLookup.Create("dpp", "lawyer", "fos", "ga");

      var tlds = new List<string> { "LAWYER" };

      lookup.DomainContactGroup = GetDomainContactGroup(tlds, domains);

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
      string[] domains = { "t9standvalidate.lrclaim" };
      var tlds = new List<string> { "lrclaim" };
      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "fos", "lr");
      lookup.DomainContactGroup = GetDomainContactGroup(tlds, domains);

      bool isSuccess = provider.GetDotTypeFormSchemas(lookup, domains, out dotTypeFormFieldsByDomain);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormFieldsByDomain != null && dotTypeFormFieldsByDomain.FormFieldsByDomain != null 
        && dotTypeFormFieldsByDomain.FormFieldsByDomain.Count > 0);
    }

    [TestMethod]
    public void DotTypeClaimsExist()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      const string domain = "validateandt9st.lrclaim";
      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "mobile", "lr");

      bool isSuccess = provider.DotTypeClaimsExist(lookup, domain);
      Assert.AreEqual(true, isSuccess);
    }

    [TestMethod]
    public void DotTypeClaimsExistForNyc()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      const string domain = "testandvalidate.nyc";
      var lookup = DotTypeFormSchemaLookup.Create("claims", "nyc", "fos", "ga");

      bool isSuccess = provider.DotTypeClaimsExist(lookup, domain);
      Assert.AreEqual(true, isSuccess);
    }

    [TestMethod]
    public void DotTypeClaimsNotExist()
    {
      var provider = ProviderContainer.Resolve<IDotTypeRegistrationProvider>();

      const string domain = "jhkjshkdfsdtrr.lrclaim";
      var lookup = DotTypeFormSchemaLookup.Create("claims", "lrclaim", "mobile", "lr");

      bool isSuccess = provider.DotTypeClaimsExist(lookup, domain);
      Assert.AreEqual(false, isSuccess);
    }

    private IDomainContactGroup GetDomainContactGroup(IEnumerable<string> tlds, IEnumerable<string> domains)
    {
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
           "Raj", "Vontela", "rvontela@gd.com", "GoDaddy", true,
          "123 Abc Rd", "Suite 45", "Scottsdale", "AZ",
          "85260", "US", "4805058800", "4805058800");
      contactGroup.TrySetContact(DomainContactType.Registrant, registrantContact);

      var techContact = DomainContactProvider.DomainContactInstance(
           "Raj", "Vontela", "rvontela@gd.com", "GoDaddy", true,
          "123 Abc Rd", "Suite 45", "Scottsdale", "AZ",
          "85260", "US", "4805058800", "4805058800");
      contactGroup.TrySetContact(DomainContactType.Technical, techContact);

      var billingContact = DomainContactProvider.DomainContactInstance(
           "Raj", "Vontela", "rvontela@gd.com", "GoDaddy", true,
          "123 Abc Rd", "Suite 45", "Scottsdale", "AZ",
          "85260", "US", "4805058800", "4805058800");
      contactGroup.TrySetContact(DomainContactType.Billing, billingContact);

      var adminContact = DomainContactProvider.DomainContactInstance(
           "Raj", "Vontela", "rvontela@gd.com", "GoDaddy", true,
          "123 Abc Rd", "Suite 45", "Scottsdale", "AZ",
          "85260", "US", "4805058800", "4805058800");
      contactGroup.TrySetContact(DomainContactType.Administrative, adminContact);

      return contactGroup;
    }

  }
}
