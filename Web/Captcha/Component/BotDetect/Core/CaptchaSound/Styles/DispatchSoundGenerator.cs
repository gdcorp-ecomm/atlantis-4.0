using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class DispatchSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // random white noise, slowly changing volume
            WhiteNoise noise = new WhiteNoise();
            noise.VolumeRange = new RandomRange(3, 6);
            noise.Duration = track.Duration / 10;
            AudioTrack noiseTrack = noise.Generate(track.Duration);
            track = Mixer.Merge(track, noiseTrack);

            // flanger 
            Flanger flanger = new Flanger();
            flanger.AttenuationPercentage = 45;
            flanger.Apply(track);

            // fades to simulate "passing by"
            FadeOut fadeOut = new FadeOut();
            fadeOut.FadeOutDurationRange = new RandomRange(100, 300);
            fadeOut.Apply(track);

            FadeIn fadeIn = new FadeIn();
            fadeIn.FadeInDurationRange = new RandomRange(100, 300);
            fadeIn.Apply(track);
        }
    }
}
