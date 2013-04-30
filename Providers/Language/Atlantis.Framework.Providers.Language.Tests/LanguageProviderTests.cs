using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
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
    private ILanguageProvider NewLanguageProvider(int privateLabelId, string countrySite, string language)
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

      return container.Resolve<ILanguageProvider>();
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

  }
}
