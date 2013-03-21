using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public static class Logging
  {
    public static void LogException(string sourceFunction, string message, string inputData)
    {
      var aex = new AtlantisException(sourceFunction, string.Empty, "0", message, inputData, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      Engine.Engine.LogAtlantisException(aex);
    }
  }
}
