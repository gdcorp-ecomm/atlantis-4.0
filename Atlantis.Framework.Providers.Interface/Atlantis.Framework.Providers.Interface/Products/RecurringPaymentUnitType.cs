namespace Atlantis.Framework.Providers.Interface.Products
{
  /// <summary>
  /// The native period this product is in the catalog
  /// </summary>
  public enum RecurringPaymentUnitType
  {
    /// <summary>
    /// No valid known value was returned from database
    /// </summary>
    Unknown,
    /// <summary>
    /// Monthly
    /// </summary>
    Monthly,
    /// <summary>
    /// Yearly
    /// </summary>
    Annual,
    /// <summary>
    /// Twice a year
    /// </summary>
    SemiAnnual,
    /// <summary>
    /// Quarterly
    /// </summary>
    Quarterly
  }
}
