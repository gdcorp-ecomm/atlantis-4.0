using System;

using BotDetect.Audio;

namespace BotDetect
{
	/// <summary>
	/// Output Captcha sound format
	/// </summary>
	public enum SoundFormat
	{
        Unknown = 0,
        WavPcm16bit8kHzMono = 1,
        WavPcm8bit8kHzMono = 2,
	}

    internal enum SoundFormatFamily
    {
        WavPcm = 0 // PCM
        // Mp3, TODO
        // Ogg, TODO
    }

    internal class SoundFormatHelper
    {
        private SoundFormatHelper()
        {
        }

        public static SoundFormatFamily GetFamily(SoundFormat format)
        {
            SoundFormatFamily family;

            switch (format)
            {
                case SoundFormat.WavPcm16bit8kHzMono:
                    family = SoundFormatFamily.WavPcm;
                    break;

                case SoundFormat.WavPcm8bit8kHzMono:
                    family = SoundFormatFamily.WavPcm;
                    break;

                default:
                    throw new AudioException("Unknown SoundFormat", format);
            }

            return family;
        }
    }
}
