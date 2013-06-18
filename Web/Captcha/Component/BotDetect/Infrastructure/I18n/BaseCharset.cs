using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace BotDetect
{
    internal enum BaseCharset
    {
        Unknown = 0,
        Arabic,
        Bopomofo,
        Cyrillic,
        Devanagari,
        Greek,
        Hangul,
        HanSimplified,
        HanTraditional,
        Hebrew,
        Hiragana,
        Katakana,
        Latin
    }

    internal sealed class CharsetCodes
    {
        private CharsetCodes() { }

        public const int CharsetCodeLength = 4;

        public static string GetCode(BaseCharset charset)
        {
            string code = "";

            switch (charset)
            {
                case BaseCharset.Arabic:
                    code = "Arab";
                    break;

                case BaseCharset.Bopomofo:
                    code = "Bopo";
                    break;

                case BaseCharset.Cyrillic:
                    code = "Cyrl";
                    break;

                case BaseCharset.Devanagari:
                    code = "Deva";
                    break;

                case BaseCharset.Greek:
                    code = "Grek";
                    break;

                case BaseCharset.Hangul:
                    code = "Hang";
                    break;

                case BaseCharset.HanSimplified:
                    code = "Hans";
                    break;

                case BaseCharset.HanTraditional:
                    code = "Hant";
                    break;

                case BaseCharset.Hebrew:
                    code = "Hebr";
                    break;

                case BaseCharset.Hiragana:
                    code = "Hira";
                    break;

                case BaseCharset.Katakana:
                    code = "Kana";
                    break;

                case BaseCharset.Latin:
                    code = "Latn";
                    break;

                default:
                    Debug.Assert(false, "Unkown base charset");
                    code = charset.ToString();
                    break;
            }

            return code;
        }

        public static BaseCharset GetCharset(string code)
        {
            BaseCharset charset = BaseCharset.Unknown;

            switch (code.ToLowerInvariant())
            {
                case "arab":
                    charset = BaseCharset.Arabic;
                    break;

                case "bopo":
                    charset = BaseCharset.Bopomofo;
                    break;

                case "cyrl":
                    charset = BaseCharset.Cyrillic;
                    break;

                case "deva":
                    charset = BaseCharset.Devanagari;
                    break;

                case "grek":
                    charset = BaseCharset.Greek;
                    break;

                case "hang":
                    charset = BaseCharset.Hangul;
                    break;

                case "hans":
                    charset = BaseCharset.HanSimplified;
                    break;

                case "hant":
                    charset = BaseCharset.HanTraditional;
                    break;

                case "hebr":
                    charset = BaseCharset.Hebrew;
                    break;

                case "hira":
                    charset = BaseCharset.Hiragana;
                    break;

                case "kana":
                    charset = BaseCharset.Katakana;
                    break;

                case "latn":
                    charset = BaseCharset.Latin;
                    break;

                default:
                    Debug.Assert(false, "Unkown base charset code");
                    break;
            }

            return charset;
        }

    }
}
