using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class FilePhraseHandler : ILanguagePhraseHandler
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILocalizationProvider> _localization;

    private readonly string _fullLanguage;

    public FilePhraseHandler(IProviderContainer container, string fullLanguage)
    {
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);

      _fullLanguage = fullLanguage;
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dictionaryName, phraseKey, _fullLanguage, _localization.Value.CountrySite, _siteContext.Value.ContextId);
      var response = (LanguagePhraseResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.FileLanguagePhrase);

      phrase = response.LanguagePhrase;

      return true;
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, bool doGlobalFallback, out string phrase)
    {
      throw new NotSupportedException("The File Phrase Handler does not support this overload.");
    }
  }
}
