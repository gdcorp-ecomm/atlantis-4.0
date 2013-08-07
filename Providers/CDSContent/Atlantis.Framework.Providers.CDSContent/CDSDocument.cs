using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal abstract class CDSDocument
  {
    public abstract int EngineRequestId { get; }

    public string RawPath { get; set; }
    public string ProcessedPath { get; set; }
    public bool ByPassDataCache { get; set; }

    public static bool IsValidContentId(string text)
    {
      bool result = false;
      if (text != null)
      {
        string pattern = @"^[0-9a-fA-F]{24}$";
        result = Regex.IsMatch(text, pattern);
      }
      return result;
    }

    public static string ToQueryString(NameValueCollection nvc)
    {
      return string.Join("&", nvc.AllKeys.SelectMany(key => nvc.GetValues(key).Select(value => string.Format("{0}={1}", key, value))).ToArray());
    }
  }
}
