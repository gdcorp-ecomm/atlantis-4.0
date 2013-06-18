using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.Diagnostics;

namespace BotDetect.Web
{
    internal class RequestTrace
    {
        // singleton, to allow lazy trace initialization
        private RequestTrace()
        {
        }

        private static readonly RequestTrace _instance = new RequestTrace();

        public static RequestTrace Instance
        {
            get
            {
                return _instance;
            }
        }

        // actual trace
        public override string ToString()
        {
            StringBuilder log = new StringBuilder();
            log.Append("HttpRequest {");

            if (null == HttpContext.Current || null == HttpContext.Current.Request)
            {
                log.Append(" null }");
            }
            else
            {
                log.AppendLine();
                HttpRequest request = HttpContext.Current.Request;

                // request method & url
                log.Append("  " + request.HttpMethod + " ");
                log.AppendLine(StringHelper.LogFriendly(request.RawUrl));

                TraceVisitorInfo(request, log);
                TraceCookies(request, log);
                TraceFormValues(request, log);

                log.Append("}");
            }

            return log.ToString();
        }

        public static void TraceVisitorInfo(HttpRequest request, StringBuilder log)
        {
            // visitor info
            log.Append("  client ip: ");
            log.AppendLine(StringHelper.LogFriendly(request.UserHostAddress));

            log.Append("  browser: ");
            log.AppendLine(StringHelper.LogFriendly(request.UserAgent));
        }

        public static void TraceCookies(HttpRequest request, StringBuilder log)
        {
            // cookies
            HttpCookieCollection cookies = request.Cookies;
            log.Append("  cookies: {");
            if (null == cookies || 0 == cookies.Count || null == cookies.AllKeys)
            {
                log.AppendLine(" empty }");
            }
            else
            {
                log.AppendLine();
                foreach (string name in cookies.AllKeys)
                {
                    log.Append("    " + name + ": ");
                    log.AppendLine(cookies[name].Value);
                }
                log.AppendLine("  }");
            }
        }

        public static void TraceFormValues(HttpRequest request, StringBuilder log)
        {
            // BotDetect form fields
            NameValueCollection formData = request.Form;
            log.Append("  form data: {");
            if (null == formData || 0 == formData.Count || null == formData.Keys)
            {
                log.AppendLine(" empty }");
            }
            else
            {
                log.AppendLine();
                foreach (string name in formData.Keys)
                {
                    if (name.ToUpperInvariant().StartsWith("LBD", StringComparison.Ordinal))
                    {
                        log.Append("    " + name + ": ");
                        log.AppendLine(formData[name]);
                    }
                }
                log.AppendLine("  }");
            }
        }
    }
}
