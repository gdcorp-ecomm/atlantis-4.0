using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using BotDetect.Persistence;
using BotDetect.Drawing;

namespace BotDetect.Web
{
    /// <summary>
    /// This class encapsulates BotDetect Captcha persistence operations, 
    /// saving and loading Captcha instance data from the configured 
    /// persistence provider.
    /// </summary>
    public sealed class CaptchaPersistence
    {
        private static IPersistenceProvider _userState = AspNetSessionPersistenceProvider.Persistence;

        /// <summary>
        /// Saves necessary state from the given CaptchaControl instance
        /// </summary>
        /// <param name="captcha"></param>
        public static void Save(CaptchaControl captcha)
        {
            string captchaId = captcha.CaptchaId;

            SaveLocale(captchaId, captcha.Locale);

            SaveCodeLength(captchaId, captcha.CodeLength);
            SaveCodeStyle(captchaId, captcha.CodeStyle);
            SaveCustomCharacterSetName(captchaId, captcha.CustomCharacterSetName);

            SaveImageFormat(captchaId, captcha.ImageFormat);
            SaveImageSize(captchaId, captcha.CaptchaBase.ImageSize);
            SaveImageStyle(captchaId, captcha.CaptchaBase.ImageStyleNullable);
            SaveCustomLightColor(captchaId, captcha.CaptchaBase.CustomLightColor);
            SaveCustomDarkColor(captchaId, captcha.CaptchaBase.CustomDarkColor);

            SaveSoundFormat(captchaId, captcha.SoundFormat);
            SaveSoundStyle(captchaId, captcha.CaptchaBase.SoundStyleNullable);

            SaveCodeCollection(captchaId, captcha.CaptchaBase.CodeCollection);

            SaveUseHorizontalIcons(captchaId, captcha._useHorizontalIcons);
            SaveUseSmallIcons(captchaId, captcha._useSmallIcons);
            SaveIconsDivWidth(captchaId, captcha._iconsDivWidth);

            SaveCaptchaImageTooltip(captchaId, captcha._captchaImageTooltip);
            SaveReloadIconTooltip(captchaId, captcha._reloadIconTooltip);
            SaveSoundIconTooltip(captchaId, captcha._soundIconTooltip);
        }

        /// <summary>
        /// Loads the CaptchaControl instance stored with the given captchaId
        /// </summary>
        /// <param name="captchaId"></param>
        /// <returns></returns>
        public static CaptchaControl Load(string captchaId)
        {
            CaptchaBase captcha = new CaptchaBase(captchaId);
            Load(captcha, captchaId);

            CaptchaControl captchaControl = new CaptchaControl(captcha);

            LoadUseHorizontalIcons(captchaControl, captchaId);
            LoadUseSmallIcons(captchaControl, captchaId);
            LoadIconsDivWidth(captchaControl, captchaId);

            LoadCaptchaImageTooltip(captchaControl, captchaId);
            LoadReloadIconTooltip(captchaControl, captchaId);
            LoadSoundIconTooltip(captchaControl, captchaId);

            captchaControl.Initialize();

            return captchaControl;
        }

        /// <summary>
        /// Updates the CaptchaControl instance according to state persisted 
        /// for the given captchaId value
        /// </summary>
        /// <param name="control"></param>
        /// <param name="captchaId"></param>
        public static void Load(CaptchaControl control, string captchaId)
        {
            Load(control.CaptchaBase, captchaId);

            LoadUseHorizontalIcons(control, captchaId);
            LoadUseSmallIcons(control, captchaId);
            LoadIconsDivWidth(control, captchaId);

            LoadCaptchaImageTooltip(control, captchaId);
            LoadReloadIconTooltip(control, captchaId);
            LoadSoundIconTooltip(control, captchaId);
        }

        /// <summary>
        /// Updates the CaptchaBase instance according to state persisted 
        /// for the given captchaId value
        /// </summary>
        /// <param name="captcha"></param>
        /// <param name="captchaId"></param>
        public static void Load(CaptchaBase captcha, string captchaId)
        {
            captcha.CaptchaId = captchaId;

            LoadLocale(captcha, captchaId);

            LoadCodeLength(captcha, captchaId);
            LoadCodeStyle(captcha, captchaId);
            LoadCustomCharacterSetName(captcha, captchaId);

            LoadImageFormat(captcha, captchaId);
            LoadImageSize(captcha, captchaId);
            LoadImageStyle(captcha, captchaId);
            LoadCustomLightColor(captcha, captchaId);
            LoadCustomDarkColor(captcha, captchaId);

            LoadSoundFormat(captcha, captchaId);
            LoadSoundStyle(captcha, captchaId);

            LoadCodeCollection(captcha, captchaId);
        }

