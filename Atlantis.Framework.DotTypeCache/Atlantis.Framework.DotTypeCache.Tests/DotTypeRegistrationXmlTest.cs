using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      ((TestContexts)siteContext).SetContextInfo(1, "832652");
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)shopperContext).SetContextInfo(1, "832652");
      IManagerContext managerContext = HttpProviderContainer.Instance.Resolve<IManagerContext>();
      ((TestContexts)managerContext).SetContextInfo(1, "832652");
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dottypecache.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Atlantis.Framework.DotTypeCache.Interface")]
    [DeploymentItem("Atlantis.Framework.DotTypeCache")]
    [DeploymentItem("Atlantis.Framework.DotTypeCache.DotCa")]
    public void GetFieldsXml()
    {

      var fieldXml = DotTypeCache.GetRegistrationFieldsXml("CA");

      var restrictedNode = XDocument.Parse(fieldXml).Root.Element(RequiredFieldKeys.RESTRICTEDS);

      Assert.IsTrue(restrictedNode.HasElements);
    }

  }
}
