using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Echo : Effect, IEffect
    {
        public const int DefaultEchoDelay = 250; // milliseconds
        public const int DefaultEchoVolumeGain = 50; // percent

        // echo delay, in milliseconds
        private RandomRange _echoDelayRange;

        /// <summary>
        /// Randomized echo delay, in milliseconds
        /// </summary>
        public RandomRange EchoDelayRange
        {
            get
            {
                return _echoDelayRange;
            }

            set
            {
                _echoDelayRange = value;
            }
        }

        /// <summary>
        /// Echo delay, in milliseconds
        /// </summary>
        public int EchoDelay
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _echoDelayRange)
                {
                    return DefaultEchoDelay;
                }

                return _echoDelayRange.Next;
            }
            set
            {
                _echoDelayRange = new RandomRange(value);
            }
        }

        // echo volume gain, as a relative percentage
        private RandomRange _echoGainRange;

        /// <summary>
        /// Randomized echo volume gain, as a relative percentage
        /// </summary>
        public RandomRange EchoGainRange
        {
            get
            {
                return _echoGainRange;
            }

            set
            {
                _echoGainRange = value;
            }
        }

        /// <summary>
        /// Echo volume gain, as a relative percentage
        /// </summary>
        public int EchoGain
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _echoGainRange)
                {
                    return DefaultEchoVolumeGain;
                }

                return _echoGainRange.Next;
            }
            set
            {
                _echoGainRange = new RandomRange(value);
            }
        }


        public override void Apply(Int16[] inputSamples)
        {
            // the samples are mixed with a delayed version of themselves
            Int16[] delayedSamples = inputSamples.Clone() as Int16[];
            Delay delay = new Delay();
            delay.DelayMilliseconds = this.EchoDelay;
            delay.Apply(delayedSamples);

            // delayed samples have their volume adjusted
            Gain gain = new Gain();
            gain.GainPercentage = this.EchoGain;
            gain.Apply(delayedSamples);
            
            // mix the original samples with delayed ones
            Int16[] mixedSamples = Mixer.Merge(inputSamples, delayedSamples);

            // we crop the extra samples resulting from the delay mixing
            List<Int16> mixed = new List<Int16>(mixedSamples);
            mixed.CopyTo(0, inputSamples, 0, inputSamples.Length);
        }
    }
}
