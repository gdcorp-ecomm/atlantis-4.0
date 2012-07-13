using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine
{
  public static class EngineLogging
  {
    static IErrorLogger _errorLogger;

    static EngineLogging()
    {
      _errorLogger = new DefaultEngineLogger();
    }

    public static IErrorLogger EngineLogger
    {
      get { return _errorLogger; }
      set { _errorLogger = value; }
    }
  }
}
