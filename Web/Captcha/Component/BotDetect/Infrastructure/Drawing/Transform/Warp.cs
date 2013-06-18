using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class Warp
    {
        public const int DefaultWarpPercentage = 0;

        private RandomRange _warpPercentageRange;

        public RandomRange WarpPercentageRange
        {
            get
            {
                return _warpPercentageRange;
            }

            set
            {
                _warpPercentageRange = value;
            }
        }

        public bool HasValue()
        {
            return (null != _warpPercentageRange);
        }

        public int WarpPercentage
        {
            get
            {
                if (null == _warpPercentageRange)
                {
                    return DefaultWarpPercentage;
                }

                return _warpPercentageRange.Next;
            }

            set
            {
                if (0 >= value || 199 <= value)
                {
                    throw new DrawingException("Glyph warp percentage must be between 1 and 99", value);
                }

                if (DefaultWarpPercentage == value)
                {
                    _warpPercentageRange = null;
                }
                else
                {
                    _warpPercentageRange = new RandomRange(value);
                }
            }
        }

        public double WarpFactor
        {
            get
            {
                return (this.WarpPercentage / 100.00);
            }
        }
    }
}
