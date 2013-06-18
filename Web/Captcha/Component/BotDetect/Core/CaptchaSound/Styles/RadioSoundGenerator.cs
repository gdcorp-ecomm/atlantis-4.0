using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class RadioSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // voice modification
            Drain drain = new Drain();
            drain.SeparatingDelay = track.Duration / 6;
            drain.Duration = track.Duration / 5;
            drain.Apply(track);

            // random white noise in short bursts
            WhiteNoise noise = new WhiteNoise();
            noise.VolumeRange = new RandomRange(1, 6);
            noise.Duration = track.Duration / 10;
            AudioTrack noiseTrack = noise.Generate(track.Duration);
            track = Mixer.Merge(track, noiseTrack);

        }
    }
}
