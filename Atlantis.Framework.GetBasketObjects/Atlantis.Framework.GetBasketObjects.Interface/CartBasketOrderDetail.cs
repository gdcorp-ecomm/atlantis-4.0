﻿using System;
using System.Web;
using System.Xml;
using System.Collections.Generic;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  public class CartBasketOrderDetail : CartBaseDictionary
  {

    private string _domainContactXml;
    private Dictionary<string, List<CartDomainContact>> _domainContactGroups = new Dictionary<string, List<CartDomainContact>>();
    public Dictionary<string, List<CartDomainContact>> DomainContactGroups { get { return _domainContactGroups; } }

    public string DomainContactXml
    {
      get { return _domainContactXml; }
      set
      {
        _domainContactXml = value;
        LoadDomainContacts(_domainContactXml);
      }
    }

    private void LoadDomainContacts(string contactXML)
    {
      XmlDocument customXML = new XmlDocument();
      customXML.LoadXml(contactXML);

      XmlNodeList contactGroups = customXML.SelectNodes("//contacts");
      foreach (XmlNode currentGroup in contactGroups)
      {
        string crc32 = currentGroup.Attributes["crc32"].Value;
        List<CartDomainContact> groupContacts = new List<CartDomainContact>();
        foreach (XmlNode currentContact in currentGroup.ChildNodes)
        {
          CartDomainContact domContact= new CartDomainContact(currentContact);
          groupContacts.Add(domContact);
        }
        _domainContactGroups[crc32] = groupContacts;
      }
    }

    public int TotalShipping
    {
      get
      {
        int thirdPartyShipping = GetIntProperty(CartBasketOrderDetailProperty.ThirdPartyAmnt, 0);
        int handling = GetIntProperty(CartBasketOrderDetailProperty.HandlingTotal, 0);
        int shipTotal = GetIntProperty(CartBasketOrderDetailProperty.ShippingTotal, 0);
        int totalAmount = shipTotal + handling + thirdPartyShipping + MarketPlaceShippingTotal;
        return totalAmount;
      }
    }

    public int Taxes
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.TaxTotal, 0); }
    }

    public int ShippingPrice
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.ShippingPrice, 0); }
    }

    public int TotalTotal
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.TotalTotal, 0); }
    }

    public int SubTotal
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.SubTotal, 0); }
    }

    public int BaseSubTotal
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.BaseSubTotal, 0); }
    }

    public string OrderId
    {
      get { return GetStringProperty(CartBasketOrderDetailProperty.OrderId, string.Empty); }
    }

    public string SourceCode
    {
      get { return GetStringProperty(CartBasketOrderDetailProperty.SourceCode, string.Empty); }
    }

    public int MarketPlaceShippingTotal
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.MarketplaceShippingTotal, 0); }
    }

    public int PrepaidDiscountTotal
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.PrepaidDiscountTotal, 0); }
    }

    public string MarketPlaceShopList
    {
      get { return GetStringProperty(CartBasketOrderDetailProperty.MarketplaceShopList, string.Empty); }
    }

    public string OrderDiscountDescription
    {
      get { return GetStringProperty(CartBasketOrderDetailProperty.OrderDiscountDescription, string.Empty); }
    }

    public int OrderDiscountAmount
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.OrderDiscountAmount, 0); }
    }

    public int FullOrderDiscountAmount
    {
      get
      {
        int subTotal = GetIntProperty(CartBasketOrderDetailProperty.SubTotal, 0);
        int baseSubTotal = GetIntProperty(CartBasketOrderDetailProperty.BaseSubTotal, 0);
        int discount = baseSubTotal - subTotal;
        discount = Math.Max(discount, 0);
        return discount;
      }
    }

    public int BasketPaymentTypeFlag
    {
      get { return GetIntProperty(CartBasketOrderDetailProperty.BasketPaymentTypeFlag, 4095); }
    }

    public string BasketType
    {
      get
      {
        string baskType = GetStringProperty(CartBasketOrderDetailProperty.BasketType, "gdshop");
        return baskType;
      }
      set
      {
        this[CartBasketOrderDetailProperty.BasketType] = value;
      }
    }

  }
}
