using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal interface IEffect
    {
        void Apply(IGraphics graphics, Rectangle bounds);
    }
}
