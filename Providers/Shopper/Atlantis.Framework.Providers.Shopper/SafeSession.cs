using System.Web;

namespace Atlantis.Framework.Providers.Shopper
{
  internal static class SafeSession
  {
    public static bool IsSessionAvailable
    {
      get
      {
        return (HttpContext.Current != null) && (HttpContext.Current.Session != null);
      }
    }

    public static object GetSessionItem(string key)
    {
      if (!IsSessionAvailable)
      {
        return null;
      }

      return HttpContext.Current.Session[key];
    }

    public static void SetSessionItem(string key, object value)
    {
      if (IsSessionAvailable)
      {
        HttpContext.Current.Session[key] = value;
      }
    }
  }
}
