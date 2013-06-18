using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Reverse : Effect, IEffect
    {
        public override void Apply(Int16[] inputSamples)
        {
            int size = inputSamples.Length;
            Int16[] inputCopy = new Int16[size];
            inputSamples.CopyTo(inputCopy, 0);
            for (int i = 0; i < size; i++)
            {
                inputSamples[i] = inputCopy[size - i - 1];
            }
        }
    }
}
