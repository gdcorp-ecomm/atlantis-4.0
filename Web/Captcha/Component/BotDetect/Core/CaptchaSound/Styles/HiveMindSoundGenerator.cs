using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class HiveMindSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GeneratePronunciation()
        {
            // extra silence at the start of the track
            IPronunciation pronouncer = PronunciationFactory.Get(localization);
            pronouncer.StartingSilenceRange = new RandomRange(300, 500);
            pronouncer.SeparatingSilenceRange = new RandomRange(-200, 800);
            pronouncer.EndingSilenceRange = new RandomRange(-20, 200);
            track = pronouncer.Pronounce(code);
        }

        protected override void GenerateNoiseAndEffects()
        {
            // background tone
            ToneNoise tone = new ToneNoise();
            tone.Frequency = 330;
            tone.Volume = 10;
            tone.Duration = track.Duration;
            AudioTrack bassTrack = tone.Generate(track.Duration);
            track = Mixer.Merge(track, bassTrack);

            // multiple "voices" and "hypnotic" tone fluctuations
            Chorus chorus = new Chorus();
            chorus.ChorusDelay = 50;
            chorus.ChorusSweep = 100;
            chorus.Apply(track);

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
