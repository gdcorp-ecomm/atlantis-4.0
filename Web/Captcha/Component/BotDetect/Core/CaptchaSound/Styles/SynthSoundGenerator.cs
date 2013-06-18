using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class SynthSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // two tones creating a randomized "melody"
            ToneNoise eTone = new ToneNoise();
            eTone.Frequency = 660;
            eTone.Volume = 5;
            eTone.StartingSilence = 0;
            eTone.SeparatingSilenceRange = new RandomRange(1, 400);
            eTone.DurationRange = new RandomRange(20, 200);
            AudioTrack e1Track = eTone.Generate(track.Duration);
            track = Mixer.Merge(track, e1Track);

            ToneNoise aTone = new ToneNoise();
            aTone.Frequency = 440;
            aTone.Volume = 5;
            aTone.StartingSilence = 0;
            aTone.SeparatingSilenceRange = new RandomRange(1, 400);
            aTone.DurationRange = new RandomRange(20, 200);
            AudioTrack a1Track = aTone.Generate(track.Duration);
            track = Mixer.Merge(track, a1Track);

            // "synthesised" voice effect and tone distortion
            Expander expander = new Expander();
            expander.Peak = track.PeakAmplitude;
            expander.MinAmplitudePercentage = 5;
            expander.Apply(track);
        }
    }
}
