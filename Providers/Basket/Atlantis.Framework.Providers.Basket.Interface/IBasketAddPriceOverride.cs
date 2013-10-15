namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Overridden Price to use when adding item to the basket
  /// </summary>
  public interface IBasketAddPriceOverride
  {
    /// <summary>
    /// Current Price in USD
    /// </summary>
    int CurrentPrice { get; }

    /// <summary>
    /// List Price in USD
    /// </summary>
    int ListPrice { get; }

    /// <summary>
    /// Price Hash
    /// </summary>
    string Hash { get; }
  }
}
