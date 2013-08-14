using System.Xml.Linq;
namespace Atlantis.Framework.Providers.BasketOrder.Interface
{
  public interface IBasketOrderProvider
  {
    bool TryGetBasketOrder(out IBasketOrder basketOrder, string orderId = "");
    bool TryGetBasketOrderFromReceiptXml(out IBasketOrder basketOrder, string orderXml);
    bool TryGetBasketOrderFromReceiptXml(out IBasketOrder basketOrder, XDocument orderXml);

    bool TryGetBasketOrderTrackingData(out IBasketOrderTrackingData basketOrder, string orderId = "");
    bool TryGetBasketOrderTrackingDataFromReceiptXml(out IBasketOrderTrackingData basketOrder, string orderXml);
    bool TryGetBasketOrderTrackingDataFromReceiptXml(out IBasketOrderTrackingData basketOrder, XDocument orderXml);
  }
}
