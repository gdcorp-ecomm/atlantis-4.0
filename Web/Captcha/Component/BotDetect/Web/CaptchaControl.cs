using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Configuration;
using System.Web;
using System.ComponentModel;
using System.Web.UI;

using BotDetect.Configuration;
using BotDetect.Logging;
using BotDetect.Persistence;

namespace BotDetect.Web
{
    /// <summary>
    /// This class builds upon the general Captcha functionality in CaptchaBase, 
    /// and abstracts functionality common to different possible kinds of 
    /// Captcha controls. For example, the Captcha control used in Web Forms 
    /// applications and the MvcCaptcha control used in ASP.NET MVC 
    /// applications both delegate common functionality to a contained 
    /// CaptchaControl instance.
    /// </summary>
    public class CaptchaControl
    {
        /// <summary>
        /// The basic CaptchaControl constructor takes only the Captcha 
        /// identifier (e.g. "RegistrationCaptcha") as a parameter
        /// </summary>
        /// <param name="captchaId"></param>
        public CaptchaControl(string captchaId)
            : this(new CaptchaBase(captchaId))
        {
        }

        internal CaptchaControl(CaptchaBase captcha)
        {
            _captchaBase = captcha;

            // initialization event, part 1
            InitializedCaptchaControlEventArgs e = new InitializedCaptchaControlEventArgs();
            e.CaptchaId = this.CaptchaId;

            CaptchaLogging.Trace("InitializedCaptchaControl", e);  
        }

        public void Initialize()
        {
            // initialization event, part 2
            InitializedCaptchaControlEventArgs e = new InitializedCaptchaControlEventArgs();
            e.CaptchaId = this.CaptchaId;

            if (null != InitializedCaptchaControl)
            {
                InitializedCaptchaControl(this, e);
            }
        }

        private CaptchaBase _captchaBase;
        internal CaptchaBase CaptchaBase
        {
            get
            {
                return _captchaBase;
            }
        }

        #region CaptchaControl events

        internal static event EventHandler<InitializedCaptchaControlEventArgs> InitializedCaptchaControl;

        // registered handlers for each captchaId
        private static Dictionary<string, bool> _registeredHandlers = new Dictionary<string, bool>();

        // event-specific locks
        private static object _InitializedCaptchaControlSyncRoot = new object();

