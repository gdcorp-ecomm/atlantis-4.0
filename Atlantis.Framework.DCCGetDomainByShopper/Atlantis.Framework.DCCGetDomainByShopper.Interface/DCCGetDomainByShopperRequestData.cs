using Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging;
using Atlantis.Framework.Interface;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.DCCGetDomainByShopper.Interface
{
  public class DCCGetDomainByShopperRequestData : RequestData
  {

    public enum DomainByProxyFilter
    {
      NoFilter = 0, //default 
      DbpOnly,
      NoDbpOnly,
    }

    public string ShopperId { get; private set; }

    public string DccDomainUser { get; private set; }

    public IDomainPaging Paging { get; private set; }

    public bool UseMaxdateAsDefaultForExpirationDate { get; set; }

    public DomainByProxyFilter DbpFilter { get; set; }

    public DCCGetDomainByShopperRequestData(string shopperId, IDomainPaging domainPaging, string dccDomainUser)
    {
      ShopperId = shopperId;
      Paging = domainPaging;
      DccDomainUser = dccDomainUser;
      RequestTimeout = TimeSpan.FromSeconds(20);
      UseMaxdateAsDefaultForExpirationDate = false;
    }

    private static XmlNode AddNode(XmlNode parentNode, string sChildNodeName)
    {
      XmlNode childNode = null;
      if (parentNode != null && parentNode.OwnerDocument != null)
      {
        childNode = parentNode.OwnerDocument.CreateElement(sChildNodeName);
        parentNode.AppendChild(childNode);
      }
      return childNode;
    }

    private static void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
    {
      if (node != null && node.Attributes != null && node.OwnerDocument != null)
      {
        XmlAttribute attribute = node.OwnerDocument.CreateAttribute(sAttributeName);
        node.Attributes.Append(attribute);
        attribute.Value = sAttributeValue;
      }
    }

    public override string ToXML()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<getdccdomainlist/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      XmlElement oUserName = (XmlElement) AddNode(oRoot, "username");
      oUserName.InnerText = DccDomainUser;

      XmlElement oSort = (XmlElement) AddNode(oRoot, "sort");
      AddAttribute(oSort, "column", Paging.SortOrderField);
      AddAttribute(oSort, "direction", Paging.SortOrder == SortOrderType.Ascending ? "ASC" : "DESC");

      XmlElement oShopper = (XmlElement) AddNode(oRoot, "shopper");
      AddAttribute(oShopper, "shopperid", ShopperId);

      if (!string.IsNullOrEmpty(Paging.SearchTerm))
      {
        AddAttribute(oShopper, "search", Paging.SearchTerm.ToUpper());
      }

      AddAttribute(oShopper, "quantity", Paging.RowsPerPage.ToString(CultureInfo.InvariantCulture));

      if (Paging.ExpirationDays.HasValue)
      {
        AddAttribute(oShopper, "expirationDays", Paging.ExpirationDays.Value.ToString(CultureInfo.InvariantCulture));
      }

      XmlElement filterElement = null;

      if (Paging.SummaryOnly)
      {
        filterElement = (XmlElement) AddNode(oRoot, "filter");
        AddAttribute(filterElement, "showdomains", "0");
      }

      if (Paging.StatusType.HasValue)
      {
        if (filterElement == null)
        {
          filterElement = (XmlElement) AddNode(oRoot, "filter");
        }
        AddAttribute(filterElement, "statustype", Paging.StatusType.Value.ToString(CultureInfo.InvariantCulture));
      }

      if (Paging.AutoRenewFilter.HasValue)
      {
        if (filterElement == null)
        {
          filterElement = (XmlElement)AddNode(oRoot, "filter");
        }
        AddAttribute(filterElement, "autorenew", Paging.AutoRenewFilter.Value.ToString(CultureInfo.InvariantCulture));
      }

      if (DbpFilter != DomainByProxyFilter.NoFilter)
      {
        if (filterElement == null)
        {
          filterElement = (XmlElement) AddNode(oRoot, "filter");
        }
        switch (DbpFilter)
        {
          case DomainByProxyFilter.DbpOnly:
            AddAttribute(filterElement, "isproxied", "1");
            break;
          case DomainByProxyFilter.NoDbpOnly:
            AddAttribute(filterElement, "isproxied", "0");
            break;
        }
      }

      if (!string.IsNullOrEmpty(Paging.BoundaryFieldValue))
      {
        XmlElement oPaging = (XmlElement) AddNode(oRoot, "paging");
        AddAttribute(oPaging, Paging.BoundaryField, Paging.BoundaryFieldValue);
        if (!string.IsNullOrEmpty(Paging.BoundaryUniquifierField) && !string.IsNullOrEmpty(Paging.BoundaryUniquifierFieldValue))
        {
          AddAttribute(oPaging, Paging.BoundaryUniquifierField, Paging.BoundaryUniquifierFieldValue);
        }
        string pagingDirectionString = (Paging.NavigatingForward) ? "forward" : "backward";
        AddAttribute(oPaging, "direction", pagingDirectionString);
        if (Paging.IncludeBoundary)
        {
          AddAttribute(oPaging, "includeSortBoundary", "1");
        }
      }

      if (Paging.TldIdList.Count > 0)
      {
        XmlElement tldsElement = (XmlElement) AddNode(oRoot, "tlds");
        foreach (int tldid in Paging.TldIdList)
        {
          XmlElement tldElement = (XmlElement) AddNode(tldsElement, "tld");
          AddAttribute(tldElement, "id", tldid.ToString(CultureInfo.InvariantCulture));
        }
      }

      return requestDoc.InnerXml;
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      var requestXml = ToXML();
      var data = Encoding.UTF8.GetBytes(requestXml);
      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}