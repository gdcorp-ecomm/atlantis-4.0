using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeCache.Tests
{
  [TestClass]
  public class DotTypeRegistrationXmlTest
  {
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dottypecache.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Atlantis.Framework.DotTypeCache.DotCa.dll")]
    public void GetFieldsXml()
    {
      var fieldXml = DotTypeCache.GetRegistrationFieldsXml("CA");

      var contactNode = XDocument.Parse(fieldXml).Root.Element(RequiredFieldKeys.CONTACTS);

      Assert.IsTrue(contactNode.HasElements);
    }

  }
}
