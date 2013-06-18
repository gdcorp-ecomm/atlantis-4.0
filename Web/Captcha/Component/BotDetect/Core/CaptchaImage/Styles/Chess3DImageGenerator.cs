using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Chess3D Captcha Image Generator
    /// </summary>
    internal class Chess3DImageGenerator : ChessImageGenerator, IImageGenerator
	{
        protected override void DrawBackground()
        {
            base.DrawBackground();

            // add a simple perspective warp to the black and white tile background
            Perspective perspective = new Perspective();
            perspective.Level = 1;
            perspective.Apply(graphics);
        }
    }
}

