using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    class PulseSoundGenerator : SoundGenerator, ISoundGenerator
    {
        protected override void GenerateNoiseAndEffects()
        {
            // tremolo 
            Tremolo tremolo = new Tremolo();
            tremolo.TremoloSpacingRange = new RandomRange(80, 100);
            tremolo.TremoloDurationRange = new RandomRange(40, 50);
            tremolo.TremoloGainPercentageRange = new RandomRange(33, 66);
            tremolo.Apply(track);

            // slow bass beats
            ToneNoise tone = new ToneNoise();
            tone.Frequency = 110;
            tone.Volume = 50;
            tone.SeparatingSilence = 330;
            tone.Duration = 330;
            AudioTrack bassTrack = tone.Generate(track.Duration);
            track = Mixer.Merge(track, bassTrack);

            // echo
            Echo echo = new Echo();
            echo.EchoDelay = 200;
            echo.EchoGain = 50;
            echo.Apply(track);  
        }
    }
}
