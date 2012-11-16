using System;

namespace Atlantis.Framework.Providers.Interface.Currency
{
  /// <summary>
  /// Options for price formating
  /// </summary>
  [Flags()]
  public enum PriceTextOptions : int
  {
    /// <summary>
    /// Standard text without masking and negatives not allowed
    /// </summary>
    None = 0,
    /// <summary>
    /// Masks the price with X.XX instead of the actual price
    /// </summary>
    MaskPrices = 1,
    /// <summary>
    /// Allows a negative price to be displayed instead of the not offered message
    /// </summary>
    AllowNegativePrice = 2
  }
}
