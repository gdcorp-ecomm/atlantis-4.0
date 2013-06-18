using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class Translation
    {
        public const int DefaultTranslationOffset = 0;

        private RandomRange _xOffsetRange;

        public RandomRange xOffsetRange
        {
            get
            {
                return _xOffsetRange;
            }

            set
            {
                _xOffsetRange = value;
            }
        }

        public int xOffset
        {
            get
            {
                if (null == _xOffsetRange)
                {
                    return DefaultTranslationOffset;
                }

                return _xOffsetRange.Next;
            }

            set
            {
                if (DefaultTranslationOffset == value)
                {
                    _xOffsetRange = null;
                }
                else
                {
                    _xOffsetRange = new RandomRange(value);
                }
            }
        }

        public int xOffsetRelative(int baseValue)
        {
            return baseValue * this.xOffset / 100;
        }

        public int xOffsetRelative(float baseValue)
        {
            return (int) Math.Round(baseValue * this.xOffset / 100);
        }

        private RandomRange _yOffsetRange;

        public RandomRange yOffsetRange
        {
            get
            {
                return _yOffsetRange;
            }

            set
            {
                _yOffsetRange = value;
            }
        }

        public int yOffset
        {
            get
            {
                if (null == _yOffsetRange)
                {
                    return DefaultTranslationOffset;
                }

                return _yOffsetRange.Next;
            }

            set
            {
                if (DefaultTranslationOffset == value)
                {
                    _yOffsetRange = null;
                }
                else
                {
                    _yOffsetRange = new RandomRange(value);
                }
            }
        }

        public int yOffsetRelative(int baseValue)
        {
            return baseValue * this.yOffset / 100;
        }

        public int yOffsetRelative(float baseValue)
        {
            return (int)Math.Round(baseValue * this.yOffset / 100);
        }

        public bool HasValue()
        {
            return (null != _xOffsetRange || null != _yOffsetRange);
        }

    }
}
