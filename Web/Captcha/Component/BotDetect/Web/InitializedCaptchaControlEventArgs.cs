using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Web;

namespace BotDetect
{
	[Serializable]
    public class InitializedCaptchaControlEventArgs : CaptchaEventArgs
    {
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("InitializedCaptchaControlEventArgs {");

            str.Append("  captcha id: ");
            str.AppendLine(StringHelper.LogFriendly(base.CaptchaId));

            str.AppendLine(RequestTrace.Instance.ToString());
            str.AppendLine(CaptchaPersistence.ToString());

            str.Append("}");

            return str.ToString();
        }
    }
}
