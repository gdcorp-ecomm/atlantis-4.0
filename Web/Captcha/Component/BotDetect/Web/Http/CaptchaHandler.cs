using System;
using System.Web;
using System.IO;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using BotDetect.Persistence;

namespace BotDetect.Web
{
    /// <summary>
    /// HttpHandler used for all BotDetect Captcha requests, generating Captcha 
    /// images and sounds and loading various Captcha resources (icons, scripts, 
    /// stylesheets...)
    /// </summary>
    public class CaptchaHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public CaptchaHandler()
        {
        }

        /// <summary>
        /// IHttpHandler override, CaptchaHandler instances can be reused to 
        /// handle multiple requests, since they don't store anything in 
        /// instance fields
        /// </summary>
        public bool IsReusable { get { return true; } }

        /// <summary>
        /// IHttpHandler override, detects and executes a CaptchaHttpCommand 
        /// appropriate to the current requests
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            // if this method gets called, we can be sure the HttpHandler
            // is properly registered in the current application
            CaptchaHandler.ConfirmCaptchaHandlerInclusion();

            HttpHelper.StartResponse(context);

            try
            {
                // handler commands
                CaptchaHttpCommand command = CaptchaCommandHelper.GetCaptchaCommand(context);
                switch (command)
                {
                    case CaptchaHttpCommand.GetImage:
                        GetImage(context);
                        break;

                    case CaptchaHttpCommand.GetSound:
                        GetSound(context);
                        break;

                    case CaptchaHttpCommand.GetValidationResult:
                        GetValidationResult(context);
                        break;

                    case CaptchaHttpCommand.GetSoundIcon:
                        GetSoundIcon(context);
                        break;

                    case CaptchaHttpCommand.GetSmallSoundIcon:
                        GetSmallSoundIcon(context);
                        break;

                    case CaptchaHttpCommand.GetDisabledSoundIcon:
                        GetDisabledSoundIcon(context);
                        break;

                    case CaptchaHttpCommand.GetSmallDisabledSoundIcon:
                        GetSmallDisabledSoundIcon(context);
                        break;

                    case CaptchaHttpCommand.GetReloadIcon:
                        GetReloadIcon(context);
                        break;

                    case CaptchaHttpCommand.GetSmallReloadIcon:
                        GetSmallReloadIcon(context);
                        break;

                    case CaptchaHttpCommand.GetDisabledReloadIcon:
                        GetDisabledReloadIcon(context);
                        break;

                    case CaptchaHttpCommand.GetSmallDisabledReloadIcon:
                        GetSmallDisabledReloadIcon(context);
                        break;

                    case CaptchaHttpCommand.GetLayoutStyleSheet:
                        GetLayoutStyleSheet(context);
                        break;

                    case CaptchaHttpCommand.GetClientScriptInclude:
                        GetClientScriptInclude(context);
                        break;

                    case CaptchaHttpCommand.GetSessionTroubleshootingReport:
                        GetSessionTroubleshootingReport(context);
                        break;

                    default:
                        HttpHelper.BadRequest(context, "GetCaptchaCommand");
                        return;
                }

                HttpHelper.EndResponse(context);
            }
            catch (Exception ex)
            {
                /// error logging is done by the configured error logger
                throw new CaptchaHttpException("Check inner expection for details.", ex);
            }
        }

        /// <summary>
        /// Returns the Captcha image binary data
        /// </summary>
        internal static void GetImage(HttpContext context)
        {
            if (!IsValidRequest(context) || IsIgnoredRequest(context)) { return; }

            // saved data for the specified Captcha control in the application
            CaptchaControl captchaControl = GetControl(context);
            if (null == captchaControl)
            {
                HttpHelper.BadRequest(context, "GetControl");
                return;
            }

            // identifier of the particular control instance 
            string instanceId = GetInstanceId(context);
            if (null == instanceId)
            {
                HttpHelper.BadRequest(context, "GetInstanceId");
                return;
            }

            // response MIME type & headers
            string imageType = captchaControl.ImageFormat.ToString().ToLowerInvariant();
            context.Response.ContentType = "image/" + imageType;
            HttpHelper.SmartDisallowCache(context);
            AddSessionTroubleshootingHeader(context, captchaControl);

            // image generation
            MemoryStream image = captchaControl.CaptchaBase.GetImage(instanceId);
            CaptchaPersistence.SaveCodes(captchaControl);

            image.WriteTo(context.Response.OutputStream);
        }

        /// <summary>
        /// Returns the Captcha sound binary data
        /// </summary>
        internal static void GetSound(HttpContext context)
        {
            if (!IsValidRequest(context) || IsIgnoredRequest(context)) { return; }

            // saved data for the specified Captcha control in the application
            CaptchaControl captchaControl = GetControl(context);
            if (null == captchaControl)
            {
                HttpHelper.BadRequest(context, "GetControl");
                return;
            }

            // sound requests can be disabled with this config switch / instance property
            if (!captchaControl.CaptchaSoundEnabled)
            {
                HttpHelper.BadRequest(context, "IsSoundEnabled");
                return;
            }

            // identifier of the particular control instance 
            string instanceId = GetInstanceId(context);
            if (null == instanceId)
            {
                HttpHelper.BadRequest(context, "GetInstanceId");
                return;
            }

            // response MIME type & headers
            context.Response.ContentType = "audio/x-wav";
            context.Response.AppendHeader("content-transfer-encoding", "binary");
            HttpHelper.SmartDisallowCache(context);

            // we don't support content chunking, since audio files 
            // are regenerated randomly on each request
            context.Response.AppendHeader("accept-ranges", "none");

            AddSessionTroubleshootingHeader(context, captchaControl);

            // attachment downloading (for the javascript sound player)
            if (null == context.Request.QueryString["d"])
            {
                string filename = String.Format(CultureInfo.InvariantCulture, "captcha_{0}.wav", instanceId);
                context.Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
            }

            // sound generation
            MemoryStream sound = captchaControl.CaptchaBase.GetSound(instanceId);
            CaptchaPersistence.SaveCodes(captchaControl);

            sound.WriteTo(context.Response.OutputStream);
        }

        /// <summary>
        /// Used for client-side validation, returns Captcha validation result as JSON
        /// </summary>
        internal static void GetValidationResult(HttpContext context)
        {
            // should not check request validity since Ajax validation requests
            // made by jQuery validation will not change the Url, and can't 
            // handle IsValidRequest failures properly
            if (/*!IsValidRequest(context) ||*/ IsIgnoredRequest(context)) { return; }

            // saved data for the specified Captcha control in the application
            CaptchaControl captchaControl = GetControl(context);
            if (null == captchaControl)
            {
                HttpHelper.BadRequest(context, "GetControl");
                return;
            }

            // identifier of the particular control instance 
            string instanceId = GetInstanceId(context);
            if (null == instanceId)
            {
                HttpHelper.BadRequest(context, "GetInstanceId");
                return;
            }

            // code to validate
            string userInput = GetUserInput(context);

            // response MIME type & headers
            context.Response.ContentType = "application/json";
            HttpHelper.SmartDisallowCache(context);
            AddSessionTroubleshootingHeader(context, captchaControl);

            // JSON-encoded validation result
            bool result = captchaControl.AjaxValidate(userInput, instanceId);
            CaptchaPersistence.SaveCodes(captchaControl);

            string resultJson = GetJsonValidationResult(result);
            context.Response.Write(resultJson);
        }

        /// <summary>
        /// Used for Http SessionTroubleshooting
        /// </summary>
        internal static void GetSessionTroubleshootingReport(HttpContext context)
        {
            // saved data for the specified Captcha control in the application
            CaptchaControl captchaControl = GetControl(context);
            if (null == captchaControl)
            {
                HttpHelper.BadRequest(context, "GetControl");
                return;
            }

            // response MIME type & headers
            context.Response.ContentType = "text/plain";
            HttpHelper.SmartDisallowCache(context);

            // Html report
            if (!SessionTroubleshooting.Enabled)
            {
                HttpHelper.BadRequest(context, "SessionTroubleshooting disabled");
                return;
            }

            string sessionReport = SessionTroubleshooting.PlainTextReport(captchaControl);
            if (StringHelper.HasValue(sessionReport))
            {
                HttpHelper.InternalServerError(context, sessionReport);
                return;
            }

            context.Response.Write("OK");
        }

        public static void AddSessionTroubleshootingHeader(HttpContext context, CaptchaControl captchaControl)
        {
            if (!SessionTroubleshooting.Enabled) { return; }

            string report = SessionTroubleshooting.HttpHeaderReport(captchaControl);
            if (!StringHelper.HasValue(report)) { return; }

            context.Response.AppendHeader("X-BotDetect-Str", report);
        }

        internal static void GetSoundIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.SoundIcon.gif", "image/gif");
        }

        internal static void GetSmallSoundIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.SmallSoundIcon.gif", "image/gif");
        }

        internal static void GetDisabledSoundIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.DisabledSoundIcon.gif", "image/gif");
        }

        internal static void GetSmallDisabledSoundIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.SmallDisabledSoundIcon.gif", "image/gif");
        }

        internal static void GetReloadIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.ReloadIcon.gif", "image/gif");
        }

        internal static void GetSmallReloadIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.SmallReloadIcon.gif", "image/gif");
        }

        internal static void GetDisabledReloadIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.DisabledReloadIcon.gif", "image/gif");
        }

        internal static void GetSmallDisabledReloadIcon(HttpContext context)
        {
            GetWebResource(context, ".Web.Resources.SmallDisabledReloadIcon.gif", "image/gif");
        }

        internal static void GetLayoutStyleSheet(HttpContext context)
        {
            GetWebResource(context, ".Web.Style.BotDetectLayout.css", "text/css");
        }

        internal static void GetSoundPackageWarningStyle(HttpContext context)
        {
            GetWebResource(context, ".Web.Style.SoundPackageWarning.css", "text/css");
        }

        internal static void GetClientScriptInclude(HttpContext context)
        {
            GetWebResource(context, ".Web.ClientSide.BotDetectScripts.js", "text/javascript");
        }

        internal static void GetWebResource(HttpContext context, string identifier, string mimeType)
        {
            // read contents from the embedded resource file
            byte[] rawData = ResourceHelper.GetResource(identifier);

            // response MIME type & headers
            context.Response.ContentType = mimeType;
            HttpHelper.AllowCache(context, TimeSpan.FromHours(1));

            MemoryStream dataStream = new MemoryStream(rawData);
            dataStream.WriteTo(context.Response.OutputStream);
        }

        /// <summary>
        /// some basic request checks
        /// </summary>
        internal static bool IsValidRequest(HttpContext context)
        {
            // some basic request checks
            CaptchaRequestValidator.RecordRequest(context);

            if (CaptchaRequestValidator.IsObviousBotAttempt(context))
            {
                HttpHelper.BadRequest(context, "IsObviousBotAttempt");
                return false;
            }

            return true;
        }


        /// <summary>
        /// some basic request checks
        /// </summary>
        internal static bool IsIgnoredRequest(HttpContext context)
        {
            if (CaptchaRequestValidator.IsRecognizedIgnoredUserAgent(context))
            {
                HttpHelper.IgnoreRequest(context);
                return true;
            }

            return false;
        }

        /// <summary>
        /// restore the CaptchaControl data associated with the given captchaId
        /// </summary>
        internal static CaptchaControl GetControl(HttpContext context)
        {
            string captchaId = context.Request.QueryString["c"] as string;
            if (!StringHelper.HasValue(captchaId))
            {
                return null;
            }

            // access the appropriate persistance provider
            CaptchaControl captchaControl = CaptchaPersistence.Load(captchaId);

            return captchaControl;
        }

        /// <summary>
        /// extract the exact Captcha code instance referenced by the request
        /// </summary>
        internal static string GetInstanceId(HttpContext context)
        {
            string instanceId = context.Request.QueryString["t"] as string;
            if (!StringHelper.HasValue(instanceId))
            {
                return null;
            }

            // instanceId consists of 32 lowercase hexadecimal digits
            if (32 != instanceId.Length)
            {
                return null;
            }

            if (!CaptchaBase.ValidCaptchaId.IsMatch(instanceId))
            {
                return null;
            }

            return instanceId;
        }

        /// <summary>
        /// extract the exact Captcha code instance referenced by the request
        /// </summary>
        internal static string GetUserInput(HttpContext context)
        {
            // BotDetect built-in Ajax Captcha validation
            string input = context.Request.QueryString["i"] as string;
            if (StringHelper.HasValue(input))
            {
                return input;
            }

            // jQuery validation support, the input key may be just about anything,
            // so we have to loop through fields and take the first unrecognized one
            foreach (string paramName in context.Request.QueryString.AllKeys)
            {
                if (!(paramName.Equals("get", StringComparison.OrdinalIgnoreCase) ||
                      paramName.Equals("c", StringComparison.OrdinalIgnoreCase) ||
                      paramName.Equals("t", StringComparison.OrdinalIgnoreCase) ||
                      paramName.Equals("d", StringComparison.OrdinalIgnoreCase)))
                {
                    input = context.Request.QueryString[paramName];
                }
            }
            return input;
        }

        /// <summary>
        /// Encodes the Captcha validation result in a simple JSON wrapper
        /// </summary>
        internal static string GetJsonValidationResult(bool result)
        {
            return result.ToString().ToLowerInvariant();
        }


        /// <summary>
        /// Reports HttpHandler problems found with the given CaptchaControl 
        /// instance, formatted for Html output
        /// </summary>
        /// <param name="captchaControl"></param>
        /// <returns></returns>
        public static string HtmlReport()
        {
            StringBuilder writer = new StringBuilder();

            if (CaptchaConfiguration.CaptchaHttpHandlerTroubleshooting.Enabled)
            {
                if (Status.False == CaptchaHandler.IsCaptchaHandlerRegistered)
                {
                    // critical error - we need the HttpHandler registered
                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">Captcha HttpHandler appears not to be registered.</p>");
                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">If you can see the Captcha image, it's safe to disable this troubleshooting check (<a target=\"_blank\" href=\"http://captcha.biz/doc/aspnet/howto/captcha-configuration.html#captcha_httphandler_troubleshooting\">documentation</a>).</p>");
                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">Please make sure your web.config file includes the following declarations:</p>");

                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">&lt;system.web&gt;</p>");
                    writer.AppendLine("<p style=\"font-size: 12px; color: red; padding-left:5px;\">&lt;httpHandlers&gt;</p>");
                    writer.AppendFormat("<p style=\"font-size: 12px; color: red; padding-left:15px;\">&lt;add verb=\"GET\" path=\"{0}\" type=\"BotDetect.Web.CaptchaHandler, BotDetect\"/&gt;</p>",
                        CaptchaConfiguration.CaptchaUrls.RequestPath);
                    writer.AppendLine();

                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">...</p>");

                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">&lt;system.webServer&gt;</p>");
                    writer.AppendLine("<p style=\"font-size: 12px; color: red; padding-left:5px;\">&lt;handlers&gt;</p>");
                    writer.AppendLine("<p style=\"font-size: 12px; color: red; padding-left:15px;\">&lt;remove name=\"BotDetectCaptchaHandler\"/&gt;</p>");
                    writer.AppendFormat("<p style=\"font-size: 12px; color: red; padding-left:15px;\">&lt;add name=\"BotDetectCaptchaHandler\" preCondition=\"integratedMode\" verb=\"GET\" path=\"{0}\" type=\"BotDetect.Web.CaptchaHandler, BotDetect\"/&gt;</p>",
                        CaptchaConfiguration.CaptchaUrls.RequestPath);
                    writer.AppendLine();

                    writer.AppendLine("<br />");
                    writer.AppendLine("<p style=\"font-size: 12px; color: red;\">If the Captcha HttpHandler is registered, it could also be inaccessible due to your Url Routing/Rewriting settings.</p>");
                }
            }

            return writer.ToString();
        }

        // CaptchaHandler registration status
        private static volatile Status isCaptchaHandlerRegistered = Status.False;
        private static readonly object isCaptchaHandlerRegisteredLock = new object();

        private static void ConfirmCaptchaHandlerInclusion()
        {
            isCaptchaHandlerRegistered = Status.True;
        }

        /// <summary>
        /// Check is the CaptchaHandler included in the current HttpApplication instance
        /// (works only in Full Trust)
        /// </summary>
        public static Status IsCaptchaHandlerRegistered
        {
            get
            {
                if (Status.False == isCaptchaHandlerRegistered)
                {
                    lock (isCaptchaHandlerRegisteredLock)
                    {
                        if (Status.False == isCaptchaHandlerRegistered)
                        {
                            // check by making a test Http request
                            try
                            {
                                MakeTestRequest();
                                isCaptchaHandlerRegistered = Status.True;
                            }
                            catch (Exception e)
                            {
                                isCaptchaHandlerRegistered = HandleError(e);
                            }
                        }
                    }
                }

                return isCaptchaHandlerRegistered;
            }
        }

        private static void MakeTestRequest()
        {
            Uri url = HttpContext.Current.Request.Url;

            string testUrl = url.Scheme + "://" + url.Host + ":" + url.Port.ToString();

            testUrl += CaptchaUrls.Absolute.ClientScriptIncludeUrl;

            WebRequest request = WebRequest.Create(testUrl);
            WebResponse response = request.GetResponse();
        }

        private static Status HandleError(Exception e)
        {
            // WebExceptions get special treatment
            System.Net.WebException ex = e as System.Net.WebException;
            if (null != ex)
            {
                // specifically check for 404 errors
                using (HttpWebResponse response = ex.Response as HttpWebResponse)
                {
                    if (null != response &&
                        HttpStatusCode.NotFound == response.StatusCode)
                    {
                        return Handle404(e);
                    }
                }

                return HandleOtherNetworkError(e);
            }

            return HandleOtherError(e);
        }

        private static Status Handle404(Exception e)
        {
            // network path unavailable, most likey means the HttpHandler is not registered
            CaptchaLogging.Trace("Warning", "Unable to access Captcha HttpHandler paths - the handler is not properly registered or is inaccessible because of Url routing/rewriting", e);
            return Status.False;
        }

        private static Status HandleOtherNetworkError(Exception e)
        {
            // in case of any other errors, the result is unknown and a warning is logged
            CaptchaLogging.Trace("Warning", "Unable to determine is the HttpHandler registered or not due to an unknown network issue", e);
            return Status.Unknown;
        }

        private static Status HandleOtherError(Exception e)
        {
            // in case of any other errors, the result is unknown and a warning is logged
            CaptchaLogging.Trace("Warning", "Unable to determine is the HttpHandler registered or not due to insufficient permissions or similar reasons", e);
            return Status.Unknown;
        }
    }
}
