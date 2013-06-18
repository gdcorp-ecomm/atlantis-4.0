using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal interface IEffect
    {
        // base method each effect implements
        void Apply(Int16[] inputSamples);

        // uttility methods for audio track processing
        void Apply(AudioTrack track);

        // duration of each individual processed section
        int Duration { get; set; }
        RandomRange DurationRange { get; set; }

        // delay before the first processed section
        int StartingDelay { get; set; }
        RandomRange StartingDelayRange { get; set; }

        // delay between each processed section
        int SeparatingDelay { get; set; }
        RandomRange SeparatingDelayRange { get; set; }

        // delay after last processed section
        int EndingDelay { get; set; }
        RandomRange EndingDelayRange { get; set; }
    }
}
