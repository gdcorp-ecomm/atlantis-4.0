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
  public class QuoteLanguageRenderHandlerTests
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
    public void QuoteLanguageInvalidProvider()
    {
      var container = NewLanguageProviderContainer(1, "www", "en");

      var renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<div>[@L[cds.sales/integrationtests/hosting/web-hosting:testkey]@QL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new QuoteLanguageRenderHandler() });

      Assert.AreNotEqual("<div>Purple River</div>", output);
    }


    [TestMethod]
    public void QuoteLanguagePhraseReplacementCDS()
    {
      var container = NewLanguageProviderContainer(1, "www", "en");

      var renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<div>[@QL[cds.sales/integrationtests/hosting/web-hosting:testkey]@QL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new QuoteLanguageRenderHandler() });

      Assert.AreEqual("<div>Purple River</div>", output);
    }

    [TestMethod]
    public void QuoteLanguagePhraseReplacementSingleQuote()
    {
      var container = NewLanguageProviderContainer(1, "www", "es");

      var renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<div>[@QL[testdictionary:WhatIsAuctionDescription1]@QL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new QuoteLanguageRenderHandler() });

      Assert.AreEqual(@"<div>Dawg ipsum dolizzle pimpin' mammasay mammasa mamma oo sa, we gonna chung adipiscing elit.</div>", output);
    }

    [TestMethod]
    public void QuoteLanguagePhraseReplacementDoubleQuote()
    {
      var container = NewLanguageProviderContainer(1, "www", "es");

      var renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<div>[@QL[testdictionary:AboutLeadGeneration3]@QL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new QuoteLanguageRenderHandler() });

      Assert.AreEqual("<div>Para indicar que estás interesado en comprar este nombre de dominio, simplemente haz clic en \\\"Obtener este dominio\\\" y completa el formulario.</div>", output);
    }

    [TestMethod]
    public void QuoteLanguagePhraseReplacementDoubleQuote2()
    {
      var container = NewLanguageProviderContainer(1, "www", "en");

      var renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<div id=\"divId\">[@QL[testdictionary:PreRegPricingDetailToolTipText]@QL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new QuoteLanguageRenderHandler() });

      Assert.AreEqual("<div id=\"divId\">Anyone may pre-register a <span data-bind=\\\"html: '.' + $parent.TLD()\\\">&nbsp;</span> domain during this phase. Multiple applications for the same domain will go to auction. If you are not awarded the domain, you will receive a full refund.</div>", output);
    }

    [TestMethod]
    public void RenderPipelineStatus_FilePhraseHandler_MissingDictionary()
    {
      IProviderContainer container = NewLanguageProviderContainer(1, "uk", "en");
      container.RegisterProvider<IRenderPipelineStatusProvider, RenderPipelineStatusProvider>();

      var renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      const string content = "<div>[@QL[foo:testkey]@QL]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new QuoteLanguageRenderHandler() });

      var statusProvider = container.Resolve<IRenderPipelineStatusProvider>();

      Assert.IsNotNull(statusProvider);
      Assert.AreEqual(statusProvider.Status, RenderPipelineResult.Success);
      Assert.AreEqual("<div></div>", output);
    }
  }
}
