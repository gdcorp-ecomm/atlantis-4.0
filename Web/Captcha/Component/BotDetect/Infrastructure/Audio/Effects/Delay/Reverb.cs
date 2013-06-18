using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Reverb : Effect, IEffect
    {
        public const int DefaultEchoDelay = 50; // milliseconds, room "size"
        public const int DefaultReflectionPercentage = 75; // ratio of energy loss

        // echo delay in milliseconds, simulates room "size"
        private RandomRange _echoDelayRange;

        /// <summary>
        /// Randomized echo delay in milliseconds, simulates room "size"
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
        /// Echo delay in milliseconds, simulates room "size"
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

        // ratio of "wall" enery reflection
        private RandomRange _reflectionPercentageRange;

        /// <summary>
        /// Randomized ratio of "wall" enery reflection
        /// </summary>
        public RandomRange ReflectionPercentageRange
        {
            get
            {
                return _reflectionPercentageRange;
            }

            set
            {
                _reflectionPercentageRange = value;
            }
        }

        /// <summary>
        /// Ratio of "wall" enery reflection
        /// </summary>
        public int ReflectionPercentage
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _reflectionPercentageRange)
                {
                    return DefaultReflectionPercentage;
                }

                return _reflectionPercentageRange.Next;
            }
            set
            {
                _reflectionPercentageRange = new RandomRange(value);
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            // we use a threshold of echo amplitude to limit the number of "wall reflections"
            double reflectionFactor = 1.0;
            
            // a series of progressively quieter echoes
            while (reflectionFactor > 0.1) 
            {
                reflectionFactor = reflectionFactor * this.ReflectionPercentage / (double) 100;

                Echo echo = new Echo();
                echo.EchoDelay = this.EchoDelay;
                echo.EchoGain = (int)Math.Round(reflectionFactor * 100);
                echo.Apply(inputSamples);
            }
            
        }
    }
}
