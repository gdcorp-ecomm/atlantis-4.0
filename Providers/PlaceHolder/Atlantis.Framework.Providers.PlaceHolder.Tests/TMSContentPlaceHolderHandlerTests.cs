using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Render.Containers;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("atlantis.config")]
  [ExcludeFromCodeCoverage]
  public class TMSContentPlaceHolderHandlerTests
  {
    public IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      providerContainer.RegisterProvider<ICDSContentProvider, MockCDSContentProvider>();
      providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
      providerContainer.RegisterProvider<ITMSContentProvider, MockTMSContentProvider>();
      providerContainer.RegisterProvider<IAppSettingsProvider, MockAppSettingsProvider>();

      return providerContainer;
    }

    public IProviderContainer Error_InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();

      return providerContainer;
    }

    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void GetContent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("HP", "First", "Atlantis", "home");

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), new List<IRenderHandler> { new ProviderContainerDataTokenRenderHandler() });

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.IsTrue(renderedContent.Contains(string.Format("data-tms-msgid=\"{0}\"", "ID1")));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void GetContentNoContent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("HP", "NoContent", "Atlantis", "home");

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), new List<IRenderHandler> { new ProviderContainerDataTokenRenderHandler() });

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.IsTrue(renderedContent.Contains("ID1 NoContentTags NoContent 1"));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void GetContentUnknownInteraction()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("HP", "UNKNOWN", "Atlantis", "home");

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), new List<IRenderHandler> { new ProviderContainerDataTokenRenderHandler() });

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 4);
        Assert.IsTrue(renderedContent.Equals("   "));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }
  }
}
