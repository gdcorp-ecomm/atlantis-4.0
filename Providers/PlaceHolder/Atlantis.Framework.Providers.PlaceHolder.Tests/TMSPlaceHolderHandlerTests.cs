﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("atlantis.config")]
  [ExcludeFromCodeCoverage]
  public class TMSPlaceHolderHandlerTests
  {
    public IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      providerContainer.RegisterProvider<ICDSContentProvider, MockCDSContentProvider>();
      providerContainer.RegisterProvider<IPersonalizationProvider, MockPersonalizationProvider>();
      providerContainer.RegisterProvider<IAppSettingsProvider, MockAppSettingsProvider>();
      providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();

      return providerContainer;
    }

    public IProviderContainer Error_InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();

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
    public void FetchesTheFirstMessageThatMatches()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "duplicate" });

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.AreEqual(renderedContent, "Id3 duplicate duplicate1 3");
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void FetchesTheMessageForTheFirstTag()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "engmtactnewcustsurveywebdlp", "", "engmtactnewcustsurveymobiledlp", "EngmtCustServMobileAppMobileHP" });

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.AreEqual(renderedContent, "ID1 engmtactnewcustsurveywebdlp engmtactnewcustsurvey 1");
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void FetchesTheMessageForTheTagThatHasOne()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "this is not to be found", "this too", "stddomxsmobiledlp" });

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        WriteOutput(renderedContent);

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.AreEqual(renderedContent, "Id2 stddomxsmobiledlp stddomxsdom 2");
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }
        
    [TestMethod]
    public void LogsAnErrorWhenNoneOfTheRequestedTagsAreFound()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IProviderContainer providerContainer = InitializeProviderContainer();

        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "this is not to be found", "this too" });

        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        Assert.IsTrue(string.IsNullOrEmpty(renderedContent));

        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains("None of the requested tags are found."));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogsAnErrorWhenInteractionPointIsEmpty()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("", new List<string> { "this is not to be found", "this too", "stddomxsmobiledlp" });

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        Assert.IsTrue(string.IsNullOrEmpty(renderedContent));
        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains(string.Format("Attributes {0} is required and at least one message tag is required", PlaceHolderAttributes.TMS_Interaction)));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogsAnErrorWhenNoTagsAreSpecified()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("", new List<string>());

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        Assert.IsTrue(string.IsNullOrEmpty(renderedContent));
        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains(string.Format("Attributes {0} is required and at least one message tag is required", PlaceHolderAttributes.TMS_Interaction)));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogsAnErrorWhenThePersonalizationProviderThrowsAnException()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("explode", new List<string> { "this doesn't matter"});

        IProviderContainer providerContainer = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        Assert.IsTrue(string.IsNullOrEmpty(renderedContent));
        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains("Exception occurred when calling the TMS webservice."));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogsAnErrorWhenRequiredProvidersAreNotRegistered()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("dosn't matter", new List<string> { "doesn't matter" });

        IProviderContainer providerContainer = Error_InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        Assert.IsTrue(string.IsNullOrEmpty(renderedContent));
        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains("Could not resolve the required providers.  CDSContent and Personalization are required."));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void DoesNotRepeatMessages()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      try
      {
        IPlaceHolder placeHolder1 = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "duplicate" });
        IPlaceHolder placeHolder2 = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "duplicate" });
        IPlaceHolder placeHolder3 = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "duplicate" });
        IPlaceHolder placeHolder4 = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "duplicate" });
        IPlaceHolder placeHolder5 = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "duplicate" });

        IProviderContainer providerContainer2 = InitializeProviderContainer();
        IPlaceHolderProvider placeHolderProvider = providerContainer2.Resolve<IPlaceHolderProvider>();

        string renderedContent1 = placeHolderProvider.ReplacePlaceHolders(placeHolder1.ToMarkup());

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.AreEqual(renderedContent1, "Id3 duplicate duplicate1 3");

        string renderedContent2 = placeHolderProvider.ReplacePlaceHolders(placeHolder2.ToMarkup());

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.AreEqual(renderedContent2, "Id4 duplicate duplicate2 4");

        string renderedContent3 = placeHolderProvider.ReplacePlaceHolders(placeHolder3.ToMarkup());

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
        Assert.AreEqual(renderedContent3, "Id5 duplicate duplicate3 5");


        string renderedContent4 = placeHolderProvider.ReplacePlaceHolders(placeHolder4.ToMarkup());

        Assert.AreEqual(renderedContent4, string.Empty);
        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains("None of the requested tags are found"));

        string renderedContent5 = placeHolderProvider.ReplacePlaceHolders(placeHolder5.ToMarkup());

        Assert.AreEqual(renderedContent5, string.Empty);
        Assert.IsTrue(mockLogger.Exceptions.Count == 2);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains("None of the requested tags are found"));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void DoesNotLogAMessageWhenTMSIsOff()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;
      IProviderContainer providerContainer = InitializeProviderContainer();
      try
      {
        IAppSettingsProvider settings = providerContainer.Resolve<IAppSettingsProvider>();
        ((MockAppSettingsProvider)settings).ReturnValue = "false";

        IPlaceHolder placeHolder = new TMSDocumentPlaceHolder("ProductUpsell", new List<string> { "this is not to be found", "this too" });

        IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

        string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup());

        Assert.IsTrue(string.IsNullOrEmpty(renderedContent));

        Assert.IsTrue(mockLogger.Exceptions.Count == 0);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
        IAppSettingsProvider settings = providerContainer.Resolve<IAppSettingsProvider>();
        ((MockAppSettingsProvider)settings).ReturnValue = "true";
      }
    }
  }
}
