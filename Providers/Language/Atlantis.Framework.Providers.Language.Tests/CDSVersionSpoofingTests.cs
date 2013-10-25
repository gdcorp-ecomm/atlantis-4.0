using Atlantis.Framework.BasePages.Providers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Providers.Language.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Language.Impl.dll")]
  public class CDSVersionSpoofingTests
  {
    private IProviderContainer NewLanguageProviderContainer(int privateLabelId, string countrySite, string language, bool isInternal = false)
    {
      var container = new MockProviderContainer();
      container.SetMockSetting(MockLocalizationProviderSettings.CountrySite, countrySite);
      container.SetMockSetting(MockLocalizationProviderSettings.FullLanguage, language);
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (isInternal)
      {
        container.SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);
      }

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      container.RegisterProvider<ILanguageProvider, LanguageProvider>();
      container.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
      container.RegisterProvider<IDebugContext, DebugProvider>();
      return container;
    }

    private void SetUrl(string url)
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void CanOneDictionaryVersionBeSpoofed()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=localization/sales/atlantistests/en|52697dcdf778fc3e88f8934e";
      SetUrl(url);

      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en", true);

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/atlantistests:sparky]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>one eyed monster</div>", output);
    }

    [TestMethod]
    public void CanMultipleDictionaryVersionsBeSpoofed()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=localization/sales/atlantistests2/en|52697d55f778fc3e88f8934d,localization/sales/atlantistests/en|52697dcdf778fc3e88f8934e";
      SetUrl(url);

      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en", true);

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/atlantistests:sparky]@L]</div><div>[@L[cds.sales/atlantistests2:shindung]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>one eyed monster</div><div>TapSomBong</div>", output);
    }

    [TestMethod]
    public void CanInvalidVersionParamStillFetchThePublishedDictionary()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=52532ac4f778fc15d8df6d32";
      SetUrl(url);

      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en", true);

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/atlantistests:cookkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      Assert.AreEqual("<div>third eye</div>", output);
    }

    [TestMethod]
    public void DoesLogDebugInfoWhenInternal()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=localization/sales/atlantistests2/en|52697d55f778fc3e88f8934d,localization/sales/atlantistests/en|52697dcdf778fc3e88f8934e";
      SetUrl(url);

      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en", true);

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/atlantistests:sparky]@L]</div><div>[@L[cds.sales/atlantistests2:shindung]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IDebugContext dc = container.Resolve<IDebugContext>();
      Assert.IsTrue((from kvp in dc.GetDebugTrackingData() where kvp.Value == "52697d55f778fc3e88f8934d" select kvp).Any());

      Assert.IsTrue((from kvp in dc.GetDebugTrackingData() where kvp.Value == "52697dcdf778fc3e88f8934e" select kvp).Any());
    }

    [TestMethod]
    public void DoesNotLogDebugInfoWhenExternal()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=52532ac4f778fc15d8df6d32";
      SetUrl(url);

      IProviderContainer container = NewLanguageProviderContainer(1, "www", "en", false);

      IRenderPipelineProvider renderPipelineProvider = container.Resolve<IRenderPipelineProvider>();

      string content = "<div>[@L[cds.sales/atlantistests:cookkey]@L]</div>";
      string output = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new LanguageRenderHandler() });

      IDebugContext dc = container.Resolve<IDebugContext>();

      Assert.IsTrue(dc.GetDebugTrackingData().Count == 0);
    }
  }
}
