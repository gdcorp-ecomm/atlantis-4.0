using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class Rotation
    {
        public const int DefaultRotationAngle = 0;

        private RandomRange _angleRange;

        public RandomRange AngleRange
        {
            get
            {
                return _angleRange;
            }

            set
            {
                _angleRange = value;
            }
        }

        public bool HasValue()
        {
            return (null != _angleRange);
        }

        public int Angle
        {
            get
            {
                if (null == _angleRange)
                {
                    return DefaultRotationAngle;
                }

                return _angleRange.Next;
            }

            set
            {
                if (-360 >= value || 360 <= value)
                {
                    throw new DrawingException("Glyph rotation angle must be between -359 and 359", value);
                }

                if (DefaultRotationAngle == value)
                {
                    _angleRange = null;
                }
                else
                {
                    _angleRange = new RandomRange(value);
                }
            }
        }
    }
}
