using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;

namespace BotDetect
{
    [System.ComponentModel.DesignerCategory("code")]
    internal class SupportedLocales
    {
        private LocalesTable _mapping = new LocalesTable();

        private static SupportedLocales _instance = new SupportedLocales();

        private SupportedLocales() 
        {
            _mapping.Rows.Add(Macrolanguage.Albanian, BaseLanguage.Unknown, BaseCharset.Latin, Country.Albania, "W", "http://sq.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Unknown, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Algeria, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Bahrain, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Egypt, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Iraq, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Jordan, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Kuwait, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Lebanon, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Libya, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Morocco, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Oman, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Qatar, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.SaudiArabia, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Syria, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Tunisia, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.UAE, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Arabic, Country.Yemen, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Unknown, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Algeria, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Bahrain, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Egypt, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Iraq, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Jordan, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Kuwait, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Lebanon, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Libya, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Morocco, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Oman, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Qatar, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.SaudiArabia, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Syria, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Tunisia, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.UAE, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Arabic, BaseLanguage.Unknown, BaseCharset.Latin, Country.Yemen, "", "http://ar.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Basque, BaseCharset.Latin, Country.Unknown, "C,Q,V,W,Y", "http://eu.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Belarusian, BaseCharset.Cyrillic, Country.Belarus, "И", "http://be.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Belarusian, BaseCharset.Latin, Country.Belarus, "И", "http://be.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Bulgarian, BaseCharset.Cyrillic, Country.Bulgaria, "Я", "http://bg.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Bulgarian, BaseCharset.Latin, Country.Bulgaria, "Я", "http://bg.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Catalan, BaseCharset.Latin, Country.Spain, "", "http://ca.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.HanSimplified, Country.PRC, "", "http://cmn.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.HanSimplified, Country.Singapore, "", "http://cmn.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.HanTraditional, Country.Taiwan, "", "http://cmn-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.HanTraditional, Country.HongKong, "", "http://cmn-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.Bopomofo, Country.Taiwan, "", "http://cmn-bopo.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Cantonese, BaseCharset.HanSimplified, Country.PRC, "", "http://yue.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Cantonese, BaseCharset.HanTraditional, Country.HongKong, "", "http://yue-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Cantonese, BaseCharset.HanTraditional, Country.Macao, "", "http://yue-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.Latin, Country.PRC, "", "http://cmn.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.Latin, Country.Singapore, "", "http://cmn.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.Latin, Country.Taiwan, "", "http://cmn-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.Latin, Country.HongKong, "", "http://cmn-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Mandarin, BaseCharset.Latin, Country.Taiwan, "", "http://cmn-bopo.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Cantonese, BaseCharset.Latin, Country.PRC, "", "http://yue.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Cantonese, BaseCharset.Latin, Country.HongKong, "", "http://yue-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Chinese, BaseLanguage.Cantonese, BaseCharset.Latin, Country.Macao, "", "http://yue-hant.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Czech, BaseCharset.Latin, Country.CzechRepublic, "Q,W", "http://cs.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Danish, BaseCharset.Latin, Country.Denmark, "C,Q,W,X,Z", "http://da.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Dutch, BaseCharset.Latin, Country.Netherlands, "", "http://nl.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Dutch, BaseCharset.Latin, Country.Belgium, "", "http://nl.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.USA, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Australia, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Belize, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Canada, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.India, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Ireland, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Jamaica, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Malaysia, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.NewZealand, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Philippines, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Singapore, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.SouthAfrica, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.TrinidadTobago, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.UK, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.English, BaseCharset.Latin, Country.Zimbabwe, "", "http://captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Estonian, BaseLanguage.Unknown, BaseCharset.Latin, Country.Estonia, "C,Q,W,X,Y", "http://et.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Faroese, BaseCharset.Latin, Country.FaroeIslands, "C,Q,W,X,Z", "http://fo.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Finnish, BaseCharset.Latin, Country.Finland, "W", "http://fi.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.French, BaseCharset.Latin, Country.France, "", "http://fr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.French, BaseCharset.Latin, Country.Belgium, "", "http://fr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.French, BaseCharset.Latin, Country.Canada, "", "http://fr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.French, BaseCharset.Latin, Country.Luxembourg, "", "http://fr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.French, BaseCharset.Latin, Country.Monaco, "", "http://fr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.French, BaseCharset.Latin, Country.Switzerland, "", "http://fr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.German, BaseCharset.Latin, Country.Germany, "", "http://de.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.German, BaseCharset.Latin, Country.Austria, "", "http://de.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.German, BaseCharset.Latin, Country.Liechtenstein, "", "http://de.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.German, BaseCharset.Latin, Country.Luxembourg, "", "http://de.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.German, BaseCharset.Latin, Country.Switzerland, "", "http://de.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Greek, BaseCharset.Greek, Country.Greece, "", "http://el.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Greek, BaseCharset.Latin, Country.Greece, "J,V", "http://el.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Greenlandic, BaseCharset.Latin, Country.Greenland, "B,C,D,W,X,Y,Z", "http://kl.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Hebrew, BaseCharset.Hebrew, Country.Israel, "", "http://he.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Hebrew, BaseCharset.Latin, Country.Israel, "", "http://he.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Hungarian, BaseCharset.Latin, Country.Hungary, "Q,W,X,Y", "http://hu.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Icelandic, BaseCharset.Latin, Country.Iceland, "C,Q,W,Z", "http://is.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Hindi, BaseCharset.Devanagari, Country.India, "", "http://hi.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Hindi, BaseCharset.Latin, Country.India, "", "http://hi.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Italian, BaseCharset.Latin, Country.Italy, "J,K,W,X,Y", "http://it.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Italian, BaseCharset.Latin, Country.Switzerland, "J,K,W,X,Y", "http://it.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Japanese, BaseCharset.Katakana, Country.Japan, "", "http://ja.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Japanese, BaseCharset.Hiragana, Country.Japan, "", "http://ja.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Japanese, BaseCharset.Latin, Country.Japan, "", "http://ja.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Korean, BaseCharset.Hangul, Country.Korea, "", "http://ko.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Korean, BaseCharset.Latin, Country.Korea, "", "http://ko.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Latvian, BaseLanguage.Unknown, BaseCharset.Latin, Country.Latvia, "Q,W,X,Y", "http://ly.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Lithuanian, BaseCharset.Latin, Country.Lithuania, "Q,W,X", "http://lt.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Luxembourgish, BaseCharset.Latin, Country.Luxembourg, "", "http://lb.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Macedonian, BaseCharset.Cyrillic, Country.Macedonia, "Я", "http://mk.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Macedonian, BaseCharset.Latin, Country.Macedonia, "Я", "http://mk.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Malay, BaseLanguage.Unknown, BaseCharset.Latin, Country.Malaysia, "", "http://ms.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Maltese, BaseCharset.Latin, Country.Malta, "C,Y", "http://mt.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.Norwegian, BaseLanguage.Unknown, BaseCharset.Latin, Country.Norway, "C,Q,W,X,Z", "http://no.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Polish, BaseCharset.Latin, Country.Poland, "Q,V,X", "http://pl.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Portuguese, BaseCharset.Latin, Country.Portugal, "", "http://pt.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Portuguese, BaseCharset.Latin, Country.Brazil, "", "http://pt.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Romanian, BaseCharset.Latin, Country.Romania, "Q,W,Y", "http://ro.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Romansh, BaseCharset.Latin, Country.Switzerland, "K,W,Y", "http://rm.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Russian, BaseCharset.Cyrillic, Country.Russia, "", "http://ru.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Russian, BaseCharset.Latin, Country.Russia, "", "http://ru.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Bosnian, BaseCharset.Latin, Country.BiH, "Q,W,X,Y", "http://bs.captcha.biz/captcha.html", "CAPTCHA", "Promjeni CAPTCHA kod", "Izgovori CAPTCHA kod");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Bosnian, BaseCharset.Cyrillic, Country.BiH, "Я", "http://bs-cyrl.captcha.biz/captcha.html", "CAPTCHA", "Пpomjeнп CAPTCHA koд", "Iзгoвopп CAPTCHA koд");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Croatian, BaseCharset.Latin, Country.Croatia, "Q,W,X,Y", "http://hr.captcha.biz/captcha.html", "CAPTCHA", "Promijeni CAPTCHA kod", "Izgovori CAPTCHA kod");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Croatian, BaseCharset.Latin, Country.BiH, "Q,W,X,Y", "http://hr.captcha.biz/captcha.html", "CAPTCHA", "Promijeni CAPTCHA kod", "Izgovori CAPTCHA kod");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Serbian, BaseCharset.Cyrillic, Country.Serbia, "Я", "http://sr.captcha.biz/captcha.html", "CAPTCHA", "Пpomeнп CAPTCHA koд", "Iзгoвopп CAPTCHA koд");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Serbian, BaseCharset.Cyrillic, Country.BiH, "Я", "http://sr.captcha.biz/captcha.html", "CAPTCHA", "Пpomeнп CAPTCHA koд", "Iзгoвopп CAPTCHA koд");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Serbian, BaseCharset.Latin, Country.Serbia, "Q,W,X,Y", "http://sr-latn.captcha.biz/captcha.html", "CAPTCHA", "Promeni CAPTCHA kod", "Izgovori CAPTCHA kod");
            _mapping.Rows.Add(Macrolanguage.SerboCroatian, BaseLanguage.Serbian, BaseCharset.Latin, Country.BiH, "Q,W,X,Y", "http://sr-latn.captcha.biz/captcha.html", "CAPTCHA", "Promeni CAPTCHA kod", "Izgovori CAPTCHA kod");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Slovak, BaseCharset.Latin, Country.Slovakia, "Q,W,X", "http://sk.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Slovenian, BaseCharset.Latin, Country.Slovenia, "Q,W,X,Y", "http://sl.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Spain, "", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Argentina, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Bolivia, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Chile, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Colombia, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.CostaRica, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.DominicanRepublic, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Ecuador, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.ElSalvador, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Guatemala, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Honduras, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Mexico, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Nicaragua, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Panama, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Paraguay, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Peru, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.PuertoRico, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.USA, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Uruguay, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Spanish, BaseCharset.Latin, Country.Venezuela, "B,V", "http://es.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Swedish, BaseCharset.Latin, Country.Sweden, "", "http://sv.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Swedish, BaseCharset.Latin, Country.Finland, "", "http://sv.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Turkish, BaseCharset.Latin, Country.Turkey, "Q,W,X", "http://tr.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Ukrainian, BaseCharset.Cyrillic, Country.Ukraine, "", "http://uk.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Ukrainian, BaseCharset.Latin, Country.Ukraine, "", "http://uk.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
            _mapping.Rows.Add(Macrolanguage.None, BaseLanguage.Vietnamese, BaseCharset.Latin, Country.Vietnam, "F,J,W,Z", "http://vi.captcha.biz/captcha.html", "CAPTCHA", "Change the CAPTCHA code", "Speak the CAPTCHA code");
        }

        public static LocalesTable Mapping
        {
            get
            {
                return _instance._mapping;
            }
        }

    }

