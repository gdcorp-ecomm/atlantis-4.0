using System;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Render.ContentInjection.RenderHandlers;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Render.ContentInjection.Tests
{
  [TestClass]
  public class ContentInjectionRenderHandlerTests
  {
    private const string HTML_CONTENT = @"<!DOCTYPE html>\r\n
                                         <html>\r\n
                                          <head>\r\n
                                            <title>Home Page</title>\r\n
                                          </head>\r\n
                                          <body>\r\n
                                            <div>Hello World!!!</div>\r\n
                                          </body>\r\n
                                         </html>";

    private const string HTML_CONTENT_BODY_AND_HEAD_WITH_ATTRIBUTES = @"<!DOCTYPE html>\r\n
                                                                       <html>\r\n
                                                                        <head style=""padding:10px;"">\r\n
                                                                          <title>Home Page</title>\r\n
                                                                        </head>\r\n
                                                                        <body style=""background-color:#000;"">\r\n
                                                                          <div>Hello World!!!</div>\r\n
                                                                        </body>\r\n
                                                                       </html>";

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          RegisterProviders();
        }

        return _providerContainer;
      }
    }

    private void RegisterProviders()
    {
      ProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      ProviderContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
    }

    private void WriteOutput(string output)
    {
      #if DEBUG
      Debug.WriteLine(output);
      #else
      Console.WriteLine(output);
      #endif
    }

    [TestMethod]
    public void SingleContentInjectionItems()
    {
      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup());

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.IsTrue(processedRenderContent.Content.Contains("<head>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("</head>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("<body>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("</body>"));

      Assert.IsTrue(processedRenderContent.Content.Contains(placeHolderProvider.ReplacePlaceHolders(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup())));
    }

    [TestMethod]
    public void MultipleContentInjectionItems()
    {
      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup());
      IContentInjectionItem headEndContentInjectionItem = new HtmlHeadEndContentInjectionItem(new UserControlPlaceHolder("~/controls/scripts/gascript.ascx", null).ToMarkup());
      IContentInjectionItem bodyBeginContentInjectionItem = new HtmlBodyBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/banners/iscbanner.ascx", null).ToMarkup());
      IContentInjectionItem bodyEndContentInjectionItem = new HtmlBodyEndContentInjectionItem(new UserControlPlaceHolder("~/controls/traffic/trafficimage.ascx", null).ToMarkup());

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem,
                                                              headEndContentInjectionItem,
                                                              bodyBeginContentInjectionItem,
                                                              bodyEndContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);
      
      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);
      
      Assert.IsTrue(processedRenderContent.Content.Contains("<head>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("</head>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("<body>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("</body>"));

      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup()));
      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/scripts/gascript.ascx", null).ToMarkup()));
      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/banners/iscbanner.ascx", null).ToMarkup()));
      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/traffic/trafficimage.ascx", null).ToMarkup()));
    }

    [TestMethod]
    public void MultipleContentInjectionItemsBodyHeadWithAttributes()
    {
      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup());
      IContentInjectionItem headEndContentInjectionItem = new HtmlHeadEndContentInjectionItem(new UserControlPlaceHolder("~/controls/scripts/gascript.ascx", null).ToMarkup());
      IContentInjectionItem bodyBeginContentInjectionItem = new HtmlBodyBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/banners/iscbanner.ascx", null).ToMarkup());
      IContentInjectionItem bodyEndContentInjectionItem = new HtmlBodyEndContentInjectionItem(new UserControlPlaceHolder("~/controls/traffic/trafficimage.ascx", null).ToMarkup());

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem,
                                                              headEndContentInjectionItem,
                                                              bodyBeginContentInjectionItem,
                                                              bodyEndContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT_BODY_AND_HEAD_WITH_ATTRIBUTES);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.IsTrue(processedRenderContent.Content.Contains("<head style=\"padding:10px;\">"));
      Assert.IsTrue(processedRenderContent.Content.Contains("</head>"));
      Assert.IsTrue(processedRenderContent.Content.Contains("<body style=\"background-color:#000;\">"));
      Assert.IsTrue(processedRenderContent.Content.Contains("</body>"));

      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup()));
      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/scripts/gascript.ascx", null).ToMarkup()));
      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/banners/iscbanner.ascx", null).ToMarkup()));
      Assert.IsTrue(processedRenderContent.Content.Contains(new UserControlPlaceHolder("~/controls/traffic/trafficimage.ascx", null).ToMarkup()));
    }

    [TestMethod]
    public void NullContentWithInjectionItems()
    {
      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup());

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(null);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(string.Empty, processedRenderContent.Content);
    }

    [TestMethod]
    public void EmptyContentWithInjectionItems()
    {
      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(new UserControlPlaceHolder("~/controls/metatags/ieedgemetatag.ascx", null).ToMarkup());

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(string.Empty);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(string.Empty, processedRenderContent.Content);
    }

    [TestMethod]
    public void NullInjectionContent()
    {
      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(null);

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(HTML_CONTENT, processedRenderContent.Content);
    }

    [TestMethod]
    public void EmptyInjectionContent()
    {
      IContentInjectionItem headBeginContentInjectionItem = new HtmlHeadBeginContentInjectionItem(string.Empty);

      IContentInjectionItem[] contentInjectionItems = new[] { headBeginContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(HTML_CONTENT, processedRenderContent.Content);
    }

    [TestMethod]
    public void NullInjectionContentInjectionItem()
    {
      IContentInjectionItem nullInjectionContentInjectionItem = new NullInjectionContentInjectionItem();

      IContentInjectionItem[] contentInjectionItems = new[] { nullInjectionContentInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(HTML_CONTENT, processedRenderContent.Content);
    }

    [TestMethod]
    public void NullPlaceHolderRegexInjectionItem()
    {
      IContentInjectionItem nullPlaceHolderRegexInjectionItem = new NullPlaceHolderRegexInjectionItem();

      IContentInjectionItem[] contentInjectionItems = new[] { nullPlaceHolderRegexInjectionItem };

      IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);

      IRenderHandler renderHandler = new ContentInjectionRenderHandler(context);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(HTML_CONTENT, processedRenderContent.Content);
    }

    [TestMethod]
    public void NullInjectionContext()
    {
      IRenderHandler renderHandler = new ContentInjectionRenderHandler(null);

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(renderHandler);

      IRenderContent renderContent = new TestRenderContent(HTML_CONTENT);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, ProviderContainer);

      WriteOutput(processedRenderContent.Content);

      Assert.AreEqual(HTML_CONTENT, processedRenderContent.Content);
    }
  }
}
