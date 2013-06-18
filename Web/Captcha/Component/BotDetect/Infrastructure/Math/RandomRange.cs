using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

using BotDetect.Serialization;

namespace BotDetect
{
	[Serializable]
    public class RandomRange : ISerializable
    {
        private int _min;
        public int Min
        {
            get
            {
                return _min;
            }
        }

        private int _max;
        public int Max
        {
            get
            {
                return _max;
            }
        }

        public RandomRange(int value)
        {
            _min = value;
            _max = value;
        }

        public RandomRange(int min, int max)
        {
            if (min > max)
            {
                throw new MathException("Minimum value has to be smaller than the maximum", min, max);
            }

            _min = min;
            _max = max;
        }

        public int Next
        {
            get
            {
                if (_min == _max)
                {
                    return _max;
                }

                return RandomGenerator.Next(_min, _max);
            }
        }

        public bool IsRandomized
        {
            get
            {
                return _min < _max;
            }
        }

        public override string ToString()
        {
            if (_min == _max)
            {
                return _min.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                string output = String.Format(CultureInfo.InvariantCulture, "BotDetect.Math.RandomRange {{ min: {0}, max: {1} }} \r\n", _min, _max);
                return output;
            }
        }

        public void Freeze()
        {
            int frozenValue = this.Next;
            _min = frozenValue;
            _max = frozenValue;
        }

        public int Frozen
        {
            get
            {
                this.Freeze();
                return this.Next;
            }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter writer = new SerializationWriter();
            writer.WriteOptimized(_min);
            writer.WriteOptimized(_max);

            info.AddValue("data", writer.ToArray());
        }

        protected RandomRange(SerializationInfo info, StreamingContext context)
        {
            SerializationReader reader = new SerializationReader((byte[])info.GetValue("data", typeof(byte[])));
            _min = reader.ReadOptimizedInt32();
            _max = reader.ReadOptimizedInt32();
        }

        #endregion
    }
}
