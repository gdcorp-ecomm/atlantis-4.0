using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    internal sealed class SoundGeneratorFactory
    {
        private SoundGeneratorFactory()
        {
        }

        public static ISoundGenerator CreateGenerator(SoundStyle style)
        {
            ISoundGenerator generator = null;

            switch (style)
            {
                case SoundStyle.Radio:
                {
                    generator = new RadioSoundGenerator();
                    break;
                }
                case SoundStyle.HiveMind:
                {
                    generator = new HiveMindSoundGenerator();
                    break;
                }
                case SoundStyle.Pulse:
                {
                    generator = new PulseSoundGenerator();
                    break;
                }
                case SoundStyle.Industrial:
                {
                    generator = new IndustrialSoundGenerator();
                    break;
                }
                case SoundStyle.Synth:
                {
                    generator = new SynthSoundGenerator();
                    break;
                }
                case SoundStyle.RedAlert:
                {
                    generator = new RedAlertSoundGenerator();
                    break;
                }
                case SoundStyle.Dispatch:
                {
                    generator = new DispatchSoundGenerator();
                    break;
                }
                case SoundStyle.Workshop:
                {
                    generator = new WorkshopSoundGenerator();
                    break;
                }
                case SoundStyle.Robot:
                {
                    generator = new RobotSoundGenerator();
                    break;
                }
                case SoundStyle.Scratched:
                {
                    generator = new ScratchedSoundGenerator();
                    break;
                }
                default:
                {
                    throw new NotImplementedException("Sound Generator not implemented!");
                }
            }

            return generator;
        }
    }
}
