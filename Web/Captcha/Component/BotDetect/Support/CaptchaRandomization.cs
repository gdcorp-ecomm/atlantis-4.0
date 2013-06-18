using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;
using BotDetect.Drawing;

using BotDetect.Configuration;

using BotDetect.CaptchaCode;

namespace BotDetect
{
    /// <summary>
    /// A helper class used for easy randomization of Captcha parameters.
    /// </summary>
    public sealed class CaptchaRandomization
    {
        private CaptchaRandomization()
        {
        }

        /// <summary>
        /// Returns a random Captcha code length, using default code length bounds
        /// </summary>
        /// <returns></returns>
        public static int GetRandomCodeLength()
        {
            return GetRandomCodeLength(0, 0);
        }

        /// <summary>
        /// Returns a random Captcha code length, using the default minimal 
        /// value and the specified maximal value
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomCodeLength(int max)
        {
            return GetRandomCodeLength(0, max);
        }

        /// <summary>
        /// Returns a random Captcha code length, using the specified minimal 
        /// and maximal values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomCodeLength(int min, int max)
        {
            if ((max > CaptchaDefaults.MaxCodeLength) || (max < CaptchaDefaults.MinCodeLength))
            {
                max = CaptchaDefaults.MaxCodeLength;
            }

            if ((min < CaptchaDefaults.MinCodeLength) || (min > max))
            {
                min = CaptchaDefaults.MinCodeLength;
            }

            return RandomGenerator.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random Captcha code style. Selects from all available 
        /// styles if no parameter is specified, or from the given value 
        /// set if specified
        /// </summary>
        /// <param name="usedValues"></param>
        /// <returns></returns>
        public static CodeStyle GetRandomCodeStyle(params CodeStyle[] usedValues)
        {
            CodeStyle codeStyle = CaptchaDefaults.CodeStyle;

            if (0 == usedValues.Length)
            {
                int max = Enum.GetValues(typeof(CodeStyle)).Length;

                codeStyle = (CodeStyle)(RandomGenerator.Next(max));
            }
            else if (1 == usedValues.Length)
            {
                codeStyle = usedValues[0];
            }
            else
            {
                int max = usedValues.Length;
                int index = RandomGenerator.Next(max);

                codeStyle = usedValues[index];
            }

            return codeStyle;
        }

        /// <summary>
        /// Returns a random Captcha image style. Selects from all available 
        /// styles if no parameter is specified, or from the given value set 
        /// if specified
        /// </summary>
        /// <param name="usedValues"></param>
        /// <returns></returns>
        public static ImageStyle GetRandomImageStyle(params ImageStyle[] usedValues)
        {
            ImageStyle? imageStyle = null;

            // working with sets is easier
            Set<ImageStyle> usedStyles = new Set<ImageStyle>(usedValues);
            
            // if the user didn't specify the possible values, use all available
            if (0 == usedStyles.Count)
            {
                usedStyles = new Set<ImageStyle>(Enum.GetValues(typeof(ImageStyle)) as IEnumerable<ImageStyle>);
            }

            // remove disabled styles from selection
            Set<ImageStyle> disabledStyles = CaptchaConfiguration.CaptchaImage.DisabledImageStyles.Styles;
            usedStyles -= disabledStyles;

            // if the disabled styles include all used ones, notify the user to correct their error
            // (this will only happen if the user disables all values they passed as usedValues, or 
            // configures BotDetect to disable ALL styles)
            if (0 == usedStyles.Count)
            {
                throw new CaptchaConfigurationException("Disabled ImageStyles include all possible values", usedValues, disabledStyles);
            }
            else
            {
                // randomly select one of the usable image styles
                imageStyle = usedStyles.Next();
            }
           
            return (ImageStyle)imageStyle;
        }

        /// <summary>
        /// Returns a random Captcha sound style. Selects from all available 
        /// styles if no parameter is specified, or from the given value 
        /// set if specified
        /// </summary>
        /// <param name="usedValues"></param>
        /// <returns></returns>
        public static SoundStyle GetRandomSoundStyle(params SoundStyle[] usedValues)
        {
            SoundStyle? soundStyle = null;

            // working with sets is easier
            Set<SoundStyle> usedStyles = new Set<SoundStyle>(usedValues);

            // if the user didn't specify the possible values, use all available
            if (0 == usedStyles.Count)
            {
                usedStyles = new Set<SoundStyle>(Enum.GetValues(typeof(SoundStyle)) as IEnumerable<SoundStyle>);
            }

            // remove disabled styles from selection
            Set<SoundStyle> disabledStyles = CaptchaConfiguration.CaptchaSound.DisabledSoundStyles.Styles;
            usedStyles -= disabledStyles;

            // if the disabled styles include all used ones, notify the user to correct their error
            // (this will only happen if the user disables all values they passed as usedValues, or 
            // configures BotDetect to disable ALL styles)
            if (0 == usedStyles.Count)
            {
                throw new CaptchaConfigurationException("Disabled SoundStyles include all possible values", usedValues, disabledStyles);
            }
            else
            {
                // randomly select one of the usable image styles
                soundStyle = usedStyles.Next();
            }

            return (SoundStyle)soundStyle;
        }
    }
}
