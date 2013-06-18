using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Split2 Captcha Image Generator
    /// </summary>
    internal class Split2ImageGenerator : SplitImageGenerator, IImageGenerator
	{
        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom thick glyph outline
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(2, backColor), 
                LineLayer.Solid(3, textColor)
            );
        }
    }
}
