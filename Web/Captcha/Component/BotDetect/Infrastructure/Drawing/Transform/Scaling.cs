using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class Scaling
    {
        public const int DefaultScalingPercentage = 100;

        private RandomRange _xScalingPercentageRange;

        public RandomRange xScalingPercentageRange
        {
            get
            {
                return _xScalingPercentageRange;
            }

            set
            {
                _xScalingPercentageRange = value;
            }
        }

        public int xScalingPercentage
        {
            get
            {
                if (null == _xScalingPercentageRange)
                {
                    return DefaultScalingPercentage;
                }

                return _xScalingPercentageRange.Next;
            }

            set
            {
                if (0 >= value || 200 <= value)
                {
                    throw new DrawingException("Glyph scaling percentage must be between 1 and 199", value);
                }

                if (DefaultScalingPercentage == value)
                {
                    _xScalingPercentageRange = null;
                }
                else
                {
                    _xScalingPercentageRange = new RandomRange(value);
                }
            }
        }

        public double xScalingFactor
        {
            get
            {
                return (this.xScalingPercentage / 100.00);
            }
        }

        private RandomRange _yScalingPercentageRange;

        public RandomRange yScalingPercentageRange
        {
            get
            {
                return _yScalingPercentageRange;
            }

            set
            {
                _yScalingPercentageRange = value;
            }
        }

        public int yScalingPercentage
        {
            get
            {
                if (null == _yScalingPercentageRange)
                {
                    return DefaultScalingPercentage;
                }

                return _yScalingPercentageRange.Next;
            }

            set
            {
                if (0 >= value || 200 <= value)
                {
                    throw new DrawingException("Glyph scaling percentage must be between 1 and 199", value);
                }

                if (DefaultScalingPercentage == value)
                {
                    _yScalingPercentageRange = null;
                }
                else
                {
                    _yScalingPercentageRange = new RandomRange(value);
                }
            }
        }

        public double yScalingFactor
        {
            get
            {
                return (this.yScalingPercentage / 100.00);
            }
        }

        public bool HasValue()
        {
            return (null != _xScalingPercentageRange || null != _yScalingPercentageRange);
        }
    }
}
