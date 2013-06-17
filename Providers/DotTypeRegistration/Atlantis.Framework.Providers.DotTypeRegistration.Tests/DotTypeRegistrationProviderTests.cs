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
      IDotTypeFormsSchema dotTypeFormsSchema;
      bool isSuccess = provider.GetDotTypeFormsSchema(1640, "MOBILE", "GA", "EN", out dotTypeFormsSchema);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeFormsSchema.FormCollection.Count > 0);
    }

    [TestMethod]
    public void DotTypeFormsSchemaFailure()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();
      IDotTypeFormsSchema dotTypeFormsSchema;
      bool isSuccess = provider.GetDotTypeFormsSchema(-1, "name of placement", "GA", "EN", out dotTypeFormsSchema);
      Assert.AreEqual(false, isSuccess);
      Assert.AreEqual(true, dotTypeFormsSchema == null);
    }

    [TestMethod]
    public void DotTypeClaimsSuccess()
    {
      IDotTypeRegistrationProvider provider = NewDotTypeRegistrationProvider();

      string[] domains = {"domain1.shop", "domain2.shop"};
      IDotTypeClaimsSchema dotTypeClaims;
      bool isSuccess = provider.GetClaimsSchema(domains, out dotTypeClaims);

      Assert.AreEqual(true, isSuccess);
      
      string claimsXml;
      Assert.AreEqual(true, dotTypeClaims.TryGetClaimsXmlByDomain("domain1.shop", out claimsXml));

      string noticeXml;
      Assert.AreEqual(true, dotTypeClaims.TryGetNoticeXmlByDomain("domain1.shop", out noticeXml));

      Assert.AreEqual(false, dotTypeClaims.TryGetClaimsXmlByDomain("domain2.shop", out claimsXml));

      Assert.AreEqual(false, dotTypeClaims.TryGetNoticeXmlByDomain("domain2.shop", out noticeXml));
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
