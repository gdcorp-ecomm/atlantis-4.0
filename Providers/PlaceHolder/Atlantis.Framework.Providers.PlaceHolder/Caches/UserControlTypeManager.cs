using System;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class UserControlTypeManager
  {
    private static readonly TypeCache _userControlTypeCache = new TypeCache();

    private static Type LoadType(string location)
    {
      Type type;

      try
      {
        Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page)HttpContext.Current.Handler;
        type = currentPage.LoadControl(location).GetType();
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error finding the type for {0}. {1}", location, ex.Message));
      }

      return type;
    }

    internal static Type GetType(string location)
    {
      Type type;

      if (!_userControlTypeCache.TryGetType(location, out type))
      {
        type = LoadType(location);
        _userControlTypeCache.SetType(location, type);
      }

      return type;
    }
  }
}
