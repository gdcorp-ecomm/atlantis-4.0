using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine
{
  /// <summary>
  /// Static class used to change the default engine logging sink
  /// </summary>
  public static class EngineLogging
  {
    static IErrorLogger _errorLogger;

    static EngineLogging()
    {
      _errorLogger = new DefaultEngineLogger();
    }

    /// <summary>
    /// Gets or Sets the <c>IErrorLogger that the Engine will use.</c>
    /// </summary>
    public static IErrorLogger EngineLogger
    {
      get { return _errorLogger; }
      set { _errorLogger = value; }
    }
  }
}
