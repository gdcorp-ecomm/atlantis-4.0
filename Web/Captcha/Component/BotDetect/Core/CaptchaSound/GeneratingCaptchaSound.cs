using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
	[Serializable]
    public class GeneratingCaptchaSoundEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance generating the sound
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

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("GeneratingCaptchaSoundEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("}");

            return str.ToString();
        }
    }
}
