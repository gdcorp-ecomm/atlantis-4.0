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
      ProviderContainer.SetData<string>("mytest", "new text");

      string content = null;

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals(string.Empty));
    }

    [TestMethod]
    public void DataTokenValid()
    {
      ProviderContainer.SetData<string>("mytest", "new text");

      const string content = @"<div>[@D[mytest]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div>new text</div>"));
    }

    [TestMethod]
    public void DataTokenInvalid()
    {
      ProviderContainer.SetData<string>("mytest", "new text");

      const string originalContent = @"<div>[@D[mytesting]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(originalContent);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div></div>"));
    }

    [TestMethod]
    public void DataTokenInvalidEmptyKey()
    {
      ProviderContainer.SetData<string>("mytest", "new text");

      const string content = @"<div>[@D[]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals("<div></div>"));
    }

    [TestMethod]
    public void DataTokenInvalidProviderContainer()
    {
      ProviderContainer.SetData<string>("mytest", "new text");

      const string content = @"<div>[@D[mytest]@D]</div>";

      var handler = new ProviderContainerDataTokenRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, new InvalidProviderContainer());

      Assert.IsTrue(processedRenderContent.Content.Equals(string.Empty));
    }

    private class InvalidProviderContainer : IProviderContainer
    {
      public void RegisterProvider<TProviderInterface, TProvider>() where TProviderInterface : class where TProvider : ProviderBase
      {
        throw new System.NotImplementedException();
      }

      public TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class
      {
        throw new System.NotImplementedException();
      }

      public bool TryResolve<TProviderInterface>(out TProviderInterface provider) where TProviderInterface : class
      {
        throw new System.NotImplementedException();
      }

      public bool CanResolve<TProviderInterface>() where TProviderInterface : class
      {
        throw new System.NotImplementedException();
      }

      public T GetData<T>(string key, T defaultValue)
      {
        throw new System.NotImplementedException();
      }

      public void SetData<T>(string key, T value)
      {
        throw new System.NotImplementedException();
      }
    }
  }
}
