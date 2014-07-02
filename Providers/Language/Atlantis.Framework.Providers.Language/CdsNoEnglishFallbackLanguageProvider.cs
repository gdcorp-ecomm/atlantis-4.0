using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Handlers;
using Atlantis.Framework.Providers.Language.Interface;
using System;

namespace Atlantis.Framework.Providers.Language
{
  public class CdsNoEnglishFallbackLanguageProvider : ProviderBase, ILanguageProvider
  {
    private readonly Lazy<CDSPhraseHandler> _cdsPhraseHandler;

    public CdsNoEnglishFallbackLanguageProvider(IProviderContainer container)
      :base(container)
    {
      _cdsPhraseHandler = new Lazy<CDSPhraseHandler>(() => new CDSPhraseHandler(Container));
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
