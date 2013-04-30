using System;
using System.Threading;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  public static class SplitTestingConfiguration
  {
    public static string DefaultCategoryName;

    internal static void LogError(string methodName, Exception ex)
    {
      try
      {
        if (ex.GetType() != typeof(ThreadAbortException))
        {
          var message = ex.Message + Environment.NewLine + ex.StackTrace;
          var source = methodName;
          var aex = new AtlantisException(source, "0", message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      catch (Exception) { }
    }
  }
}
