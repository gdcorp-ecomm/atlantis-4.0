using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Render.Containers.Tests
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
      ProviderContainer.SetData("mytest123", "new text");

      const string content = @"<div>[@D[mytest123]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div>new text</div>"));
    }

    [TestMethod]
    public void DataTokenValidWithDots()
    {
      ProviderContainer.SetData("mytest.somekey123", "new text");

      const string content = @"<div>[@D[mytest.somekey123]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div>new text</div>"));
    }

    [TestMethod]
    public void DataTokenValidWithDashes()
    {
      ProviderContainer.SetData("mytest-somekey123", "new text");

      const string content = @"<div>[@D[mytest-somekey123]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div>new text</div>"));
    }

    [TestMethod]
    public void DataTokenValidWithDotsAndDashes()
    {
      ProviderContainer.SetData("mytest.namespace-somekey123", "new text");

      const string content = @"<div>[@D[mytest.namespace-somekey123]@D]</div>";

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
