using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  //TODO: Add SubTotal to Cart - SubTotal: Total of Unit Price for all products in cart?

  public class CartBasketOrder
  {
    const int ExpiredAuctionPFID = 738;

    CartXMLSearchProvider _searchProvider;

    public CartBasketOrder(string basketXml)
    {
      Detail = new CartBasketOrderDetail();
      Items = new List<CartBasketOrderItem>();
      _searchProvider = new CartXMLSearchProvider(basketXml);
      if (!string.IsNullOrEmpty(basketXml))
      {
        ProcessBasketXml(basketXml);
      }
    }

    public bool IsEntryInCart(string xpath)
    {
      XmlNode regElement = _searchProvider.SearchElementByXPath(xpath);
      if (regElement != null)
        return true;
      return false;
    }

    public string CommissionJunctionXML
    {
      get
      {
        string junctionXML = string.Empty;
        try
        {
          const string xpath = "/ORDER/ORDERDETAIL/CUSTOMXML/commissionJunction";
          XmlNode regCJunct = _searchProvider.SearchElementByXPath(xpath);
          if (regCJunct != null)
            junctionXML = regCJunct.OuterXml;
        }
        catch
        {

        }
        return junctionXML;
      }
    }

    public XmlNodeList GetNodesByXPath(string xPath)
    {
      XmlNodeList regElements = _searchProvider.SearchElementsByXPath(xPath);
      return regElements;
    }

    private int GetRowIDByXPath(string xPath, int depth)
    {
      try
      {
        XmlNode regElement = _searchProvider.SearchElementByXPath(xPath);
        if (regElement == null)
          return -1;
        for (int x = 1; x < depth; x++)
        {
          regElement = regElement.ParentNode;
        }
        int rowID;
        string tempAtt = regElement.Attributes["_row_id"].Value;
        int.TryParse(tempAtt, out rowID);
        return rowID;
      }
      catch
      {
        return -1;
      }
    }

    public void GetDomainParts(string Domain, out string sld, out string tld)
    {
      if (string.IsNullOrEmpty(Domain))
      {
        sld = string.Empty;
        tld = string.Empty;
        return;
      }
      sld = string.Empty;
      tld = string.Empty;
      string currentDomain = Domain;
      int dotPoint = currentDomain.IndexOf(".");
      if (dotPoint > -1)
      {
        sld = currentDomain.Substring(0, dotPoint);
        tld = currentDomain.Substring(dotPoint + 1);
      }
    }


    #region Get Domain Products by tld and sld

    public int GetExpiredAuctionDomainRowID(string domaintld, string domainsld)
    {
      int iRow = GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBackorder[@sld = '", domainsld, "' and @tld = '", domaintld, "']"), 3);
      if (iRow > -1)
      {
        XmlNode regElement = _searchProvider.SearchElementByXPath(String.Concat("/ORDER/ITEMS/ITEM[@_product_unifiedproductid = '", ExpiredAuctionPFID, "' and @_row_id = '", iRow, "']"));
        if (regElement == null)
        {
          iRow = -1;
        }
      }
      return iRow;
    }

    public int GetAuctionDomainRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/ItemWinningBids/domain[@sld = '", domainsld, "' and @tld = '", domaintld, "']"), 4);
    }

    public int GetBulkDomainRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRegistration/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }


    public int GetBulkDomainRenewalRowID(string domainTld, string domainSld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRenewal/domain[@sld = '", domainSld, "' and @tld='", domainTld, "']"), 4);
    }

    public int GetBulkDomainRedemptionRowID(string domainTld, string domainSld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRedemption/domain[@sld = '", domainSld, "' and @tld='", domainTld, "']"), 4);
    }

    public int GetBulkDomainExpirationProtectionRenewalRowID(string domainTld, string domainSld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/expirationProtectionRenewal/domain[@sld = '", domainSld, "' and @tld='", domainTld, "']"), 4);
    }

    public int GetPrivacyRegistrationRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainByProxyBulk/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }

    public int GetPrivacyRenewalRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainByProxyBulkRenewal/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }

    public int GetCertifiedDomainRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/certifiedDomain/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }

    public int GetCertifiedDomainRenewalRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/certifiedDomainRenewal/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }

    public int GetBusinessRegistrationRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/Proxima/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }

    public int GetBusinessRenewalRowID(string domaintld, string domainsld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/ProximaRenewal/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 4);
    }

    public int GetProtectedRegistrationRowID(string domainsld, string domaintld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/BUNDLE/BUNDLEITEM/domainByProxyBulk/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 6);
    }

    public int GetProtectedRenewalRowID(string domainsld, string domaintld)
    {
      return GetRowIDByXPath(String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/BUNDLE/BUNDLEITEM/domainByProxyBulkRenewal/domain[@sld = '", domainsld, "' and @tld='", domaintld, "']"), 6);
    }

    #endregion

    public bool IsItemInCartByProductId(string productID)
    {
      return IsEntryInCart(String.Concat("/ORDER/ITEMS/ITEM[@_product_unifiedproductid='", productID, "']"));
    }

    public bool IsItemInCartByResourceId(string resourceID)
    {
      return IsEntryInCart(String.Concat("//ITEM[@resource_id='", resourceID, "']"));
    }

    #region ProductTypesInCart

    public bool IsTransferFound()
    {
      const string xPath = "/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkTransfer";
      return IsEntryInCart(xPath);
    }

    public bool IsRefund()
    {
      //basket.asp line 2646
      string orderID = Detail.OrderId;
      if (orderID.Length > 0 && orderID.Substring(orderID.Length - 1, 1).ToUpper() == "R")
      {
        return true;
      }
      return false;
    }

    public bool IsDomainInCart(string domain)
    {
      if (string.IsNullOrEmpty(domain))
        return false;
      string sld;
      string tld;
      GetDomainParts(domain, out sld, out tld);
      return IsDomainInCart(sld, tld);
    }

    public bool IsDomainInCart(string sld, string tld)
    {
      string xPath = String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRegistration/domain[@sld = '", sld, "' and @tld='", tld, "']");
      return IsEntryInCart(xPath);
    }

    public bool IsDomainRenewalInCart(string domain)
    {
      if (string.IsNullOrEmpty(domain))
        return false;
      string sld;
      string tld;
      GetDomainParts(domain, out sld, out tld);
      return IsDomainRenewalInCart(sld, tld);
    }

    public bool IsDomainRenewalInCart(string sld, string tld)
    {
      string xPath = String.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRenewal/domain[@sld = '", sld, "' and @tld='", tld, "']");
      return IsEntryInCart(xPath);
    }

    public bool IsRegistrationFound()
    {
      const string xPath = "/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRegistration";
      return IsEntryInCart(xPath);
    }

    #region Get Domain Product by domain name

    public int GetPrivacyRenewalRowID(string domain)
    {
      string domaintld;
      string domainsld;
      GetDomainParts(domain, out domainsld, out domaintld);
      return GetPrivacyRenewalRowID(domaintld, domainsld);
    }

    public int GetPrivacyRegistrationRowID(string domain)
    {
      string domaintld;
      string domainsld;
      GetDomainParts(domain, out domainsld, out domaintld);
      return GetPrivacyRegistrationRowID(domaintld, domainsld);

    }

    public int GetProtectedRegistrationRowID(string domain)
    {
      string domaintld;
      string domainsld;
      GetDomainParts(domain, out domainsld, out domaintld);
      return GetProtectedRegistrationRowID(domainsld, domaintld);
    }

    public int GetBusinessRegistrationRowID(string domain)
    {
      string domaintld;
      string domainsld;
      GetDomainParts(domain, out domainsld, out domaintld);
      return GetBusinessRegistrationRowID(domaintld, domainsld);
    }

    #endregion

    public int GetProtectedRegistrationRowID(CartBasketOrderItem testItem)
    {
      foreach (CartDomainEntry currentDomain in testItem.DomainInfo.DomainList)
      {
        int rowID = GetProtectedRegistrationRowID(currentDomain.SecondLevelDomain, currentDomain.TopLevelDomain);
        if (rowID != -1)
          return rowID;
      }
      return -1;
    }


    #region Get First Occurence of Domain Product

    public int GetPrivacyRegistrationRowID()
    {
      return GetRowIDByXPath("/ORDER/ITEMS/ITEM/CUSTOMXML/domainByProxyBulk", 3);
    }

    public int GetBusinessRegistrationRowID()
    {
      return GetRowIDByXPath("/ORDER/ITEMS/ITEM/CUSTOMXML/Proxima", 3);
    }

    #endregion

    #endregion

    public string GetItemXML(string itemId)
    {
      string xPath = String.Concat("/ORDER/ITEMS/ITEM[@item_id ='", itemId, "']");
      XmlNode regElement = _searchProvider.SearchElementByXPath(xPath) as XmlNode;
      if (regElement != null) return regElement.OuterXml;
      return String.Empty;
    }

    public CartBasketOrderItem GetByItemID(int itemID)
    {
      CartBasketOrderItem thisItem;
      if (LinearItems.ContainsKey(itemID))
      {
        thisItem = LinearItems[itemID];
      }
      else
      {
        thisItem = new CartBasketOrderItem(_searchProvider);
      }
      return thisItem;
    }

    public CartBasketOrderItem GetByRowID(int rowID)
    {
      CartBasketOrderItem thisItem;
      XmlNodeList itemNode = GetNodesByXPath(String.Concat("/ORDER/ITEMS/ITEM[@", CartBasketOrderItemProperty.RowId, "='", rowID, "']"));
      int itemID = -1;
      if (itemNode.Count > 0)
      {
        if (itemNode[0].Attributes[CartBasketOrderItemProperty.ItemId] != null)
        {
          string tempID = itemNode[0].Attributes[CartBasketOrderItemProperty.ItemId].Value;
          int.TryParse(tempID, out itemID);
        }
      }
      if (LinearItems.ContainsKey(itemID))
      {
        thisItem = LinearItems[itemID];
      }
      else
      {
        thisItem = new CartBasketOrderItem(_searchProvider);
      }
      return thisItem;
    }

    private int _totalBasketListPrice = 0;
    public int TotalBasketListPrice
    {
      get
      {
        return _totalBasketListPrice;
      }
    }

    public List<CartBasketOrderItem> Items { get; set; }

    public Dictionary<int, CartBasketOrderItem> LinearItems
    {
      get { return _parsedItems; }
    }

    public CartBasketOrderDetail Detail { get; set; }

    public int TotalItemsInCart
    {
      get
      {
        return _parsedItems.Count;
      }
    }

    private int _totalProductSavings;

    public int TotalProductSavings
    {
      get { return _totalProductSavings; }
    }

    public void AddProductSavings(int savings)
    {
      _totalProductSavings = _totalProductSavings + savings;
    }

    #region Constructors

    HashSet<int> _parentItemIds;
    HashSet<int> _uniqueItemIds = new HashSet<int>();

    Dictionary<string, CartBasketOrderItem> _parentItems = new Dictionary<string, CartBasketOrderItem>();
    Dictionary<string, CartBasketOrderItem> _bundleParents = new Dictionary<string, CartBasketOrderItem>();
    Dictionary<int, CartBasketOrderItem> _parsedItems = new Dictionary<int, CartBasketOrderItem>();
    CartBasketOrderItem _currentItem = null;

    public HashSet<int> UniqueIDs
    {
      get
      {
        return _uniqueItemIds;
      }
    }

    public void ProcessBasketXml(string basketXml)
    {
      //System.Diagnostics.Debug.WriteLine(basketXml);
      _parentItemIds = new HashSet<int>();
      _parentItems = new Dictionary<string, CartBasketOrderItem>();
      _bundleParents = new Dictionary<string, CartBasketOrderItem>();
      _parsedItems = new Dictionary<int, CartBasketOrderItem>();

      XDocument basketDoc = XDocument.Parse(basketXml);
      XmlReader reader = basketDoc.CreateReader();

      try
      {
        while (reader.Read())
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            ParseElement(reader);
          }
        }
      }
      finally
      {
        reader.Close();
      }

      // Finally add to the collection parent level items and add the child items to parents.
      // TODO: can we do this during the first loop?
      foreach (KeyValuePair<int, CartBasketOrderItem> item in _parsedItems)
      {
        AddProductSavings(item.Value.AdjustedDiscount);
        DomainProcessing(item.Value);

        CartBasketOrderItem parentItem;

        if ((item.Value.ParentBundleId.Length > 0) &&
          (_bundleParents.TryGetValue(item.Value.ParentBundleId, out parentItem)))
        {
          item.Value.ParentItemRowID = parentItem.RowId;
          item.Value.ParentItemItemID = parentItem.ItemId;
          parentItem.ChildItems.Add(item.Value);
        }
        else if ((item.Value.GroupId.Length > 0) &&
          (!_parentItemIds.Contains(item.Value.ItemId)) &&
          (_parentItems.TryGetValue(item.Value.GroupId, out parentItem)))
        {
          item.Value.ParentItemRowID = parentItem.RowId;
          item.Value.ParentItemItemID = parentItem.ItemId;
          parentItem.ChildItems.Add(item.Value);
        }
        else
        {
          if (!item.Value.IsInItems)
          {
            Items.Add(item.Value);
            item.Value.IsInItems = true;
          }
        }
        ProcessDependantItems(item.Value);
        if (!string.IsNullOrEmpty(item.Value.PeriodDuration))
        {
          double itemDuration = 1;
          double.TryParse(item.Value.PeriodDuration, out itemDuration);
          double itemListPrice = item.Value.ListPrice * itemDuration;
          _totalBasketListPrice = _totalBasketListPrice + (int)Math.Truncate(itemListPrice * item.Value.Quantity);
        }
        else
        {
          _totalBasketListPrice = _totalBasketListPrice + item.Value.ListPrice * item.Value.Quantity;
        }
      }

    }

    private void ProcessDependantItems(CartBasketOrderItem currentItem)
    {
      //Add Dependant Items immediatly following current item
      foreach (CartDomainEntry currentDomain in currentItem.DomainInfo.DomainList)
      {
        int DBPItemRowID = GetPrivacyRegistrationRowID(currentDomain.TopLevelDomain, currentDomain.SecondLevelDomain);
        if (DBPItemRowID != -1)
        {
          //Find Item and add it
          CartBasketOrderItem dbpItem = GetByRowID(DBPItemRowID);
          if (dbpItem.ParentItemItemID == -1 && !dbpItem.IsInItems && dbpItem.ParentBundleId.Length == 0 && dbpItem.GroupId.Length == 0)
          {
            Items.Add(dbpItem);
            dbpItem.IsInItems = true;
            break;
          }
        }
      }
    }

    #endregion

    private CartBasketOrderItem ParseOrderItem(XmlReader reader)
    {
      CartBasketOrderItem item = new CartBasketOrderItem(_searchProvider);
      while (reader.MoveToNextAttribute())
      {
        item[reader.Name] = reader.Value;
      }
      return item;
    }

    private void ParseOrderDetail(XmlReader reader)
    {
      while (reader.MoveToNextAttribute())
      {
        Detail[reader.Name] = reader.Value;
      }
    }

    private void ParseElement(XmlReader reader)
    {
      if (reader.Name == CartBasketElementTypes.OrderDetail)
      {
        ParseOrderDetail(reader);
      }
      else if (reader.Name == CartBasketElementTypes.Item)
      {
        _currentItem = ParseOrderItem(reader);
        _parsedItems.Add(_currentItem.ItemId, _currentItem);
        _uniqueItemIds.Add(_currentItem.ProductId);
        if (IsFirstGroupParent(_currentItem))
        {
          _parentItems[_currentItem.ParentGroupId] = _currentItem;
          _parentItemIds.Add(_currentItem.ItemId);
        }
        else if (_currentItem.IsBundleParent)
        {
          _bundleParents[_currentItem.BundleId] = _currentItem;
        }
      }
      else if (reader.Name == CartBasketElementTypes.CustomXml)
      {

        //Load Custom XML entities
        //domainByProxyBulk
        //domainBulkRegistration
        //Proxima
        //smartDomain
        if (_currentItem != null)
        {
          _currentItem.CustomXml = reader.ReadInnerXml();
          _currentItem.DomainInfo.LoadDomainInfo(_currentItem.CustomXmlDoc);
        }
      }
      else if (reader.Name == CartBasketElementTypes.DomainContactXml)
      {
        Detail.DomainContactXml = reader.ReadInnerXml();
       
      }
    }    

    public bool IsEzPrintShipping()
    {
      XmlNode regElement = _searchProvider.SearchElementByXPath("/ORDER/ITEMS/ITEM[@trans_method='ship_ezprint']") as XmlNode;
      if (regElement != null)
        return true;
      return false;
    }

    public bool IsProductShipping()
    {
      XmlElement regElement = _searchProvider.SearchElementByXPath("/ORDER/ITEMS/ITEM[@trans_method='shipbundle' or @trans_method='shipping' or @trans_method='shippingincluded' or @trans_method='ship_ezprint']") as XmlElement;
      if (regElement != null)
        return true;
      return false;
    }

    private bool IsFirstGroupParent(CartBasketOrderItem item)
    {
      bool result = false;
      if ((item.IsGroupParent) && (!_parentItems.ContainsKey(item.ParentGroupId)))
        if (!item.IsBundle)
          result = true;

      return result;
    }

    #region Domains

    private int _totalDomains;

    public int TotalDomains
    {
      get { return _totalDomains; }
    }

    private List<CartBasketOrderItem> _domainsInCart = new List<CartBasketOrderItem>();

    public List<CartBasketOrderItem> DomainsInCart
    {
      get
      {
        return _domainsInCart;
      }
    }

    private void DomainProcessing(CartBasketOrderItem orderItem)
    {
      if (orderItem.DomainInfo.IsDomain)
      {
        if (!orderItem.DomainInfo.IsProxy)
        {
          _domainsInCart.Add(orderItem);
          if (orderItem.DomainInfo.DomainType == CartDomainItemTypes.BulkDomain || orderItem.DomainInfo.DomainType == CartDomainItemTypes.DomainTransfer ||
               (orderItem.DomainInfo.DomainType == CartDomainItemTypes.DomainBackorder && orderItem.ProductId == ExpiredAuctionPFID))
          {
            _totalDomains = _totalDomains + orderItem.Quantity;
          }
        }
      }
    }

    #endregion

  }
}
