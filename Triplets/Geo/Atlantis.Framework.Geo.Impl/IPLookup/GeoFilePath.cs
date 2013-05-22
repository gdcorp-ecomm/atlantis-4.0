using Atlantis.Framework.Geo.Interface;
using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Atlantis.Framework.Geo.Impl.IPLookup
{
  internal static class GeoFilePath
  {
    public static string GetFilePath(string file)
    {
      string result = file;
      if (IPLookupDataFiles.PathType == IPLookupPathTypes.AssemblyLocation)
      {
        result = Path.Combine(AssemblyPath, file);
      }
      else if (IPLookupDataFiles.PathType == IPLookupPathTypes.WebVirtualPath)
      {
        result = HttpContext.Current.ApplicationInstance.Server.MapPath(file);
      }
      return result;
    }

    static string _assemblyPath;
    static string AssemblyPath
    {
      get
      {
        if (_assemblyPath == null)
        {
          Uri pathUri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
          _assemblyPath = pathUri.LocalPath;
        }

        return _assemblyPath;
      }
    }

  }
}
