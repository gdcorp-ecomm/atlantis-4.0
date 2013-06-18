using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// WavyChess Captcha Image Generator
    /// </summary>
    internal class WavyChessImageGenerator : ChessImageGenerator, IImageGenerator
	{
        protected override void DrawBackground()
        {
            base.DrawBackground();

            // add a simple wave distortion to the black and white tile background
            Wave wave = new Wave();
            wave.LevelRange = new RandomRange(2, 4);
            wave.Apply(graphics);
        }
    }
}
