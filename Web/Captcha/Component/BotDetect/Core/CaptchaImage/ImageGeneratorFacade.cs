using System;
using System.IO;

using BotDetect.Drawing;

using BotDetect.CaptchaCode;
using BotDetect.CaptchaImage;

namespace BotDetect
{
	/// <summary>
	/// Summary description for ImageGenerator.
	/// </summary>
	internal sealed class ImageGeneratorFacade
	{
        private ImageGeneratorFacade()
        {
        }

        public static MemoryStream GenerateImage(string code, ImageStyle imageStyle, Localization localization, ImageSize imageSize,
           ImageFormat imageFormat, Color customLightColor, Color customDarkColor)
        {
            IImageGenerator generator = ImageGeneratorFactory.CreateGenerator(imageStyle);
            IGraphics captchaGraphics = generator.GenerateImage(code, localization, imageSize, customLightColor, customDarkColor);
            return captchaGraphics.GetStream(imageFormat);
        }
	}
}
