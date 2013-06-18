using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Invert : Effect, IEffect
    {
        public override void Apply(Int16[] inputSamples)
        {
            for (int i = 0; i < inputSamples.Length; i++)
            {
                inputSamples[i] = (Int16)(-1 * inputSamples[i]);
            }
        }
    }
}
