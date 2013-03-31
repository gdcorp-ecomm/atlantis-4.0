using System.Text.RegularExpressions;
namespace Atlantis.Framework.BasePages.Json
{
  public static class JsonSecurity
  {
    static Regex _callbackValidationEx;

    static JsonSecurity()
    {
      ValidateCallbackValue = false;
      AllowCrossDomainJsonDefault = true; // defaulted to true to support backwards compatibility
      _callbackValidationEx = new Regex(@"^[A-Za-z0-9_!$\-\.]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }

    public static bool AllowCrossDomainJsonDefault { get; set; }
    public static bool ValidateCallbackValue { get; set; }

    internal static bool IsCallbackValid(string callback)
    {
      if (!ValidateCallbackValue)
      {
        return true;
      }
      return _callbackValidationEx.IsMatch(callback);
    }
  }
}
