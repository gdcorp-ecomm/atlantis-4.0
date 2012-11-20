namespace Atlantis.Framework.Providers.Interface.Products
{
  /// <summary>
  /// How prices should be rounded
  /// </summary>
  public enum PriceRoundingType
  {
    /// <summary>
    /// This is the proper way to round prices so we don't show a lower price on the page than the cart will show.
    /// </summary>
    RoundFractionsUpProperly = 0,
    /// <summary>
    /// AVOID. This type of rounding should be avoided; marketing should change the price in the databae so proper rounding yields what they want.
    /// </summary>
    DropFractionsForCompatibility = 1
  }
}
