using System;

namespace BotDetect
{
	/// <summary>
	/// Summary description for ImageFormat.
	/// </summary>
	public enum ImageFormat
	{
        Bmp,
        Jpeg,
		Gif,
		Png
	}

    internal sealed class ImageFormatHelper
    {
        private ImageFormatHelper()
        {
        }

        /// <summary>
        /// converts control's ImageFormat to System.Drawing ImageFormat
        /// </summary>
        public static System.Drawing.Imaging.ImageFormat GetGdiImageFormat(ImageFormat imageFormat)
        {
            System.Drawing.Imaging.ImageFormat _format = null;

            switch (imageFormat)
            {
                case ImageFormat.Jpeg:
                    _format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;

                case ImageFormat.Bmp:
                    _format = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;

                case ImageFormat.Gif:
                    _format = System.Drawing.Imaging.ImageFormat.Gif;
                    break;

                case ImageFormat.Png:
                    _format = System.Drawing.Imaging.ImageFormat.Png;
                    break;

                default:
                    _format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
            }

            return _format;
        }
    }
}
