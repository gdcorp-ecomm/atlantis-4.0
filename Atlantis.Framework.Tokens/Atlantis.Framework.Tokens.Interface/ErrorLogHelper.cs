using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  internal static class ErrorLogHelper
  {
    internal static void LogErrors(string key, string errorMessage, string sourceFunction, string data, IProviderContainer container)
    {
      IDebugContext debugContext;
      if (container.TryResolve(out debugContext))
      {
        debugContext.LogDebugTrackingData(key, errorMessage + " | " + data);
      }

      Engine.EngineLogging.EngineLogger.LogAtlantisException(new AtlantisException(sourceFunction, 0, errorMessage, data));
    }
  }
}
