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

    public FilePhraseHandler(IProviderContainer container)
    {
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);
    }

    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dictionaryName, phraseKey, _localization.Value.FullLanguage, _localization.Value.CountrySite, _siteContext.Value.ContextId);
      var response = (LanguagePhraseResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.FileLanguagePhrase);
      return response.LanguagePhrase;
    }
  }
}
