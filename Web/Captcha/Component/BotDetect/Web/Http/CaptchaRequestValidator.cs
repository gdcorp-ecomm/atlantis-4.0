using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;

using BotDetect.Web.UI;

namespace BotDetect.Web
{
    internal sealed class CaptchaRequestValidator
    {
        private CaptchaRequestValidator()
        {
        }

        /// Since human users will always request the Captcha with an unique querystring, after a certain number
        /// of requests with the same (expired) querystring, all future identical requests return an error Http 
        /// response. This request count is kept in the Cache. This will prevent some obvious/naive bots from 
        /// collecting a large number of Captcha images or sounds for analysis.
        /// By default, 5 requests with an identical querystring are tolerated; if you want to change this number, 
        /// use <appSettings><key add="LBD_AllowedRepeatedRequests" value="NUMBER" /><appSettings> in the web.config

        public static string GetCaptchaRequestId(HttpContext context)
        {
            string id = "LBD_CaptchaRequestTracker_";

            string fragment = context.Request.QueryString["c"] as string;
            if (StringHelper.HasValue(fragment))
            {
                id += fragment;
            }

            fragment = context.Request.QueryString["t"] as string;
            if (StringHelper.HasValue(fragment))
            {
                id += fragment;
            }

            fragment = context.Request.QueryString["s"] as string;
            if (StringHelper.HasValue(fragment))
            {
                id += fragment;
            }

            fragment = context.Request.QueryString["d"] as string;
            if (StringHelper.HasValue(fragment))
            {
                id += fragment;
            }

            return id;
        }

        public static int AllowedRepeatedRequests
        {
            get
            {
                return CaptchaConfiguration.CaptchaRequestFilter.AllowedRepeatedRequests;
            }
        }

        public static bool IsFilteringEnabled
        {
            get
            {
                return CaptchaConfiguration.CaptchaRequestFilter.Enabled;
            }
        }

        public static void RecordRequest(HttpContext context)
        {
            if (!IsFilteringEnabled)
            {
                return;
            }

            string key = GetCaptchaRequestId(context);
            if (null != HttpRuntime.Cache[key])
            {
                HttpRuntime.Cache[key] = ((int)HttpRuntime.Cache[key]) + 1;
            }
            else
            {
                HttpRuntime.Cache[key] = 1;
            }
        }

        public static bool IsObviousBotAttempt(HttpContext context)
        {
            if (!IsFilteringEnabled)
            {
                return false;
            }

            string key = GetCaptchaRequestId(context);

            int count = 0;
            if (null != HttpRuntime.Cache)
            {
                object saved = HttpRuntime.Cache[key];
                if (null != saved)
                {
                    try
                    {
                        count = (int)saved;
                    }
                    catch (System.InvalidCastException ex)
                    {
                        // ignore corrupted cache data
                        Debug.Assert(false, ex.Message);
                    }
                }
            }

            bool isObviousBotAttempt = false;
            if (AllowedRepeatedRequests < count)
            {
                isObviousBotAttempt = true;
            }

            return isObviousBotAttempt;
        }


        /// <summary>
        /// GbPlugin workaround, see http://captcha.biz/doc/aspnet/known-issues.html#gbplugin
        /// </summary>
        public static bool IsRecognizedIgnoredUserAgent(HttpContext context)
        {
            if (null != context &&
                null != context.Request &&
                StringHelper.HasValue(context.Request.UserAgent) &&
                IgnoredUserAgents.IsMatch(context.Request.UserAgent))
            {
                return true;
            }

            return false;
        }



        public static readonly Regex IgnoredUserAgents =
            new Regex("(^GbPlugin$)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
    }
}
