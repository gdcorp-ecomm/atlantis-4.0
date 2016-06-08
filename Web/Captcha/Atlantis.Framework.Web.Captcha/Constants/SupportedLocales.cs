using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Web.CaptchaCtl.Constants
{
  public static class SupportedLocales
  {
    private const string _defaultLocale = "en-US";
    private const string _taiwan = "zh-TW";
    private const string _hongKong = "zh-HK";
    private const string _singapore = "zh-SG";

    public static string GetLocaleOrReturnDefaultIfNotSupported(string localeForCaptcha)
    {
      string returnLocale = _defaultLocale;

      if (!string.IsNullOrEmpty(localeForCaptcha))
      {
        string subCulture = localeForCaptcha.Split('-')[0];
        bool isChina = subCulture.Equals("zh", StringComparison.OrdinalIgnoreCase);

        if (!isChina)
        {
          if (_supportedLocales.Contains(localeForCaptcha, StringComparer.OrdinalIgnoreCase))
          {
            returnLocale = localeForCaptcha;
          }
          else if (_supportedLocales.Contains(subCulture, StringComparer.OrdinalIgnoreCase))
          {
            returnLocale = subCulture;
          }
        }
        else
        {
          //we need to do some conversions for china because the locales used in captcha are their own made up locales.
          if (localeForCaptcha.Equals(_taiwan, StringComparison.OrdinalIgnoreCase))
          {
            returnLocale = "zh-cmn-Hant-TW";
          }
          else if (localeForCaptcha.Equals(_hongKong, StringComparison.OrdinalIgnoreCase))
          {
            returnLocale = "zh-yue-HK";
          }
          else
          {
            returnLocale = "zh-cmn-CN";
          }
        }
      }

      return returnLocale;
    }

    // EP-10631 - Japanese wants ASCII, removing "ja"
    private static HashSet<string> _supportedLocales = new HashSet<string> {
      "en-US",
      "en-CA",
      "en-GB",
      "en-IN",
      "en-AU",
      "es",
      "es-MX",
      "es-AR",
      "es-CL",
      "pt-BR",
      "fr",
      "fr-CA",
      "bg",
      "cs",
      "da",
      "el",
      "de",
      "et",
      "fi",
      "hr",
      "hu",
      "is",
      "it",
      "lt",
      "lv",
      "nl",
      "no",
      "pl",
      "pt",
      "ro",
      "sl",
      "sr",
      "sv",
      "uk",
      "ar-AE",
      "he",
      "ko",
      "ru",
      "tr",
      "vi",
      "zh-cmn-CN",
      "zh-cmn-Hant-TW",
      "zh-yue-HK"};
  }
}
