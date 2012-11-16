namespace Atlantis.Framework.Providers.Interface.Currency
{
  /// <summary>
  /// Specifies how to round when doing currency conversion. Default is Round
  /// </summary>
  public enum CurrencyConversionRoundingType
  {
    /// <summary>
    /// Rounds the price conversion the same way Ecomm rounds (default)
    /// </summary>
    Round = 0,
    /// <summary>
    /// Ceilings the price conversion
    /// </summary>
    Ceiling = 1,
    /// <summary>
    /// Floors the price conversion
    /// </summary>
    Floor = 2
  }
}
