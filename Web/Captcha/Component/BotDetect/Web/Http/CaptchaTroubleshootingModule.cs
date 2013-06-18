using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace BotDetect.Web
{
    /// <summary>
    /// HttpModule used to detect BotDetect Captcha errors on the server and 
    /// write them to the configured BotDetect logging provider.
    /// </summary>
    public class CaptchaTroubleshootingModule : IHttpModule
    {
        /// <summary>
        /// IHttpModule override, registers a custom handler for the 
        /// HttpApplicationError event
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(Application_OnError);
        }

        /// <summary>
        /// Logs BotDetect errors to the configured logging provider
        /// </summary>
        void Application_OnError(object sender, System.EventArgs e)
        {
            LogCurrentContextError();
        }

        /// <summary>
        /// Searches HttpContext.Current.ApplicationInstance.Server.GetLastError() 
        /// for BotDetect errors and logs them to the configured BotDetect 
        /// logging provider
        /// </summary>
        public static void LogCurrentContextError()
        {
            HttpServerUtility server = HttpContext.Current.ApplicationInstance.Server;
            if (null == server) { return; }

            // search error hierarchy for BotDetect exceptions
            Exception err = server.GetLastError();
            BotDetect.BaseException ex = null;
            while (null != err)
            {
                ex = err as BotDetect.BaseException;
                if (null != ex)
                {
                    break;
                }

                err = err.InnerException;
            }

            // log error details
            if (null != ex)
            {
                CaptchaLogging.LogError(ex, RequestTrace.Instance, CaptchaPersistence.ToString());
            }
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
