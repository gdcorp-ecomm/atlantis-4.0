using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
	[Serializable]
    public class GeneratingCaptchaImageEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance generating the image
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
            str.AppendLine("GeneratingCaptchaImageEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("}");

            return str.ToString();
        }
    }
}
