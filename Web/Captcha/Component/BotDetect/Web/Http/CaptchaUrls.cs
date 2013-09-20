using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;

using BotDetect.Persistence;

namespace BotDetect.Web
{
    /// <summary>
    /// Allow multiple schemes of Url generation, conforming to a simple interface
    /// </summary>
    public interface ICaptchaUrlsGenerator
    {
        /// <summary>
        /// Generates the Captcha image Url with the given parameter values
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        string CaptchaImageUrl(string captchaId, string instanceId);

        /// <summary>
        /// Generates the Captcha sound Url with the given parameter values
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        string CaptchaSoundUrl(string captchaId, string instanceId);

        /// <summary>
        /// Generates the Captcha Ajax validation result Url with the given parameter values
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        string CaptchaValidationResultUrl(string captchaId, string instanceId);

        /// <summary>
        /// Gets the BotDetect reload icon Url
        /// </summary>
        string ReloadIconUrl { get; }

        /// <summary>
        /// Gets the BotDetect small reload icon Url (used when the Captcha 
        /// image height is < 40px;)
        /// </summary>
        string SmallReloadIconUrl { get; }

        /// <summary>
        /// Gets the BotDetect sound icon Url
        /// </summary>
        string SoundIconUrl { get; }

        /// <summary>
        /// Gets the BotDetect small sound icon Url (used when the Captcha 
        /// image height is < 40px;)
        /// </summary>
        string SmallSoundIconUrl { get; }

        /// <summary>
        /// Gets the BotDetect disabled sound icon Url (used when the BotDetect 
        /// pronunciation sound package for the current locale can not be found 
        /// and the "warn about missing sound packages" setting is active)
        /// </summary>
        string DisabledSoundIconUrl { get; }

        /// <summary>
        /// Gets the BotDetect layout stylesheet Url
        /// </summary>
        string LayoutStyleSheetUrl { get; }

        /// <summary>
        /// Gets the BotDetect client-side include Url
        /// </summary>
        string ClientScriptIncludeUrl { get; }
    }


    /// <summary>
    /// Abstract class wrapping translation of CaptchaHttpCommand values to querystrings
    /// </summary>
    internal abstract class CaptchaUrlsGeneratorBase : ICaptchaUrlsGenerator
    {
        /// <summary>
        /// Gets the currently configured BotDetect Captcha HttpHandler path
        /// </summary>
        public abstract string GetCaptchaHandlerPath();


        public string CaptchaHandlerPath
        {
            get
            {
                return this.GetCaptchaHandlerPath();
            }
        }


        #region ICaptchaUrlsGenerator Members

        public string CaptchaImageUrl(string captchaId, string instanceId)
        {
            string url = this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetImage);

            url += String.Format(CultureInfo.InvariantCulture, "&amp;c={0}&amp;t={1}", captchaId, instanceId);

            if (null != HttpContext.Current && HttpContext.Current.Request != null && null != HttpContext.Current.Request.Url && null != HttpContext.Current.Request.Url.OriginalString && null != HttpContext.Current.Request.Url.LocalPath)
            {
              string baseUrl = string.Concat(HttpContext.Current.Request.Url.OriginalString.Replace(HttpContext.Current.Request.Url.LocalPath, string.Empty), "/");
              url = baseUrl + url;
            }
            // ensure cookieless ASP.NET Session compatibility
            if (null != HttpContext.Current && null != HttpContext.Current.Response)
            {
                url = HttpContext.Current.Response.ApplyAppPathModifier(url);
            }

            return url;
        }

        public string CaptchaSoundUrl(string captchaId, string instanceId)
        {
            string url = this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetSound);

            url += String.Format(CultureInfo.InvariantCulture, "&amp;c={0}&amp;t={1}", captchaId, instanceId);

            // ensure sound functionality even in clients that manage to forget the SessionID
            string sid = CustomSessionIdManager.EncryptedSessionId;
            if (StringHelper.HasValue(sid))
            {
                url += String.Format(CultureInfo.InvariantCulture, "&amp;s={0}", sid);
            }

            // ensure cookieless ASP.NET Session compatibility
            if (null != HttpContext.Current && null != HttpContext.Current.Response)
            {
                url = HttpContext.Current.Response.ApplyAppPathModifier(url);
            }

