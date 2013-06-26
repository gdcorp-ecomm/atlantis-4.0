using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Engine
{
  /// <summary>
  /// Interface for the Completed request wrapper created by all types of engine requests
  /// </summary>
  public interface ICompletedRequest
  {
    /// <summary>
    /// Returns the <c>ConfigElement</c> used for the request.
    /// <remarks>Can be null in exception situations.</remarks>
    /// </summary>
    ConfigElement Config { get; }

    /// <summary>
    /// Returns the <c>RequestData</c> object used for the request.
    /// <remarks>Can be null in exception situations.</remarks>
    /// </summary>
    RequestData RequestData { get; }

    /// <summary>
    /// Returns the IResponseData created by the request.
    /// <remarks>Can be null if an exception occurred.</remarks>
    /// </summary>
    IResponseData ResponseData { get; }

    /// <summary>
    /// Returns the time taken for the request to execute.
    /// <remarks>Can be zero in some exception situations.</remarks>
    /// </summary>
    TimeSpan ElapsedTime { get; }

    /// <summary>
    /// Returns an exception if an exception occured or null if no exception occurred
    /// </summary>
    AtlantisException Exception { get; }
  }
}
