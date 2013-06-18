using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using BotDetect.Drawing;

namespace BotDetect
{
    [Serializable]
    internal class Localization
    {
        public Localization(LocaleRow row)
        {
            _macrolanguage = row.Macrolanguage;
            _language = row.Language;
            _charset = row.Charset;
            _country = row.Country;
            _charsetDiff = StringHelper.GetCodePoints(row.CharsetDiff);
            _helpPage = row.HelpLink;
            _captchaImageTooltip = row.HelpTooltip;
            _reloadIconTooltip = row.ReloadTooltip;
            _soundIconTooltip = row.SoundTooltip;

            this.Init();
        }

        private Macrolanguage _macrolanguage;
        public Macrolanguage Macrolanguage
        {
            get
            {
                return _macrolanguage;
            }

            set
            {
                _macrolanguage = value;
            }
        }

        private BaseLanguage _language;
        private string _languageCode;

        public BaseLanguage Language
        {
            get
            {
                return _language;
            }

            set
            {
                _language = value;
            }
        }

        private BaseCharset _charset;
        private string _charsetCode;

        public BaseCharset Charset
        {
            get
            {
                return _charset;
            }

            set
            {
                _charset = value;
            }
        }

        private Set<Int32> _charsetDiff;
        public Set<Int32> CharsetDiff
        {
            get
            {
                return _charsetDiff;
            }
        }

        private bool _isRtl;
        public bool IsRtl
        {
            get
            {
                return _isRtl;
            }
        }

        private FontCollection _fonts;
        public FontCollection Fonts
        {
            get
            {
                return _fonts;
            }
        }

        private Country _country;
        private string _countryCode;

        public Country Country
        {
            get
            {
                return _country;
            }

            set
            {
                _country = value;
            }
        }

        public void Init()
        {
            // init dependent values
            InitLanguage();
            InitCharset();
            InitCountry();
            InitCanonicalLocaleString();
            InitPronunciationFilename();
        }

        private void InitLanguage()
        {
            _languageCode = "";

            if (Macrolanguage.None != _macrolanguage)
            {
                _languageCode += MacrolanguageCodes.GetCode(_macrolanguage);
            }

            if (Macrolanguage.None != _macrolanguage &&
                BaseLanguage.Unknown != _language)
            {
                _languageCode += "-";
            }

            if (BaseLanguage.Unknown != _language)
            {
                _languageCode += LanguageCodes.GetCode(_language);
            }
        }

        private void InitCharset()
        {
            InitRtl();
            InitFonts();
            _charsetCode = CharsetCodes.GetCode(_charset);
        }

        private void InitRtl()
        {
            _isRtl = false;

            if (BaseCharset.Arabic == _charset ||
                BaseCharset.Hebrew == _charset)
            {
                _isRtl = true;
            }
        }

        private void InitFonts()
        {
            _fonts = new FontCollection();

            switch (_charset)
            {
                case BaseCharset.Bopomofo:
                case BaseCharset.Hangul:
                case BaseCharset.HanSimplified:
                case BaseCharset.HanTraditional:
                case BaseCharset.Hiragana:
                case BaseCharset.Katakana:
                    _fonts[0] = Font.From("Arial", FontCase.Uppercase, FontWeight.Normal);
                    _fonts[1] = Font.From("Courier New", FontCase.Uppercase, FontWeight.Bold);
                    _fonts[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Bold);
                    _fonts[3] = Font.From("Times New Roman", FontCase.Uppercase, FontWeight.Normal);
                    _fonts[4] = Font.From("Tahoma", FontCase.Uppercase, FontWeight.Normal);
                    _fonts[5] = Font.From("Verdana", FontCase.Uppercase, FontWeight.Normal);
                    /*_fonts[6] = Font.From("SimSun", FontCase.Uppercase, FontWeight.Bold);
                    _fonts[7] = Font.From("MingLiU", FontCase.Uppercase, FontWeight.Bold);*/
                    break;

                default:
                    _fonts[0] = Font.From("Arial", FontCase.Uppercase, FontWeight.Normal);
                    _fonts[1] = Font.From("Courier New", FontCase.Uppercase, FontWeight.Bold);
                    _fonts[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Bold);
                    _fonts[3] = Font.From("Times New Roman", FontCase.Uppercase, FontWeight.Normal);
                    _fonts[4] = Font.From("Tahoma", FontCase.Uppercase, FontWeight.Normal);
                    _fonts[5] = Font.From("Verdana", FontCase.Uppercase, FontWeight.Normal);
                    break;
            }

        }

        private void InitCountry()
        {
            _countryCode = CountryCodes.GetCode(_country);
        }

        // canonical Locale string
        private string _canonicalLocaleString;

        public override string ToString()
        {
            return _canonicalLocaleString;
        }

        private void InitCanonicalLocaleString()
        {
            if (Country.Unknown == _country)
            {
                _canonicalLocaleString = String.Format(CultureInfo.InvariantCulture,
                    "{0}-{1}", _languageCode, _charsetCode);
                return;
            }

            _canonicalLocaleString = String.Format(CultureInfo.InvariantCulture,
                "{0}-{1}-{2}", _languageCode, _charsetCode, _countryCode);
        }

        // sound Captcha resource location
        private string _soundPackagesFolder;

        public string SoundPackagesFolder
        {
            get
            {
                return _soundPackagesFolder;
            }
            set
            {
                _soundPackagesFolder = value;
            }
        }

        // sound Captcha resource
        private string _pronunciationFilename;

        public string PronunciationFilename
        {
            get
            {
                return _pronunciationFilename;
            }
        }

        private void InitPronunciationFilename()
        {
            // depends on language and country

            // language name: macrolanguage name can be used 
            // if the variant isn't specified
            string languageName = "Unknown";
            if (_language != BaseLanguage.Unknown)
            {
                languageName = _language.ToString();
            }
            else if (_macrolanguage != Macrolanguage.None)
            {
                languageName = _macrolanguage.ToString();
            }

            // ensure non-country-specific SoundPackage files are named 
            // properly (e.g. "Pronunciation_Arabic.bdsp")
            if (Country.Unknown == _country)
            {
                _pronunciationFilename = String.Format(CultureInfo.InvariantCulture,
                    "Pronunciation_{0}.bdsp", languageName);
            }
            else
            {
                _pronunciationFilename = String.Format(CultureInfo.InvariantCulture,
                    "Pronunciation_{0}_{1}.bdsp", languageName, _countryCode);
            }
        }

        // depends on language and base charset
        private string _helpPage;

        public string HelpPage
        {
            get
            {
                return _helpPage;
            }
        }

        // depends on language and base charset
        private string _captchaImageTooltip;
        public string CaptchaImageTooltip
        {
            get
            {
                return _captchaImageTooltip;
            }
        }

        // depends on language and base charset
        private string _reloadIconTooltip;
        public string ReloadIconTooltip
        {
            get
            {
                return _reloadIconTooltip;
            }
        }

        // depends on language and base charset
        private string _soundIconTooltip;
        public string SoundIconTooltip
        {
            get
            {
                return _soundIconTooltip;
            }
        }
    }
}
