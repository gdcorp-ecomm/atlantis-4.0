using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers.DataToken.RenderHandlers;
using Atlantis.Framework.Providers.Containers.Tests.RenderContent;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  [TestClass]
  public class ProviderContainerDataTokenTests
  {
    private static IProviderContainer _providerContainer;
    public static IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        }

        return _providerContainer;
      }
    }

    [TestMethod]
    public void DataTokenValidEmptyOrigContent()
    {
      ProviderContainer.SetData("mytest", "new text");

      string content = null;

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals(string.Empty));
    }

    [TestMethod]
    public void DataTokenValid()
    {
      ProviderContainer.SetData("mytest", "new text");

      const string content = @"<div>[@D[mytest]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div>new text</div>"));
    }

    [TestMethod]
    public void DataTokenInvalid()
    {
      ProviderContainer.SetData("mytest", "new text");

      const string originalContent = @"<div>[@D[mytesting]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(originalContent);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div></div>"));
    }

    [TestMethod]
    public void DataTokenInvalidEmptyKey()
    {
      ProviderContainer.SetData("mytest", "new text");

      const string content = @"<div>[@D[]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div></div>"));
    }
  }
}
