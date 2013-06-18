using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class IndustrialSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // fast bass beat
            ToneNoise tone = new ToneNoise();
            tone.Frequency = 110;
            tone.VolumeRange = new RandomRange(40, 50);
            tone.DurationRange = new RandomRange(100, 120);
            AudioTrack bassTrack = tone.Generate(track.Duration);
            track = Mixer.Merge(track, bassTrack);

            // heavy distortion
            Overdrive overdrive = new Overdrive();
            overdrive.Level = 6;
            overdrive.AdjustedVolume = 25;
            overdrive.SeparatingDelay = track.Duration / 6;
            overdrive.Duration = track.Duration / 5;
            overdrive.Apply(track);
        }
    }
}
