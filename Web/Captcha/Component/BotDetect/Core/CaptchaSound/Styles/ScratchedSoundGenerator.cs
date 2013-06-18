using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class ScratchedSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // vinyl "scratches"
            Compressor expander = new Compressor();
            expander.Peak = track.PeakAmplitude;
            expander.MaxAmplitudePercentage = 65;
            expander.SeparatingDelayRange = new RandomRange(25, 250);
            expander.DurationRange = new RandomRange(25, 50);
            expander.Apply(track);

            // quiet noise
            WhiteNoise noise = new WhiteNoise();
            noise.Volume = 1;
            noise.Duration = track.Duration;
            AudioTrack noiseTrack = noise.Generate(track.Duration);
            track = Mixer.Merge(track, noiseTrack); 
        }
    }
}
