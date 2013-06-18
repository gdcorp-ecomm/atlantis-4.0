using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
	/// <summary>
	/// Summary description for IImageGenerator.
	/// </summary>
	internal interface IImageGenerator
	{
        IGraphics GenerateImage(string text, Localization localization, ImageSize imageSize, Color customLightColor, Color customDarkColor);
	}
}
