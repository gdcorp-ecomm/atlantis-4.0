using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BotDetect
{
	[Serializable]
    public class GeneratedCaptchaCodeEventArgs : CaptchaEventArgs
    {
        string _currentInstanceId;
        /// <summary>
        /// Globally unique identifier of the current Captcha instance which generated the code
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

        string _code;
        /// <summary>
        /// The random Captcha code generated
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        CodeCollection _storedCodes;
        /// <summary>
        /// Captcha codes stored for this captchaId after the new code has been generated
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

        CodeGenerationPurpose _purpose;
        /// <summary>
        /// Which purpose was the Captcha code generated for
        /// </summary>
        public CodeGenerationPurpose Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                _purpose = value;
            }
        }

        CodeStyle _codeStyle;
        /// <summary>
        /// The Captcha code style used for code generation
        /// </summary>
        public CodeStyle CodeStyle
        {
            get
            {
                return _codeStyle;
            }
            set
            {
                _codeStyle = value;
            }
        }

        int _codeLength;
        /// <summary>
        /// The Captcha code length used for code generation
        /// </summary>
        public int CodeLength
        {
            get
            {
                return _codeLength;
            }
            set
            {
                _codeLength = value;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("GeneratedCaptchaCodeEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.Append("  instance id: ");
            str.AppendLine(StringHelper.LogFriendly(_currentInstanceId));

            str.Append("  generation purpose: ");
            str.AppendLine(_purpose.ToString());

            str.Append("  code style: ");
            str.AppendLine(_codeStyle.ToString());

            str.Append("  code length: ");
            str.AppendLine(_codeLength.ToString(CultureInfo.InvariantCulture));

            str.Append("  generated code: ");
            str.AppendLine(StringHelper.LogFriendly(_code));

            str.Append("  stored codes: ");
            str.AppendLine(StringHelper.ToString(_storedCodes));

            str.Append("}");

            return str.ToString();
        }
    }
}
