using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class PhrasePredicate
  {
    const string _DEFAULTCOUNTRYSITE = "www";
    const string _DEFAULTLANGUAGE = "en";
    const int _DEFAULTCONTEXT = 0;

    internal static string BuildKey(int contextId, string countrySite, string language)
    {
      return string.Concat(contextId.ToString(), "|", countrySite, "|", language);
    }

    List<string> _phraseKeys;

    internal PhrasePredicate(int contextId, string countrySite, string language)
    {
      if (string.IsNullOrEmpty(countrySite))
      {
        countrySite = _DEFAULTCOUNTRYSITE;
      }

      if (string.IsNullOrEmpty(language))
      {
        language = _DEFAULTLANGUAGE;
      }

      _phraseKeys = new List<string>(4);
      _phraseKeys.Add(BuildKey(contextId, countrySite, language));

      if (_DEFAULTCONTEXT != contextId)
      {
        _phraseKeys.Add(BuildKey(_DEFAULTCONTEXT, countrySite, language));
      }

      string shortLangugage = null;
      int dashPosition = language.IndexOf('-');
      if (dashPosition > 1)
      {
        shortLangugage = language.Substring(0, dashPosition);
        _phraseKeys.Add(BuildKey(contextId, countrySite, shortLangugage));

        if (_DEFAULTCONTEXT != contextId)
        {
          _phraseKeys.Add(BuildKey(_DEFAULTCONTEXT, countrySite, shortLangugage));
        }
      }

      if (!_DEFAULTCOUNTRYSITE.Equals(countrySite, StringComparison.OrdinalIgnoreCase))
      {
        _phraseKeys.Add(BuildKey(contextId, _DEFAULTCOUNTRYSITE, language));

        if (_DEFAULTCONTEXT != contextId)
        {
          _phraseKeys.Add(BuildKey(_DEFAULTCONTEXT, _DEFAULTCOUNTRYSITE, language));
        }

        if (shortLangugage != null)
        {
          _phraseKeys.Add(BuildKey(contextId, _DEFAULTCOUNTRYSITE, shortLangugage));
          if (_DEFAULTCONTEXT != contextId)
          {
            _phraseKeys.Add(BuildKey(_DEFAULTCONTEXT, _DEFAULTCOUNTRYSITE, shortLangugage));
          }
        }

      }

      // degrade language - show english
      if (!language.StartsWith(_DEFAULTLANGUAGE, StringComparison.OrdinalIgnoreCase))
      {
        _phraseKeys.Add(BuildKey(contextId, countrySite, _DEFAULTLANGUAGE));

        if (_DEFAULTCONTEXT != contextId)
        {
          _phraseKeys.Add(BuildKey(_DEFAULTCONTEXT, countrySite, _DEFAULTLANGUAGE));
        }

        // last chance, WWW English
        if (!_DEFAULTCOUNTRYSITE.Equals(countrySite, StringComparison.OrdinalIgnoreCase))
        {
          _phraseKeys.Add(BuildKey(contextId, _DEFAULTCOUNTRYSITE, _DEFAULTLANGUAGE));
          if (_DEFAULTCONTEXT != contextId)
          {
            _phraseKeys.Add(BuildKey(_DEFAULTCONTEXT, _DEFAULTCOUNTRYSITE, _DEFAULTLANGUAGE));
          }
        }
      }
    }

    internal IEnumerable<string> PhraseKeys
    {
      get { return _phraseKeys; }
    }
  }
}
