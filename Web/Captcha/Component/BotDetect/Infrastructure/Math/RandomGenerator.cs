using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
    internal class RandomGenerator
    {
        private RandomGenerator()
        {
            // constuctor omitted, static methods only
        }

        /// a single global generator is used for all random numbers
        private static readonly CryptoRandom _generator = new CryptoRandom();

        public static int Next()
        {
            return _generator.Next();
        }

        public static int Next(int max)
        {
            return _generator.Next(max);
        }

        public static int Next(int min, int max)
        {
            return _generator.Next(min, max);
        }

        public static Int16 NextInt16()
        {
            return (Int16) _generator.Next(Int16.MinValue, Int16.MaxValue);
        }

        public static Int16[] NextInt16(int count)
        {
            byte[] randomized = new byte[count * sizeof(Int16)];
            _generator.NextBytes(randomized);

            Int16[] samples = new Int16[count];
            Buffer.BlockCopy(randomized, 0, samples, 0, randomized.Length);

            return samples;
        }

        public static byte[] NextByte(int count)
        {
            byte[] randomized = new byte[count];
            _generator.NextBytes(randomized);
            return randomized;
        }

        public static Int16 Next(Int16 max)
        {
            return (Int16) _generator.Next(max);
        }

        public static Int16 Next(Int16 min, Int16 max)
        {
            return (Int16) _generator.Next(min, max);
        }

        public static long Next(long max)
        {
            return (long)Math.Round(_generator.NextDouble() * max);
        }

        public static long Next(long min, long max)
        {
            return (long)Math.Round(_generator.NextDouble() * (max-min)) + min;
        }

        public static float Next(float max)
        {
            return (float)_generator.NextDouble() * max;
        }

        public static float Next(float min, float max)
        {
            return (float)_generator.NextDouble() * (max - min) + min;
        }

        public static double Next(double max)
        {
            return _generator.NextDouble() * max;
        }

        public static double Next(double min, double max)
        {
            return _generator.NextDouble() * (max - min) + min;
        }

        public static string Next(string value)
        {
            if (null == value ||
                0 == value.Length)
            {
                return null;
            }

            return value.Substring(Next(value.Length), 1);
        }

        public static string Next(string[] values)
        {
            if (null == values ||
                0 == values.Length)
            {
                return null;
            }

            return values[Next(values.Length)];
        }

    }
}
