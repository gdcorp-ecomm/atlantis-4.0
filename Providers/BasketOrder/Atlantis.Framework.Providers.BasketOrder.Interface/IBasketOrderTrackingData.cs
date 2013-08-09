using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.BasketOrder.Interface
{
  public interface IBasketOrderTrackingData : IBasketOrder
  {
    string CustomerCity { get; }
    string CustomerState { get; }
    string CustomerCountry { get; }

    string TotalPriceUsdFormatted { get; }
    string TaxTotalUsdFormatted { get; }
    string ShippingTotalUsdFormatted { get; }
  }
}
