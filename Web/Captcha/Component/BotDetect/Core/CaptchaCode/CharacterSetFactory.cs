using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using BotDetect.Configuration;

namespace BotDetect.CaptchaCode
{
    // not a factory, since it returns singletons?
    internal class CharacterSetFactory
    {
        private CharacterSetFactory()
        {
        }

        public static CharacterSet Get(Localization localization, string customCharacterSetName)
        {
            // any custom charset setting takes precedence
            CharacterSet charset = GetCharacterSet(customCharacterSetName);

            // otherwise, the charset used depends on the locale
            if (null == charset)
            {
                charset = GetCharacterSet(localization);
            }

            return charset;
        }


        // we keep a cache of constructed CharacterSet objects, to avoid
        // expensive object initialization on every call
        private static Dictionary<string, CharacterSet> _customCharacterSetCache = new Dictionary<string, CharacterSet>();

        private static readonly object customCharsetCacheLock = new object();

        // by locale definition
        private static CharacterSet GetCharacterSet(Localization localization)
        {
            CharacterSet charset = null;
            string locale = localization.ToString();

            if (_customCharacterSetCache.ContainsKey(locale))
            {
                charset = _customCharacterSetCache[locale];
            }
            else
            {
                lock (customCharsetCacheLock)
                {
                    if (!_customCharacterSetCache.ContainsKey(locale))
                    {
                        CharacterSet predefined = GetPredefined(localization.Charset);
                        charset = new CharacterSet(predefined, localization.CharsetDiff, locale);
                        _customCharacterSetCache[locale] = charset;
                    }
                }
            }

            return charset;
        }

        private static CharacterSet GetPredefined(BaseCharset charset)
        {
            CharacterSet predefined = null;

            switch (charset)
            {
                case BaseCharset.Arabic:
                    predefined = ArabicCharacterSet.Instance;
                    break;

                case BaseCharset.Bopomofo:
                    predefined = BopomofoCharacterSet.Instance;
                    break;

                case BaseCharset.Cyrillic:
                    predefined = CyrillicCharacterSet.Instance;
                    break;

                case BaseCharset.Devanagari:
                    predefined = DevanagariCharacterSet.Instance;
                    break;

                case BaseCharset.Greek:
                    predefined = GreekCharacterSet.Instance;
                    break;

                case BaseCharset.Hangul:
                    predefined = HangulCharacterSet.Instance;
                    break;

                case BaseCharset.HanSimplified:
                    predefined = HanSimplifiedCharacterSet.Instance;
                    break;

                case BaseCharset.HanTraditional:
                    predefined = HanTraditionalCharacterSet.Instance;
                    break;

                case BaseCharset.Hebrew:
                    predefined = HebrewCharacterSet.Instance;
                    break;

                case BaseCharset.Hiragana:
                    predefined = HiraganaCharacterSet.Instance;
                    break;

                case BaseCharset.Katakana:
                    predefined = KatakanaCharacterSet.Instance;
                    break;

                case BaseCharset.Latin:
                    predefined = LatinCharacterSet.Instance;
                    break;
            }

            return predefined;
        }

        // by name and app configuration
        private static CharacterSet GetCharacterSet(string customCharacterSetName)
        {
            CharacterSet charset = null;

            if (StringHelper.HasValue(customCharacterSetName))
            {
                if (_customCharacterSetCache.ContainsKey(customCharacterSetName))
                {
                    charset = _customCharacterSetCache[customCharacterSetName];
                }
                else
                {
                    ICharacterSetCollectionConfiguration customCharsets = CaptchaConfiguration.CaptchaCodes.CharacterSets;
                    if (null != customCharsets)
                    {
                        ICharacterSetConfiguration customCharset = customCharsets[customCharacterSetName];
                        if (null != customCharset)
                        {
                            lock (customCharsetCacheLock)
                            {
                                if (!_customCharacterSetCache.ContainsKey(customCharacterSetName))
                                {
                                    StringCollection alpha = customCharset.Alpha;
                                    StringCollection numeric = customCharset.Numeric;
                                    StringCollection alphanumeric = customCharset.Alphanumeric;
									charset = new CharacterSet(alpha, numeric, alphanumeric, customCharacterSetName);

                                    _customCharacterSetCache[customCharacterSetName] = charset;
                                }
                            }
                        }
                    }
                }
            }

            return charset;
        }
    }
}
