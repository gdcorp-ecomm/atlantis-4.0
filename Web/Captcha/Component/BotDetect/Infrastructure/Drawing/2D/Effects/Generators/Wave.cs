using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class Wave : Effect, IEffect
    {
        // should horizontal distortion be applied
        private bool _horizontal = true;
        public bool Horizontal
        {
            get
            {
                return _horizontal;
            }
            set
            {
                _horizontal = value;
            }
        }

        // should vertical distortion be applied
        private bool _vertical = true;
        public bool Vertical
        {
            get
            {
                return _vertical;
            }
            set
            {
                _vertical = value;
            }
        }

        // sine amplitude (distance between -1 and 1 in value, in pixels)
        private RandomRange _amplitudeRange;

        public RandomRange AmplitudeRange
        {
            get
            {
                return _amplitudeRange;
            }

            set
            {
                if (0 > value.Min || 0 > value.Max)
                {
                    throw new DrawingException("Sine amplitude must be a positive number", value);
                }

                _amplitudeRange = value;
            }
        }

        public const int DefaultAmplitude = 32;

        public int Amplitude
        {
            get
            {
                if (null == _amplitudeRange)
                {
                    return Wave.DefaultAmplitude;
                }

                return _amplitudeRange.Next;
            }
            set
            {
                this.AmplitudeRange = new RandomRange(value);
            }
        }

        private Color _overflowColor;
        public Color OverflowColor
        {
            get
            {
                return _overflowColor;
            }
            set
            {
                _overflowColor = value;
            }
        }

        public override void Apply(IGraphics graphics, Rectangle bounds)
        {
            int distortion = this.Level;
            double amplitude = (double)this.Amplitude;
            int maxWidth = bounds.Width;
            int maxHeight = bounds.Height;

            graphics.LockPixels();

            for (int y1 = 0; y1 < maxHeight; y1++)
            {
                for (int x1 = 0; x1 < maxWidth; x1++)
                {
                    bool overflow = false;

                    // horizontal displacement
                    int newX = x1;
                    if (_horizontal)
                    {
                        newX = (int)(x1 + (distortion * Math.Sin(Math.PI * y1 / amplitude)));
                        if (newX <= 0 || newX >= maxWidth) { overflow = true; newX = maxWidth - 1; }
                    }

                    // vertical displacement
                    int newY = y1;
                    if (_vertical)
                    {
                        newY = (int)(y1 + (distortion * Math.Cos(Math.PI * x1 / amplitude)));
                        if (newY <= 0 || newY >= maxHeight) { overflow = true; newY = maxHeight - 1; }
                    }

                    // replace original pixel with the selected neighbouring one
                    if (overflow && null != _overflowColor)
                    {
                        graphics.SetPixel(x1, y1, _overflowColor);
                    }
                    else
                    {
                        graphics.SwapPixels(x1, y1, newX, newY);
                    }
                }
            }

            graphics.UnlockPixels();
        }
    }
}
