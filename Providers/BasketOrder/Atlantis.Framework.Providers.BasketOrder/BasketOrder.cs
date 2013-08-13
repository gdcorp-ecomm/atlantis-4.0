using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Providers.BasketOrder.Interface;

namespace Atlantis.Framework.Providers.BasketOrder
{
  public class BasketOrder : IBasketOrder
  {
    protected const double DEFAULT_TAX_TOTAL = 0.0;
    protected const double DEFAULT_TOTAL_PRICE = 0.0;
    protected const double DEFAULT_SHIPPING_TOTAL = 0.0;
    protected const double DEFAULT_CONVERSION_RATE_TO_USD = 1.0;
    protected const string USD_CURRENCY_CODE = "USD";
    protected const string DEFAULT_CURRENCY = USD_CURRENCY_CODE;
    protected static string DEFAULT_ORDER_ID
    {
      get { return string.Empty; }
    }

    protected const string ORDER_DETAIL_ELEMENT_NAME = "ORDERDETAIL";
    protected const string ORDER_ITEM_ELEMENT_NAME = "ITEM";
    protected const string ORDER_ITEM_LIST_ELEMENT_NAME = "ITEMS";
    protected const string ORDER_ELEMENT_NAME = "ORDER";
    protected const string EMPTY_ORDER_ELEMENT = "<ORDER/>";

    private static IBasketOrder _emptyBasketOrder;
    internal static IBasketOrder EmptyBasketOrder
    {
      get
      {
        if (_emptyBasketOrder == null)
        {
          var emptyOrderXml = XDocument.Parse(EMPTY_ORDER_ELEMENT);
          _emptyBasketOrder = new BasketOrder(emptyOrderXml);
        }
        return _emptyBasketOrder;
      }
    }

    internal BasketOrder(XDocument orderXml)
    {
      OrderXml = orderXml ?? XDocument.Parse(EMPTY_ORDER_ELEMENT);
    }

    internal BasketOrder(string orderXml)
    {
      try
      {
        OrderXml = XDocument.Parse(orderXml);
      }
      catch
      {
        OrderXml = XDocument.Parse(EMPTY_ORDER_ELEMENT);
      }
    }

    private void InitializeFromOrderXml()
    {
      var itemElements = OrderItemsElement.Elements(ORDER_ITEM_ELEMENT_NAME);
      _orderItemsDictionary = new Dictionary<string, IBasketOrderItem>(4);
      foreach (var itemElement in itemElements)
      {
        var orderItem = new BasketOrderItem(itemElement);
        if (OrderItemsDictionary.Keys.Contains(orderItem.SKU))
        {
          OrderItemsDictionary[orderItem.SKU].AddItem(orderItem.Quantity, orderItem.UnitPrice);
        }
        else
        {
          OrderItemsDictionary[orderItem.SKU] = orderItem;
        }
      }
    }

    private XElement _orderDetailElement;
    protected XElement OrderDetailElement
    {
      get 
      {
        if (_orderDetailElement == null)
        {
          _orderDetailElement = OrderXmlRootElement.Element(ORDER_DETAIL_ELEMENT_NAME) ?? new XElement(ORDER_DETAIL_ELEMENT_NAME);
        }
        return _orderDetailElement;
      }
    }

    private XElement _orderItemsElement;
    protected XElement OrderItemsElement
    {
      get
      {
        if (_orderItemsElement == null)
        {
          _orderItemsElement = OrderXmlRootElement.Element(ORDER_ITEM_LIST_ELEMENT_NAME) ?? new XElement(ORDER_ITEM_LIST_ELEMENT_NAME);
        }
        return _orderItemsElement;
      }
    }

    private XDocument _orderXml;
    protected XDocument OrderXml
    {
      get { return _orderXml ?? (_orderXml = XDocument.Parse(EMPTY_ORDER_ELEMENT)); }
      set { _orderXml = value; }
    }

    private XElement _orderXmlRootElement;
    protected XElement OrderXmlRootElement
    {
      get
      {
        if (_orderXmlRootElement == null)
        {
          _orderXmlRootElement = OrderXml.Element(ORDER_ELEMENT_NAME) ?? XElement.Parse(EMPTY_ORDER_ELEMENT);
        }
        return _orderXmlRootElement;
      }
    }

    private string _orderId;
    public string OrderId
    {
      get
      {
        if (_orderId == null)
        {
          var orderIdAttribute = OrderDetailElement.Attribute(BasketOrderDetailAttributes.OrderId);
          _orderId = orderIdAttribute == null ? DEFAULT_ORDER_ID : orderIdAttribute.Value;
        }
        return _orderId;
      }
    }

