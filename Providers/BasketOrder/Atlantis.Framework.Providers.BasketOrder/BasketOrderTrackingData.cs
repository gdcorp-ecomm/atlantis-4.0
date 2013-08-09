using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Providers.BasketOrder.Interface;

namespace Atlantis.Framework.Providers.BasketOrder
{
  public class BasketOrderTrackingData : BasketOrder, IBasketOrderTrackingData
  {
    internal BasketOrderTrackingData(XDocument orderXml)
      : base(orderXml)
    {
    }

    internal BasketOrderTrackingData(string orderXml)
      : base(orderXml)
    {
    }

    private static IBasketOrderTrackingData _emptyBasketOrder;
    internal static IBasketOrderTrackingData EmptyBasketOrder
    {
      get
      {
        if (_emptyBasketOrder == null)
        {
          var emptyOrderXml = XDocument.Parse(EMPTY_ORDER_ELEMENT);
          _emptyBasketOrder = new BasketOrderTrackingData(emptyOrderXml);
        }
        return _emptyBasketOrder;
      }
    }

    private string _customerCity;
    public string CustomerCity
    {
      get
      {
        if (_customerCity == null)
        {
          var customerCityAttribute = (string)(OrderDetailElement.Attribute(BasketOrderDetailAttributes.BillToCity) ??
                                        OrderDetailElement.Attribute(BasketOrderDetailAttributes.ShipToCity));
          _customerCity = customerCityAttribute ?? string.Empty;
        }
        return _customerCity;
      }
    }

    private string _customerState;
    public string CustomerState
    {
      get
      {
        if (_customerState == null)
        {
          var customerStateAttribute = (string)(OrderDetailElement.Attribute(BasketOrderDetailAttributes.BillToState) ??
                                       OrderDetailElement.Attribute(BasketOrderDetailAttributes.ShipToState));
          _customerState = customerStateAttribute ?? string.Empty;
        }
        return _customerState;
      }
    }

    private string _customerCountry;
    public string CustomerCountry
    {
      get
      {
        if (_customerCountry == null)
        {
          try
          {
            var customerCountryAttribute = (string)(OrderDetailElement.Attribute(BasketOrderDetailAttributes.BillToCountry) ??
                                           OrderDetailElement.Attribute(BasketOrderDetailAttributes.ShipToCountry));
            _customerCountry = customerCountryAttribute ?? string.Empty;
          }
          catch
          {
            _customerCountry = string.Empty;
          }
        }
        return _customerCountry;
      }
    }

    private string _totalPriceUsdFormatted;
    public string TotalPriceUsdFormatted
    {
      get
      {
        if (_totalPriceUsdFormatted == null)
        {
          var nfi = new NumberFormatInfo { CurrencyDecimalDigits = 2 };
          _totalPriceUsdFormatted = TotalPriceUsd.ToString(nfi);
        }
        return _totalPriceUsdFormatted;
      }
    }

    private string _taxTotalUsdFormatted;
    public string TaxTotalUsdFormatted
    {
      get
      {
        if (_taxTotalUsdFormatted == null)
        {
          var currencyFormat = new NumberFormatInfo { CurrencyDecimalDigits = 2 };
          _taxTotalUsdFormatted = TaxTotalUsd.ToString(currencyFormat);
        }
        return _taxTotalUsdFormatted;
      }
    }

    private string _shippingTotalUsdFormatted;
    public string ShippingTotalUsdFormatted
    {
      get
      {
        if (_shippingTotalUsdFormatted == null)
        {
          var currencyFormat = new NumberFormatInfo { CurrencyDecimalDigits = 2 };
          _shippingTotalUsdFormatted = ShippingTotalUsd.ToString(currencyFormat);
        }
        return _shippingTotalUsdFormatted;
      }
    }
  }
}
