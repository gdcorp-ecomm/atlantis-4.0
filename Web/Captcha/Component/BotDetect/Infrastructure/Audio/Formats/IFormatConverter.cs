using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal interface IFormatConverter
    {
        IAudio Convert(IAudio inputSound, SoundFormat outputFormat);

        // instead of having a many-to-many map allowing conversion directly 
        // from each SoundFormat to every other SoundFormat, we use an intermediary
        // format (used for all sound processing) to simplify the process
        byte[] ConvertToWavPcm16bit8kHzMono(byte[] inputHeaderlessSound);
        byte[] ConvertFromWavPcm16bit8kHzMono(byte[] inputHeaderlessSound);
    }
}
