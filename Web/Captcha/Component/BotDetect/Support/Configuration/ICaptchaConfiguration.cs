using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace BotDetect.Configuration
{
    /// <summary>
    /// Captcha global configuration settings
    /// </summary>
    public interface ICaptchaConfiguration
    {
        ICaptchaCodesConfiguration CaptchaCodes { get; }
        ICaptchaImageConfiguration CaptchaImage { get; }
        ICaptchaSoundConfiguration CaptchaSound { get; }
        ICaptchaReloadingConfiguration CaptchaReloading { get; }
        ICaptchaUserInputConfiguration CaptchaUserInput { get; }
        ICaptchaLoggingConfiguration CaptchaLogging { get; }
        ICaptchaUrlConfiguration CaptchaUrls { get; }
        ICaptchaEncryptionConfiguration CaptchaEncryption { get; }
        ICaptchaRequestFilterConfiguration CaptchaRequestFilter { get; }
        ICaptchaHttpHandlerTroubleshootingConfiguration CaptchaHttpHandlerTroubleshooting { get; }
        ICaptchaSessionTroubleshootingConfiguration CaptchaSessionTroubleshooting { get; }
    }

    /// <summary>
    /// Captcha codes settings
    /// </summary>
    public interface ICaptchaCodesConfiguration
    {
        /// <summary>
        /// Captcha code timeout in seconds - the Captcha can only be successfuly 
        /// solved within the specified time after generation. This is an optional 
        /// security improvement that narrows the window of opportunity for attacks 
        /// based on re-using the Captcha image on another site controlled by the
        /// attacker, or similar human-solver-based attacks on Captcha-protected 
        /// forms.
        /// </summary>
        int Timeout { get; }

        ICharacterSetCollectionConfiguration CharacterSets { get; }

        ITestModeConfiguration TestMode { get; }
    }

    /// <summary>
    /// Defines custom character sets for Captcha code generation. You can then 
    /// map the custom character sets defined here to appropriate Captcha control 
    /// instances by name in code-behind or .aspx designer.
    /// </summary>
    public interface ICharacterSetCollectionConfiguration
    {
        /// <summary>
        /// Access a single custom character set declaration by name
        /// </summary>
        /// <param name="name">Custom charset name</param>
        /// <returns></returns>
        ICharacterSetConfiguration this[string name] { get; }
    }

    /// <summary>
    /// A single custom character set declaration
    /// </summary>
    public interface ICharacterSetConfiguration
    {
        /// <summary>
        /// custom charset name, by which it can be applied to Captcha control 
        /// instances
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Comma-separated list of alphanumeric characters. If you don't want to 
        /// distinguish between code styles, you can specify the alphanumeric 
        /// characters only and they will be used for all Captcha codes regardless 
        /// of the codeStyle value set 
        /// </summary>
        StringCollection Alphanumeric { get; }

        /// <summary>
        /// Comma-separated list of characters used for "Alpha" CodeStyle codes
        /// </summary>
        StringCollection Alpha { get; }

        /// <summary>
        /// Comma-separated list of characters used for "Numeric" CodeStyle codes
        /// </summary>
        StringCollection Numeric { get; }
    }

    /// <summary>
    /// Test mode switch
    /// </summary>
    public interface ITestModeConfiguration
    {
        /// <summary>
        /// Test mode, set to "true" during automated testing of your page to make
        /// the Captcha trivially solvable (always use the "TEST" code). Be careful
        /// not to enable this on production websites since it will allow trivial 
        /// Captcha bypassing for bots but human users will still have to solve it.
        /// </summary>
        bool Enabled { get; }
    }

    /// <summary>
    /// Captcha image settings
    /// </summary>
    public interface ICaptchaImageConfiguration
    {
        /// <summary>
        /// Custom Captcha image alt text
        /// </summary>
        ILocalizedStringConfiguration Tooltip { get; }

        /// <summary>
        /// Allows disabling individual Captcha image styles
        /// </summary>
        IDisabledImageStylesConfiguration DisabledImageStyles { get; }

        /// <summary>
        /// The Captcha image is also a link to a locale-dependent help page
        /// </summary>
        IHelpLinkConfiguration HelpLink { get; }
    }


    [Serializable]
    public enum HelpLinkMode
    {
        Text = 0,
        Image
    }

    public interface IHelpLinkConfiguration
    {
        /// <summary>
        /// Should a help link be added to Captcha markup
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// How should the help link be displayed
        /// </summary>
        HelpLinkMode Mode { get; }

        /// <summary>
        /// Custom help page Url (abosulte or application-relative)
        /// </summary>
        ILocalizedStringConfiguration HelpPage { get; }

        /// <summary>
        /// Text/title of the help link
        /// </summary>
        ILocalizedStringConfiguration HelpText { get; }
    }

    public interface IDisabledImageStylesConfiguration
    {
        Set<ImageStyle> Styles { get; }

        /// <summary>
        /// Comma-separated list of disabled ImageStyle names
        /// </summary>
        StringCollection Names { get; }
    }

    /// <summary>
    /// Captcha sound settings
    /// </summary>
    public interface ICaptchaSoundConfiguration
    {
        /// <summary>
        /// Are Captcha sounds enabled
        /// </summary>
        bool Enabled { get; }

        int StartDelay { get; }

        ISoundPackagesConfiguration SoundPackages { get; }

        ICaptchaSoundIconConfiguration SoundIcon { get; }

        /// <summary>
        /// Allows disabling individual Captcha sound styles
        /// </summary>
        IDisabledSoundStylesConfiguration DisabledSoundStyles { get; }
    }

    /// <summary>
    /// Custom BotDetect SoundPackage location
    /// </summary>
    public interface ISoundPackagesConfiguration
    {
        /// <summary>
        /// Custom sound packages folder, you can reuse a single location for all 
        /// ASP.NET applications using BotDetect Captcha on the same server, and 
        /// don't have to copy the SoundPackage files to each and every 
        /// application's Bin folder.
        /// </summary>
        string FolderPath { get; }

        /// <summary>
        /// Captcha sounds in BotDetect 3.0 require locale-dependent pronunciation 
        /// SoundPackage (.bdsp) files. If the required SoundPackage in not deployed,
        /// the sound icon is not clickable and displays a warning tooltip by default. 
        /// If you want to disable this warning and simply not display the sound 
        /// Captcha icon at all for locales whose pronunciations are not deployed 
        /// with the application, set this property to "false".
        /// </summary>
        bool WarnAboutMissingSoundPackages { get; }
    }

    public interface IDisabledSoundStylesConfiguration
    {
        Set<SoundStyle> Styles { get; }

        /// <summary>
        /// Comma-separated list of disabled SoundStyle names
        /// </summary>
        StringCollection Names { get; }
    }

    /// <summary>
    /// Sound Captcha icon settings
    /// </summary>
    public interface ICaptchaSoundIconConfiguration
    {
        /// <summary>
        /// Custom Captcha sound icon image file
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Custom Captcha sound icon image width (affects icon layout)
        /// </summary>
        int IconWidth { get; }

        /// <summary>
        /// Custom Captcha sound icon title
        /// </summary>
        ILocalizedStringConfiguration Tooltip { get; }
    }

    /// <summary>
    /// Captcha reloading settings
    /// </summary>
    public interface ICaptchaReloadingConfiguration
    {
        /// <summary>
        /// Is Captcha reloading enabled
        /// </summary>
        bool Enabled { get; }

        ICaptchaReloadIconConfiguration ReloadIcon { get; }

        IAutoReloadExpiredCaptchasConfiguration AutoReloadExpiredCaptchas { get; }
    }

    /// <summary>
    /// Reload Captcha icon settings
    /// </summary>
    public interface ICaptchaReloadIconConfiguration
    {
        /// <summary>
        /// Custom Captcha reload icon image file
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Custom Captcha sound icon image width (affects icon layout)
        /// </summary>
        int IconWidth { get; }

        /// <summary>
        /// Custom Captcha reload icon title
        /// </summary>
        ILocalizedStringConfiguration Tooltip { get; }
    }

    /// <summary>
    /// Captcha images are automatically reloaded when the Captcha code expires 
    /// (as set in the <captchaCodes timeout="value"> element, or the ASP.NET 
    /// Session timeout), but only within a certain interval from their first 
    /// generation. This allows you to have a short Captcha code timeout (e.g. 
    /// 2 minutes) to narrow the window of opportunity for Captcha reusing on 
    /// other sites or human-solver-powered bots, and actual visitors can still 
    /// fill out your form at their own pace and without rushing (since the 
    /// Captcha image will be reloaded automatically when it is no longer valid). 
    /// Since we don't want infinite sessions when the user leaves the form open 
    /// in a background browser tab over the weekend (for example), we set a 
    /// reasonable upper limit on the auto-reload period (e.g. 2 hours = 7200 
    /// seconds).
    /// </summary>
    public interface IAutoReloadExpiredCaptchasConfiguration
    {
        /// <summary>
        /// Should auto-reloading be enabled
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Max time during which the auto-reloading will work, in seconds
        /// </summary>
        int Timeout { get; }
    }

    /// <summary>
    /// Captcha user input textbox client ID can be registered for each Captcha 
    /// control instance in code-behind or the .aspx designer, with several 
    /// client-side options then becoming available.
    /// </summary>
    public interface ICaptchaUserInputConfiguration
    {
        /// <summary>
        /// Anything the users type in the input textbox will be uppercase on 
        /// the fly, since Captcha validation is not and should not be case-
        /// sensitive. This is a small usability improvement that helps 
        /// communicate that fact to the users clearly.
        /// </summary>
        bool AutoUppercase { get; }

        /// <summary>
        /// The input texbox will be cleared on all Reload icon clicks and auto-
        /// reloads, since any previous input in the textbox will be invalidated 
        /// by Captcha reloading. This is a small usability improvement that 
        /// helps users avoid having to delete the previous input themselves.
        /// </summary>
        bool AutoClear { get; }

        /// <summary>
        /// The input texbox will be assigned focus on all Captcha Sound and 
        /// Captcha Reload icon clicks, allowing the users to more easily type 
        /// in the code as they hear it or as the new image loads. This does not 
        /// apply to auto-reloading of expired Captchas, since the user might 
        /// be filling out another field on the form when the auto-reload 
        /// starts and shouldn't be distracted.
        /// </summary>
        bool AutoFocus { get; }
    }

    /// <summary>
    /// Captcha logging settings
    /// </summary>
    public interface ICaptchaLoggingConfiguration
    {
        /// <summary>
        /// Should Captcha errors be logged
        /// </summary>
        bool ErrorLoggingEnabled { get; }

        /// <summary>
        /// Should Captcha traces be logged
        /// </summary>
        bool TraceEnabled { get; }

        /// <summary>
        /// Only events whose eventType matches this regex will be traced
        /// </summary>
        string EventFilter { get; }

        /// <summary>
        /// Fully qualified class name of the ILoggingProvider implementation to use
        /// </summary>
        string LoggingProvider { get; }
    }

    /// <summary>
    /// Captcha Url settings
    /// </summary>
    public interface ICaptchaUrlConfiguration
    {
        /// <summary>
        /// Custom Captcha HttpHandler path, in case the default 
        /// "BotDetectCaptcha.ashx" doesn't suit your application. You can 
        /// customize both the filename and the extension, but you must ensure 
        /// the IIS mapping for the custom extension is set to be processed by 
        /// the ASP.NET runtime. For example, you can use the ".jpg" extension
        /// only if you re-map ".jpg" requests to be handled by ASP.NET in your 
        /// application's virtual folder, instead of the default IIS file 
        /// system mapping for ".jpg" files.
        /// </summary>
        string RequestPath { get; }
    }

    /// <summary>
    /// Captcha encryption settings
    /// </summary>
    public interface ICaptchaEncryptionConfiguration
    {
        /// <summary>
        /// Custom SessionID encryption key, used to secure the SessionID value 
        /// that has to be passed in the sound Captcha querystring to avoid sound 
        /// mismatch problems in some older browsers. The SessionID is never 
        /// passed in plaintext querystring to avoid Session hijacking attacks, 
        /// and you should specify your own unique encryption key to secure it 
        /// properly.
        /// </summary>
        string EncryptionPassword { get; }
    }

    /// <summary>
    /// Specialized Captcha Http reqest filtering settings
    /// </summary>
    public interface ICaptchaRequestFilterConfiguration
    {
        /// <summary>
        /// Is specialized Captcha Http reqest filtering enabled
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Custom Captcha Http request validator, allowing only a certain number 
        /// of repeated requests with identical querystrings. Human users in 
        /// normal browsers will always use unique querystrings to access Captcha 
        /// images and sounds (the way the Captcha control works guarantees this), 
        /// while simple bots will often repeat the same request in short time 
        /// intervals. This is a simple measure that increases Captcha security 
        /// by stopping to serve Captcha images and sounds to such obvious bots 
        /// (Captcha is used for bot detection, and in such a case we can detect 
        /// that a bot is involved even without wasting resources on generating 
        /// more Captcha images or sounds).
        /// </summary>
        int AllowedRepeatedRequests { get; }
    }

    /// <summary>
    /// CaptchaSessionTroubleshooting switch
    /// </summary>
    public interface ICaptchaSessionTroubleshootingConfiguration
    {
        /// <summary>
        /// Is BotDetect Session state troubleshooting enabled
        /// </summary>
        bool Enabled { get; }
    }

    /// <summary>
    /// CaptchaHttpHandlerTroubleshooting switch
    /// </summary>
    public interface ICaptchaHttpHandlerTroubleshootingConfiguration
    {
        /// <summary>
        /// Is BotDetect HttpHandler troubleshooting enabled
        /// </summary>
        bool Enabled { get; }
    }

    /// <summary>
    /// A collection of Captcha locale-dependent strings
    /// </summary>
    public interface ILocalizedStringConfiguration
    {
        /// <summary>
        /// Access configured string by locale regex
        /// </summary>
        /// <param name="locale">locale regex</param>
        /// <returns></returns>
        string this[String locale] { get; }
    }
}
