using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Reflection;

using BotDetect.Audio;
using BotDetect.Drawing;

using BotDetect.Configuration;
using BotDetect.Logging;

using BotDetect.CaptchaCode;
using BotDetect.CaptchaImage;
using BotDetect.CaptchaSound;

namespace BotDetect
{
    /// <summary>
    /// A base Captcha object, representing a generic Captcha instance capable 
    /// of generating Captcha codes, images and sounds + validating user inputs. 
    /// The CaptchaControl instances tailored for System.Web use all delegate 
    /// core Captcha functionality to a contained CaptchaBase instance.
    /// Also, if you were for example implementing a Captcha class tailored 
    /// for Windows Forms applications, you would implement a 
    /// CaptchaWinFormControl class, which would implement desktop 
    /// context-specific functionality and delegate Captcha functionality 
    /// to a contained CaptchaBase instance.
    /// </summary>
	[Serializable]
    public class CaptchaBase
    {
        public CaptchaBase(string captchaId)
        {
            _captchaId = captchaId;
            _codeCollection = new CodeCollection();
            _instanceId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            _localization = CaptchaDefaults.Localization;
            _localization.SoundPackagesFolder = CaptchaBase.SoundPackagesFolder;
        }

        /// <summary>
        /// The global regular expression object used to validate CaptchaId 
        /// values and ensure they match the persistence medium key limitations
        /// </summary>
        public static readonly Regex ValidCaptchaId = 
            new Regex("[a-f0-9]+", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private string _captchaId;
        /// <summary>
        /// Captcha identifier, distinguishing multiple Captcha instances 
        /// within the system. Each Captcha used in a different location 
        /// (web page or Windows Form or mobile screen) must have a unique 
        /// identifier
        /// </summary>
        public string CaptchaId
        {
            get
            {
                return _captchaId;
            }
            set
            {
                _captchaId = value;
            }
        }

        private string _instanceId;
        /// <summary>
        /// Globally unique identifier of the current CaptchaBase object 
        /// instance
        /// </summary>
        public string InstanceId
        {
            get
            {
                return _instanceId;
            }
        }

        private Localization _localization;
        internal Localization Localization
        {
            get
            {
                return _localization;
            }
            set
            {
                _localization = value;
            }
        }

        /// <summary>
        /// Locale string, affects the character set used for Captcha code 
        /// generation and the pronunciation language used for Captcha 
        /// sound generation
        /// </summary>
        public string Locale
        {
            get
            {
                return _localization.ToString();
            }
            set
            {
                _localization = LocaleParser.Parse(value);
                _localization.SoundPackagesFolder = CaptchaBase.SoundPackagesFolder;
            }
        }

        /// <summary>
        /// Is the BotDetect pronunciation sound package for the currently 
        /// used locale available
        /// </summary>
        public bool IsLocalizedPronunciationAvailable
        {
            get
            {
                return SoundGeneratorFacade.IsPronunciationAvailable(_localization);
            }
        }

        private int _codeLength = CaptchaDefaults.CodeLength;
        /// <summary>
        /// Length (number of characters) of the Captcha code rendered; 
        /// the default value is 5
        /// </summary>
        public int CodeLength
        {
            get
            {
                return _codeLength;
            }
            set
            {
                _codeLength = value;
            }
        }

        private CodeStyle _codeStyle = CaptchaDefaults.CodeStyle;
        /// <summary>
        /// Code style, i.e. the algorithm used to generate Captcha codes; 
        /// the default value is Alphanumeric
        /// </summary>
        public CodeStyle CodeStyle
        {
            get
            {
                return _codeStyle;
            }
            set
            {
                _codeStyle = value;
            }
        }

        private string _customCharacterSetName = CaptchaDefaults.CustomCharacterSetName;
        /// <summary>
        /// Optional name of the user-defined character set used for Captcha 
        /// code generation. A collection of custom character sets can be 
        /// defined in the currently used Captcha configuration provider
        /// </summary>
        public string CustomCharacterSetName
        {
            get
            {
                return _customCharacterSetName;
            }
            set
            {
                _customCharacterSetName = value;
            }
        }

        private ImageStyle? _imageStyle = null;
        
        public ImageStyle? ImageStyleNullable
        {
            get
            {
                return _imageStyle;
            }
            set
            {
                _imageStyle = value;
            }
        }

        /// <summary>
        /// Image style, i.e. the algorithm used to render Captcha codes 
        /// in images; if no ImageStyle is set, it is randomized by default
        /// </summary>
        public ImageStyle ImageStyle
        {
            get
            {
                if (null != _imageStyle)
                {
                    return (ImageStyle) _imageStyle;
                }
                else
                {
                    return CaptchaRandomization.GetRandomImageStyle();
                }
            }
            set
            {
                _imageStyle = value;
            }
        }


        private ImageFormat _imageFormat = CaptchaDefaults.ImageFormat;
        /// <summary>
        /// Image format in which the Captcha image will be rendered; 
        /// the default format is JPEG
        /// </summary>
        public ImageFormat ImageFormat
        {
            get
            {
                return _imageFormat;
            }
            set
            {
                _imageFormat = value;
            }
        }

        private ImageSize _imageSize = CaptchaDefaults.ImageSize;
        public ImageSize ImageSize
        {
            get
            {
                return _imageSize;
            }
            set
            {
                _imageSize = value;
            }
        }

        private Color _customLightColor = CaptchaDefaults.CustomLightColor;
        /// <summary>
        /// Optional custom light color point, modifies the color palette used 
        /// for Captcha image drawing
        /// </summary>
        public Color CustomLightColor
        {
            get
            {
                return _customLightColor;
            }
            set
            {
                _customLightColor = value;
            }
        }

        private Color _customDarkColor = CaptchaDefaults.CustomDarkColor;
        /// <summary>
        /// Optional custom dark color point, modifies the color palette used 
        /// for Captcha image drawing
        /// </summary>
        public Color CustomDarkColor
        {
            get
            {
                return _customDarkColor;
            }
            set
            {
                _customDarkColor = value;
            }
        }

        
        private SoundStyle? _soundStyle = null;

        public SoundStyle? SoundStyleNullable
        {
            get
            {
                return _soundStyle;
            }
            set
            {
                _soundStyle = value;
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
                if (null != _soundStyle)
                {
                    return (SoundStyle)_soundStyle;
                }
                else
                {
                    return CaptchaRandomization.GetRandomSoundStyle();
                }
            }
            set
            {
                _soundStyle = value;
            }
        }


        private SoundFormat _soundFormat = CaptchaDefaults.SoundFormat;
        /// <summary>
        /// Audio format in which the Captcha sound will be generated; 
        /// the default format is WawPcm16bit8kHzMono
        /// </summary>
        public SoundFormat SoundFormat
        {
            get
            {
                return _soundFormat;
            }
            set
            {
                _soundFormat = value;
            }
        }


        /// <summary>
        /// constant help link height
        /// </summary>
        public const int HelpLinkHeight = 10;


        /// <summary>
        /// Captcha codes for the Captcha Id
        /// </summary>
        private CodeCollection _codeCollection;

        internal CodeCollection CodeCollection
        {
            get
            {
                return _codeCollection;
            }
            set
            {
                _codeCollection = value;
            }
        }


        // list of regexes defining banned sequences, used for swear word-filtering and the like
        private static List<string> _bannedSequences = new List<string>();
        private static readonly object _bannedSequencesLock = new object();

        /// <summary>
        /// immutable sequence collection
        /// </summary>
        internal static List<string> BannedSequences
        {
            get
            {
                return new List<string>(_bannedSequences);
            }
            set
            {
                if (!CollectionHelper.UnorderedEqual<string>(value, _bannedSequences))
                {
                    lock (_bannedSequencesLock)
                    {
                        if (!CollectionHelper.UnorderedEqual<string>(value, _bannedSequences))
                        {
                            _bannedSequences = new List<string>(value);
                            _bannedCharacterSequences = new BannedCharacterSequences(_bannedSequences);
                        }
                    }
                }
            }
        }

        private static BannedCharacterSequences _bannedCharacterSequences;
        /// <summary>
        /// Optional global list of sequences which will be filtered out of 
        /// the randomly generated Captcha codes, used for example for 
        /// swear-word filtering. The BannedCharacterSequences object is 
        /// created from the string values passed by the delegating classes
        /// </summary>
        public static BannedCharacterSequences BannedCharacterSequences
        {
            get
            {
                return _bannedCharacterSequences;
            }
        }

        private static string SoundPackagesFolder
        {
            get
            {
                string specifiedFolder = CaptchaConfiguration.CaptchaSound.SoundPackages.FolderPath;
                string resolvedPath = ServerHelper.ResolvePhysicalFolderPath(specifiedFolder);
                if (StringHelper.HasValue(resolvedPath))
                {
                    specifiedFolder = resolvedPath;
                }
                return specifiedFolder;
            }
        }

        private CharacterSet _customCharset = null;
        internal CharacterSet CustomCharset
        {
            get
            {
                return _customCharset;
            }
            set
            {
                _customCharset = value;
            }
        }

        # region Captcha events

        // captcha global events 
        internal static event EventHandler<GeneratingCaptchaCodeEventArgs> GeneratingCaptchaCode;
        internal static event EventHandler<GeneratedCaptchaCodeEventArgs> GeneratedCaptchaCode;

        internal static event EventHandler<GeneratingCaptchaImageEventArgs> GeneratingCaptchaImage;
        internal static event EventHandler<GeneratedCaptchaImageEventArgs> GeneratedCaptchaImage;

        internal static event EventHandler<GeneratingCaptchaSoundEventArgs> GeneratingCaptchaSound;
        internal static event EventHandler<GeneratedCaptchaSoundEventArgs> GeneratedCaptchaSound;

        internal static event EventHandler<ValidatingUserInputEventArgs> ValidatingUserInput;
        internal static event EventHandler<ValidatedUserInputEventArgs> ValidatedUserInput;

        // registered handlers for each captchaId
        private static Dictionary<string, bool> _registeredHandlers = new Dictionary<string, bool>();

        // event-specific locks
        private static object _GeneratingCaptchaCodeSyncRoot = new object();
        private static object _GeneratedCaptchaCodeSyncRoot = new object();
        private static object _GeneratingCaptchaImageSyncRoot = new object();
        private static object _GeneratedCaptchaImageSyncRoot = new object();
        private static object _GeneratingCaptchaSoundSyncRoot = new object();
        private static object _GeneratedCaptchaSoundSyncRoot = new object();
        private static object _ValidatingUserInputSyncRoot = new object();
        private static object _ValidatedUserInputSyncRoot = new object();

        // event-forwarding helpers
        private static string GetHandlerKey(string captchaId, string eventName, Delegate method)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}_{3}", captchaId, eventName, method.Method.Name, method.Method.DeclaringType.AssemblyQualifiedName);
        }

