using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class ErrorLogger
  {
    internal static void LogException(string message, string sourceFunction, string data)
    {
      AtlantisException aex = new AtlantisException(sourceFunction, 0, message, data);

      Engine.Engine.LogAtlantisException(aex);
    }
  }
}
