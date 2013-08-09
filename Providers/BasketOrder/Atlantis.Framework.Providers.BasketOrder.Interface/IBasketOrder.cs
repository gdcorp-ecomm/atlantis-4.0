using System.Collections.Generic;

namespace Atlantis.Framework.Providers.BasketOrder.Interface
{
  public interface IBasketOrder
  {
    IEnumerable<IBasketOrderItem> OrderItems { get; }

    string OrderId { get; }
    string TransactionalCurrency { get; }

    double TotalPriceUsd { get; }
    double TotalPrice { get; }
    
    double TaxTotalUsd { get; }
    double TaxTotal { get; }

    double ShippingTotalUsd { get; }
    double ShippingTotal { get; }

    string ToXml();
  }
}
