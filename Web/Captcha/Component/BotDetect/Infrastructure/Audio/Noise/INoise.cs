using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal interface INoise
    {
        // base method each noise generator implements
        Int16[] Generate(int length, int volume);

        // utility method for generating noise tracks
        AudioTrack Generate(int totalDuration);

        // absolute volume of the noise
        int Volume { get; set; }
        RandomRange VolumeRange { get; set; }

        // duration of each individual noise section
        int Duration { get; set; }
        RandomRange DurationRange { get; set; }

        // silence before the first noise section
        int StartingSilence { get; set; }
        RandomRange StartingSilenceRange { get; set; }

        // silence between each noise section
        int SeparatingSilence { get; set; }
        RandomRange SeparatingSilenceRange { get; set; }

        // silence after last noise section
        int EndingSilence { get; set; }
        RandomRange EndingSilenceRange { get; set; }
    }
}
