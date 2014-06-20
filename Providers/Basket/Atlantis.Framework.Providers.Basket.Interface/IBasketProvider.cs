namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Basket Provider. Used to interact with the Basket Service.
  /// </summary>
  public interface IBasketProvider
  {
    /// <summary>
    /// Creates a new IBasketAddRequest item which can be used to post multiple IBasketAddItems to the shoppers Basket
    /// </summary>
    /// <returns>a new instances of an IBasketAddRequest</returns>
    IBasketAddRequest NewAddRequest();

    /// <summary>
    /// Creates a new IBasketDeleteRequest item which can be used to delete multiple IBasketDeleteItem from 
    /// the shopper's basket
    /// </summary>
    /// <returns>A new instance of IBasketDeleteRequest</returns>
    IBasketDeleteRequest NewDeleteRequest();

    /// <summary>
    /// Sends the IBasketAddRequest data to the basket service to add all the items to the basket.
    /// </summary>
    /// <param name="basketAddRequest">IBasketAddRequest to post to the basket</param>
    /// <returns></returns>
    IBasketResponse PostToBasket(IBasketAddRequest basketAddRequest);

    /// <summary>
    /// Sends the IBasketDeleteRequest data to the basket service to delete all the items.
    /// </summary>
    /// <param name="deleteRequest">IBasketDeleteRequest to send to the basket</param>
    /// <returns></returns>
    IBasketResponse DeleteFromBasket(IBasketDeleteRequest deleteRequest);

    /// <summary>
    /// Creates a new IBasketAddItem
    /// </summary>
    /// <param name="unifiedProductId">UnifiedProductId</param>
    /// <param name="itemTrackingCode">Item tracking code</param>
    /// <returns>a new IBasketAddItem</returns>
    IBasketAddItem NewBasketAddItem(int unifiedProductId, string itemTrackingCode);

    /// <summary>
    /// Returns the total items in the basket
    /// </summary>
    int TotalItems { get; }
  }
}
