using System;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using BotDetect.Audio;
using BotDetect.Drawing;

namespace BotDetect
{
    internal sealed class CaptchaDefaults
    {
        private CaptchaDefaults()
        {
            // constructor omitted, static methods only
        }

        // i8n
        public const string Locale = "en-US";
        public static readonly Localization Localization = LocaleParser.Parse(CaptchaDefaults.Locale);

        // Captcha code
        public const string CustomCharacterSetName = null;
        public static readonly CodeStyle CodeStyle = CodeStyle.Alphanumeric;
        public const int CodeLength = 5;
        public const int MinCodeLength = 1;
        public const int MaxCodeLength = 15;
        public const int CodeTimeout = 1200; // 20 minutes

        // Captcha image
        public static readonly ImageFormat ImageFormat = ImageFormat.Jpeg;
        public static readonly ImageSize ImageSize = new ImageSize(250, 50);
        public static readonly System.Drawing.Size GdiImageSize = new System.Drawing.Size(250, 50);
        public static readonly ImageSize MinImageSize = new ImageSize(20, 20);
        public static readonly ImageSize MaxImageSize = new ImageSize(500, 200);
        public static readonly Color CustomLightColor = null;
        public static readonly Color CustomDarkColor = null;
        public const bool HelpLinkEnabled = true;

        // Captcha sound
        public static readonly SoundFormat SoundFormat = SoundFormat.WavPcm16bit8kHzMono;
        public const bool SoundEnabled = true;
        public const string SoundIconPath = null;
        public const int SoundIconWidth = 0; 
        public const string SoundPackagesFolder = @"\BotDetectSounds";
        public const bool WarnAboutMissingSoundPackages = true;
        public const string MissingSoundPackageTooltip = "<em>Captcha sound is enabled, but the pronunciation sound package required for the current locale can not be found.</em> \n<em>To enable Captcha sound for this locale, please deploy the appropriate sound package from the <code>\\BotDetectSounds\\</code> folder in the BotDetect installation to the <code>\\Bin\\BotDetectSounds\\</code> folder of your application. For example, use <code>Pronunciation_English_GB.bdsp</code> for British English Captcha sounds.</em> \n<em>To disable this warning and remove the sound icon for the current Captcha locale, set <code>warnAboutMissingSoundPackages</code> to <code>false</code> in the <code>&lt;botDetect&gt;</code> section of your <code>web.config</code> file. To remove the sound icon for all locales, simply set <code>&lt;captchaSound enabled=\"false\"&gt;</code>.</em>";
        public const int SoundStartDelay = 0;

        // Reload
        public const bool ReloadingEnabled = true;
        public const string ReloadIconPath = null;
        public const int ReloadIconWidth = 0; 
        public const bool AutoReloadExpiredCaptchas = true;
        public const int AutoReloadTimeout = 7200; // 2 hours

        // input settings
        public const string UserInputClientID = null;
        public const bool AutoUppercaseInput = true;
        public const bool AutoClearInput = true;
        public const bool AutoFocusInput = true;

        // Captcha configuration
        public const bool TestModeEnabled = false;
        public const bool ErrorLoggingEnabled = true;
        public const bool TraceEnabled = true;
        public const string EventFilter = ".*";
        public const string LoggingProvider = "BotDetect.Logging.NullLoggingProvider, BotDetect";
        public const bool CaptchaSessionTroubleshootingEnabled = true;
        public const bool CaptchaHttpHandlerTroubleshootingEnabled = false;

        // Http settings
        public const string RequestPath = "BotDetectCaptcha.ashx";
        public const bool RequestFilterEnabled = true;
        public const int AllowedRepeatedRequests = 5;
        public const int MinAllowedRepeatedRequests = 1;
        public const int MaxAllowedRepeatedRequests = 100;

        // string & encryption settings
        public static readonly Regex DisallowedCharacters =
            new Regex("[^a-zA-Z0-9_]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string EncryptionPassword
        {
            get
            {
                string password = ServerHelper.PhysicalApplicationPath;
                if (StringHelper.HasValue(password))
                {
                    password = CaptchaDefaults.DisallowedCharacters.Replace(password, "");
                }
                else
                {
                    // TODO
                    password = "TODO";
                }

                return password;
            }
        }
    }
}
