namespace Atlantis.Framework.Providers.BasketOrder.Interface
{
  public interface IBasketOrderProvider
  {
    bool TryGetBasketOrder(string orderId, out IBasketOrder basketOrder, string basketType = null);
    bool TryGetBasketOrderTrackingData(string orderId, out IBasketOrderTrackingData basketOrder, string basketType = null);
  }
}
