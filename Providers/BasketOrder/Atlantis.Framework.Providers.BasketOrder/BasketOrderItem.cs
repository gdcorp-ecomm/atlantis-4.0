using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Providers.BasketOrder.Interface;

namespace Atlantis.Framework.Providers.BasketOrder
{
  internal class BasketOrderItem : IBasketOrderItem
  {
    private const string EmptyItemXml = "<ITEM></ITEM>";

    internal BasketOrderItem(XElement itemElement)
    {
      ItemElement = itemElement ?? XElement.Parse(EmptyItemXml);

      InitializeFirstItem();
    }

    public BasketOrderItem(string itemElementXml)
    {
      if (itemElementXml == null)
      {
        itemElementXml = EmptyItemXml;
      }
      ItemElement = XElement.Parse(itemElementXml);

      InitializeFirstItem();
    }


    private void InitializeFirstItem()
    {
      var priceAttribute = ((string) ItemElement.Attribute("_oadjust_adjustedprice")) ?? "0.0";
      double price;
      if (double.TryParse(priceAttribute, out price))
      {
        price = price*0.01;
      }
      else
      {
        price = 0.0;
      }

      var quantityAttribute = ((string) ItemElement.Attribute("quantity")) ?? "0";
      int quantity;
      if (!int.TryParse(quantityAttribute, out quantity))
      {
        quantity = 0;
      }

      AddItem(quantity, price);
    }

    private XElement ItemElement { get; set; }

    private string _sku;
    public string SKU {
      get
      {
        if (_sku == null)
        {
          var skuAttribute = (string) ItemElement.Attribute(BasketOrderItemAttributes.UnifiedProductId);
          _sku = skuAttribute ?? "-1";
        }
        return _sku;
      }
    }

    private string _productName;
    public string ProductName {
      get
      {
        if (_productName == null)
        {
          var productNameAttribute = (string) ItemElement.Attribute(BasketOrderItemAttributes.ProductName);
          _productName = productNameAttribute ?? string.Empty;
        }
        return _productName;
      }
    }

    private string _productCategory;
    public string ProductCategory {
      get
      {
        if (_productCategory == null)
        {
          var productCategoryAttribute = (string) ItemElement.Attribute(BasketOrderItemAttributes.ProductTypeId);
          _productCategory = productCategoryAttribute ?? string.Empty;
        }
        return _productCategory;
      }
    }

    private double? _unitPrice;
    public double UnitPrice
    {
      get
      {
        if (_unitPrice == null)
        {
          if (Quantity == 0)
          {
            _unitPrice = 0.0;
          }
          else
          {
            _unitPrice = TotalPrice/Quantity;
          }
        }

        return _unitPrice.Value;
      }
    }

    private int? _quantity;
    public int Quantity
    {
      get
      {
        if (_quantity == null)
        {
          _quantity = 0;
        }
        return _quantity.Value;
      }
      private set { _quantity = value; }
    }

    private double? _totalPrice;
    private double TotalPrice
    {
      get
      {
        if (_totalPrice == null)
        {
          _totalPrice = 0.0;
        }
        return _totalPrice.Value;
      }
      set { _totalPrice = value; }
    }

    private IList<PriceQuantityPair> _priceQuantityPairs;
    private IList<PriceQuantityPair> PriceQuantityPairs
    {
      get { return _priceQuantityPairs ?? (_priceQuantityPairs = new List<PriceQuantityPair>(4)); }
    }

    public void AddItem(int quantity, double unitPriceUsd)
    {
      PriceQuantityPairs.Add(new PriceQuantityPair(quantity, unitPriceUsd));

      Quantity += quantity;
      TotalPrice += unitPriceUsd;
    }
  }
}