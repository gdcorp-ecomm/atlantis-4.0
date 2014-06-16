namespace Atlantis.Framework.Basket.Impl
{
  /// <summary>
  /// Exposes the GDBasketService
  /// </summary>
  public interface IBasketService
  {
    /// <summary>
    /// Delete specified item pairs.
    /// </summary>
    /// <param name="shopperID">Shopper id.</param>
    /// <param name="basketType">Basket type.</param>
    /// <param name="pairs">Item pairs to be deleted.</param>
    /// <param name="lockMode">An integer that represents lock mode.</param>
    /// <param name="wsUrl">Web service Url.</param>
    /// <param name="timeout">Timeout in miliseconds.</param>
    /// <returns>A string that has response data.</returns>
    string DeleteItemsByPairWithType(string shopperID, string basketType, string pairs, int lockMode, string wsUrl,
      int timeout);
  }
}
