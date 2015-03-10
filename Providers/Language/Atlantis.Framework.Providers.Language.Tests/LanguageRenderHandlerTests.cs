using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
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
    private IProviderContainer NewLanguageProviderContainer(int privateLabelId, string countrySite, string language, bool isInternal = false)
    {
      var container = new MockProviderContainer();
      container.SetData(MockLocalizationProviderSettings.CountrySite, countrySite);
      container.SetData(MockLocalizationProviderSettings.FullLanguage, language);
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (isInternal)
      {
        container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      }

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      container.RegisterProvider<ILanguageProvider, LanguageProvider>();
      container.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();

      return container;
    }


    [TestMethod]
    public void ValidLanaguagePhraseReplacementUk()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      
      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> {new LanguageRenderHandler()});
      
      Assert.AreEqual("<div>Thames River</div>", output);
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementWww()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>GoDaddy Green River</div>", output);
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementWwwDuplicate()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:testkey]@L]</div><div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>GoDaddy Green River</div><div>GoDaddy Green River</div>", output);
    }

    [TestMethod]
    public void MissedLanaguagePhraseReplacement()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[wrongdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div></div>", output);
    }

    [TestMethod]
    public void NoLanaguagePhraseReplacement()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>Hello</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>Hello</div>", output);
    }


    private class TestContent : IRenderContent
    {
      public TestContent(string content)
      {
        Content = content;
      }

      public string Content { get; private set; }
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementQaNotInternal()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "qa-qa");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreNotEqual("<div>[testdictionary:testkey]</div>", output);
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementQa()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "qa-qa", true);

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>[testdictionary:testkey]</div>", output);
    }

    [TestMethod]
    public void ValidLanaguagePhraseReplacementEmbeddedTokens()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:AboutLeadGeneration1]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>[@T[companyname:name]@T] does not own this domain name - you will be negotiating the potential sale directly with the domain owner.</div>", output);
    }

    [TestMethod]
    public void ValidLanguagePhraseReplacementCDS()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/integrationtests/hosting/web-hosting:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>Purple River</div>", output);
    }

    [TestMethod]
    public void ValidLanguagePhraseReplacementInShortLanguageCDS()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en-US");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/integrationtests/hosting/web-hosting:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>Purple River</div>", output);
    }

    [TestMethod]
    public void ValidLanguagePhraseReplacementNotValidLanguageCDS()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "es-mx");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/integrationtests/hosting/web-hosting:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>Purple River</div>", output);
    }

    #region Encoded Language Tests

    [TestMethod]
    public void ValidEncodedLanaguagePhraseReplacementWww()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "[@EL[testdictionary:textkeywithquotes]@EL]";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new EncodedLanguageRenderHandler() });

      Assert.AreEqual("It might sound complex, but it&#39;s really not. Click &quot;Add to Cart&quot; for more.", output);
    }

    [TestMethod]
    public void ValidEncodedAndNotEncodedLanguagePhraseReplacementWww()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<h1>no phrase</h1> <div>[@EL[testdictionary:textkeywithquotes]@EL]</div> <div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> {new LanguageRenderHandler(), new EncodedLanguageRenderHandler() });

      Assert.AreEqual("<h1>no phrase</h1> <div>It might sound complex, but it&#39;s really not. Click &quot;Add to Cart&quot; for more.</div> <div>GoDaddy Green River</div>", output);
    }

    [TestMethod]
    public void EncodedLanaguageNoPhraseTest()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en");

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<h1>no phrase</h1>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new EncodedLanguageRenderHandler() });

      Assert.AreEqual("<h1>no phrase</h1>", output);
    }

    #endregion Encoded Language Tests

    #region RenderPipelineStatusProvider tests

    [TestMethod]
    public void RenderPipelineStatus_CdsPhraseHandler_Success()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/integrationtests/hosting/web-hosting:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.Success);
      Assert.AreEqual("<div>Purple River</div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_CdsPhraseHandler_MissingKey()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/integrationtests/hosting/web-hosting:foo]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.SuccessWithErrors);
      Assert.AreEqual("<div></div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_CdsPhraseHandler_MissingDictionary()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/integrationtests/hosting/foo:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.SuccessWithErrors);
      Assert.AreEqual("<div></div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_FilePhraseHandler_Success()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.Success);
      Assert.AreEqual("<div>Thames River</div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_FilePhraseHandler_MissingKey()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[testdictionary:foo]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.Success);
      Assert.AreEqual("<div></div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_FilePhraseHandler_MissingDictionary()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[foo:testkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.Success);
      Assert.AreEqual("<div></div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_FilePhraseHandler_MissingDictionaryEL()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@EL[foo:testkey]@EL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new EncodedLanguageRenderHandler() });

      IRenderPipelineStatusProvider statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.Success);
      Assert.AreEqual("<div></div>", output);
    }

    #endregion //RenderPipelineStatusProvider tests
  }
}
