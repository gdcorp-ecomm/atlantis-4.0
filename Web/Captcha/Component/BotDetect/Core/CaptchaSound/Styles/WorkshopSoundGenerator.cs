using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class WorkshopSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // random machine noise in short spaced bursts
            WhiteNoise noise = new WhiteNoise();
            noise.VolumeRange = new RandomRange(4, 6);
            noise.Duration = track.Duration / 5;
            noise.StartingSilenceRange = new RandomRange(10, track.Duration / 5);
            noise.SeparatingSilence = track.Duration / 6;
            AudioTrack noiseTrack = noise.Generate(track.Duration);
            track = Mixer.Merge(track, noiseTrack);

            // jagged distortion
            Phaser phaser = new Phaser();
            phaser.AttenuationPercentage = 40;
            phaser.Apply(track);
        }
    }
}
