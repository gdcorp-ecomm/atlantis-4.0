using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class ContentDocumentTests
  {
    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get { return _objectProviderContainer ?? (_objectProviderContainer = new ObjectProviderContainer()); }
    }

    private void RegisterProviders()
    {
      ObjectProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ObjectProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ObjectProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
    }

    private void SetupHttpContext()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    private void ApplicationStart()
    {
      SetupHttpContext();
      RegisterProviders();
    }

    [TestInitialize]
    public void Initialize()
    {
      ApplicationStart();
    }

    [TestMethod]
    public void CDT_PublishedDocNotFound()
    {
      string rawPath = "content/blah blah/hosting/email-hosting";
      ContentDocument contentDoc = new ContentDocument(ObjectProviderContainer, rawPath);
      Assert.IsTrue(contentDoc.GetContent() == ContentDocument.NullRenderContent);
    }

    [TestMethod]
    public void CDT_PublishedDocFound()
    {
      string rawPath = "content/sales/unittest/defaultcontentpath_getcontenttests";
      ContentDocument contentDoc = new ContentDocument(ObjectProviderContainer, rawPath);
      IRenderContent renderContent = contentDoc.GetContent();
      Assert.IsTrue(renderContent.Content.Contains("Current DataCenter:"));
    }
  }
}
