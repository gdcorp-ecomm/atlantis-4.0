using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Handlers;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;

namespace Atlantis.Framework.Providers.Language
{
  public class LanguageProvider : ProviderBase, ILanguageProvider
  {
    const string _QALANGUAGESHOW = "qa-qa";
    const string _QAPSLANGUAGESHOW = "qa-ps";
    const string _QAPZLANGUAGESHOW = "qa-pz";

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<QAPhraseHandler> _qaPhraseHandler;
    private readonly Lazy<QaPsPhraseHandler> _qaPSPhraseHandler;
    private readonly Lazy<QaPzPhraseHandler> _qaPZPhraseHandler;

    private readonly Lazy<CDSPhraseHandler> _cdsPhraseHandler;
    private readonly Lazy<FilePhraseHandler> _filePhraseHandler; 

    public LanguageProvider(IProviderContainer container)
      :base(container)
    {
      _qaPhraseHandler = new Lazy<QAPhraseHandler>(() => new QAPhraseHandler());
      _qaPSPhraseHandler = new Lazy<QaPsPhraseHandler>(() => new QaPsPhraseHandler(Container));
      _qaPZPhraseHandler = new Lazy<QaPzPhraseHandler>(() => new QaPzPhraseHandler(Container));
      _cdsPhraseHandler = new Lazy<CDSPhraseHandler>(() => new CDSPhraseHandler(Container));
      _filePhraseHandler = new Lazy<FilePhraseHandler>(() => new FilePhraseHandler(Container));
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
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
        retVal = handler.TryGetLanguagePhrase(dictionaryName, phraseKey, out phrase);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("LanguageProvider.GetLanguagePhrase", 0, ex.Message + ex.StackTrace, phraseKey);
        Engine.Engine.LogAtlantisException(exception);
      }
      return retVal;
    }

    private ILanguagePhraseHandler GetLanguagePhraseHandler(string dictionaryName)
    {
      if (ShowDictionaryAndKeyRaw)
      {
        return _qaPhraseHandler.Value;
      }
      if (ShowQAPSPhrase)
      {
        return _qaPSPhraseHandler.Value;
      }
      if (ShowQAPZPhrase)
      {
        return _qaPZPhraseHandler.Value;
      }
      if (dictionaryName.StartsWith("cds.", StringComparison.OrdinalIgnoreCase))
      {
        return _cdsPhraseHandler.Value;
      }

      return _filePhraseHandler.Value;
    }

    private bool? _showDictionaryAndKeyRaw;
    private bool ShowDictionaryAndKeyRaw
    {
      get
      {
        if (!_showDictionaryAndKeyRaw.HasValue)
        {
          _showDictionaryAndKeyRaw = false;
          ILocalizationProvider localization;

          if ((_siteContext.Value.IsRequestInternal) && (Container.TryResolve(out localization)))
          {
            _showDictionaryAndKeyRaw = localization.IsActiveLanguage(_QALANGUAGESHOW);
          }
        }
        return _showDictionaryAndKeyRaw.Value;
      }
    }

    private bool? _showQA_PSPhrase;
    private bool ShowQAPSPhrase
    {
      get
      {
        if (!_showQA_PSPhrase.HasValue)
        {
          _showQA_PSPhrase = false;
          ILocalizationProvider localization;

          if ((_siteContext.Value.IsRequestInternal) && (Container.TryResolve(out localization)))
          {
            _showQA_PSPhrase = localization.IsActiveLanguage(_QAPSLANGUAGESHOW);
          }
        }
        return _showQA_PSPhrase.Value;
      }
    }

    private bool? _showQA_PZPhrase;
    private bool ShowQAPZPhrase
    {
      get
      {
        if (!_showQA_PZPhrase.HasValue)
        {
          _showQA_PZPhrase = false;
          ILocalizationProvider localization;

          if ((_siteContext.Value.IsRequestInternal) && (Container.TryResolve(out localization)))
          {
            _showQA_PZPhrase = localization.IsActiveLanguage(_QAPZLANGUAGESHOW);
          }
        }
        return _showQA_PZPhrase.Value;
      }
    }
  }
}
