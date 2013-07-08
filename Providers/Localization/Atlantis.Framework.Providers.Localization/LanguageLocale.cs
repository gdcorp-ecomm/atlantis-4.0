using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Localization
{
  internal class LanguageLocale
  {
    static Dictionary<string, string> _countrySiteToLocaleMapping;

    static LanguageLocale()
    {
      _countrySiteToLocaleMapping = new Dictionary<string, string>(2, StringComparer.OrdinalIgnoreCase);
      _countrySiteToLocaleMapping["www"] = "us";
      _countrySiteToLocaleMapping["uk"] = "gb";
    }

    internal static LanguageLocale FromLanguageAndCountrySite(string language, string countrySite)
    {
      string locale = countrySite;
      if (_countrySiteToLocaleMapping.ContainsKey(countrySite))
      {
        locale = _countrySiteToLocaleMapping[countrySite];
      }

      return new LanguageLocale(language, locale);
    }

    private string _language;
    private string _locale;
    private string _fullLanguage;
    
    private LanguageLocale(string language, string locale)
    {
      _language = ValidateLanguage(language);
      _locale = ValidateLocale(locale);
      
      DetermineFullLanguage();
    }

    private string ValidateLanguage(string language)
    {
      string result = "en";
      if (!string.IsNullOrEmpty(language))
      {
        int dashPosition = language.IndexOf('-');
        if (dashPosition > 1)
        {
          result = language.Substring(0, dashPosition);
        }
        else
        {
          result = language;
        }
      }

      return result;
    }

    private string ValidateLocale(string locale)
    {
      string result = string.Empty;

      if (!string.IsNullOrEmpty(locale))
      {
        result = locale;
      }

      return result;
    }

    private void DetermineFullLanguage()
    {
      if (!string.IsNullOrEmpty(_locale))
      {
        _fullLanguage = string.Concat(_language, "-", _locale);
      }
      else
      {
        _fullLanguage = _language;
      }
    }

    public string FullLanguage
    {
      get { return _fullLanguage; }
    }

    public string ShortLanguage
    {
      get { return _language; }
    }
  }
}
