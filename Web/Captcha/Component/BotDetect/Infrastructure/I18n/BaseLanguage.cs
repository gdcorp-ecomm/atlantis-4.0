using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace BotDetect
{
    internal enum BaseLanguage
    {
        Unknown = 0,
        Basque,
        Belarusian,
        Bosnian,
        Bulgarian,
        Cantonese,
        Catalan,
        Croatian,
        Czech,
        Danish,
        Dutch,
        English,
        Faroese,
        Finnish,
        French,
        German,
        Greek,
        Greenlandic,
        Hebrew,
        Hindi,
        Hungarian,
        Icelandic,
        Italian,
        Japanese,
        Korean,
        Lithuanian,
        Luxembourgish,
        Macedonian,
        Malay,
        Maltese,
        Mandarin,
        Polish,
        Portuguese,
        Romanian,
        Romansh,
        Russian,
        Serbian,
        Slovak,
        Slovenian,
        Spanish,
        Swedish,
        Turkish,
        Ukrainian,
        Vietnamese
    }

    internal sealed class LanguageCodes
    {
        private LanguageCodes() { }

        public const int SubLanguageCodeLength = 3;

        public static string GetCode(BaseLanguage language)
        {
            string code = "";

            switch (language)
            {
                case BaseLanguage.Basque:
                    code = "eu";
                    break;
                
                case BaseLanguage.Belarusian:
                    code = "be";
                    break;

                case BaseLanguage.Bosnian:
                    code = "bs";
                    break;

                case BaseLanguage.Bulgarian:
                    code = "bg";
                    break;

                case BaseLanguage.Cantonese:
                    code = "yue";
                    break;

                case BaseLanguage.Catalan:
                    code = "ca";
                    break;

                case BaseLanguage.Croatian:
                    code = "hr";
                    break;

                case BaseLanguage.Czech:
                    code = "cs";
                    break;

                case BaseLanguage.Danish:
                    code = "da";
                    break;
                
                case BaseLanguage.Dutch:
                    code = "nl";
                    break;

                case BaseLanguage.English:
                    code = "en";
                    break;
                
                case BaseLanguage.Faroese:
                    code = "fo";
                    break;

                case BaseLanguage.Finnish:
                    code = "fi";
                    break;
                
                case BaseLanguage.French:
                    code = "fr";
                    break;

                case BaseLanguage.German:
                    code = "de";
                    break;

                case BaseLanguage.Greek:
                    code = "el";
                    break;
                
                case BaseLanguage.Greenlandic:
                    code = "kl";
                    break;

                case BaseLanguage.Hebrew:
                    code = "he";
                    break;

                case BaseLanguage.Hindi:
                    code = "hi";
                    break;

                case BaseLanguage.Hungarian:
                    code = "hu";
                    break;
                
                case BaseLanguage.Icelandic:
                    code = "is";
                    break;

                case BaseLanguage.Italian:
                    code = "it";
                    break;
                
                case BaseLanguage.Japanese:
                    code = "ja";
                    break;

                case BaseLanguage.Korean:
                    code = "ko";
                    break;
                
                case BaseLanguage.Lithuanian:
                    code = "lt";
                    break;

                case BaseLanguage.Luxembourgish:
                    code = "lb";
                    break;

                case BaseLanguage.Macedonian:
                    code = "mk";
                    break;

                case BaseLanguage.Malay:
                    code = "zlm";
                    break;

                case BaseLanguage.Mandarin:
                    code = "cmn";
                    break;
                
                case BaseLanguage.Maltese:
                    code = "mt";
                    break;
                
                case BaseLanguage.Polish:
                    code = "pl";
                    break;

                case BaseLanguage.Portuguese:
                    code = "pt";
                    break;

                case BaseLanguage.Romanian:
                    code = "ro";
                    break;
                
                case BaseLanguage.Romansh:
                    code = "rm";
                    break;

                case BaseLanguage.Russian:
                    code = "ru";
                    break;

                case BaseLanguage.Serbian:
                    code = "sr";
                    break;
                
                case BaseLanguage.Slovak:
                    code = "sk";
                    break;

                case BaseLanguage.Slovenian:
                    code = "sl";
                    break;

                case BaseLanguage.Spanish:
                    code = "es";
                    break;
                
                case BaseLanguage.Swedish:
                    code = "sv";
                    break;

                case BaseLanguage.Turkish:
                    code = "tr";
                    break;

                case BaseLanguage.Ukrainian:
                    code = "uk";
                    break;
                
                case BaseLanguage.Vietnamese:
                    code = "vi";
                    break;

                default:
                    Debug.Assert(false, "Unkown base language");
                    code = language.ToString();
                    break;
            }

            return code;
        }

        public static BaseLanguage GetLanguage(string code)
        {
            BaseLanguage language = BaseLanguage.Unknown;

            switch (code.ToLowerInvariant())
            {
                case "eu":
                case "eus":
                case "baq":
                    language = BaseLanguage.Basque;
                    break;

                case "be":
                case "bel":
                    language = BaseLanguage.Belarusian;
                    break;

                case "bs":
                case "bos":
                    language = BaseLanguage.Bosnian;
                    break;

                case "bg":
                case "bul":
                    language = BaseLanguage.Bulgarian;
                    break;
             
                case "yue":
                    language = BaseLanguage.Cantonese;
                    break;

                case "ca":
                case "cat":
                    language = BaseLanguage.Catalan;
                    break;

                case "hr":
                case "hrv":
                    language = BaseLanguage.Croatian;
                    break;

                case "cs":
                case "ces":
                case "cze":
                    language = BaseLanguage.Czech;
                    break;

                case "da":
                case "dan":
                    language = BaseLanguage.Danish;
                    break;

                case "nl":
                case "nld":
                case "dut":
                    language = BaseLanguage.Dutch;
                    break;

                case "en":
                case "eng":
                    language = BaseLanguage.English;
                    break;

                case "fo":
                case "fao":
                    language = BaseLanguage.Faroese;
                    break;

                case "fi":
                case "fin":
                    language = BaseLanguage.Finnish;
                    break;

                case "fr":
                case "fra":
                case "fre":
                    language = BaseLanguage.French;
                    break;

                case "de":
                case "deu":
                case "ger":
                    language = BaseLanguage.German;
                    break;

                case "el":
                case "ell":
                case "gre":
                    language = BaseLanguage.Greek;
                    break;

                case "kl":
                case "kal":
                    language = BaseLanguage.Greenlandic;
                    break;

                case "he":
                case "heb":
                    language = BaseLanguage.Hebrew;
                    break;

                case "hi":
                case "hin":
                    language = BaseLanguage.Hindi;
                    break;

                case "hu":
                case "hun":
                    language = BaseLanguage.Hungarian;
                    break;

                case "is":
                case "isl":
                case "ice":
                    language = BaseLanguage.Icelandic;
                    break;

                case "it":
                case "ita":
                    language = BaseLanguage.Italian;
                    break;

                case "ja":
                case "jp": // wrong, but kept for backwards compatibility
                case "jpn":
                    language = BaseLanguage.Japanese;
                    break;

                case "ko":
                case "kor":
                    language = BaseLanguage.Korean;
                    break;

                case "lt":
                case "lit":
                    language = BaseLanguage.Lithuanian;
                    break;

                case "lb":
                case "ltz":
                    language = BaseLanguage.Luxembourgish;
                    break;

                case "mk":
                case "mkd":
                case "mac":
                    language = BaseLanguage.Macedonian;
                    break;

                case "zlm":
                    language = BaseLanguage.Malay;
                    break;

                case "mt":
                case "mlt":
                    language = BaseLanguage.Maltese;
                    break;

                case "cmn":
                    language = BaseLanguage.Mandarin;
                    break;

                case "pl":
                case "pol":
                    language = BaseLanguage.Polish;
                    break;

                case "pt":
                case "por":
                    language = BaseLanguage.Portuguese;
                    break;

                case "ro":
                case "ron":
                case "rum":
                    language = BaseLanguage.Romanian;
                    break;

                case "rm":
                case "roh":
                    language = BaseLanguage.Romansh;
                    break;

                case "ru":
                case "rus":
                    language = BaseLanguage.Russian;
                    break;

                case "sr":
                case "srp":
                    language = BaseLanguage.Serbian;
                    break;

                case "sk":
                case "slk":
                case "slo":
                    language = BaseLanguage.Slovak;
                    break;

                case "sl":
                case "slv":
                    language = BaseLanguage.Slovenian;
                    break;

                case "es":
                case "spa":
                    language = BaseLanguage.Spanish;
                    break;

                case "sv":
                case "swe":
                    language = BaseLanguage.Swedish;
                    break;

                case "tr":
                case "tur":
                    language = BaseLanguage.Turkish;
                    break;

                case "uk":
                case "ukr":
                    language = BaseLanguage.Ukrainian;
                    break;

                case "vi":
                case "vie":
                    language = BaseLanguage.Vietnamese;
                    break;

                default:
                    Debug.Assert(false, "Unkown base language code");
                    break;
            }

            return language;
        }

    }
}
