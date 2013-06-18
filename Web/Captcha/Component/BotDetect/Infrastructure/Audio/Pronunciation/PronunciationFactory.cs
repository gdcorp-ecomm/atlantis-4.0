using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal abstract class PronunciationFactory
    {
        public static IPronunciation Get(Localization localization)
        {
            IPronunciation generator = new Pronunciation(localization);
            return generator;
        }
    }
}
