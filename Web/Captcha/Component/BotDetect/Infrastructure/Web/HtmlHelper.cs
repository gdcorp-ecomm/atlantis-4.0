using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BotDetect.Web
{
    /// <summary>
    /// This helper class is used for markup generation in various Captcha control variants.
    /// </summary>
    public sealed class HtmlHelper
    {
        private HtmlHelper()
        {
        }

        /// <summary>
        /// Creates hidden field markup with the given values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HiddenField(string name, string value)
        {
            return String.Format(CultureInfo.InvariantCulture, "    <input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", name, value);
        }

        /// <summary>
        /// Creates JavaScript include markup pointing to the given Url
        /// </summary>
        /// <param name="scriptUrl"></param>
        /// <returns></returns>
        public static string ScriptInclude(string scriptUrl)
        {
            return String.Format(CultureInfo.InvariantCulture, "    <script src=\"{0}\" type=\"text/javascript\"></script>", scriptUrl);
        }

        /// <summary>
        /// Creates JavaScript fragment markup containing the given script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static string ScriptFragment(string script)
        {
            return String.Format(CultureInfo.InvariantCulture, "    <script type=\"text/javascript\">\r\n    //<![CDATA[\r\n{0}\r\n    //]]>\r\n    </script>", script);
        }

        /// <summary>
        /// Creates Css include markup pointing to the given Url
        /// </summary>
        /// <param name="stylesheetUrl"></param>
        /// <returns></returns>
        public static string StylesheetInclude(string stylesheetUrl)
        {
            return String.Format(CultureInfo.InvariantCulture, "    <link type='text/css' rel='Stylesheet' href='" + stylesheetUrl + "' />");
        }
    }
}
