using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
	[Serializable]
    public abstract class CaptchaEventArgs : BaseEventArgs
    {
        string _captchaId;
        /// <summary>
        /// Identifies which Captcha does the event apply to
        /// </summary>
        public string CaptchaId
        {
            get
            {
                return _captchaId;
            }
            set
            {
                _captchaId = value;
            }
        }
    }
}
