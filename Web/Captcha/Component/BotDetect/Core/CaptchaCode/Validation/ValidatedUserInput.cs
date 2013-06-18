using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
	[Serializable]
    public class ValidatedUserInputEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance which validated the user input
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

        string _validatingInstanceId;
        /// <summary>
        /// Globally unique identifier of the previous Captcha instance which generated the Captcha code
        /// </summary>
        public string ValidatingInstanceId
        {
            get
            {
                return _validatingInstanceId;
            }
            set
            {
                _validatingInstanceId = value;
            }
        }

        string _userInput;
        /// <summary>
        /// User input that was compared to stored codes
        /// </summary>
        public string UserInput
        {
            get
            {
                return _userInput;
            }
            set
            {
                _userInput = value;
            }
        }

        ValidationAttemptOrigin _origin;
        /// <summary>
        /// Origin of the Captcha validation attempt
        /// </summary>
        public ValidationAttemptOrigin Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
            }
        }

        CodeCollection _storedCodes;
        /// <summary>
        /// Captcha codes stored for this captchaId after user input was validated
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

        bool _result;
        /// <summary>
        /// Captcha validation result
        /// </summary>
        public bool Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("ValidatedUserInputEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("  stored codes: ");
            str.AppendLine(StringHelper.ToString(_storedCodes));

            str.Append("  validating instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_validatingInstanceId));

            str.Append("  validation origin: ");
            str.AppendLine(_origin.ToString());

            str.Append("  user input: ");
            str.AppendLine(StringHelper.LogFriendly(_userInput));

            str.Append("  validation result: ");
            str.AppendLine(_result.ToString());

            str.Append("}");

            return str.ToString();
        }
    }
}
