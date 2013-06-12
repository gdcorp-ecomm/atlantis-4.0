using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Language.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("testdictionary-en.language")]
  [DeploymentItem("testdictionary-es-mx.language")]
  [DeploymentItem("testdictionary-es.language")]
  [DeploymentItem("Atlantis.Framework.Language.Impl.dll")]
  public class LanguageRenderHandlerTests
  {
    private IProviderContainer NewLanguageProviderContainer(int privateLabelId, string countrySite, string language)
    {
      var container = new MockProviderContainer();
      container.SetMockSetting(MockLocalizationProviderSettings.CountrySite, countrySite);
      container.SetMockSetting(MockLocalizationProviderSettings.FullLanguage, language);
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      container.RegisterProvider<ILanguageProvider, LanguageProvider>();

      return container;
    }


    [TestMethod]
    public void ValidLanaguagePhraseReplacementUk()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      TestContent content = new TestContent("<div>[@L[testdictionary:testkey]@L]</div>");

      var pipeline = new RenderPipelineManager();
      pipeline.AddRenderHandler(new LanguageRenderHandler());
      IProcessedRenderContent processedContent = pipeline.RenderContent(content, container);

      Assert.AreEqual("<div>Thames River</div>", processedContent.Content);
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementWww()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");
      TestContent content = new TestContent("<div>[@L[testdictionary:testkey]@L]</div>");

      var pipeline = new RenderPipelineManager();
      pipeline.AddRenderHandler(new LanguageRenderHandler());
      IProcessedRenderContent processedContent = pipeline.RenderContent(content, container);

      Assert.AreEqual("<div>GoDaddy Green River</div>", processedContent.Content);
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementWwwDuplicate()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");
      TestContent content = new TestContent("<div>[@L[testdictionary:testkey]@L]</div><div>[@L[testdictionary:testkey]@L]</div>");

      var pipeline = new RenderPipelineManager();
      pipeline.AddRenderHandler(new LanguageRenderHandler());
      IProcessedRenderContent processedContent = pipeline.RenderContent(content, container);

      Assert.AreEqual("<div>GoDaddy Green River</div><div>GoDaddy Green River</div>", processedContent.Content);
    }

    [TestMethod]
    public void MissedLanaguagePhraseReplacement()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");
      TestContent content = new TestContent("<div>[@L[wrongdictionary:testkey]@L]</div>");

      var pipeline = new RenderPipelineManager();
      pipeline.AddRenderHandler(new LanguageRenderHandler());
      IProcessedRenderContent processedContent = pipeline.RenderContent(content, container);

      Assert.AreEqual("<div></div>", processedContent.Content);
    }

    [TestMethod]
    public void NoLanaguagePhraseReplacement()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");
      TestContent content = new TestContent("<div>Hello</div>");

      var pipeline = new RenderPipelineManager();
      pipeline.AddRenderHandler(new LanguageRenderHandler());
      IProcessedRenderContent processedContent = pipeline.RenderContent(content, container);

      Assert.AreEqual("<div>Hello</div>", processedContent.Content);
    }


    private class TestContent : IRenderContent
    {
      public TestContent(string content)
      {
        Content = content;
      }

      public string Content { get; private set; }
    }

  }
}