        /// <summary>
        /// Saves only updated code information from the given CaptchaControl
        /// instance
        /// </summary>
        /// <param name="captcha"></param>
        public static void SaveCodes(CaptchaControl captcha)
        {
            string captchaId = captcha.CaptchaId;
            SaveCodeCollection(captchaId, captcha.CaptchaBase.CodeCollection);
        }

        /// the Locale is stored in user persistence using this key
        private static string GetLocaleKey(string captchaId)
        {
            return "LBD_Locale_" + captchaId;
        }

        private static void LoadLocale(CaptchaBase captcha, string captchaId)
        {
            string LocaleKey = GetLocaleKey(captchaId);
            if ((null != _userState) && (_userState.Contains(LocaleKey)))
            {
                captcha.Locale = (string)_userState[LocaleKey];
            }
        }

        private static void SaveLocale(string captchaId, string value)
        {
            string LocaleKey = GetLocaleKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.Locale != value)
                {
                    _userState[LocaleKey] = value;
                }
                else
                {
                    _userState.Remove(LocaleKey);
                }
            }
        }

        /// the codeLength is stored in user persistence using this key
        private static string GetCodeLengthKey(string captchaId)
        {
            return "LBD_CodeLength_" + captchaId;
        }

        private static void LoadCodeLength(CaptchaBase captcha, string captchaId)
        {
            string codeLengthKey = GetCodeLengthKey(captchaId);
            if ((null != _userState) && (_userState.Contains(codeLengthKey)))
            {
                captcha.CodeLength = (int)_userState[codeLengthKey];
            }
        }

        private static void SaveCodeLength(string captchaId, int value)
        {
            string codeLengthKey = GetCodeLengthKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.CodeLength != value)
                {
                    _userState[codeLengthKey] = value;
                }
                else
                {
                    _userState.Remove(codeLengthKey);
                }
            }
        }

        /// the codeStyle is stored in user persistence using this key
        private static string GetCodeStyleKey(string captchaId)
        {
            return "LBD_CodeStyle_" + captchaId;
        }

        private static void LoadCodeStyle(CaptchaBase captcha, string captchaId)
        {
            string codeStyleKey = GetCodeStyleKey(captchaId);
            if ((null != _userState) && (_userState.Contains(codeStyleKey)))
            {
                captcha.CodeStyle = (CodeStyle)_userState[codeStyleKey];
            }
        }

        private static void SaveCodeStyle(string captchaId, CodeStyle value)
        {
            string codeStyleKey = GetCodeStyleKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.CodeStyle != value)
                {
                    _userState[codeStyleKey] = value;
                }
                else
                {
                    _userState.Remove(codeStyleKey);
                }
            }
        }

        /// the customCharacterSetName is stored in user persistence using this key
        private static string GetCustomCharacterSetNameKey(string captchaId)
        {
            return "LBD_CustomCharacterSetName_" + captchaId;
        }

        private static void LoadCustomCharacterSetName(CaptchaBase captcha, string captchaId)
        {
            string customCharacterSetNameKey = GetCustomCharacterSetNameKey(captchaId);
            if ((null != _userState) && (_userState.Contains(customCharacterSetNameKey)))
            {
                captcha.CustomCharacterSetName = (string)_userState[customCharacterSetNameKey];
            }
        }

        private static void SaveCustomCharacterSetName(string captchaId, string value)
        {
            string customCharacterSetNameKey = GetCustomCharacterSetNameKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.CustomCharacterSetName != value)
                {
                    _userState[customCharacterSetNameKey] = value;
                }
                else
                {
                    _userState.Remove(customCharacterSetNameKey);
                }
            }
        }

        /// the imageStyle is stored in user persistence using this key
        private static string GetImageStyleKey(string captchaId)
        {
            return "LBD_ImageStyle_" + captchaId;
        }

        private static void LoadImageStyle(CaptchaBase captcha, string captchaId)
        {
            string imageStyleKey = GetImageStyleKey(captchaId);
            if ((null != _userState) && (_userState.Contains(imageStyleKey)))
            {
                captcha.ImageStyleNullable = (ImageStyle?) _userState[imageStyleKey];
            }
        }

        private static void SaveImageStyle(string captchaId, ImageStyle? value)
        {
            string imageStyleKey = GetImageStyleKey(captchaId);

            if (null != _userState)
            {
                if (null != value)
                {
                    _userState[imageStyleKey] = value;
                }
                else
                {
                    _userState.Remove(imageStyleKey);
                }
            }
        }

        /// the imageFormat is stored in user persistence using this key
        private static string GetImageFormatKey(string captchaId)
        {
            return "LBD_ImageFormat_" + captchaId;
        }

        private static void LoadImageFormat(CaptchaBase captcha, string captchaId)
        {
            string imageFormatKey = GetImageFormatKey(captchaId);
            if ((null != _userState) && (_userState.Contains(imageFormatKey)))
            {
                captcha.ImageFormat = (ImageFormat)_userState[imageFormatKey];
            }
        }

        private static void SaveImageFormat(string captchaId, ImageFormat value)
        {
            string imageFormatKey = GetImageFormatKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.ImageFormat != value)
                {
                    _userState[imageFormatKey] = value;
                }
                else
                {
                    _userState.Remove(imageFormatKey);
                }
            }
        }

        /// the imageSize is stored in user persistence using this key
        private static string GetImageSizeKey(string captchaId)
        {
            return "LBD_ImageSize_" + captchaId;
        }

        private static void LoadImageSize(CaptchaBase captcha, string captchaId)
        {
            string imageSizeKey = GetImageSizeKey(captchaId);
            if ((null != _userState) && (_userState.Contains(imageSizeKey)))
            {
                captcha.ImageSize = (ImageSize)_userState[imageSizeKey];
            }
        }

        private static void SaveImageSize(string captchaId, ImageSize value)
        {
            string imageSizeKey = GetImageSizeKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.ImageSize != value)
                {
                    _userState[imageSizeKey] = value;
                }
                else
                {
                    _userState.Remove(imageSizeKey);
                }
            }
        }

        /// the soundStyle is stored in user persistence using this key
        private static string GetSoundStyleKey(string captchaId)
        {
            return "LBD_SoundStyle_" + captchaId;
        }

        private static void LoadSoundStyle(CaptchaBase captcha, string captchaId)
        {
            string soundStyleKey = GetSoundStyleKey(captchaId);
            if ((null != _userState) && (_userState.Contains(soundStyleKey)))
            {
                captcha.SoundStyleNullable = (SoundStyle?)_userState[soundStyleKey];
            }
        }

        private static void SaveSoundStyle(string captchaId, SoundStyle? value)
        {
            string soundStyleKey = GetSoundStyleKey(captchaId);

            if (null != _userState)
            {
                if (null != value)
                {
                    _userState[soundStyleKey] = value;
                }
                else
                {
                    _userState.Remove(soundStyleKey);
                }
            }
        }
       

        /// the soundFormat is stored in user persistence using this key
        private static string GetSoundFormatKey(string captchaId)
        {
            return "LBD_SoundFormat_" + captchaId;
        }

        private static void LoadSoundFormat(CaptchaBase captcha, string captchaId)
        {
            string soundFormatKey = GetSoundFormatKey(captchaId);
            if ((null != _userState) && (_userState.Contains(soundFormatKey)))
            {
                captcha.SoundFormat = (SoundFormat)_userState[soundFormatKey];
            }
        }

        private static void SaveSoundFormat(string captchaId, SoundFormat value)
        {
            string soundFormatKey = GetSoundFormatKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.SoundFormat != value)
                {
                    _userState[soundFormatKey] = value;
                }
                else
                {
                    _userState.Remove(soundFormatKey);
                }
            }
        }


        /// the codeCollection is stored in user persistence using this key
        private static string GetCodeCollectionKey(string captchaId)
        {
            return "LBD_CodeCollection_" + captchaId;
        }

        private static void LoadCodeCollection(CaptchaBase captcha, string captchaId)
        {
            string codeCollectionKey = GetCodeCollectionKey(captchaId);
            if ((null != _userState) && (_userState.Contains(codeCollectionKey)))
            {
                captcha.CodeCollection = (CodeCollection)_userState[codeCollectionKey];
            }
        }

        private static void SaveCodeCollection(string captchaId, CodeCollection value)
        {
            string codeCollectionKey = GetCodeCollectionKey(captchaId);

            if (null != _userState)
            {
                if (null != value)
                {
                    _userState[codeCollectionKey] = value;
                }
                else
                {
                    _userState.Remove(codeCollectionKey);
                }
            }
        }

        // the customLightColor is saved in user persistence using this key
        private static string GetCustomLightColorKey(string captchaId)
        {
            return "LBD_CustomLightColor_" + captchaId;
        }

        private static void LoadCustomLightColor(CaptchaBase captcha, string captchaId)
        {
            string customLightColorKey = GetCustomLightColorKey(captchaId);
            if ((null != _userState) && (_userState.Contains(customLightColorKey)))
            {
                captcha.CustomLightColor = (Color)_userState[customLightColorKey];
            }
        }

        private static void SaveCustomLightColor(string captchaId, Color value)
        {
            string customLightColorKey = GetCustomLightColorKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.CustomLightColor != value)
                {
                    _userState[customLightColorKey] = value;
                }
                else
                {
                    _userState.Remove(customLightColorKey);
                }
            }
        }

        // the customDarkColor is saved in user persistence using this key
        private static string GetCustomDarkColorKey(string captchaId)
        {
            return "LBD_CustomDarkColor_" + captchaId;
        }

        private static void LoadCustomDarkColor(CaptchaBase captcha, string captchaId)
        {
            string customDarkColorKey = GetCustomDarkColorKey(captchaId);
            if ((null != _userState) && (_userState.Contains(customDarkColorKey)))
            {
                captcha.CustomDarkColor = (Color)_userState[customDarkColorKey];
            }
        }

        private static void SaveCustomDarkColor(string captchaId, Color value)
        {
            string customDarkColorKey = GetCustomDarkColorKey(captchaId);

            if (null != _userState)
            {
                if (CaptchaDefaults.CustomDarkColor != value)
                {
                    _userState[customDarkColorKey] = value;
                }
                else
                {
                    _userState.Remove(customDarkColorKey);
                }
            }
        }


        /// the captchaImageTooltip string is stored in user persistence using this key
        private static string GetCaptchaImageTooltipKey(string captchaId)
        {
            return "LBD_CaptchaImageTooltip_" + captchaId;
        }

        private static void LoadCaptchaImageTooltip(CaptchaControl captcha, string captchaId)
        {
            string captchaImageTooltipKey = GetCaptchaImageTooltipKey(captchaId);
            if ((null != _userState) && (_userState.Contains(captchaImageTooltipKey)))
            {
                captcha._captchaImageTooltip = (String)_userState[captchaImageTooltipKey];
            }
        }

        private static void SaveCaptchaImageTooltip(string captchaId, String value)
        {
            string captchaImageTooltipKey = GetCaptchaImageTooltipKey(captchaId);

            if (null != _userState)
            {
                if (StringHelper.HasValue(value))
                {
                    _userState[captchaImageTooltipKey] = value;
                }
                else
                {
                    _userState.Remove(captchaImageTooltipKey);
                }
            }
        }


        /// the reloadIconTooltip string is stored in user persistence using this key
        private static string GetReloadIconTooltipKey(string captchaId)
        {
            return "LBD_ReloadIconTooltip_" + captchaId;
        }

        private static void LoadReloadIconTooltip(CaptchaControl captcha, string captchaId)
        {
            string reloadIconTooltipKey = GetReloadIconTooltipKey(captchaId);
            if ((null != _userState) && (_userState.Contains(reloadIconTooltipKey)))
            {
                captcha._reloadIconTooltip = (String)_userState[reloadIconTooltipKey];
            }
        }

        private static void SaveReloadIconTooltip(string captchaId, String value)
        {
            string reloadIconTooltipKey = GetReloadIconTooltipKey(captchaId);

            if (null != _userState)
            {
                if (StringHelper.HasValue(value))
                {
                    _userState[reloadIconTooltipKey] = value;
                }
                else
                {
                    _userState.Remove(reloadIconTooltipKey);
                }
            }
        }


        /// the soundIconTooltip string is stored in user persistence using this key
        private static string GetSoundIconTooltipKey(string captchaId)
        {
            return "LBD_SoundIconTooltip_" + captchaId;
        }

        private static void LoadSoundIconTooltip(CaptchaControl captcha, string captchaId)
        {
            string soundIconTooltipKey = GetSoundIconTooltipKey(captchaId);
            if ((null != _userState) && (_userState.Contains(soundIconTooltipKey)))
            {
                captcha._soundIconTooltip = (String)_userState[soundIconTooltipKey];
            }
        }

        private static void SaveSoundIconTooltip(string captchaId, String value)
        {
            string soundIconTooltipKey = GetSoundIconTooltipKey(captchaId);

            if (null != _userState)
            {
                if (StringHelper.HasValue(value))
                {
                    _userState[soundIconTooltipKey] = value;
                }
                else
                {
                    _userState.Remove(soundIconTooltipKey);
                }
            }
        }


        /// the useHorizontalIcons flag is stored in user persistence using this key
        private static string GetUseHorizontalIconsKey(string captchaId)
        {
            return "LBD_UseHorizontalIcons_" + captchaId;
        }

        private static void LoadUseHorizontalIcons(CaptchaControl captcha, string captchaId)
        {
            string useHorizontalIconsKey = GetUseHorizontalIconsKey(captchaId);
            if ((null != _userState) && (_userState.Contains(useHorizontalIconsKey)))
            {
                captcha._useHorizontalIcons = (Status)_userState[useHorizontalIconsKey];
            }
        }

        private static void SaveUseHorizontalIcons(string captchaId, Status value)
        {
            string useHorizontalIconsKey = GetUseHorizontalIconsKey(captchaId);

            if (null != _userState)
            {
                if (Status.Unknown != value)
                {
                    _userState[useHorizontalIconsKey] = value;
                }
                else
                {
                    _userState.Remove(useHorizontalIconsKey);
                }
            }
        }


        /// the useSmallIcons flag is stored in user persistence using this key
        private static string GetUseSmallIconsKey(string captchaId)
        {
            return "LBD_UseSmallIcons_" + captchaId;
        }

        private static void LoadUseSmallIcons(CaptchaControl captcha, string captchaId)
        {
            string useSmallIconsKey = GetUseSmallIconsKey(captchaId);
            if ((null != _userState) && (_userState.Contains(useSmallIconsKey)))
            {
                captcha._useSmallIcons = (Status)_userState[useSmallIconsKey];
            }
        }

        private static void SaveUseSmallIcons(string captchaId, Status value)
        {
            string useSmallIconsKey = GetUseSmallIconsKey(captchaId);

            if (null != _userState)
            {
                if (Status.Unknown != value)
                {
                    _userState[useSmallIconsKey] = value;
                }
                else
                {
                    _userState.Remove(useSmallIconsKey);
                }
            }
        }


        /// the iconsDivWidth value is stored in user persistence using this key
        private static string GetIconsDivWidthKey(string captchaId)
        {
            return "LBD_IconsDivWidth_" + captchaId;
        }

        private static void LoadIconsDivWidth(CaptchaControl captcha, string captchaId)
        {
            string iconsDivWidthKey = GetIconsDivWidthKey(captchaId);
            if ((null != _userState) && (_userState.Contains(iconsDivWidthKey)))
            {
                captcha._iconsDivWidth = (int)_userState[iconsDivWidthKey];
            }
        }

        private static void SaveIconsDivWidth(string captchaId, int value)
        {
            string iconsDivWidthKey = GetIconsDivWidthKey(captchaId);

            if (null != _userState)
            {
                if (0 < value)
                {
                    _userState[iconsDivWidthKey] = value;
                }
                else
                {
                    _userState.Remove(iconsDivWidthKey);
                }
            }
        }


        /// <summary>
        /// Checks does persisted info exist for the supplied captchaId
        /// </summary>
        public static bool IsValid(string captchaId)
        {
            string codeCollectionKey = GetCodeCollectionKey(captchaId);

            if ((null != _userState) && (_userState.Contains(codeCollectionKey)))
            {
                return true;
            }

            return false;
        }

        public static new string ToString()
        {
            StringBuilder log = new StringBuilder();
            log.Append("BotDetect.CaptchaControlPersistence {");

            if (null == _userState)
            {
                log.Append(" null }");
            }
            else
            {
                log.AppendLine();

                // provider info
                log.AppendLine(StringHelper.ToString(_userState));

                // stored data
                TraceCaptchaPersistence(log);

                log.Append("}");
            }

            return log.ToString();
        }

        private static void TraceCaptchaPersistence(StringBuilder log)
        {
            log.Append("  persisted values {");
            if (0 == _userState.Count || null == _userState.Keys)
            {
                log.AppendLine(" empty }");
            }
            else
            {
                log.AppendLine();
                foreach (string key in _userState.Keys)
                {
                    if (BotDetectKeyPattern.IsMatch(key))
                    {
                        int start = key.IndexOf("LBD_", StringComparison.Ordinal);
                        string externalKey = key.Substring(start);

                        log.Append("    " + externalKey + ": ");
                        log.AppendLine(StringHelper.ToString(_userState[externalKey]));
                    }
                }
                log.AppendLine("  }");
            }
        }

        private static readonly Regex BotDetectKeyPattern = new Regex("PersistanceProvider_LBD_",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
}
