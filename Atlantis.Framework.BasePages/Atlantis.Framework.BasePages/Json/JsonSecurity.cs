using System.Text.RegularExpressions;
namespace Atlantis.Framework.BasePages.Json
{
  public static class JsonSecurity
  {
    static Regex _callbackValidationEx;

    static JsonSecurity()
    {
      AllowCrossDomainJsonDefault = true; // defaulted to true to support backwards compatibility
      _callbackValidationEx = new Regex(@"^[A-Za-z0-9_!$\-]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }

    public static bool AllowCrossDomainJsonDefault { get; set; }

    internal static bool IsCallbackValid(string callback)
    {
      return _callbackValidationEx.IsMatch(callback);
    }
  }
}
