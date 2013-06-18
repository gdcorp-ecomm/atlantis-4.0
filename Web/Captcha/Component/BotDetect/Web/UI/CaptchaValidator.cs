using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Util;

namespace BotDetect.Web.UI
{
    /// <summary>
    /// An ASP.NET Validator used to integrate Captcha validation with other 
    /// field validators and validation summaries. Requires a Captcha control 
    /// instance and a text input field.
    /// </summary>
    public class CaptchaValidator : BaseValidator
    {
        /// <summary>
        /// ASP.NET ID of the Captcha control the validator validates
        /// </summary>
        public string CaptchaControl
        {
           get
           {
                object obj = this.ViewState["CaptchaControlId"];
                if (obj != null)
                {
                    return (string) obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["CaptchaControlId"] = value;
            }
        }

        protected override bool ControlPropertiesValid()
        {
            ITextControl userInput = this.FindControl(this.ControlToValidate) as ITextControl;
            Captcha captchaControl = this.FindControl(this.CaptchaControl) as Captcha;

            return (userInput != null && captchaControl != null);

        }

        protected override bool EvaluateIsValid()
        {
            ITextControl userInput = this.FindControl(this.ControlToValidate) as ITextControl;
            Captcha captchaControl = this.FindControl(this.CaptchaControl) as Captcha;

            return captchaControl.Validate(userInput.Text.Trim().ToUpper());
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.DetermineRenderUplevel() && this.EnableClientScript)
            {
                Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "CheckCaptchaIsNotEmpty");
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<script type=""text/javascript"">function CheckCaptchaIsNotEmpty(ctrl){");
                sb.Append(@"var textBox = document.getElementById(document.getElementById(ctrl.id).controltovalidate);");
                sb.Append(@"if(textBox.value) {");
                sb.Append(@"return true;}");
                sb.Append(@"return false; ");
                sb.Append(@"}</script>");
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "ClientCaptchaValidation", sb.ToString());
            }

            base.OnPreRender(e);
        }

    }
}
