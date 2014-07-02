using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Parsers.LanguageFile;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Language.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Language.Impl.dll")]
  public class CdsNoEnglishFallbackLanguageProviderTests
  {
    private ILanguageProvider NewLanguageProvider(int privateLabelId, string countrySite, string language, bool isInternal = false)
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
      container.RegisterProvider<ILanguageProvider, CdsNoEnglishFallbackLanguageProvider>();

      return container.Resolve<ILanguageProvider>();
    }

    [TestCleanup]
    public void Cleanup()
    {
      EngineRequestMocking.ClearOverrides();
    }

    [TestMethod]
    public void FullLanguageDictionary()
    {
      var mockRequest = new Func<RequestData, ConfigElement, IResponseData>((data, config) =>
      {
        var requestData = (CDSLanguageRequestData)data;
        return new CDSLanguageResponseData(MockCdsPhraseDictionary.GetPhraseDictionary(requestData.Language));
      });

      EngineRequestMocking.RegisterOverride(LanguageProviderEngineRequests.CDSLanguagePhrase, mockRequest);

      ILanguageProvider language = NewLanguageProvider(1, "www", "fr-FR");
      string phraseText = language.GetLanguagePhrase("cds.tes/general/general/html", "desk");

      Assert.AreEqual("Un bureau fabriqué en France", phraseText);
    }

    [TestMethod]
    public void ShortLanguageDictionary()
    {
      var mockRequest = new Func<RequestData, ConfigElement, IResponseData>((data, config) =>
      {
        var requestData = (CDSLanguageRequestData)data;
        return new CDSLanguageResponseData(MockCdsPhraseDictionary.GetPhraseDictionary(requestData.Language));
      });

      EngineRequestMocking.RegisterOverride(LanguageProviderEngineRequests.CDSLanguagePhrase, mockRequest);

      ILanguageProvider language = NewLanguageProvider(1, "www", "fr-FR");
      string phraseText = language.GetLanguagePhrase("cds.tes/general/general/html", "chair");

      Assert.AreEqual("Une chaise assez bleu", phraseText);
    }

    [TestMethod]
    public void DefaultLanguageDictionary()
    {
      var mockRequest = new Func<RequestData, ConfigElement, IResponseData>((data, config) =>
      {
        var requestData = (CDSLanguageRequestData)data;
        return new CDSLanguageResponseData(MockCdsPhraseDictionary.GetPhraseDictionary(requestData.Language));
      });

      EngineRequestMocking.RegisterOverride(LanguageProviderEngineRequests.CDSLanguagePhrase, mockRequest);

      ILanguageProvider language = NewLanguageProvider(1, "www", "fr-FR");
      string phraseText = language.GetLanguagePhrase("cds.tes/general/general/html", "employee");

      Assert.IsTrue(string.IsNullOrEmpty(phraseText));
    }

    [TestMethod]
    public void PhraseKeyMissing()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "en");
      string phrase = language.GetLanguagePhrase("cds.sales/integrationtests/hosting/web-hosting", "foo");
      Assert.IsTrue(string.IsNullOrEmpty(phrase));
    }

    [TestMethod]
    public void InvalidDictionary()
    {
      var mockRequest = new Func<RequestData, ConfigElement, IResponseData>((data, config) =>
      {
        var requestData = (CDSLanguageRequestData)data;
        return new CDSLanguageResponseData(MockCdsPhraseDictionary.GetPhraseDictionary(requestData.Language));
      });

      EngineRequestMocking.RegisterOverride(LanguageProviderEngineRequests.CDSLanguagePhrase, mockRequest);

      ILanguageProvider language = NewLanguageProvider(1, "www", "fr-FR");
      string phraseText = language.GetLanguagePhrase("tes/general/general/html", "employee");

      Assert.IsTrue(string.IsNullOrEmpty(phraseText));
    }
  }
}
