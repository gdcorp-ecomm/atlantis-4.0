using System;
using System.IO;
using System.Reflection;

using BotDetect.Audio;
using BotDetect.Audio.Wav;

namespace BotDetect.CaptchaSound
{
	/// <summary>
	/// Summary description for SoundGenerator.
	/// </summary>
	internal sealed class SoundGeneratorFacade
	{
        private SoundGeneratorFacade()
		{
		}

        /// <summary>
        /// genetrates a sound stream using the specified parameters
        /// </summary>
        public static MemoryStream GenerateSound(string code, SoundStyle style, Localization localization,
            SoundFormat format)
        {

            

            if (CaptchaConfiguration.CaptchaCodes.TestMode.Enabled)
            {
                // just return a hard-coded test.wav file with the pronunciation
                return SoundGeneratorFacade.GetTestSound(format);
            }

            // normal sound generation
            return SoundGeneratorFacade.GetSound(code, style, localization, format);

        }

        public static MemoryStream GetDemoSound(SoundFormat format)
        {
            IAudio sound = GetSoundFromResource(".Infrastructure.Audio.Pronunciation.Resources.demo.wav");
            return sound.GetStream(format);
        }

        public static MemoryStream GetTestSound(SoundFormat format)
        {
            IAudio sound = GetSoundFromResource(".Infrastructure.Audio.Pronunciation.Resources.test.wav");
            return sound.GetStream(format);
        }

        public static MemoryStream GetSound(string code, SoundStyle style, Localization localization, SoundFormat format)
        {
            ISoundGenerator generator = SoundGeneratorFactory.CreateGenerator(style);
            IAudio sound = generator.GenerateSound(code, localization);
            return sound.GetStream(format);
		}

        private static IAudio GetSoundFromResource(string identifier)
        {
            return new PcmSound(ResourceHelper.GetResource(identifier));
        }

        public static bool IsPronunciationAvailable(Localization localization)
        {
            // the default sound package is embedded in the .dll, so we don't have to check
            if (localization.Language == CaptchaDefaults.Localization.Language && 
                localization.Country == CaptchaDefaults.Localization.Country)
            {
                return true;
            }

            IPronunciation pronouncer = PronunciationFactory.Get(localization);
            return pronouncer.IsPronunciationAvailable();
        }
	}
}
