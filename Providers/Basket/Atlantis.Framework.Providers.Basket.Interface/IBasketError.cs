namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Error returned by basket service call
  /// </summary>
  public interface IBasketError
  {
    /// <summary>
    /// Error number
    /// </summary>
    string Number { get; }

    /// <summary>
    /// Error description
    /// </summary>
    string Description { get; }
  }
}
