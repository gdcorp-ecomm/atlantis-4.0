using System;
using System.Text.RegularExpressions;
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
  [DeploymentItem("testdictionary-en.language")]
  [DeploymentItem("testdictionary-es-mx.language")]
  [DeploymentItem("testdictionary-es.language")]
  [DeploymentItem("Atlantis.Framework.Language.Impl.dll")]
  public class LanguageProviderTests
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
      container.RegisterProvider<ILanguageProvider, LanguageProvider>();

      return container.Resolve<ILanguageProvider>();
    }

    [TestCleanup]
    public void Cleanup()
    {
      EngineRequestMocking.ClearOverrides();
    }

    [TestMethod]
    public void DefaultPhrase()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "en");
      string phrase = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.AreEqual("GoDaddy Green River", phrase);
    }

    [TestMethod]
    public void DefaultPhraseCached()
    {
      ILanguageProvider language = NewLanguageProvider(6, "www", "en");
      string phrase = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.AreEqual("Green River", phrase);

      string phrase2 = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.ReferenceEquals(phrase2, phrase);
    }

    [TestMethod]
    public void DefaultPhraseQANonInternal()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-qa");
      string phrase = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.AreNotEqual("[testdictionary:testkey]", phrase);
    }

    [TestMethod]
    public void DefaultPhraseQA()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-qa", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.AreEqual("[testdictionary:testkey]", phrase);
    }

    [TestMethod]
    public void DefaultPhraseQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
    }

    [TestMethod]
    public void DefaultPhraseWithHtmlQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "testkeywithhtml");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("<ul>"));
    }

    [TestMethod]
    public void DefaultPhraseWithRandomHtmlQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithrandomhtml");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("<strong>"));
    }

    [TestMethod]
    public void DefaultPhraseWithComplexHtmlQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithcomplexhtml");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("<span onmouseover"));
    }

    [TestMethod]
    public void DefaultPhraseWithTokenQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithtoken");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("[@T[companyname:name]@T]"));

    }

    [TestMethod]
    public void DefaultPhraseWithTwoTokensQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithtwotokens");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(Regex.Matches(phrase, "\\[@T[^\\s]*]@T\\]").Count == 2);
    }

    [TestMethod]
    public void DefaultPhraseWithSpecialCharQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithspecialcharacter");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("®"));
    }

    [TestMethod]
    public void DefaultPhraseWithManySpecialCharsQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithmanydifferentspecialchars");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("Ã"));
    }

    [TestMethod]
    public void DefaultPhraseWithHtmlCharsQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithhtmlchars");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("&nbsp;"));
    }

    [TestMethod]
    public void DefaultPhraseQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "testkey");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
    }

    [TestMethod]
    public void DefaultPhraseWithHtmlQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "testkeywithhtml");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("<ul>"));
    }

    [TestMethod]
    public void DefaultPhraseWithRandomHtmlQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithrandomhtml");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("<strong>"));
    }

    [TestMethod]
    public void DefaultPhraseWithComplexHtmlQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithcomplexhtml");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("<span onmouseover"));
    }

    [TestMethod]
    public void DefaultPhraseWithTokenQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithtoken");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("[@T[companyname:name]@T]"));
    }

    [TestMethod]
    public void DefaultPhraseWithTwoTokensQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithtwotokens");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(Regex.Matches(phrase, "\\[@T[^\\s]*]@T\\]").Count == 2);
    }

    [TestMethod]
    public void DefaultPhraseWithSpecialCharQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithspecialcharacter");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("®"));
    }

    [TestMethod]
    public void DefaultPhraseWithManySpecialCharsQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithmanydifferentspecialchars");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("Ã"));
    }

    [TestMethod]
    public void DefaultPhraseWithHtmlCharsQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithhtmlchars");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("&nbsp;"));
    }

    [TestMethod]
    public void DefaultPhraseWithXmlTokenQAPZ()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-pz", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithxmltypetoken");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("zzz"));
      Assert.IsTrue(phrase.Contains("[@T[productline:<Auctions contextid=\"1\" />]@T]"));
    }
    
    [TestMethod]
    public void DefaultPhraseWithXmlTokenQAPS()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "qa-ps", true);
      string phrase = language.GetLanguagePhrase("testdictionary", "textkeywithxmltypetoken");
      Assert.IsNotNull(phrase);
      Assert.IsTrue(phrase.Contains("!!!"));
      Assert.IsTrue(phrase.Contains("[@T[productline:<Auctions contextid=\"1\" />]@T]"));
    }


    [TestMethod]
    public void PhraseKeyMissing()
    {
      ILanguageProvider language = NewLanguageProvider(1, "www", "en");
      string phrase = language.GetLanguagePhrase("cds.sales/integrationtests/hosting/web-hosting", "foo");
      Assert.IsTrue(string.IsNullOrWhiteSpace(phrase));
    }

    [TestMethod]
    public void CdsFullLanguageDictionary()
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
    public void CdsShortLanguageDictionary()
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
    public void CdsDefaultLanguageDictionary()
    {
      var mockRequest = new Func<RequestData, ConfigElement, IResponseData>((data, config) =>
      {
        var requestData = (CDSLanguageRequestData)data;
        return new CDSLanguageResponseData(MockCdsPhraseDictionary.GetPhraseDictionary(requestData.Language));
      });

      EngineRequestMocking.RegisterOverride(LanguageProviderEngineRequests.CDSLanguagePhrase, mockRequest);

      ILanguageProvider language = NewLanguageProvider(1, "www", "fr-FR");
      string phraseText = language.GetLanguagePhrase("cds.tes/general/general/html", "employee");

      Assert.AreEqual("A Go Daddy employee", phraseText);

    }

    [TestMethod]
    public void CdsInvalidDictionary()
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
