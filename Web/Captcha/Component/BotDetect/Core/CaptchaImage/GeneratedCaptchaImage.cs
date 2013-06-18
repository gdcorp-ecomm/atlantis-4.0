using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using BotDetect.Drawing;

namespace BotDetect
{
	[Serializable]
    public class GeneratedCaptchaImageEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance which generated the image
        /// </summary>
        public string CurrentInstanceId
        {
            get
            {
                return _currentInstanceId;
            }
            set
            {
                _currentInstanceId = value;
            }
        }

        ImageStyle _imageStyle;
        /// <summary>
        /// Image style used for Captcha image drawing
        /// </summary>
        public ImageStyle ImageStyle
        {
            get
            {
                return _imageStyle;
            }
            set
            {
                _imageStyle = value;
            }
        }

        ImageFormat _imageFormat;
        /// <summary>
        /// Image format the Captcha image was generated in
        /// </summary>
        public ImageFormat ImageFormat
        {
            get
            {
                return _imageFormat;
            }
            set
            {
                _imageFormat = value;
            }
        }

        ImageSize _imageSize;
        internal ImageSize ImageSize
        {
            get
            {
                return _imageSize;
            }
            set
            {
                _imageSize = value;
            }
        }

        /// <summary>
        /// Size of the generated image, in pixels
        /// </summary>
        public System.Drawing.Size GdiImageSize
        {
            get
            {
                return _imageSize.GdiSize;
            }
        }

        Color _customLightColor;
        internal Color CustomLightColor
        {
            get
            {
                return _customLightColor;
            }
            set
            {
                _customLightColor = value;
            }
        }

        Color _customDarkColor;
        internal Color CustomDarkColor
        {
            get
            {
                return _customDarkColor;
            }
            set
            {
                _customDarkColor = value;
            }
        }

        long _bytes;
        /// <summary>
        /// Size of the generated image file
        /// </summary>
        public long Bytes
        {
            get
            {
                return _bytes;
            }
            set
            {
                _bytes = value;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("GeneratedCaptchaImageEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("  image style: ");
            str.AppendLine(_imageStyle.ToString());

            str.Append("  image format: ");
            str.AppendLine(_imageFormat.ToString());

            str.Append("  image size: ");
            str.AppendLine(StringHelper.ToString(_imageSize));

            str.Append("  custom light color: ");
            str.AppendLine(StringHelper.ToString(_customLightColor));

            str.Append("  custom dark color: ");
            str.AppendLine(StringHelper.ToString(_customDarkColor));

            str.Append("  generated image bytes: ");
            str.AppendLine(_bytes.ToString(CultureInfo.InvariantCulture));

            str.Append("}");

            return str.ToString();
        }
    }
}
