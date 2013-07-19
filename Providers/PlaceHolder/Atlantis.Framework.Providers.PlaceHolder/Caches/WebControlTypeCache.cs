using System;
using System.Reflection;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlTypeCache
  {
    private static readonly GenericCache<Type> _webControlTypeCache = new GenericCache<Type>();

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

      if (!_webControlTypeCache.TryGet(key, out type))
      {
        type = LoadType(assembly, typeName);
        _webControlTypeCache.Set(key, type);
      }

      return type;
    }
  }
}
