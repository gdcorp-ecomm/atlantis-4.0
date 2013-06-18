using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
	[Serializable]
    public class GeneratingCaptchaCodeEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance generating the code
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

        CodeCollection _storedCodes;
        /// <summary>
        /// Captcha codes stored for this captchaId before the new code is generated
        /// </summary>
        public CodeCollection StoredCodes
        {
            get
            {
                return _storedCodes;
            }
            set
            {
                _storedCodes = value;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("GeneratingCaptchaCodeEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("  stored codes: ");
            str.AppendLine(StringHelper.ToString(_storedCodes));

            str.Append("}");

            return str.ToString();
        }
    }
}
