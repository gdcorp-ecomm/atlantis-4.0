using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BotDetect
{
	[Serializable]
    public class GeneratedCaptchaSoundEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance which generated the sound
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

        SoundStyle _soundStyle;
        /// <summary>
        /// Sound style used for Captcha sound generation
        /// </summary>
        public SoundStyle SoundStyle
        {
            get
            {
                return _soundStyle;
            }
            set
            {
                _soundStyle = value;
            }
        }

        SoundFormat _soundFormat;
        /// <summary>
        /// Sound format the Captcha sound was generated in
        /// </summary>
        public SoundFormat SoundFormat
        {
            get
            {
                return _soundFormat;
            }
            set
            {
                _soundFormat = value;
            }
        }

        long _bytes;
        /// <summary>
        /// Size of the generated sound file
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

        long _duration;
        /// <summary>
        /// Duration of the generated sound file, in milliseconds
        /// </summary>
        public long Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("GeneratedCaptchaSoundEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("  sound style: ");
            str.AppendLine(_soundStyle.ToString());

            str.Append("  sound format: ");
            str.AppendLine(_soundFormat.ToString());

            str.Append("  generated sound bytes: ");
            str.AppendLine(_bytes.ToString(CultureInfo.InvariantCulture));

            str.Append("  generated sound duration: ");
            str.Append(_duration.ToString(CultureInfo.InvariantCulture));
            str.AppendLine(" milliseconds");

            str.Append("}");

            return str.ToString();
        }
    }
}
