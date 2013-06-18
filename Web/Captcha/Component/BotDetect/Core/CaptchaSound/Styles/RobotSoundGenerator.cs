using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class RobotSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GeneratePronunciation()
        {
            // extra silence at the start of the track
            IPronunciation pronouncer = PronunciationFactory.Get(localization);
            pronouncer.StartingSilenceRange = new RandomRange(500, 700);
            pronouncer.SeparatingSilenceRange = new RandomRange(-100, 800);
            pronouncer.EndingSilenceRange = new RandomRange(-20, 200);
            track = pronouncer.Pronounce(code);
        }

        protected override void GenerateNoiseAndEffects()
        {
            // keep a copy of the clear pronunciation
            AudioTrack clear = new AudioTrack(track);

            // random quiet white noise
            WhiteNoise noise = new WhiteNoise();
            noise.Volume = 4;
            noise.Duration = track.Duration;
            AudioTrack noiseTrack = noise.Generate(track.Duration);
            track = Mixer.Merge(track, noiseTrack);

            // ring modulation for sharp distortion
            ToneNoise tone = new ToneNoise();
            tone.Frequency = 330;
            tone.Volume = 1;
            tone.Duration = track.Duration;
            AudioTrack bassTrack = tone.Generate(track.Duration);
            AudioTrack modulated = Mixer.RingModulate(track, bassTrack);
            track = Mixer.Merge(track, modulated);

            // repeated clear pronunciation for greater clarity
            Gain silencer = new Gain();
            silencer.GainPercentage = 33;
            silencer.Apply(clear);
            track = Mixer.Merge(track, clear);
        }
    }
}
