using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Util;
using System.Web.SessionState;
using System.Security;
using System.Reflection;

namespace BotDetect.Web
{
    /// <summary>
    /// Runs a series of Session state checks and reports the findings in several formats.
    /// </summary>
    public sealed class SessionTroubleshooting
    {
        private SessionTroubleshooting() { }

        /// <summary>
        /// Is Session troubleshooting currently enabled
        /// </summary>
        public static bool Enabled
        {
            get
            {
                return CaptchaConfiguration.CaptchaSessionTroubleshooting.Enabled;
            }
        }

        /// <summary>
        /// Are any possible Session problems detected for the current request
        /// </summary>
        /// <returns></returns>
        public static bool IsSessionProblemDetected()
        {
            return SessionTroubleshooting.IsSessionProblemDetected(null);
        }

        /// <summary>
        /// Are any possible Session problems detected from the given 
        /// CaptchaControl instance
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static bool IsSessionProblemDetected(CaptchaControl captchaControl)
        {
            bool detected = false;

            if (CaptchaConfiguration.CaptchaSessionTroubleshooting.Enabled)
            {
                if (Status.False == SessionTroubleshooting.IsSessionHttpModuleRunning)
                {
                    // critical error - we need the module
                    return true;
                }

                /*if (Status.False == SessionTroubleshooting.IsCustomSessionIdManagerIncluded)
                {
                    // critical error, except if sound Captchas are irrelevant
                    return true;
                }*/

                if (SessionTroubleshooting.IsSessionStateEmpty)
                {
                    // critical error - there should be a value saved by this point
                    return true;
                }

                if (Status.True == SessionTroubleshooting.IsNewSessionOnSumbit(captchaControl))
                {
                    // warning: could also be an expired session?
                    return true;
                }

                if (Status.True == SessionTroubleshooting.IsCaptchaPersistenceEmpty(captchaControl))
                {
                    // critical error - there should be a value saved by this point
                    return true;
                }
            }

            return detected;
        }

        /// <summary>
        /// Reports Session state problems found with the current request, 
        /// formatted for Html output
        /// </summary>
        /// <returns></returns>
        public static string HtmlReport()
        {
            return HtmlReport(null);
        }

