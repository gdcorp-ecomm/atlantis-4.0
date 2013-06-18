using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal sealed class Defaults
    {
        private Defaults()
        {
        }

        public static readonly RandomRange StartingSilence = new RandomRange(-20, 200);
        public static readonly RandomRange SeparatingSilence = new RandomRange(-100, 700);
        public const int MinAverageSeparatingSilence = 800;
        public static readonly RandomRange EndingSilence = new RandomRange(-20, 200);
    }
}