        // event-forwarding helpers
        private static string GetHandlerKey(string captchaId, string eventName, Delegate method)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}_{3}", captchaId, eventName, method.Method.Name, method.Method.DeclaringType.AssemblyQualifiedName);
        }

        private static bool IsHandlerRegistered(string captchaId, string eventName, Delegate method)
        {
            bool isRegistered = false;
            string key = GetHandlerKey(captchaId, eventName, method);

            if (null != _registeredHandlers &&
                _registeredHandlers.ContainsKey(key))
            {
                isRegistered = (bool)_registeredHandlers[key];
            }

            return isRegistered;
        }

        /// <summary>
        /// "smart" event handler registration, avoids duplicate handler registrations
        /// </summary>
        public static void RegisterInitializedCaptchaControlHandler(string captchaId, EventHandler<InitializedCaptchaControlEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaControl.InitializedCaptchaControl)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "InitializedCaptchaControl", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_InitializedCaptchaControlSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaControl.InitializedCaptchaControl)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "InitializedCaptchaControl", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaControl.InitializedCaptchaControl += method as EventHandler<InitializedCaptchaControlEventArgs>;

                                string key = GetHandlerKey(captchaId, "InitializedCaptchaControl", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        #endregion CaptchaControl events


        /// <summary>
        /// Instead of accessing the CaptchaUrls helper directly, we encapsulate the
        /// access through a field, so switching between relative and absolute urls
        /// can be done per CaptchaControl instance
        /// </summary>
        private ICaptchaUrlsGenerator _urls = CaptchaUrls.Relative;
        public ICaptchaUrlsGenerator Urls
        {
            get
            {
                return this._urls;
            }

            set
            {
                this._urls = value;
            }
        }


        /// <summary>
        /// Globally unique identifier of the current CaptchaControl object 
        /// instance, used to ensure each page load keeps separate Captcha 
        /// codes, for example when opening the same form in multiple browser tabs
        /// </summary>
        public string CurrentInstanceId
        {
            get
            {
                return _captchaBase.InstanceId;
            }
        }

        /// <summary>
        /// Unique identifier of the CaptchaControl within the application (for example, 
        /// if you placed one CaptchaControl on the Registration page and another on the 
        /// Contact Us, they would have distinct CaptchaId values)
        /// </summary>
        public string CaptchaId
        {
            get 
            {
                return _captchaBase.CaptchaId;
            }
        }

        internal Localization Localization
        {
            get
            {
                return _captchaBase.Localization;
            }
            set
            {
                _captchaBase.Localization = value;
            }
        }

        /// <summary>
        /// Locale string, affects the character set used for Captcha code
        /// generation and the pronunciation language used for Captcha sound 
        /// generation
        /// </summary>
        public string Locale
        {
            get
            {
                return _captchaBase.Locale;
            }
            set
            {
                _captchaBase.Locale = value;
            }
        }
        
        /// <summary>
        /// Length (number of characters) of the Captcha code rendered; 
        /// the default value is 5
        /// </summary>
        public int CodeLength
        {
            get
            {
                return _captchaBase.CodeLength;
            }
            set
            {
                _captchaBase.CodeLength = value;
            }
        }

        /// <summary>
        /// Code style, i.e. the algorithm used to generate Captcha codes; 
        /// the default value is Alphanumeric
        /// </summary>
        public CodeStyle CodeStyle
        {
            get
            {
                return _captchaBase.CodeStyle;
            }
            set
            {
                _captchaBase.CodeStyle = value;
            }
        }

        /// <summary>
        /// Optional name of the user-defined character set used for Captcha code generation. 
        /// A collection of custom character sets can be defined in the <botDetect> section 
        /// of the web.config file
        /// </summary>
        public string CustomCharacterSetName
        {
            get
            {
                return _captchaBase.CustomCharacterSetName;
            }
            set
            {
                _captchaBase.CustomCharacterSetName = value;
            }
        }

        /// <summary>
        /// Optional global list of sequences which will be filtered out of 
        /// the randomly generated Captcha codes, used for example for swear-word filtering
        /// </summary>
        public static List<string> BannedSequences
        {
            get
            {
                return CaptchaBase.BannedSequences;
            }
            set
            {
                CaptchaBase.BannedSequences = value;
            }
        }
        
        /// <summary>
        /// Image style, i.e. the algorithm used to render Captcha codes in images; 
        /// if no ImageStyle is set, it is randomized by default
        /// </summary>
        public ImageStyle ImageStyle
        {
            get
            {
                return _captchaBase.ImageStyle;
            }
            set
            {
                _captchaBase.ImageStyle = value;
            }
        }

        /// <summary>
        /// Image format in which the Captcha image will be rendered; 
        /// the default format is JPEG
        /// </summary>
        public ImageFormat ImageFormat
        {
            get
            {
                return _captchaBase.ImageFormat;
            }
            set
            {
                _captchaBase.ImageFormat = value;
            }
        }

        /// <summary>
        /// Size of the Captcha image rendered; the default size is (250, 50)
        /// </summary>
        public System.Drawing.Size ImageSize
        {
            get
            {
                return _captchaBase.ImageSize.GdiSize;
            }
            set
            {
                _captchaBase.ImageSize = new ImageSize(value);
            }
        }

        /// <summary>
        /// Optional custom light color point, modifies the color palette 
        /// used for Captcha image drawing
        /// </summary>
        public System.Drawing.Color CustomLightColor
        {
            get
            {
                if (null == _captchaBase.CustomLightColor)
                {
                    return System.Drawing.Color.Empty;
                }

                return _captchaBase.CustomLightColor.GdiColor;
            }
            set
            {
                if (System.Drawing.Color.Empty == value)
                {
                    _captchaBase.CustomLightColor = null;
                    return;
                }

                _captchaBase.CustomLightColor = BotDetect.Drawing.Color.Gdi(value);
            }
        }

        /// <summary>
        /// Optional custom dark color point, modifies the color palette 
        /// used for Captcha image drawing
        /// </summary>
        public System.Drawing.Color CustomDarkColor
        {
            get
            {
                if (null == _captchaBase.CustomDarkColor)
                {
                    return System.Drawing.Color.Empty;
                }

                return _captchaBase.CustomDarkColor.GdiColor;
            }
            set
            {
                if (System.Drawing.Color.Empty == value)
                {
                    _captchaBase.CustomDarkColor = null;
                    return;
                }

                _captchaBase.CustomDarkColor = BotDetect.Drawing.Color.Gdi(value);
            }
        }
        
        /// <summary>
        /// Sound style, i.e. the algorithm used to pronounce Captcha codes 
        /// in sounds; if no SoundStyle is set, it is randomized by default
        /// </summary>
        public SoundStyle SoundStyle
        {
            get
            {
                return _captchaBase.SoundStyle;
            }
            set
            {
                _captchaBase.SoundStyle = value;
            }
        }
        
        /// <summary>
        /// Audio format in which the Captcha sound will be generated; 
        /// the default format is WawPcm16bit8kHzMono
        /// </summary>
        public SoundFormat SoundFormat
        {
            get
            {
                return _captchaBase.SoundFormat;
            }
            set
            {
                _captchaBase.SoundFormat = value;
            }
        }

        /// <summary>
        /// Image alt text used for Captcha images (read from configuration, allows per-instance overriding)
        /// </summary>
        internal string _captchaImageTooltip = null;

        public string CaptchaImageTooltip
        {
            get
            {
                // if the instance value is set by the calling code, always use it first
                if (StringHelper.HasValue(_captchaImageTooltip))
                {
                    return _captchaImageTooltip;
                }

                // read the application-level configured value next
                string configuredTooltip = CaptchaConfiguration.CaptchaImage.Tooltip[_captchaBase.Locale];
                if (StringHelper.HasValue(configuredTooltip))
                {
                    return configuredTooltip;
                }

                // use the default value otherwise
                return _captchaBase.Localization.CaptchaImageTooltip;
            }
            set
            {
                // setting the per-instance text will override any other values
                _captchaImageTooltip = value;
            }
        }

        /// <summary>
        /// Is the Captcha help link enabled (depends on license)
        /// </summary>
        public bool HelpLinkEnabled
        {
            get
            {
                bool helpLinkEnabled = false;



                    helpLinkEnabled =  CaptchaConfiguration.CaptchaImage.HelpLink.Enabled;


                return helpLinkEnabled;
            }
        }


        /// <summary>
        /// How should the Captcha help link be displayed (depends on license)
        /// </summary>
        public HelpLinkMode HelpLinkMode
        {
            get
            {
                return CaptchaConfiguration.CaptchaImage.HelpLink.Mode;
            }
        }


        /// <summary>
        /// Url of the Captcha image help link (read from configuration)
        /// </summary>
        public string HelpPage
        {
            get
            {


                string helpPage = CaptchaConfiguration.CaptchaImage.HelpLink.HelpPage[_captchaBase.Locale];
                if (!StringHelper.HasValue(helpPage))
                {
                    // default value
                    return _captchaBase.Localization.HelpPage;
                }

                // user-configured application-relative path
                if (helpPage.StartsWith("~/"))
                {
                    return VirtualPathUtility.ToAbsolute(helpPage);
                }

                // user-configured absolute path
                if (helpPage.StartsWith("http"))
                {
                    return helpPage;
                }

                // default value
                return _captchaBase.Localization.HelpPage;

            }
        }

        /*/// <summary>
        /// Helper used to provide backwards compatibility for help link changes introduced in v3.0.10 
        /// </summary>
        public bool IsHelpPageCustomized
        {
            get
            {
                bool isHelpPageCustomized = true;

                

                if (!isHelpPageCustomized)
                {
                    return false;
                }

                string helpPage = CaptchaConfiguration.CaptchaImage.HelpLink.HelpPage[_captchaBase.Locale];
                if (StringHelper.HasValue(helpPage) && 
                   (0 != String.Compare(helpPage, _captchaBase.Localization.HelpPage, StringComparison.InvariantCultureIgnoreCase)))
                {
                    // only fall back to previous version behavior in the full version when the help page is specified and
                    // different from the default value
                    return true;
                }

                return false;
            }
        }*/


        /// <summary>
        /// Help link text adjusts to image height
        /// </summary>
        public string HelpLinkText
        {
            get
            {

                string helpText = null;


                helpText = CaptchaConfiguration.CaptchaImage.HelpLink.HelpText[_captchaBase.Locale];
                if (!StringHelper.HasValue(helpText))
                {
                    helpText = this.DefaultHelpText;
                }


                return helpText;
            }
        }


        private string DefaultHelpText
        {
            get
            {
                int width = this.ImageSize.Width;
                if (width < 100)
                {
                    return "CAPTCHA";
                }
                else if (width < 113)
                {
                    return ".NET CAPTCHA";
                }
                else if (width < 125)
                {
                    return "CAPTCHA Control";
                }
                else if (width < 138)
                {
                    return "ASP.NET CAPTCHA";
                }
                else if (width < 150)
                {
                    return "BotDetect CAPTCHA";
                }
                else if (width < 163)
                {
                    return ".NET CAPTCHA Control";
                }
                else if (width < 175)
                {
                    return "BotDetect .NET CAPTCHA";
                }
                else if (width < 188)
                {
                    return "ASP.NET CAPTCHA Control";
                }
                else if (width < 200)
                {
                    return "BotDetect CAPTCHA Control";
                }
                else if (width < 213)
                {
                    return "BotDetect ASP.NET CAPTCHA";
                }
                else if (width < 225)
                {
                    return "BotDetect .NET CAPTCHA Control";
                }
                else if (width < 238)
                {
                    return "BotDetect ASP.NET CAPTCHA Control";
                }
                else if (width < 250)
                {
                    return "What is BotDetect CAPTCHA Control?";
                }
                else
                {
                    return "What is BotDetect .NET CAPTCHA Control?";
                }
            }
        }


        /// <summary>
        /// Image height modified by help link height
        /// </summary>
        public int AdjustedHeight
        {
            get
            {
                return _captchaBase.ImageSize.Height - CaptchaBase.HelpLinkHeight;
            }
        }


        public int HelpLinkHeight
        {
            get
            {
                return CaptchaBase.HelpLinkHeight;
            }
        }


        public int HelpLinkFontSize
        {
            get
            {
                return CaptchaBase.HelpLinkHeight - 1;
            }
        }


        /// <summary>
        /// Is Captcha image reloading enabled (read from configuration)
        /// </summary>
        public bool ReloadIconEnabled
        {
            get
            {
                return CaptchaConfiguration.CaptchaReloading.Enabled;
            }
        }

        /// <summary>
        /// Link title and image alt text used for the Captcha Reload icon 
        /// (read from configuration, allows per-instance overriding)
        /// </summary>
        internal string _reloadIconTooltip = null;

        public string ReloadIconTooltip
        {
            get
            {
                // if the instance value is set by the calling code, always use it first
                if (StringHelper.HasValue(_reloadIconTooltip))
                {
                    return _reloadIconTooltip;
                }

                // read the application-level configured value next
                string configuredTooltip = CaptchaConfiguration.CaptchaReloading.ReloadIcon.Tooltip[_captchaBase.Locale];
                if (StringHelper.HasValue(configuredTooltip))
                {
                    return configuredTooltip;
                }

                // use the default value otherwise
                return _captchaBase.Localization.ReloadIconTooltip;
            }
            set
            {
                // setting the per-instance text will override any other values
                _reloadIconTooltip = value;
            }
        }

        /// <summary>
        /// Are Captcha sounds enabled (read from configuration)
        /// </summary>
        public bool CaptchaSoundEnabled
        {
            get
            {
                bool configuredValue = CaptchaConfiguration.CaptchaSound.Enabled;

                if (CaptchaConfiguration.CaptchaSound.SoundPackages.WarnAboutMissingSoundPackages)
                {
                    return configuredValue;
                }
                else
                {
                    return configuredValue && this.CaptchaSoundAvailable;
                }
            }
        }

        /// <summary>
        /// Starting delay (in miliseconds) for the sound-playing JavaScript
        /// </summary>
        public int SoundStartDelay
        {
            get
            {
                return CaptchaConfiguration.CaptchaSound.StartDelay;
            }
        }

        /// <summary>
        /// Are Captcha sounds for the current Locale available
        /// </summary>
        public bool CaptchaSoundAvailable
        {
            get
            {
                bool fileAvailable = _captchaBase.IsLocalizedPronunciationAvailable;

                return fileAvailable;
            }
        }

        /// <summary>
        /// Is the BotDetect pronunciation sound package for the current Locale missing
        /// </summary>
        public bool SoundPackageMissing
        {
            get
            {
                return (this.CaptchaSoundEnabled && !this.CaptchaSoundAvailable);
            }
        }


        /// <summary>
        /// Link title and image alt text used for the Captcha Sound icon (read from configuration, allows per-instance overriding)
        /// </summary>
        internal string _soundIconTooltip = null;

        public string SoundIconTooltip
        {
            get
            {
                if (!this.CaptchaSoundAvailable)
                {
                    return CaptchaDefaults.MissingSoundPackageTooltip;
                }

                // if the instance value is set by the calling code, always use it first
                if (StringHelper.HasValue(_soundIconTooltip))
                {
                    return _soundIconTooltip;
                }

                // read the application-level configured value next
                string configuredTooltip = CaptchaConfiguration.CaptchaSound.SoundIcon.Tooltip[_captchaBase.Locale];
                if (StringHelper.HasValue(configuredTooltip))
                {
                    return configuredTooltip;
                }

                // use the default value otherwise
                return _captchaBase.Localization.SoundIconTooltip;
            }
            set
            {
                // setting the per-instance text will override any other values
                _soundIconTooltip = value;
            }
        }

        /// <summary>
        /// BotDetect script include fragment, including script tags
        /// </summary>
        public string ClientScriptIncludeFragment
        {
            get
            {
                return HtmlHelper.ScriptInclude(this._urls.ClientScriptIncludeUrl);
            }
        }


        /// <summary>
        /// BotDetect client-side initialization function
        /// </summary>
        public string ClientScriptInitializationFragment
        {
            get
            {
                string scriptFragment = String.Format(CultureInfo.InvariantCulture,
                    "      BotDetect.Init({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9});",
                    JavaScriptHelper.String(this.CaptchaId),
                    JavaScriptHelper.String(this.CurrentInstanceId),
                    JavaScriptHelper.String(this.UserInputClientID),
                    JavaScriptHelper.Boolean(this.AutoFocusInput),
                    JavaScriptHelper.Boolean(this.AutoClearInput),
                    JavaScriptHelper.Boolean(this.AutoUppercaseInput),
                    JavaScriptHelper.Boolean(this.AutoReloadExpiredCaptchas),
                    CodeCollection.CodeTimeout,
                    this.AutoReloadTimeout,
                    this.SoundStartDelay
                );

                return HtmlHelper.ScriptFragment(scriptFragment);
            }
        }

        string _userInputClientID;
        /// <summary>
        /// User input textbox client-side identifier (the ASP.NET ClientID 
        /// property), used for all client-side user input processing, e.g. 
        /// automatic user input uppercasing and focusing
        /// </summary>
        public string UserInputClientID
        {
            get
            {
                return _userInputClientID;
            }
            set
            {
                _userInputClientID = value;
            }
        }

        /// <summary>
        /// Is automatic user input focusing on Reload and Sound icon clicks 
        /// enabled (read from the configuration)
        /// </summary>
        public bool AutoFocusInput
        {
            get
            {
                return CaptchaConfiguration.CaptchaUserInput.AutoFocus;
            }
        }

        /// <summary>
        /// Is automatic user input clearing on Reload and Sound icon clicks 
        /// enabled (read from the configuration)
        /// </summary>
        public bool AutoClearInput
        {
            get
            {
                return CaptchaConfiguration.CaptchaUserInput.AutoClear;
            }
        }

        /// <summary>
        /// Is automatic user input uppercasing enabled (read from the configuration)
        /// </summary>
        public bool AutoUppercaseInput
        {
            get
            {
                return CaptchaConfiguration.CaptchaUserInput.AutoUppercase;
            }
        }

        /// <summary>
        /// Is automatic reloading of Captcha images after the Captcha code 
        /// expires enabled (read from the configuration)
        /// </summary>
        public bool AutoReloadExpiredCaptchas
        {
            get
            {
                return CaptchaConfiguration.CaptchaReloading.AutoReloadExpiredCaptchas.Enabled;
            }
        }

        /// <summary>
        /// After how many seconds does the automatic reloading stop, to keep 
        /// the control from generating endless sessions (read from the configuration)
        /// </summary>
        public int AutoReloadTimeout
        {
            get
            {
                return CaptchaConfiguration.CaptchaReloading.AutoReloadExpiredCaptchas.Timeout;
            }
        }

        /// <summary>
        /// Detects is the current Http request a Captcha request (handled 
        /// by the BotDetect HttpHandler) or not
        /// </summary>
        public static bool IsCaptchaRequest
        {
            get
            {
                bool isCaptchaRequest = false;

                if (HttpContext.Current.Request.Path.Contains(CaptchaConfiguration.CaptchaUrls.RequestPath.Replace("~/", "")))
                {
                    isCaptchaRequest = true;
                }

                return isCaptchaRequest;
            }
        }

        /// <summary>
        /// Url of Captcha image generated for the current control instance
        /// </summary>
        public string CaptchaImageUrl
        {
            get
            {
                return this._urls.CaptchaImageUrl(CaptchaId, CurrentInstanceId);
            }
        }


        /// <summary>
        /// Url of Captcha sound generated for the current control instance
        /// </summary>
        public string CaptchaSoundUrl
        {
            get
            {
                return this._urls.CaptchaSoundUrl(CaptchaId, CurrentInstanceId);
            }
        }


        /// <summary>
        /// Should the icons be rendered (true if at least one of the icons will be rendered)
        /// </summary>
        public bool RenderIcons
        {
            get
            {
                return (this.ReloadIconEnabled || this.CaptchaSoundEnabled);
            }
        }


        internal Status _useSmallIcons = Status.Unknown;
        /// <summary>
        /// Is the selected Captcha image height so low that we should switch 
        /// to smaller icons
        /// </summary>
        public bool UseSmallIcons
        {
            get
            {
                bool useSmallIcons = false;
                switch (_useSmallIcons)
                {
                    case Status.True:
                        useSmallIcons = true;
                        break;

                    case Status.False:
                        useSmallIcons = false;
                        break;

                    case Status.Unknown: // only used if not set by user
                        useSmallIcons = (this.ImageSize.Height < 50);
                        break;
                }
                return useSmallIcons;
            }

            set
            {
                if (value)
                {
                    _useSmallIcons = Status.True;
                }
                else
                {
                    _useSmallIcons = Status.False;
                }
            }
        }


        internal Status _useHorizontalIcons = Status.Unknown;

        /// <summary>
        /// Is the selected Captcha image height so low that we should switch 
        /// to horizontal icon rendering
        /// </summary>
        public bool UseHorizontalIcons
        {
            get
            {
                bool useHorizontalIcons = false;
                switch (_useHorizontalIcons)
                {
                    case Status.True:
                        useHorizontalIcons = true;
                        break;

                    case Status.False:
                        useHorizontalIcons = false;
                        break;

                    case Status.Unknown: // only used if not set by user
                        useHorizontalIcons = (this.ImageSize.Height < 40);
                        break;
                }
                return useHorizontalIcons;
            }

            set
            {
                if (value)
                {
                    _useHorizontalIcons = Status.True;
                }
                else
                {
                    _useHorizontalIcons = Status.False;
                }
            }
        }

        // icon layout definitions
        public const int IconSize = 22;
        public const int SmallIconSize = 17;
        public const int IconSpacing = 2;

        /// <summary>
        /// width of the custom icon file, if used
        /// </summary>
        public int IconWidth
        {
            get
            {
                // user-set value takes precedence
                int iconWidth = 0;
                if (CaptchaConfiguration.CaptchaReloading.Enabled)
                {
                    iconWidth = CaptchaConfiguration.CaptchaReloading.ReloadIcon.IconWidth;
                }
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    iconWidth = Math.Max(iconWidth, CaptchaConfiguration.CaptchaSound.SoundIcon.IconWidth);
                }
                if (0 < iconWidth)
                {
                    return iconWidth;
                }

                // default values, image size dependent
                if (this.UseSmallIcons)
                {
                    return SmallIconSize;
                }

                return IconSize;
            }
        }


        internal int _iconsDivWidth;

        /// <summary>
        ///  width of the CaptchaIconsDiv element, affecting icon layout
        /// </summary>
        public int IconsDivWidth
        {
            get
            {
                // use user-set value, if available
                if (0 < _iconsDivWidth)
                {
                    return _iconsDivWidth;
                }

                // default values, layout dependent
                if (this.UseHorizontalIcons)
                {
                    return 2 * this.IconWidth + 4 * IconSpacing;
                }
                else
                {
                    return this.IconWidth + IconSpacing;
                }
            }

            set
            {
                _iconsDivWidth = value;
            }
        }

        /// <summary>
        /// Calculated width of the Captcha container <div>, based on Captcha 
        /// image width and should the icons be rendered or not
        /// </summary>
        public int TotalWidth
        {
            get
            {
                return this.ImageSize.Width + 6 + this.IconsDivWidth;
            }
        }

        /// <summary>
        /// Calculated height of the Captcha container <div>, based on Captcha image height
        /// </summary>
        public int TotalHeight
        {
            get
            {
                return this.ImageSize.Height;
            }
        }

        /// <summary>
        /// Url of the currently configured Sound icon
        /// </summary>
        public string SoundIconUrl
        {
            get
            {
                if (CaptchaConfiguration.CaptchaSound.SoundPackages.WarnAboutMissingSoundPackages && 
                    !this.CaptchaSoundAvailable)
                {
                    return this._urls.DisabledSoundIconUrl;
                }

                string soundIconPath = CaptchaConfiguration.CaptchaSound.SoundIcon.FilePath;
                if (!StringHelper.HasValue(soundIconPath))
                {
                    return DefaultSoundIconUrl;
                }

                // user-configured application-relative path
                if (soundIconPath.StartsWith("~/"))
                {
                    return VirtualPathUtility.ToAbsolute(soundIconPath);
                }

                // user-configured absolute path
                if (soundIconPath.StartsWith("http"))
                {
                    return soundIconPath;
                }

                return DefaultSoundIconUrl;
            }
        }


        /// <summary>
        /// Url of the default Captcha Sound icon, depends on the current 
        /// Captcha image height (a smaller icon is used for Captcha images 
        /// less than 40 pixels high)
        /// </summary>
        public string DefaultSoundIconUrl
        {
            get
            {
                // default value
                if (UseSmallIcons)
                {
                    return this._urls.SmallSoundIconUrl;
                }
                else
                {
                    return this._urls.SoundIconUrl;
                }
            }
        }

        /// <summary>
        /// Url of the currently configured Reload icon
        /// </summary>
        public string ReloadIconUrl
        {
            get
            {
                string reloadIconPath = CaptchaConfiguration.CaptchaReloading.ReloadIcon.FilePath;
                if (!StringHelper.HasValue(reloadIconPath))
                {
                    // default value
                    return DefaultReloadIconUrl;
                }

                // user-configured application-relative path
                if (reloadIconPath.StartsWith("~/"))
                {
                    return VirtualPathUtility.ToAbsolute(reloadIconPath);
                }

                // user-configured absolute path
                if (reloadIconPath.StartsWith("http"))
                {
                    return reloadIconPath;
                }

                // default value
                return DefaultReloadIconUrl;
            }
        }


        /// <summary>
        /// Url of the default Captcha Reload icon, depends on the current 
        /// Captcha image height (a smaller icon is used for Captcha images 
        /// less than 40 pixels high)
        /// </summary>
        public string DefaultReloadIconUrl
        {
            get
            {
                // default value
                if (UseSmallIcons)
                {
                    return this._urls.SmallReloadIconUrl;
                }
                else
                {
                    return this._urls.ReloadIconUrl;
                }
            }
        }


        const short TabIndexNotSet = -1;
        private short _tabindexStart = TabIndexNotSet;

        /// <summary>
        /// Starting tabindex for the Captcha control Html output. There are 
        /// three keyboard-selectable Captcha markup elements: the Captcha 
        /// image help link, the Captcha sound icon and the Captcha reload
        /// icon. Depending on your settings (whether the Captcha help link
        /// is enabled, are Captcha sounds enabled, is Captcha reloading 
        /// enabled), the next available tabindex on the page can be from 0 to
        /// 3 greater than this value.
        /// </summary>
        public short TabIndex
        {
            get
            {
                return _tabindexStart;
            }
            set
            {
                _tabindexStart = value;
            }
        }

        /// <summary>
        /// Has the TabIndex property been set
        /// </summary>
        public bool IsTabIndexSet
        {
            get
            {
                return (TabIndexNotSet != _tabindexStart);
            }
        }

        /// <summary>
        /// Client-side ID of the <img> element used for Captcha images
        /// </summary>
        public string ImageClientId
        {
            get
            {
                return CaptchaId + "_CaptchaImage";
            }
        }

        /// <summary>
        /// Client-side ID of the empty placeholder <div> used for Captcha sound playback
        /// </summary>
        public string AudioPlaceholderClientId
        {
            get
            {
                return CaptchaId + "_AudioPlaceholder";
            }
        }

        /// <summary>
        /// Name of the hidden field used to pass InstanceId between postbacks
        /// </summary>
        public string ValidatingInstanceKey
        {
            get { return "LBD_VCID_" + CaptchaId; }
        }

        /// <summary>
        /// InstanceId of the previous CaptchaControl instance that is being 
        /// submitted, set only if IsSubmit is true
        /// </summary>
        public string ValidatingInstanceId
        {
            get
            {
                string validatingInstanceId = null;
                if (null != HttpContext.Current.Request.Form[this.ValidatingInstanceKey])
                {
                    validatingInstanceId = HttpContext.Current.Request.Form[this.ValidatingInstanceKey] as string;
                }
                return validatingInstanceId;
            }
        }

        /// <summary>
        /// Detect is the current CaptchaControl instance created after a 
        /// postback which includes Captcha code input for the previous instance
        /// </summary>
        public bool IsSumbit
        {
            get
            {
                bool isSubmit = false;
                string validatingInstanceId = this.ValidatingInstanceId;

                if (StringHelper.HasValue(validatingInstanceId))
                {
                    isSubmit = CaptchaBase.ValidCaptchaId.IsMatch(validatingInstanceId);
                }

                return isSubmit;
            }
        }

        /// <summary>
        /// Did the SessionTroubleshooting helper detect any problems with the 
        /// current control instance and Session persistence
        /// </summary>
        public bool IsSessionProblemDetected
        {
            get
            {
                return SessionTroubleshooting.IsSessionProblemDetected(this);
            }
        }

        // different Validate() overloads for different concrete control implementations

        /// <summary>
        /// Validate the Captcha from a server-side call, using the specified user 
        /// input and the instanceId automatically propagated using a hidden field
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public bool Validate(string userInput)
        {
            return _captchaBase.Validate(userInput, this.ValidatingInstanceId, ValidationAttemptOrigin.Server);
        }

        /// <summary>
        /// Validate the Captcha from an Ajax call, using the specified user 
        /// input and the instanceId automatically propagated using a hidden field
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public bool AjaxValidate(string userInput)
        {
            return _captchaBase.Validate(userInput, this.ValidatingInstanceId, ValidationAttemptOrigin.Client);
        }

        /// <summary>
        /// Validate the Captcha from a server-side call, using the specified 
        /// user input and instanceId. Used when automatic hidden field 
        /// propagation doesn't work
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public bool Validate(string userInput, string instanceId)
        {
            return _captchaBase.Validate(userInput, instanceId, ValidationAttemptOrigin.Server);
        }

        /// <summary>
        /// Validate the Captcha from an Ajax call, using the specified user 
        /// input and instanceId. Used when automatic hidden field propagation 
        /// doesn't work
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public bool AjaxValidate(string userInput, string instanceId)
        {
            return _captchaBase.Validate(userInput, instanceId, ValidationAttemptOrigin.Client);
        }

        /// <summary>
        /// Validate the Captcha from a server-side call, specifying all 
        /// validation parameters. Used in cases when a CaptchaControl 
        /// instance is not available
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="userInput"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public static bool Validate(string captchaId, string userInput, string instanceId)
        {
            CaptchaControl captchaControl = CaptchaPersistence.Load(captchaId);
            return captchaControl.Validate(userInput, instanceId);
        }

        /// <summary>
        /// Validate the Captcha from an Ajax call, specifying all 
        /// validation parameters. Used in cases when a CaptchaControl 
        /// instance is not available
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="userInput"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public static bool AjaxValidate(string captchaId, string userInput, string instanceId)
        {
            CaptchaControl captchaControl = CaptchaPersistence.Load(captchaId);
            return captchaControl.AjaxValidate(userInput, instanceId);
        }
    }
}
