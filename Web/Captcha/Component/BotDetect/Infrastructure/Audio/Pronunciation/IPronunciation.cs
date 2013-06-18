using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal interface IPronunciation
    {
        // base method each pronunciation generator implements
        Int16[] Pronounce(char character);

        // utility methods for generating pronunciation tracks
        AudioTrack Pronounce(string textToPronounce);
        AudioTrack TestPronounce(string textToPronounce);

        // silence before the first character pronunciation
        int StartingSilence { get; set; }
        RandomRange StartingSilenceRange { get; set; }

        // silence between each character pronunciation
        int SeparatingSilence { get; set; }
        RandomRange SeparatingSilenceRange { get; set; }

        // silence after last character pronunciation
        int EndingSilence { get; set; }
        RandomRange EndingSilenceRange { get; set; }

        // configuration - the path to the pronunciation SoundPackage
        string SoundPackageFilePath{ get; }

        // check does the SoundPackage exist and can be parsed
        bool IsPronunciationAvailable();
    }
}
