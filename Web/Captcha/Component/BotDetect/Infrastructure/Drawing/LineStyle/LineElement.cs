using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class LineElement
    {
        public const int Continuous = 0;

        private Color _elementColor;

        public Color Color
        {
            get
            {
                return _elementColor;
            }

            set
            {
                _elementColor = value;
            }
        }

        private RandomRange _lengthRange;

        public RandomRange LengthRange
        {
            get
            {
                return _lengthRange;
            }

            set
            {
                _lengthRange = value;
            }
        }

        public int Length
        {
            get
            {
                if (null == _lengthRange)
                {
                    return 0;
                }

                return _lengthRange.Next;
            }
            set
            {
                if (value <= 1)
                {
                    throw new DrawingException("The length of a line segment has to be a positive integer >= 1", value);
                }

                _lengthRange = new RandomRange(value);
            }
        }

        public LineElement(Color color, RandomRange lengthRange)
        {
            this._elementColor = color;
            this._lengthRange = lengthRange;
        }

        public LineElement(Color color, int length)
            : this(color, new RandomRange(length))
        {
        }

        public LineElement(Color color)
            : this(color, null)
        {
        }
    }
}
