using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web;

namespace BotDetect
{
    internal sealed class ServerHelper
    {
        private ServerHelper() { }


        public static string PhysicalApplicationPath
        {
            get
            {
                string path = null;

                if (null != HttpContext.Current)
                {
                    try
                    {
                        path = HttpContext.Current.Request.PhysicalApplicationPath;
                    }
                    catch (Exception ex) // ignore errors
                    {
                        System.Diagnostics.Debug.Assert(false, ex.Message);
                    }
                }

                return path;
            }
        }


        public static string ResolvePhysicalFolderPath(string specifiedFolder)
        {
            string resolvedPath = null;
            
            if (specifiedFolder.StartsWith("/", StringComparison.OrdinalIgnoreCase) ||
                (specifiedFolder.StartsWith("\\", StringComparison.OrdinalIgnoreCase) && 
                 !specifiedFolder.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
               ) 
            {
                // assembly location relative path
                resolvedPath = ServerHelper.GetPhysicalPathFromAssemblyRelative(specifiedFolder);
            }
            else if (specifiedFolder.StartsWith("~/", StringComparison.OrdinalIgnoreCase)) 
            {
                // application root relative path
                resolvedPath = ServerHelper.GetPhysicalPathFromApplicationRelative(specifiedFolder);
            }
            else 
            {
                // absolute path specification
                resolvedPath = specifiedFolder;
            }

            return resolvedPath;
        }


        private static string GetPhysicalPathFromAssemblyRelative(string specifiedFolder)
        {
            string path = null;

            try
            {
                // Assembly folder relative path
                string containingFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
                string subFolder = specifiedFolder.Substring(1);
                path = Path.Combine(containingFolder, subFolder);
            }
            catch (Exception ex) // ignore errors
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

            return path;
        }


        private static string GetPhysicalPathFromApplicationRelative(string specifiedFolder)
        {
            string path = null;

            try
            {
                // ASP.NET Application root relative path
                string containingFolder = PhysicalApplicationPath;
                string subFolder = specifiedFolder.Substring(2).Replace("/", "\\");
                path = Path.Combine(containingFolder, subFolder);
            }
            catch (Exception ex) // ignore errors
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

            return path;
        }
    }
}
