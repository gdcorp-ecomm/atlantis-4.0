using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class RedAlertSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // "siren"
            ToneNoise tone = new ToneNoise();
            tone.FrequencyRange = new RandomRange(740, 780);
            tone.VolumeRange = new RandomRange(3, 8);
            tone.StartingSilenceRange = new RandomRange(20, 200);
            tone.SeparatingSilenceRange = new RandomRange(300, 350);
            tone.DurationRange = new RandomRange(300, 350);
            AudioTrack bassTrack = tone.Generate(track.Duration);
            track = Mixer.Merge(track, bassTrack);

            // reverb
            Reverb reverb = new Reverb();
            reverb.EchoDelay = 60;
            reverb.ReflectionPercentage = 30;
            reverb.Apply(track);

            // short fades to simulate "appearance"
            FadeOut fadeOut = new FadeOut();
            fadeOut.FadeOutDurationRange = new RandomRange(10, 50);
            fadeOut.Apply(track);

            FadeIn fadeIn = new FadeIn();
            fadeIn.FadeInDurationRange = new RandomRange(10, 50);
            fadeIn.Apply(track);
        }
    }
}