    private double? _conversionRateToUsd;
    protected double ConversionRateToUsd
    {
      get
      {
        if (_conversionRateToUsd == null)
        {
          var conversionRateAttribute = OrderDetailElement.Attribute(BasketOrderDetailAttributes.ConversionRateToUsd);
          if (conversionRateAttribute == null)
          {
            _conversionRateToUsd = DEFAULT_CONVERSION_RATE_TO_USD;
          }
          else
          {
            double conversionRateToUsd;
            _conversionRateToUsd = double.TryParse(conversionRateAttribute.Value, out conversionRateToUsd) ? conversionRateToUsd : DEFAULT_CONVERSION_RATE_TO_USD;
          }
        }
        return _conversionRateToUsd.Value;
      }
    }

    private string _transactionalCurrency;
    public string TransactionalCurrency
    {
      get
      {
        if (_transactionalCurrency == null)
        {
          var transactionalCurrencyAttribute = OrderDetailElement.Attribute(BasketOrderDetailAttributes.TransactionCurrency);
          _transactionalCurrency = transactionalCurrencyAttribute == null ? DEFAULT_CURRENCY : transactionalCurrencyAttribute.Value;
        }
        return _transactionalCurrency;
      }
    }

    protected double ConvertPriceToUsd(double price)
    {
      return ConversionRateToUsd * price;
    }

    private double? _totalPrice;
    public double TotalPrice
    {
      get
      {
        if (_totalPrice == null)
        {
          var totalTotalAttribute = OrderDetailElement.Attribute(BasketOrderDetailAttributes.TotalTotal);
          if (totalTotalAttribute == null)
          {
            _totalPrice = DEFAULT_TOTAL_PRICE;
          }
          else
          {
            double totalPrice;
            _totalPrice = double.TryParse(totalTotalAttribute.Value, out totalPrice) ? totalPrice * 0.01 : DEFAULT_TOTAL_PRICE;
          }
        }
        return _totalPrice.Value;
      }
    }

    private double? _totalPriceUsd;
    public double TotalPriceUsd
    {
      get
      {
        if (_totalPriceUsd == null)
        {
          _totalPriceUsd = TransactionalCurrency.Equals(USD_CURRENCY_CODE, StringComparison.OrdinalIgnoreCase) ? TotalPrice : ConvertPriceToUsd(TotalPrice);
        }
        return _totalPriceUsd.Value;
      }
    }

    private double? _taxTotal;
    public double TaxTotal
    {
      get
      {
        if (_taxTotal == null)
        {
          var taxTotalAttribute = OrderDetailElement.Attribute(BasketOrderDetailAttributes.TaxTotal);
          if (taxTotalAttribute == null)
          {
            _taxTotal = DEFAULT_TAX_TOTAL;
          }
          else
          {
            double taxTotal;
            _taxTotal = double.TryParse(taxTotalAttribute.Value, out taxTotal) ? taxTotal * 0.01 : DEFAULT_TAX_TOTAL;
          }
        }
        return _taxTotal.Value;
      }
    }

    private double? _taxTotalUsd;
    public double TaxTotalUsd
    {
      get
      {
        if (_taxTotalUsd == null)
        {
          _taxTotalUsd = TransactionalCurrency.Equals(USD_CURRENCY_CODE, StringComparison.OrdinalIgnoreCase) ? TaxTotal : ConvertPriceToUsd(TaxTotal);
        }
        return _taxTotalUsd.Value;
      }
    }

    private double? _shippingTotal;
    public double ShippingTotal
    {
      get
      {

        if (_shippingTotal == null)
        {
          var shippingTotalAttribute = OrderDetailElement.Attribute(BasketOrderDetailAttributes.ShippingTotal);
          if (shippingTotalAttribute == null)
          {
            _shippingTotal = DEFAULT_SHIPPING_TOTAL;
          }
          else
          {
            double shippingTotal;
            _shippingTotal = double.TryParse(shippingTotalAttribute.Value, out shippingTotal) ? shippingTotal * 0.01 : DEFAULT_SHIPPING_TOTAL;
          }
        }
        return _shippingTotal.Value;
      }
    }

    private double? _shippingTotalUsd;
    public double ShippingTotalUsd
    {
      get
      {
        if (_shippingTotalUsd == null)
        {
          _shippingTotalUsd = TransactionalCurrency.Equals(USD_CURRENCY_CODE, StringComparison.OrdinalIgnoreCase) ? ShippingTotal : ConvertPriceToUsd(ShippingTotal);
        }
        return _shippingTotalUsd.Value;
      }
    }

    public IEnumerable<IBasketOrderItem> OrderItems
    {
      get { return OrderItemsDictionary.Values; }
    }

    private IDictionary<string, IBasketOrderItem> _orderItemsDictionary;
    private IDictionary<string, IBasketOrderItem> OrderItemsDictionary
    {
      get
      {
        if (_orderItemsDictionary == null)
        {
          _orderItemsDictionary = new Dictionary<string, IBasketOrderItem>(4);
          try
          {
            InitializeFromOrderXml();
          }
          catch
          {
            _orderItemsDictionary = new Dictionary<string, IBasketOrderItem>(0);
          }
        }
        return _orderItemsDictionary;
      }
    }

    public string ToXml()
    {
      return OrderXml.ToString(SaveOptions.DisableFormatting);
    }
  }
}