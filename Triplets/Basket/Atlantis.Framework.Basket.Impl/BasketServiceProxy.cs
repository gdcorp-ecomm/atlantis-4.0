using Atlantis.Framework.Basket.Impl.GdBasketService;

namespace Atlantis.Framework.Basket.Impl
{
  /// <summary>
  /// The BasketServiceProxy class exposes the GDBasketService.
  /// items from cart.
  /// </summary>
  public class BasketServiceProxy : IBasketService
  {
    /// <summary>
    /// Deletes specified item pairs.
    /// </summary>
    /// <param name="shopperID">Shopper id.</param>
    /// <param name="basketType">Basket type.</param>
    /// <param name="pairs">Item pairs to be deleted.</param>
    /// <param name="lockMode">An integer that represents lock mode.</param>
    /// <param name="wsUrl">Web service Url.</param>
    /// <param name="timeout">Timeout in miliseconds.</param>
    /// <returns>A string that has response data.</returns>
    public string DeleteItemsByPairWithType(string shopperID, string basketType, string pairs, int lockMode, string wsUrl,
      int timeout)
    {
      string responseXml;

      using (var basketService = new WscgdBasketService())
      {
        basketService.Url = wsUrl;
        basketService.Timeout = timeout;
        responseXml = basketService.DeleteItemsByPairWithType(shopperID, basketType, pairs, lockMode);
      }

      return responseXml;
    }
  }
}