        /// <summary>
        /// Reports Session state problems found with the given CaptchaControl 
        /// instance, formatted for Html output
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static string HtmlReport(CaptchaControl captchaControl)
        {
            StringBuilder output = new StringBuilder();

            if (CaptchaConfiguration.CaptchaSessionTroubleshooting.Enabled)
            {
                if (Status.False == SessionTroubleshooting.IsSessionHttpModuleRunning)
                {
                    // critical error - we need the module
                    output.AppendLine("<p class=\"LBD_Warning\">SessionTroubleshooting: SessionHttpModule not running (<a target=\"_blank\" href=\"http://captcha.biz/doc/aspnet/howto/captcha-configuration.html#captcha_session_troubleshooting\">documentation</a>).</p>");
                }

                /*if (CaptchaConfiguration.CaptchaSound.Enabled &&
                    Status.False == SessionTroubleshooting.IsCustomSessionIdManagerIncluded)
                {
                    // critical error, except if sound Captchas are irrelevant
                    output.AppendLine("<p class=\"LBD_Warning\">SessionTroubleshooting: CustomSessionIdManager not configured (<a target=\"_blank\" href=\"http://captcha.biz/doc/aspnet/howto/captcha-configuration.html#captcha_session_troubleshooting\">documentation</a>).</p>");
                }*/

                if (SessionTroubleshooting.IsSessionStateEmpty)
                {
                    // critical error - there should be a value saved by this point
                    output.AppendLine("<p class=\"LBD_Warning\">SessionTroubleshooting: SessionState is empty (<a target=\"_blank\" href=\"http://captcha.biz/doc/aspnet/howto/captcha-configuration.html#captcha_session_troubleshooting\">documentation</a>).</p>");
                }

                if (Status.True == SessionTroubleshooting.IsNewSessionOnSumbit(captchaControl))
                {
                    // warning: could also be an expired session?
                    output.AppendLine("<p class=\"LBD_Warning\">SessionTroubleshooting: New Session initialized on PostBack, potential timeout or Session resume problem (<a target=\"_blank\" href=\"http://captcha.biz/doc/aspnet/howto/captcha-configuration.html#captcha_session_troubleshooting\">documentation</a>).</p>");
                }

                if (Status.True == SessionTroubleshooting.IsCaptchaPersistenceEmpty(captchaControl))
                {
                    // critical error - there should be a value saved by this point
                    output.AppendLine("<p class=\"LBD_Warning\">SessionTroubleshooting: CaptchaPersistence is empty (<a target=\"_blank\" href=\"http://captcha.biz/doc/aspnet/howto/captcha-configuration.html#captcha_session_troubleshooting\">documentation</a>).</p>");
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Reports Session state problems found with the current request, 
        /// formatted for plain text output
        /// </summary>
        /// <returns></returns>
        public static string PlainTextReport()
        {
            return PlainTextReport(null);
        }

        /// <summary>
        /// Reports Session state problems found with the given CaptchaControl 
        /// instance, formatted for plain text output
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static string PlainTextReport(CaptchaControl captchaControl)
        {
            StringBuilder output = new StringBuilder();

            if (CaptchaConfiguration.CaptchaSessionTroubleshooting.Enabled)
            {
                if (Status.False == SessionTroubleshooting.IsSessionHttpModuleRunning)
                {
                    // critical error - we need the module
                    output.AppendLine("SessionTroubleshooting: SessionHttpModule not running");
                }

                /*if (Status.False == SessionTroubleshooting.IsCustomSessionIdManagerIncluded)
                {
                    // critical error, except if sound Captchas are irrelevant
                    output.AppendLine("SessionTroubleshooting: CustomSessionIdManager not configured");
                }*/

                if (SessionTroubleshooting.IsSessionStateEmpty)
                {
                    // critical error - there should be a value saved by this point
                    output.AppendLine("SessionTroubleshooting: SessionState is empty");
                }

                if (Status.False == SessionTroubleshooting.IsResumedSession)
                {
                    // warning: a potentially expired session, or a session problem
                    output.AppendLine("SessionTroubleshooting: New Session initialized, potential timeout or Session resume problem");
                }

                if (Status.False == SessionTroubleshooting.DoesCaptchaPersistenceExist(captchaControl))
                {
                    // critical error - there should be a value saved by this point
                    output.AppendLine("SessionTroubleshooting: CaptchaPersistence is empty");
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Reports Session state problems found with the current request, 
        /// formatted for Http header output
        /// </summary>
        /// <returns></returns>
        public static string HttpHeaderReport()
        {
            return HttpHeaderReport(null);
        }

        /// <summary>
        /// Reports Session state problems found with the given CaptchaControl 
        /// instance, formatted for Http header output
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static string HttpHeaderReport(CaptchaControl captchaControl)
        {
            StringBuilder output = new StringBuilder();

            if (CaptchaConfiguration.CaptchaSessionTroubleshooting.Enabled)
            {
                output.Append((int)SessionTroubleshooting.IsSessionHttpModuleRunning);
                output.Append((int)SessionTroubleshooting.IsCustomSessionIdManagerIncluded);
                output.Append(SessionTroubleshooting.IsSessionStateEmpty ? 0 : 1);
                output.Append((int)SessionTroubleshooting.IsResumedSession);
                output.Append((int)SessionTroubleshooting.DoesCaptchaPersistenceExist(captchaControl));
            }

            return output.ToString();
        }

        // SessionStateModule registration status
        private static Status isSessionStateModuleRunning = Status.False;
        private static readonly object isSessionStateModuleRunningLock = new object();

        public static void ConfirmSessionModuleInclusion()
        {
            if (Status.True != isSessionStateModuleRunning)
            {
                isSessionStateModuleRunning = Status.True;
            }
        }

        /// <summary>
        /// Check is the SessionStateModule included in the current HttpApplication instance
        /// (works only in Full Trust)
        /// </summary>
        public static Status IsSessionHttpModuleRunning
        {
            get
            {
                if (Status.False == isSessionStateModuleRunning)
                {
                    lock (isSessionStateModuleRunningLock)
                    {
                        if (Status.False == isSessionStateModuleRunning)
                        {
                            // check module presence "manually"
                            try
                            {
                                IHttpModule sessionModule =
                                    HttpContext.Current.ApplicationInstance.Modules["Session"];

                                if (null != sessionModule)
                                {
                                    isSessionStateModuleRunning = Status.True;
                                }
                                else
                                {
                                    isSessionStateModuleRunning = Status.False;
                                }
                            }
                            catch (SecurityException e)
                            {
                                // this check above will only run in Full Trust,
                                // so we ignore Medium Trust security errors
                                isSessionStateModuleRunning = Status.Unknown;

                                CaptchaLogging.Trace("Warning",
                                    "Unable to determine is the SessionState HttpModule running or not due to insufficient permissions", e);
                            }
                        }
                    }
                }

                return isSessionStateModuleRunning;
            }
        }


        // CustomSessionIdManager inclusion status
        private static Status isCustomSessionIdManagerIncluded = Status.False;
        private static object isCustomSessionIdManagerIncludedLock = new object();

        public static void ConfirmSessionIdManagerInclusion()
        {
            if (Status.True != isCustomSessionIdManagerIncluded)
            {
                isCustomSessionIdManagerIncluded = Status.True;
            }
        }

        /// <summary>
        /// Check is the custom BotDetect SessionIdManager configured correctly
        /// (works only in Full Trust)
        /// </summary>
        public static Status IsCustomSessionIdManagerIncluded
        {
            get
            {
                if (Status.False == isCustomSessionIdManagerIncluded)
                {
                    lock (isCustomSessionIdManagerIncludedLock)
                    {
                        if (Status.False == isCustomSessionIdManagerIncluded)
                        {
                            // check CustomSessionIdManager presence "manually"
                            try
                            {
                                IHttpModule sessionModule =
                                    HttpContext.Current.ApplicationInstance.Modules["Session"];

                                if (null != sessionModule)
                                {
                                    FieldInfo fi = typeof(SessionStateModule).GetField("_idManager",
                                        BindingFlags.NonPublic | BindingFlags.Instance);

                                    ISessionIDManager manager = fi.GetValue(sessionModule) as ISessionIDManager;

                                    if ("BotDetect.Web.CustomSessionIdManager" == manager.GetType().ToString())
                                    {
                                        isCustomSessionIdManagerIncluded = Status.True;
                                    }
                                    else
                                    {
                                        isCustomSessionIdManagerIncluded = Status.False;
                                    }
                                }
                            }
                            catch (SecurityException e)
                            {
                                // this check above will only run in Full Trust,
                                // so we ignore Medium Trust errors
                                isCustomSessionIdManagerIncluded = Status.Unknown;

                                CaptchaLogging.Trace("Warning",
                                    "Unable to determine is the CustomSessionIdManager registered or not due to insufficient permissions", e);
                            }
                        }
                    }
                }

                return isCustomSessionIdManagerIncluded;
            }
        }

        /// <summary>
        /// Detects is the Session state for the current request empty
        /// </summary>
        public static bool IsSessionStateEmpty
        {
            get
            {
                if (null == HttpContext.Current)
                {
                    return true;
                }

                if (null == HttpContext.Current.Session)
                {
                    return true;
                }

                // if the condition above doesn't return, we can be sure the
                // SessionStateModule is running
                SessionTroubleshooting.ConfirmSessionModuleInclusion();

                if (0 == HttpContext.Current.Session.Count)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Detects is a new Session started with the current request
        /// </summary>
        public static Status IsNewSession
        {
            get
            {
                if (null != HttpContext.Current &&
                    null != HttpContext.Current.Session)
                {
                    // if the condition above evaluates to true, we can be sure the
                    // SessionStateModule is running
                    SessionTroubleshooting.ConfirmSessionModuleInclusion();

                    if (HttpContext.Current.Session.IsNewSession)
                    {
                        return Status.True;
                    }
                    else
                    {
                        return Status.False;
                    }
                }

                return Status.Unknown;
            }
        }

        /// <summary>
        /// Detects is a previous Session resumed with the current request
        /// </summary>
        public static Status IsResumedSession
        {
            get
            {
                Status status = Status.Unknown;

                switch (SessionTroubleshooting.IsNewSession)
                {
                    case Status.True:
                        status = Status.False;
                        break;

                    case Status.False:
                        status = Status.True;
                        break;
                }

                return status;
            }
        }

        /// <summary>
        /// Detects does the Session state for the current request contain 
        /// any BotDetect Captcha stored values
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static Status IsCaptchaPersistenceEmpty(CaptchaControl captchaControl)
        {
            if (null == captchaControl)
            {
                return Status.Unknown;
            }

            if (!CaptchaPersistence.IsValid(captchaControl.CaptchaId))
            {
                return Status.True;
            }

            return Status.False;
        }

        /// <summary>
        /// Detects does the Session state for the current request contain a 
        /// BotDetect CaptchaPersistence set
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static Status DoesCaptchaPersistenceExist(CaptchaControl captchaControl)
        {
            if (null == captchaControl)
            {
                return Status.Unknown;
            }

            if (CaptchaPersistence.IsValid(captchaControl.CaptchaId))
            {
                return Status.True;
            }

            return Status.False;
        }

        /// <summary>
        /// If a new Session is started on postback or callback, it could be a
        /// symptom of Session problems
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static Status IsNewSessionOnSumbit(CaptchaControl captchaControl)
        {
            if (null == captchaControl)
            {
                return Status.Unknown;
            }

            if (captchaControl.IsSumbit &&
                Status.True == SessionTroubleshooting.IsNewSession)
            {
                return Status.True;
            }

            return Status.False;
        }

    }
}
