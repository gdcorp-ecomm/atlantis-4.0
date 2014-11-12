using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Handlers;
using Atlantis.Framework.Providers.Language.Interface;
using System;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Language
{
  public class CdsNoEnglishFallbackLanguageProvider : ProviderBase, ILanguageProvider
  {
    private readonly Lazy<CDSPhraseHandler> _cdsPhraseHandler;

    private readonly Lazy<ILocalizationProvider> _localization;

    public CdsNoEnglishFallbackLanguageProvider(IProviderContainer container)
      :base(container)
    {
      _cdsPhraseHandler = new Lazy<CDSPhraseHandler>(() => new CDSPhraseHandler(Container, FullLanguage, ShortLanguage));
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);
    }

    protected virtual string FullLanguage
    {
        get { return _localization.Value.FullLanguage; }
    }

    protected virtual string ShortLanguage
    {
        get { return _localization.Value.ShortLanguage; }
    }

    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      string phrase;

      if (TryGetLanguagePhrase(dictionaryName, phraseKey, out phrase))
      {
        return phrase;
      }

      return string.Empty;
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      bool retVal = false;
      phrase = string.Empty;
      try
      {
        ILanguagePhraseHandler handler = GetLanguagePhraseHandler(dictionaryName);
        retVal = handler.TryGetLanguagePhrase(dictionaryName, phraseKey, false, out phrase);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("CdsNoEnglishFallbackLanguageProvider.GetLanguagePhrase", 0, ex.Message + ex.StackTrace, phraseKey);
        Engine.Engine.LogAtlantisException(exception);
      }
      return retVal;
    }

    private ILanguagePhraseHandler GetLanguagePhraseHandler(string dictionaryName)
    {
      if (dictionaryName.StartsWith("cds.", StringComparison.OrdinalIgnoreCase))
      {
        return _cdsPhraseHandler.Value;
      }

      throw new Exception("Invalid use of CdsNoEnglishFallbackLanguageProvider");
    }
  }
}
