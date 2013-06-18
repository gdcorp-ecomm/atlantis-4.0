using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal abstract class Audio
    {
        // exact sound format
        protected SoundFormat format;
        public SoundFormat Format
        {
            get
            {
                return format;
            }
        }

        /*public Audio()
        {
        }

        public Audio(byte[] bRawData)
        {
        }

        public Audio(byte[] bHeaderlessData, SoundFormat format)
        {
        }

        public Audio(AudioTrack track)
        {
        }*/

        public static IAudio GetAudio(byte[] headerlessData, SoundFormat format)
        {
            IAudio sound = null;

            switch (format)
            {
                case SoundFormat.WavPcm16bit8kHzMono:
                case SoundFormat.WavPcm8bit8kHzMono:
                    sound = new PcmSound(headerlessData, format);
                    break;

                default:
                    throw new AudioException("Unknown SoundFormat", format);
            }

            return sound;
        }
    }
}
