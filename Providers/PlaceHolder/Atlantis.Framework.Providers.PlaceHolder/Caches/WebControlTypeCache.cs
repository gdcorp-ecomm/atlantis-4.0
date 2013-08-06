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
        Assembly loadedAssembly = Assembly.Load(assembly);
        if (loadedAssembly == null)
        {
          throw new Exception("Assembly not found");
        }

        type = loadedAssembly.GetType(typeName);
        if (type == null)
        {
          throw new Exception("Type not found in assembly");
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("{0}. Assembly: {1}, Type: {2}", ex.Message, assembly, typeName));
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