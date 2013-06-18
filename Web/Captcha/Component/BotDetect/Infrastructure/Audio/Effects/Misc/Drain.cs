using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Drain : Effect, IEffect
    {
        public override void Apply(Int16[] inputSamples)
        {
            int coin = RandomGenerator.Next(-1, 0);
            int sign = Math.Sign(coin);

            for (int i = 0; i < inputSamples.Length; i++)
            {
                if (inputSamples[i] > 0)
                {
                    inputSamples[i] = (Int16)(sign * inputSamples[i]);
                }
                else
                {
                    inputSamples[i] = 0;
                }
            }

            Normalize normalize = new Normalize();
            normalize.Apply(inputSamples);
        }
    }
}
