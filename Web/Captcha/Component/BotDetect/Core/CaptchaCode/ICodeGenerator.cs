using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal interface ICodeGenerator
    {
        Code GenerateCode(CodeStyle codeStyle, int length);
    }
}
