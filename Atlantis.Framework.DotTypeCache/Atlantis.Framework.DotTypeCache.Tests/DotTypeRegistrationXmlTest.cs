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
    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      MockHttpRequest request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dottypecache.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
    [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
    public void GetFieldsXml()
    {
      var fieldXml = DotTypeCache.GetRegistrationFieldsXml("CA");

      var contactNode = XDocument.Parse(fieldXml).Root.Element(RequiredFieldKeys.CONTACTS);

      Assert.IsTrue(contactNode.HasElements);
    }
  }
}