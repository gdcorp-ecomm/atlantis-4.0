using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class Fuzz : Effect, IEffect
    {
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

        public override void Apply(IGraphics graphics, Rectangle bounds)
        {
            int distortion = this.Level + 1;
            int maxWidth = bounds.Width;
            int maxHeight = bounds.Height;

            graphics.LockPixels();

            int seedFactor = 3;
            int bufferSize = (((maxWidth * maxHeight) / seedFactor) + 1);
            byte[] randomBytes = RandomGenerator.NextByte(bufferSize);

            for (int y1 = 0; y1 < maxHeight; y1++)
            {
                for (int x1 = 0; x1 < maxWidth; x1++)
                {
                    int xIndex = (y1 * maxWidth + x1) / seedFactor;
                    int yIndex = xIndex / 2;

                    // horizontal displacement
                    int newX = x1;
                    if (_horizontal)
                    {
                        double xFactor = (randomBytes[xIndex] / 255.0) - 0.5;
                        newX = (int)Math.Round(x1 + xFactor * distortion);
                    }
                    if (newX <= 0 || newX >= maxWidth) { newX = 1; }

                    // vertical displacement
                    int newY = y1;
                    if (_vertical)
                    {
                        double yFactor = (randomBytes[yIndex] / 255.0) - 0.5;
                        newY = (int)Math.Round(y1 + yFactor * distortion);
                    }
                    if (newY <= 0 || newY >= maxHeight) { newY = 1; }

                    // replace original pixel with the selected neighbouring one
                    graphics.SwapPixels(x1, y1, newX, newY);
                }
            }

            graphics.UnlockPixels();
        }
    }
}
