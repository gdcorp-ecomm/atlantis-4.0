using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.CDSContent;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Providers.TMSContentData.Interface;
using Atlantis.Framework.Render.Containers;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("atlantis.config")]
  [ExcludeFromCodeCoverage]
  public class TMSContentPlaceHolderHandlerTests
  {
    [ClassInitialize()]
    public static void ClassInit(TestContext context)
    {
      TMSContentConfig.Application = "TMS";
      TMSContentConfig.Location = "dev";
      TMSMessageConfig.AppID = "FOS";
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

    [TestMethod]
    public void TestGetContent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      MockErrorLogger mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("FOS", "First", "test", "home");

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.IsTrue(renderedContent.Contains("test/message1/home"));
      }                                                      
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void TestGetContentNoMessages()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      MockErrorLogger mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("FOS", "FailGetMessages", "test", "home");

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.IsTrue(renderedContent.Contains("test/home"));
        Assert.IsTrue(renderedContent.Contains("ID: [@D[tms.message.message_id]@D]"));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void TestGetContentMissingTemplate()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      MockErrorLogger mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("FOS", "First", "test", "home-missing");

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.IsTrue(renderedContent.Contains("test/home-missing"));
        Assert.IsTrue(renderedContent.Contains("ID: [@D[tms.message.message_id]@D]"));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void TestGetContentWithRank()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      MockErrorLogger mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        IPlaceHolder placeHolder = new TMSContentPlaceHolder("FOS", "Multiple", "test", "home", 1);

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.IsTrue(renderedContent.Contains("test/message2/home"));
        Assert.IsTrue(renderedContent.Contains("ID: ID2"));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    public IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      providerContainer.RegisterProvider<ICDSContentProvider, MockCDSContentProvider>();
      providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
      providerContainer.RegisterProvider<IAppSettingsProvider, MockAppSettingsProvider>();

      providerContainer.RegisterProvider<ITMSContentDataProvider, MockTMSContentDataProvider>();
      providerContainer.RegisterProvider<ITMSContentProvider, MockTMSContentProvider>();

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
  }
}
