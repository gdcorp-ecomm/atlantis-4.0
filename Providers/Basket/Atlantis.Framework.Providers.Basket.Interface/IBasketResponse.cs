using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Response data from basket actions
  /// </summary>
  public interface IBasketResponse
  {
    /// <summary>
    /// overview message on response
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Number of errors that occurred
    /// </summary>
    int ErrorCount { get; }

    /// <summary>
    /// Basket errors
    /// </summary>
    IEnumerable<IBasketError> Errors { get; }

    /// <summary>
    /// Searches the basket errors for errors with the given errorNumber
    /// </summary>
    /// <param name="errorNumber">error number to search for</param>
    /// <param name="error">if found the found IBasketError will be set to the output variable</param>
    /// <returns>true if an IBasketError was found with the given number</returns>
    bool TryGetError(string errorNumber, out IBasketError error);
  }
}
