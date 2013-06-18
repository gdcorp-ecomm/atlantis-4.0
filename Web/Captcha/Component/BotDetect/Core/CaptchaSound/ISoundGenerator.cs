using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    internal interface ISoundGenerator
    {
        IAudio GenerateSound(string code, Localization localization);
        int Volume { get; set; }
    }
}
