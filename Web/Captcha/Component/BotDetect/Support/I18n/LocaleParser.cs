using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
    internal sealed class LocaleParser
    {
        private LocaleParser() { }

        // lazy Locale flyweight
        private static Dictionary<string, Localization> _initializedLocalizations =
            new Dictionary<string, Localization>();

        public static Localization Parse(string locale)
        {
            locale = PreProcess(locale);

            // cache lookup
            if (_initializedLocalizations.ContainsKey(locale))
            {
                return _initializedLocalizations[locale];
            }

            // Locale object contruction
            Localization localization = InterpretLocale(locale);

            // keep a static cache by input value, with different equivalent
            // inputs (e.g. "en", "en-Latn", "en-US") all mapping to the same
            // Localization object (with the normalized code "en-Latn-US") so we
            // don't construct & keep unnecessary Localization object instances
            string canonicalLocale = localization.ToString();
            if (_initializedLocalizations.ContainsKey(canonicalLocale))
            {
                localization = _initializedLocalizations[canonicalLocale];
            }
            _initializedLocalizations[locale] = localization;

            return localization;
        }

        private static string PreProcess(string locale)
        {
            if (!StringHelper.HasValue(locale))
            {
                return "en";
            }

            locale = locale.ToLowerInvariant();

            // deprecated values, but some customers might still use them
            locale = locale.Replace("zh-chs", "zh-hans");
            locale = locale.Replace("zh-cht", "zh-hant");

            return locale;
        }

        private static Localization InterpretLocale(string locale)
        {
            // up to four variables are used in locale strings
            Macrolanguage macrolanguage = Macrolanguage.None;
            BaseLanguage language = BaseLanguage.Unknown;
            BaseCharset charset = BaseCharset.Unknown;
            Country country = Country.Unknown;

            // dash-separated
            string[] parts = locale.Split('-');

            // the first part is either a macrolanguage prefix ...
            string languageCode = parts[0];
            macrolanguage = MacrolanguageCodes.GetMacrolanguage(languageCode);
            if (Macrolanguage.None == macrolanguage)
            {
                // ... or a language code
                language = LanguageCodes.GetLanguage(languageCode);
            }

            // all further parts are optional, with distinguishing lengths
            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];
                int length = part.Length;

                if (CharsetCodes.CharsetCodeLength == length)
                {
                    // parse charset
                    charset = CharsetCodes.GetCharset(part);
                }
                else if (LanguageCodes.SubLanguageCodeLength == length)
                {
                    // parse sub-language
                    language = LanguageCodes.GetLanguage(part);
                }
                else if (CountryCodes.CountryCodeLength == length)
                {
                    // parse country
                    country = CountryCodes.GetCountry(part);
                }
            }

            // mandatory locale validation & expansion
            LocaleRow bestMatch = SupportedLocales.Mapping.FindBestMatch(
                macrolanguage, language, charset, country);

            // Localization object
            Localization localization = new Localization(bestMatch);
            return localization;
        }
    }
}
