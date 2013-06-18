using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Drawing;
using System.Security;
using System.Security.Permissions;

namespace BotDetect.Web.UI
{
    /// <summary>
    /// changes the appearence of the control on the form when working in the Visual Studio designer
    /// </summary>
    internal class CaptchaDesigner : System.Web.UI.Design.ControlDesigner
    {
        public CaptchaDesigner()
        {
        }

        protected Captcha captchaControlInstance
        {
            get
            {
                return base.Component as Captcha;
            }
        }

        private DesignerVerbCollection verbs;

        public override System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                {
                    InitVerbs();
                }

                return verbs;
            }
        }

        /// <summary>
        /// Add custom designer context menu operations
        /// </summary>
        private void InitVerbs()
        {
            verbs = new DesignerVerbCollection();
        }

        public override string GetDesignTimeHtml()
        {
            StringBuilder writer = new StringBuilder();

            bool shouldRenderIcons = (CaptchaConfiguration.CaptchaReloading.Enabled || CaptchaConfiguration.CaptchaSound.Enabled);

            writer.AppendLine();
            if (CaptchaConfiguration.CaptchaCodes.TestMode.Enabled)
            {
                writer.AppendLine("<p class=\"LBD_TestModeWarning\">Test Mode Enabled</p>");
            }

            int imgWidth = captchaControlInstance.ImageSize.Width;
            int imgHeight = captchaControlInstance.ImageSize.Height;
            int divWidth = imgWidth + 6; if (shouldRenderIcons) { divWidth += 22; }
            int divHeight = imgHeight + 4;

            writer.AppendLine(String.Format("  <div class=\"LBD_CaptchaDiv\" style=\"border: 1px solid silver; padding: 0; margin: 0 0 0 1px; overflow: visible; {2} width: {0}px; height: {1}px;\">",
                divWidth, divHeight, captchaControlInstance.Style.Value));

            writer.AppendLine(String.Format("    <div class=\"LBD_CaptchaImage\" style=\"float: left; margin: 0; padding: 0; width: {0}px; height: {1} px;\">",
                imgWidth, imgHeight));
            writer.AppendLine(String.Format("      <img id=\"{0}\" src=\"{1}\" alt=\"{2}\" />",
                "CaptchaImage", "", CaptchaConfiguration.CaptchaImage.Tooltip));
            writer.AppendLine("    </div>");

            if (shouldRenderIcons)
            {
                writer.AppendLine("    <div class=\"LBD_CaptchaIcons\" style=\"width: 22px; float: right; text-align: left; margin: 0; padding: 0; margin-bottom: -4px; margin-top: -1px; margin-right: 2px;\">");
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    writer.AppendLine(String.Format("      <a href=\"{1}\" onclick=\"{0}.PlaySound(); this.blur(); return false;\" title=\"{3}\" style=\"margin: 0 !important; padding: 0; display: block; background-color: transparent; text-decoration: none; border: none;\"><img src=\"{2}\" alt=\"{3}\" /></a>",
                        captchaControlInstance.CaptchaId, "", "", CaptchaConfiguration.CaptchaSound.SoundIcon.Tooltip));
                }
                if (CaptchaConfiguration.CaptchaReloading.Enabled)
                {
                    writer.AppendLine(String.Format("      <a href=\"#\" onclick=\"{0}.ReloadImage(); this.blur(); return false;\" title=\"{2}\" style=\"margin: 0 !important; padding: 0; display: block; background-color: transparent; text-decoration: none; border: none;\"><img src=\"{1}\" alt=\"{2}\" /></a>",
                        captchaControlInstance.CaptchaId, "", CaptchaConfiguration.CaptchaReloading.ReloadIcon.Tooltip));
                }
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    writer.AppendLine(String.Format("      <div id=\"{0}\" class=\"LBD_placeholder\" style=\"visibility: hidden; width: 0; height: 0;\">&nbsp;</div>",
                        "Captcha1_AudioPlaceholder"));
                }
                writer.AppendLine("    </div>");
            }

            // finish
            writer.AppendLine("  </div>");
            writer.AppendLine();

            return writer.ToString();
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            StringBuilder writer = new StringBuilder();

            bool shouldRenderIcons = (CaptchaConfiguration.CaptchaReloading.Enabled || CaptchaConfiguration.CaptchaSound.Enabled);

            writer.AppendLine();
            if (CaptchaConfiguration.CaptchaCodes.TestMode.Enabled)
            {
                writer.AppendLine("<p class=\"LBD_TestModeWarning\">Test Mode Enabled</p>");
            }

            int imgWidth = CaptchaDefaults.ImageSize.Width;
            int imgHeight = CaptchaDefaults.ImageSize.Height;
            int divWidth = imgWidth + 6; if (shouldRenderIcons) { divWidth += 22; }
            int divHeight = imgHeight + 4;

            writer.AppendLine(String.Format("  <div class=\"LBD_CaptchaDiv\" style=\"width: {0}px; height: {1}px;\">",
                divWidth, divHeight));

            writer.AppendLine(String.Format("    <div class=\"LBD_CaptchaImage\" style=\"width: {0}px; height: {1} px;\">", 
                imgWidth, imgHeight));
            writer.AppendLine(String.Format("      <img id=\"{0}\" src=\"{1}\" alt=\"{2}\" />",
                "CaptchaImage", "", CaptchaConfiguration.CaptchaImage.Tooltip));
            writer.AppendLine("    </div>");

            if (shouldRenderIcons)
            {
                writer.AppendLine("    <div class=\"LBD_CaptchaIcons\">");
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    writer.AppendLine(String.Format("      <a href=\"{1}\" onclick=\"{0}.PlaySound(); this.blur(); return false;\" title=\"{3}\"><img src=\"{2}\" alt=\"{3}\" /></a>",
                        "Captcha1", "", "", CaptchaConfiguration.CaptchaSound.SoundIcon.Tooltip));
                }
                if (CaptchaConfiguration.CaptchaReloading.Enabled)
                {
                    writer.AppendLine(String.Format("      <a href=\"#\" onclick=\"{0}.ReloadImage(); this.blur(); return false;\" title=\"{2}\"><img src=\"{1}\" alt=\"{2}\" /></a>",
                        "Captcha1", "", CaptchaConfiguration.CaptchaReloading.ReloadIcon.Tooltip));
                }
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    writer.AppendLine(String.Format("      <div id=\"{0}\" class=\"LBD_placeholder\">&nbsp;</div>",
                        "Captcha1_AudioPlaceholder"));
                }
                writer.AppendLine("    </div>");
            }

            // finish
            writer.AppendLine("  </div>");
            writer.AppendLine();

            return writer.ToString();
        }

        protected override string GetErrorDesignTimeHtml(System.Exception e)
        {
            StringBuilder writer = new StringBuilder();

            bool shouldRenderIcons = (CaptchaConfiguration.CaptchaReloading.Enabled || CaptchaConfiguration.CaptchaSound.Enabled);

            writer.AppendLine();
            if (CaptchaConfiguration.CaptchaCodes.TestMode.Enabled)
            {
                writer.AppendLine("<p class=\"LBD_TestModeWarning\">Test Mode Enabled</p>");
            }

            int imgWidth = CaptchaDefaults.ImageSize.Width;
            int imgHeight = CaptchaDefaults.ImageSize.Height;
            int divWidth = imgWidth + 6; if (shouldRenderIcons) { divWidth += 22; }
            int divHeight = imgHeight + 4;

            writer.AppendLine(String.Format("  <div class=\"LBD_CaptchaDiv\" style=\"width: {0}px; height: {1}px;\">",
                divWidth, divHeight));

            writer.AppendLine(String.Format("    <div class=\"LBD_CaptchaImage\" style=\"width: {0}px; height: {1} px;\">",
                imgWidth, imgHeight));
            writer.AppendLine(String.Format("      <img id=\"{0}\" src=\"{1}\" alt=\"{2}\" />",
                "CaptchaImage", "", e.Message));
            writer.AppendLine("    </div>");

            if (shouldRenderIcons)
            {
                writer.AppendLine("    <div class=\"LBD_CaptchaIcons\">");
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    writer.AppendLine(String.Format("      <a href=\"{1}\" onclick=\"{0}.PlaySound(); this.blur(); return false;\" title=\"{3}\"><img src=\"{2}\" alt=\"{3}\" style=\"border: 0; margin: 0; padding: 0; margin-top: 2px; margin-bottom: 3px; display: block;\" /></a>",
                        "Captcha1", "", "", CaptchaConfiguration.CaptchaSound.SoundIcon.Tooltip));
                }
                if (CaptchaConfiguration.CaptchaReloading.Enabled)
                {
                    writer.AppendLine(String.Format("      <a href=\"#\" onclick=\"{0}.ReloadImage(); this.blur(); return false;\" title=\"{2}\"><img src=\"{1}\" alt=\"{2}\" style=\"border: 0; margin: 0; padding: 0; margin-top: 2px; margin-bottom: 3px; display: block;\" /></a>",
                        "Captcha1", "", CaptchaConfiguration.CaptchaReloading.ReloadIcon.Tooltip));
                }
                if (CaptchaConfiguration.CaptchaSound.Enabled)
                {
                    writer.AppendLine(String.Format("      <div id=\"{0}\" class=\"LBD_placeholder\">&nbsp;</div>",
                        "Captcha1_AudioPlaceholder"));
                }
                writer.AppendLine("    </div>");
            }

            // finish
            writer.AppendLine("  </div>");
            writer.AppendLine();

            writer.AppendFormat("<p>{0}</p>", e.ToString());

            return writer.ToString();
        }

        private IComponentChangeService changeService;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // Acquire a reference to IComponentChangeService.
            this.changeService = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
        }

        protected override void PostFilterProperties(System.Collections.IDictionary properties)
        {
            base.PostFilterProperties(properties);
        }
    }
}