    internal class LocalesTable : DataTable
    {
        public LocalesTable()
        {
            Columns.Add(new DataColumn("Macrolanguage", typeof(Macrolanguage)));
            Columns.Add(new DataColumn("Language", typeof(BaseLanguage)));
            Columns.Add(new DataColumn("Charset", typeof(BaseCharset)));
            Columns.Add(new DataColumn("Country", typeof(Country)));
            Columns.Add(new DataColumn("CharsetDiff", typeof(string)));
            Columns.Add(new DataColumn("HelpLink", typeof(string)));
            Columns.Add(new DataColumn("HelpTooltip", typeof(string)));
            Columns.Add(new DataColumn("ReloadTooltip", typeof(string)));
            Columns.Add(new DataColumn("SoundTooltip", typeof(string)));
        }

        protected override Type GetRowType()
        {
            return typeof(LocaleRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new LocaleRow(builder);
        }

        public void Add(LocaleRow row)
        {
            Rows.Add(row);
        }

        public new LocaleRow NewRow()
        {
            LocaleRow row = NewRow() as LocaleRow;

            return row;
        }

        public LocaleRow FindBestMatch(Macrolanguage macrolanguage,
            BaseLanguage language, BaseCharset charset, Country country)
        {
            List<LocaleRow> matches = SupportedLocales.Mapping.FindMatchingRows(
               macrolanguage, language, charset, country);

            if (0 == matches.Count)
            {
                throw new LocalizationException("Invalid or unknown locale definition",
                    macrolanguage, language, charset, country);
            }

            return matches[0];
        }

        public List<LocaleRow> FindMatchingRows(Macrolanguage macrolanguage, 
            BaseLanguage language, BaseCharset charset, Country country)
        {
            List<LocaleRow> matches = new List<LocaleRow>();

            if (Macrolanguage.None == macrolanguage &&
                BaseLanguage.Unknown == language &&
                BaseCharset.Unknown == charset &&
                Country.Unknown == country)
            {
                // completely unspecified locales are an error
                return matches;
            }

            int count = this.Rows.Count;
            for (int i = 0; i < count;  i++)
            {
                LocaleRow defined = this.Rows[i] as LocaleRow;

                // check macrolanguage match
                if (Macrolanguage.None != macrolanguage)
                {
                    if (macrolanguage != defined.Macrolanguage)
                    {
                        continue;
                    }
                }

                // check base language match
                if (BaseLanguage.Unknown != language)
                {
                    if (language != defined.Language)
                    {
                        continue;
                    }
                }

                // check charset match
                if (BaseCharset.Unknown != charset)
                {
                    if (charset != defined.Charset)
                    {
                        continue;
                    }
                }

                // check country match
                if (Country.Unknown != country)
                {
                    // allow any country te be a match for supported 
                    // locale rows with Country.Unknown (e. g. Basque)
                    if (Country.Unknown != defined.Country &&
                        country != defined.Country)
                    {
                        continue;
                    }
                }

                // if everything matches, add to collection
                matches.Add(defined);
            }

            // if a country is specified and found, remove less specific matches
            if (0 != matches.Count && 
                Country.Unknown != country)
            {
                List<LocaleRow> specificMatches = new List<LocaleRow>();
                int matchCount = matches.Count;
                for (int i = 0; i < matchCount; i++)
                {
                    if (country == matches[i].Country)
                    {
                        specificMatches.Add(matches[i]);
                    }
                }

                if (0 != specificMatches.Count)
                {
                    return specificMatches;
                }
            }

            return matches;
        }
    }

