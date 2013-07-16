using System;
using System.Reflection;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlTypeManager
  {
    private static readonly TypeCache _webControlTypeCache = new TypeCache();

    private static Type LoadType(string assembly, string typeName)
    {
      Type type;

      try
      {
        type = Assembly.Load(assembly).GetType(typeName);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error finding the type {0}. {1}", typeName, ex.Message));
      }

      return type;
    }

    internal static Type GetType(string assembly, string typeName)
    {
      string key = assembly + typeName;
      Type type;

      if (!_webControlTypeCache.TryGetType(key, out type))
      {
        type = LoadType(assembly, typeName);
        _webControlTypeCache.SetType(key, type);
      }

      return type;
    }
  }
}
