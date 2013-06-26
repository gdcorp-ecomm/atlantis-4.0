namespace Atlantis.Framework.Engine
{
  /// <summary>
  /// Engine Logging Status
  /// </summary>
  public enum LoggingStatusType
  {
    /// <summary>
    /// The last attempt to log was successful
    /// </summary>
    WorkingNormally = 0,
    
    /// <summary>
    /// The last attempt to log was unsuccessful
    /// </summary>
    Error = 1,

    /// <summary>
    /// The current IErrorLogger reference is null
    /// </summary>
    NullLogger = 2
  }
}