        private static bool IsHandlerRegistered(string captchaId, string eventName, Delegate method)
        {
            bool isRegistered = false;
            string key = GetHandlerKey(captchaId, eventName, method);

            if(null != _registeredHandlers && 
                _registeredHandlers.ContainsKey(key))
            {
                isRegistered = (bool)_registeredHandlers[key];
            }

            return isRegistered;
        }

        /// <summary>
        /// Propagate GeneratingCaptchaCode event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterGeneratingCaptchaCodeHandler(string captchaId, EventHandler<GeneratingCaptchaCodeEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.GeneratingCaptchaCode)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratingCaptchaCode", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_GeneratingCaptchaCodeSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.GeneratingCaptchaCode)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratingCaptchaCode", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.GeneratingCaptchaCode += method as EventHandler<GeneratingCaptchaCodeEventArgs>;

                                string key = GetHandlerKey(captchaId, "GeneratingCaptchaCode", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate GeneratedCaptchaCode event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterGeneratedCaptchaCodeHandler(string captchaId, EventHandler<GeneratedCaptchaCodeEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.GeneratedCaptchaCode)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratedCaptchaCode", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_GeneratedCaptchaCodeSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.GeneratedCaptchaCode)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratedCaptchaCode", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.GeneratedCaptchaCode += method as EventHandler<GeneratedCaptchaCodeEventArgs>;

                                string key = GetHandlerKey(captchaId, "GeneratedCaptchaCode", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate GeneratingCaptchaImage event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterGeneratingCaptchaImageHandler(string captchaId, EventHandler<GeneratingCaptchaImageEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.GeneratingCaptchaImage)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratingCaptchaImage", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_GeneratingCaptchaImageSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.GeneratingCaptchaImage)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratingCaptchaImage", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.GeneratingCaptchaImage += method as EventHandler<GeneratingCaptchaImageEventArgs>;

                                string key = GetHandlerKey(captchaId, "GeneratingCaptchaImage", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate GeneratedCaptchaImage event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterGeneratedCaptchaImageHandler(string captchaId, EventHandler<GeneratedCaptchaImageEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.GeneratedCaptchaImage)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratedCaptchaImage", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_GeneratedCaptchaImageSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.GeneratedCaptchaImage)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratedCaptchaImage", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.GeneratedCaptchaImage += method as EventHandler<GeneratedCaptchaImageEventArgs>;

                                string key = GetHandlerKey(captchaId, "GeneratedCaptchaImage", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate GeneratingCaptchaSound event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterGeneratingCaptchaSoundHandler(string captchaId, EventHandler<GeneratingCaptchaSoundEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.GeneratingCaptchaSound)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratingCaptchaSound", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_GeneratingCaptchaSoundSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.GeneratingCaptchaSound)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratingCaptchaSound", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.GeneratingCaptchaSound += method as EventHandler<GeneratingCaptchaSoundEventArgs>;

                                string key = GetHandlerKey(captchaId, "GeneratingCaptchaSound", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate GeneratedCaptchaSound event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterGeneratedCaptchaSoundHandler(string captchaId, EventHandler<GeneratedCaptchaSoundEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.GeneratedCaptchaSound)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratedCaptchaSound", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_GeneratedCaptchaSoundSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.GeneratedCaptchaSound)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "GeneratedCaptchaSound", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.GeneratedCaptchaSound += method as EventHandler<GeneratedCaptchaSoundEventArgs>;

                                string key = GetHandlerKey(captchaId, "GeneratedCaptchaSound", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate ValidatingUserInput event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterValidatingUserInputHandler(string captchaId, EventHandler<ValidatingUserInputEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.ValidatingUserInput)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "ValidatingUserInput", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_ValidatingUserInputSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.ValidatingUserInput)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "ValidatingUserInput", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.ValidatingUserInput += method as EventHandler<ValidatingUserInputEventArgs>;

                                string key = GetHandlerKey(captchaId, "ValidatingUserInput", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagate ValidatedUserInput event handlers registered in 
        /// delegating classes to the global event delegate.
        /// Uses "smart" event handler registration, avoiding duplicate handler 
        /// registrations.
        /// </summary>
        public static void RegisterValidatedUserInputHandler(string captchaId, EventHandler<ValidatedUserInputEventArgs> handler)
        {
            if (null != handler)
            {
                Delegate[] methodsToRegister = handler.GetInvocationList();
                foreach (Delegate method in methodsToRegister)
                {
                    // check is the method already registered as a handler
                    bool alreadyRegistered = false;
                    if (null != CaptchaBase.ValidatedUserInput)
                    {
                        alreadyRegistered = IsHandlerRegistered(captchaId, "ValidatedUserInput", handler);
                    }

                    // only register the passed handler if it's not already registered
                    if (!alreadyRegistered)
                    {
                        lock (_ValidatedUserInputSyncRoot)
                        {
                            // re-check, in case the lock could not be acquired immediately
                            if (null != CaptchaBase.ValidatedUserInput)
                            {
                                alreadyRegistered = IsHandlerRegistered(captchaId, "ValidatedUserInput", handler);
                            }

                            if (!alreadyRegistered)
                            {
                                CaptchaBase.ValidatedUserInput += method as EventHandler<ValidatedUserInputEventArgs>;

                                string key = GetHandlerKey(captchaId, "ValidatedUserInput", method);
                                _registeredHandlers.Add(key, true);
                            }
                        }
                    }
                }
            }
        }

# endregion

        #region GetCode

        /// <summary>
        /// Generates a random Captcha code for the given purpose, using 
        /// settings stored in instance data belonging to the specified 
        /// instanceId
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="purpose"></param>
        /// <returns></returns>
        public string GetCode(string instanceId, CodeGenerationPurpose purpose)
        {
            // preGenerate event
            GeneratingCaptchaCodeEventArgs ePre = new GeneratingCaptchaCodeEventArgs();
            ePre.CaptchaId = this.CaptchaId;
            ePre.CurrentInstanceId = this._instanceId;
            ePre.StoredCodes = _codeCollection;

            CaptchaLogging.Trace("GeneratingCaptchaCode", ePre);

            if (null != GeneratingCaptchaCode)
            {
                GeneratingCaptchaCode(this, ePre);
            }

            // generation
            if (null != _customCharset) // if custom charset is set through com interface... 
            {
                _codeCollection.CharacterSet = _customCharset;            
            }
            else
            {
                _codeCollection.CharacterSet = CharacterSetFactory.Get(_localization, _customCharacterSetName);
            }

            string code = _codeCollection.GetCode(instanceId, purpose, _codeStyle, _codeLength);

            // postGenerate event
            GeneratedCaptchaCodeEventArgs ePost = new GeneratedCaptchaCodeEventArgs();
            ePost.CaptchaId = this.CaptchaId;
            ePost.CurrentInstanceId = this._instanceId;
            ePost.Purpose = purpose;
            ePost.CodeStyle = _codeStyle;
            ePost.CodeLength = _codeLength;
            ePost.Code = code;
            ePost.StoredCodes = _codeCollection;

            CaptchaLogging.Trace("GeneratedCaptchaCode", ePost);

            if (null != GeneratedCaptchaCode)
            {
                GeneratedCaptchaCode(this, ePost);
            }

            return code;
        }

        #endregion

        #region GetImage

        /// <summary>
        /// Generates a Captcha image, using settings stored in instance 
        /// data belonging to the specified instanceId
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public MemoryStream GetImage(string instanceId)
        {
            // preGenerate event
            GeneratingCaptchaImageEventArgs ePre = new GeneratingCaptchaImageEventArgs();
            ePre.CaptchaId = this.CaptchaId;
            ePre.CurrentInstanceId = this._instanceId;

            CaptchaLogging.Trace("GeneratingCaptchaImage", ePre);

            if (null != GeneratingCaptchaImage)
            {
                GeneratingCaptchaImage(this, ePre);
            }

            // generation
            string code = this.GetCode(instanceId, CodeGenerationPurpose.ImageGeneration);

            ImageSize size = _imageSize;
            if (CaptchaConfiguration.CaptchaImage.HelpLink.Enabled && CaptchaConfiguration.CaptchaImage.HelpLink.Mode == HelpLinkMode.Text)
            {
                // image height adjustment is necessary when rendering a text link
                size = new ImageSize(size.Width, size.Height - CaptchaBase.HelpLinkHeight);
            }

            // if the user code specified a disabled image style, revert to default randomization
            if (CaptchaConfiguration.CaptchaImage.DisabledImageStyles.Styles.Contains(this.ImageStyle))
            {
                this.ImageStyle = CaptchaRandomization.GetRandomImageStyle();
            }

            MemoryStream image = ImageGeneratorFacade.GenerateImage(code, this.ImageStyle, _localization, size, _imageFormat, _customLightColor, _customDarkColor);

            // postGenerate event
            GeneratedCaptchaImageEventArgs ePost = new GeneratedCaptchaImageEventArgs();
            ePost.CaptchaId = this.CaptchaId;
            ePost.CurrentInstanceId = this._instanceId;
            ePost.ImageStyle = this.ImageStyle;
            ePost.ImageFormat = _imageFormat;
            ePost.ImageSize = _imageSize;
            ePost.CustomLightColor = _customLightColor;
            ePost.CustomDarkColor = _customDarkColor;
            ePost.Bytes = image.Length;

            CaptchaLogging.Trace("GeneratedCaptchaImage", ePost);

            if (null != GeneratedCaptchaImage)
            {
                GeneratedCaptchaImage(this, ePost);
            }

            return image;
        }

        #endregion

        #region GetSound

        /// <summary>
        /// Generates a Captcha image, using settings stored in instance 
        /// data belonging to the specified instanceId
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public MemoryStream GetSound(string instanceId)
        {
            // preGenerate event
            GeneratingCaptchaSoundEventArgs ePre = new GeneratingCaptchaSoundEventArgs();
            ePre.CaptchaId = this.CaptchaId;
            ePre.CurrentInstanceId = this._instanceId;

            CaptchaLogging.Trace("GeneratingCaptchaSound", ePre);

            if (null != GeneratingCaptchaSound)
            {
                GeneratingCaptchaSound(this, ePre);
            }

            // generation
            string code = this.GetCode(instanceId, CodeGenerationPurpose.SoundGeneration);

            // if the user code specified a disabled sound style, revert to default randomization
            if (CaptchaConfiguration.CaptchaSound.DisabledSoundStyles.Styles.Contains(this.SoundStyle))
            {
                this.SoundStyle = CaptchaRandomization.GetRandomSoundStyle();
            }

            MemoryStream sound = SoundGeneratorFacade.GenerateSound(code, this.SoundStyle, _localization, _soundFormat);

            // postGenerate event
            GeneratedCaptchaSoundEventArgs ePost = new GeneratedCaptchaSoundEventArgs();
            ePost.CaptchaId = this.CaptchaId;
            ePost.CurrentInstanceId = this._instanceId;
            ePost.SoundStyle = this.SoundStyle;
            ePost.SoundFormat = _soundFormat;
            ePost.Bytes = sound.Length;
            ePost.Duration = PcmSound.ByteCountToMilliseconds(sound.Length, _soundFormat);

            CaptchaLogging.Trace("GeneratedCaptchaSound", ePost);

            if (null != GeneratedCaptchaImage)
            {
                GeneratedCaptchaSound(this, ePost);
            }

            return sound;
        }

        # endregion

        # region Validate

        /// <summary>
        /// Compares the user input to the Captcha code stored for the given 
        /// instanceId, using validation rules specified for the current 
        /// ValidationAttemptOrigin
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="instanceId"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public bool Validate(string userInput, string instanceId, ValidationAttemptOrigin origin)
        {
            // preValidate event
            ValidatingUserInputEventArgs ePre = new ValidatingUserInputEventArgs();
            ePre.CaptchaId = _captchaId;
            ePre.CurrentInstanceId = this._instanceId;
            ePre.StoredCodes = _codeCollection;
            ePre.ValidatingInstanceId = instanceId;
            ePre.Origin = origin;
            ePre.UserInput = userInput;

            CaptchaLogging.Trace("ValidatingUserInput", ePre);

            if (null != ValidatingUserInput)
            {
                ValidatingUserInput(this, ePre);
            }

            // validation
            bool result = _codeCollection.Validate(userInput, instanceId, origin);

            // postValidate event
            ValidatedUserInputEventArgs ePost = new ValidatedUserInputEventArgs();
            ePost.CaptchaId = _captchaId;
            ePost.CurrentInstanceId = this._instanceId;
            ePost.StoredCodes = _codeCollection;
            ePost.ValidatingInstanceId = instanceId;
            ePost.Origin = origin;
            ePost.UserInput = userInput;
            ePost.Result = result;

            CaptchaLogging.Trace("ValidatedUserInput", ePost);

            if (null != ValidatedUserInput)
            {
                ValidatedUserInput(this, ePost);
            }

            return result;
        }

        # endregion
    }
}
