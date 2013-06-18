using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal abstract class Effect
    {
        // noise volume, as percentage of absolute sound peak
        private RandomRange _levelRange;

        public RandomRange LevelRange
        {
            get
            {
                return _levelRange;
            }

            set
            {
                if (0 > value.Min || 10 < value.Min || 0 > value.Max || 10 < value.Max)
                {
                    throw new DrawingException("Distortion level must be between 1 and 10", value);
                }

                _levelRange = value;
            }
        }

        public int Level
        {
            get
            {
                if (null == _levelRange)
                {
                    return 0;
                }

                return _levelRange.Next;
            }
            set
            {
                if (0 > value || 10 < value)
                {
                    throw new DrawingException("Distortion level must be between 1 and 10", value);
                }

                _levelRange = new RandomRange(value);
            }
        }

        public abstract void Apply(IGraphics graphics, Rectangle bounds);

        public void Apply(IGraphics graphics)
        {
            this.Apply(graphics, graphics.VisibleBounds);
        }
    }
}
