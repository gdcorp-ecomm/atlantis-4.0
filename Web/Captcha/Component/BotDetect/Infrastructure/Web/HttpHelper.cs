using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.Diagnostics;

namespace BotDetect.Web
{
    internal sealed class HttpHelper
    {
        private HttpHelper()
        {
        }

        public static void StartResponse(HttpContext context)
        {
            FixEscapedQuerystrings(context);

            context.Response.Clear();
            DisallowIndexing(context);
        }

        public static void EndResponse(HttpContext context)
        {
            // skip all further events in ASP.Net pipeline and execute the EndRequest event
            context.ApplicationInstance.CompleteRequest();
        }

        public static void IgnoreRequest(HttpContext context)
        {
            // GbPlugin and other repeated requests
            context.Response.StatusCode = 200; // OK
            context.Response.ContentType = "text/plain";
            context.Response.Write("OK");
            context.ApplicationInstance.CompleteRequest();
        }

        public static void BadRequest(HttpContext context)
        {
            // usually, invalid querystring params or obvious bot requests
            context.Response.StatusCode = 400; // Bad Request
            context.ApplicationInstance.CompleteRequest();
        }

        public static void BadRequest(HttpContext context, string message)
        {
            // usually, invalid querystring params or obvious bot requests
            context.Response.StatusCode = 400; // Bad Request
            context.Response.ContentType = "text/plain";
            context.Response.Write(message);
            context.ApplicationInstance.CompleteRequest();
        }

        public static void InternalServerError(HttpContext context)
        {
            // invalid server state
            context.Response.StatusCode = 500; // Internal Server Error
            context.ApplicationInstance.CompleteRequest();
        }

        public static void InternalServerError(HttpContext context, string message)
        {
            // invalid server state
            context.Response.StatusCode = 500; // Internal Server Error
            context.Response.ContentType = "text/plain";
            context.Response.Write(message);
            context.ApplicationInstance.CompleteRequest();
        }

        public static void SmartDisallowCache(HttpContext context)
        {
            /// SSL offloading workaround - context.Request.IsSecureConnection will not be 
            /// enough in that case, we need to detect SSL on the client-side - using a 
            /// special "e" (as in "encrypted") querystring param
            bool clientSideSslDetected = (null != context.Request.QueryString["e"]);

            if (context.Request.IsSecureConnection || clientSideSslDetected)
            {
                DisallowCacheSsl(context);
            }
            else
            {
                DisallowCache(context);
            }
        }

        public static void DisallowCache(HttpContext context)
        {
            // ensure Http caching is disabled
            HttpCachePolicy cachePolicy = context.Response.Cache;
            cachePolicy.SetNoStore();
            cachePolicy.SetCacheability(HttpCacheability.NoCache);
            cachePolicy.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cachePolicy.SetExpires(DateTime.MinValue);
        }

        public static void DisallowCacheSsl(HttpContext context)
        {
            // avoid headers causing IE file-access problems over SSL
            HttpCachePolicy cachePolicy = context.Response.Cache;
            cachePolicy.SetCacheability(HttpCacheability.Public);
            cachePolicy.SetExpires(DateTime.MinValue);
        }

        public static void AllowCache(HttpContext context, DateTime lastModified, TimeSpan validity)
        {
            // allow limited duration Http caching
           
            // determine safe DateTime value
            DateTime safeDate = lastModified;
            if (lastModified > DateTime.UtcNow)
            {
                safeDate = DateTime.UtcNow;
            }

            // set headers
            HttpCachePolicy cachePolicy = context.Response.Cache;
            cachePolicy.SetCacheability(HttpCacheability.Public);
            cachePolicy.SetLastModified(safeDate);
        }

        public static void AllowCache(HttpContext context, TimeSpan validity)
        {
            // allow limited duration Http caching

            // set headers
            HttpCachePolicy cachePolicy = context.Response.Cache;
            cachePolicy.SetCacheability(HttpCacheability.Public);
            cachePolicy.SetExpires(DateTime.UtcNow + validity);
        }

        public static void DisallowIndexing(HttpContext context)
        {
            // disallow search-engine indexing of the Http response
            context.Response.AppendHeader("X-Robots-Tag", "noindex, nofollow, noarchive, nosnippet");
        }

        public static void FixEscapedQuerystrings(HttpContext context)
        {
            // fix problems caused by simple-minded bots not 
            // expanding XHTML escaped link href's
            try
            {
                string path = context.Request.Url.PathAndQuery;

                bool unescaped = false;
                while (path.Contains("&amp;"))
                {
                    if (!unescaped) { unescaped = true; }
                    path = path.Replace("&amp;", "&");
                }

                if (unescaped)
                {
                    context.RewritePath(path);
                }
            }
            catch (Exception ex) 
            {
				Debug.Assert(false, ex.Message);
								
                /// if the request comes with unescaped &amp; and the virtual folder contains
                /// spaces, RewritePath will fail - in that case, the simplest solution is to
                /// reject the request
                HttpHelper.BadRequest(context, "FixEscapedQuerystrings");
                return;
            }
        }
    }
}