            return url;
        }

        public string CaptchaValidationResultUrl(string captchaId, string instanceId)
        {
            string url = this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetValidationResult);

            url += String.Format(CultureInfo.InvariantCulture, "&amp;c={0}&amp;t={1}", captchaId, instanceId);

            // ensure cookieless ASP.NET Session compatibility
            if (null != HttpContext.Current && null != HttpContext.Current.Response)
            {
                url = HttpContext.Current.Response.ApplyAppPathModifier(url);
            }

            return url;
        }


        public string SoundIconUrl
        {
            get 
            { 
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetSoundIcon); 
            }
        }

        public string SmallSoundIconUrl
        {
            get 
            { 
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetSmallSoundIcon);
            }
        }

        public string DisabledSoundIconUrl
        {
            get 
            { 
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetDisabledSoundIcon); 
            }
        }

        public string ReloadIconUrl
        {
            get
            {
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetReloadIcon);
            }
        }

        public string SmallReloadIconUrl
        {
            get
            {
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetSmallReloadIcon);
            }
        }

        public string LayoutStyleSheetUrl
        {
            get
            {
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetLayoutStyleSheet);
            }
        }

        public string ClientScriptIncludeUrl
        {
            get
            {
                return this.CaptchaHandlerPath + CaptchaCommandHelper.GetQuerystring(CaptchaHttpCommand.GetClientScriptInclude);
            }
        }

        #endregion
    }


    /// <summary>
    /// Uses the relative handler path as base for all urls
    /// </summary>
    internal class RelativeCaptchaUrls : CaptchaUrlsGeneratorBase, ICaptchaUrlsGenerator
    {
        public override string GetCaptchaHandlerPath()
        {
            string handlerPath = CaptchaConfiguration.CaptchaUrls.RequestPath;

            // only use absolute paths if the handler path has been explicitly 
            // configured as such by the user in web.config
            if (handlerPath.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            {
                handlerPath = VirtualPathUtility.ToAbsolute(handlerPath);
            }

            return handlerPath;
        }
    }


    /// <summary>
    /// Uses the absolute handler path as base for all urls
    /// </summary>
    internal class AbsoluteCaptchaUrls : CaptchaUrlsGeneratorBase, ICaptchaUrlsGenerator
    {
        public override string GetCaptchaHandlerPath()
        {
            string handlerPath = CaptchaConfiguration.CaptchaUrls.RequestPath;

            // always use absolute paths, but avoid double transformations (in case both the
            // per-instance and app config settings specify url absoluteness
            if (!handlerPath.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            {
                handlerPath = "~/" + handlerPath;
            }

            handlerPath = VirtualPathUtility.ToAbsolute(handlerPath);

            return handlerPath;
        }
    }


    /// <summary>
    /// Encapsulates Captcha Url generation according to current state and configured BotDetect options.
    /// </summary>
    public sealed class CaptchaUrls
    {
        private CaptchaUrls() { }

        public static ICaptchaUrlsGenerator Relative = new RelativeCaptchaUrls();

        public static ICaptchaUrlsGenerator Absolute = new AbsoluteCaptchaUrls();


        // the top-level helper class also exposes the same interface via static access 
        // for backwards compatibility (and as a sort of a default when the underlying
        // scheme is not explicitly specified
        #region ICaptchaUrlsGenerator Members

        public static string CaptchaImageUrl(string captchaId, string instanceId)
        {
            return Relative.CaptchaImageUrl(captchaId, instanceId);
        }

        public static string CaptchaSoundUrl(string captchaId, string instanceId)
        {
            return Relative.CaptchaSoundUrl(captchaId, instanceId);
        }

        public static string CaptchaValidationResultUrl(string captchaId, string instanceId)
        {
            return Relative.CaptchaValidationResultUrl(captchaId, instanceId);
        }

        public static string SoundIconUrl
        {
            get { return Relative.SoundIconUrl; }
        }

        public static string SmallSoundIconUrl
        {
            get { return Relative.SmallSoundIconUrl; }
        }

        public static string DisabledSoundIconUrl
        {
            get { return Relative.DisabledSoundIconUrl; }
        }

        public static string ReloadIconUrl
        {
            get { return Relative.ReloadIconUrl; }
        }

        public static string SmallReloadIconUrl
        {
            get { return Relative.SmallReloadIconUrl; }
        }

        public static string LayoutStyleSheetUrl
        {
            get { return Relative.LayoutStyleSheetUrl; }
        }

        public static string ClientScriptIncludeUrl
        {
            get { return Relative.ClientScriptIncludeUrl; }
        }

        #endregion
    }
}