    internal class LocaleRow : DataRow
    {
        internal LocaleRow(DataRowBuilder builder)
            : base(builder)
        {
        }

        public Macrolanguage Macrolanguage
        {
            get { return (Macrolanguage) base["Macrolanguage"]; }
            set { base["Macrolanguage"] = value; }
        }

        public BaseLanguage Language
        {
            get { return (BaseLanguage) base["Language"]; }
            set { base["Language"] = value; }
        }

        public BaseCharset Charset
        {
            get { return (BaseCharset) base["Charset"]; }
            set { base["Charset"] = value; }
        }

        public Country Country
        {
            get { return (Country) base["Country"]; }
            set { base["Country"] = value; }
        }

        public string CharsetDiff
        {
            get { return (string)base["CharsetDiff"]; }
            set { base["CharsetDiff"] = value; }
        }

        public string HelpLink
        {
            get { return (string)base["HelpLink"]; }
            set { base["HelpLink"] = value; }
        }

        public string HelpTooltip
        {
            get { return (string)base["HelpTooltip"]; }
            set { base["HelpTooltip"] = value; }
        }

        public string ReloadTooltip
        {
            get { return (string)base["ReloadTooltip"]; }
            set { base["ReloadTooltip"] = value; }
        }

        public string SoundTooltip
        {
            get { return (string)base["SoundTooltip"]; }
            set { base["SoundTooltip"] = value; }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                "Macrolanguage: {0}, Language: {1}, Charset: {2}, Country: {3}", 
                this.Macrolanguage, this.Language, this.Charset, this.Country
            );
        }
    }
}
