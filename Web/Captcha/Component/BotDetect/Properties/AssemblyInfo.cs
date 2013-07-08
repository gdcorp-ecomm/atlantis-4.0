using System;
using System.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;


// assembly information
#if (DEBUG)

[assembly: AssemblyTitle("BotDetect ASP.NET 4.0 CAPTCHA - Debug - GODADDY UPDATE")]
[assembly: AssemblyConfiguration("Debug")]

#else

[assembly: AssemblyTitle("BotDetect ASP.NET 4.0 CAPTCHA - Release - GODADDY UPDATE")]
[assembly: AssemblyConfiguration("Release")]

#endif

[assembly: AssemblyDescription("ASP.NET 4.0 Captcha Web Control - GODADDY UPDATE")]
[assembly: AssemblyCompany("Lanapsoft, Inc.")]
[assembly: AssemblyProduct("BotDetect")]
[assembly: AssemblyCopyright("Copyright © Lanapsoft, Inc. 2004-2012")]
[assembly: AssemblyTrademark("BotDetect, BotDetect CAPTCHA, Lanap, Lanap Software, Lanap CAPTCHA, Lanap BotDetect, Lanap BotDetect CAPTCHA, Lanapsoft, Lanapsoft CAPTCHA, Lanapsoft BotDetect, Lanapsoft BotDetect CAPTCHA")]
[assembly: AssemblyCulture("")]	
[assembly: TagPrefix("BotDetect", "BotDetect")]
[assembly: AssemblyVersion("3.13.12.1")]
[assembly: AssemblyFileVersion("4.13.07.08")]


// assembly settings
[assembly: SecurityRules(SecurityRuleSet.Level1)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]


namespace BotDetect
{
    internal sealed class AssemblyInfo
    {
        private AssemblyInfo() { }

        // hardcoding the assembly name works even in Minimal trust, 
        // while getting it dynamically via:
        // Assembly.GetExecutingAssembly().GetName().Name
        // requires Medium trust at least
        public static readonly string AssemblyName = "BotDetect";
    }
}