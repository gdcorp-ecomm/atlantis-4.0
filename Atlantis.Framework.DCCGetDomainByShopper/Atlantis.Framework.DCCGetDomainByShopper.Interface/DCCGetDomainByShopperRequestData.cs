using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainByShopper.Interface
{
  public class DCCGetDomainByShopperRequestData : RequestData
  {
    public string DccDomainUser { get; private set; }

    public IDomainPaging Paging { get; private set; }

    public bool UseMaxdateAsDefaultForExpirationDate { get; set; }

    public DCCGetDomainByShopperRequestData(string shopperId,
                                            string sourceUrl,
                                            string orderId,
                                            string pathway,
                                            int pageCount,
                                            IDomainPaging domainPaging,
                                            string dccDomainUser)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
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

      XmlElement oUserName = (XmlElement)AddNode(oRoot, "username");
      oUserName.InnerText = DccDomainUser;

      XmlElement oSort = (XmlElement)AddNode(oRoot, "sort");
      AddAttribute(oSort, "column", Paging.SortOrderField);
      AddAttribute(oSort, "direction", Paging.SortOrder == SortOrderType.Ascending ? "ASC" : "DESC");

      XmlElement oShopper = (XmlElement)AddNode(oRoot, "shopper");
      AddAttribute(oShopper, "shopperid", ShopperID);
      
      if(!string.IsNullOrEmpty(Paging.SearchTerm))
      {
        AddAttribute(oShopper, "search", Paging.SearchTerm.ToUpper());
      }
      
       AddAttribute(oShopper, "quantity", Paging.RowsPerPage.ToString());

      if (Paging.ExpirationDays.HasValue)
      {
        AddAttribute(oShopper, "expirationDays", Paging.ExpirationDays.Value.ToString());
      }

      XmlElement filterElement = null;

      if (Paging.SummaryOnly)
      {
        filterElement = (XmlElement)AddNode(oRoot, "filter");
        AddAttribute(filterElement, "showdomains", "0");
      }

      if (Paging.StatusType.HasValue)
      {
        if (filterElement == null)
        {
          filterElement = (XmlElement)AddNode(oRoot, "filter");
        }
        AddAttribute(filterElement, "statustype", Paging.StatusType.Value.ToString());
      }

      if (!string.IsNullOrEmpty(Paging.BoundaryFieldValue))
      {
        XmlElement oPaging = (XmlElement)AddNode(oRoot, "paging");
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
        XmlElement tldsElement = (XmlElement)AddNode(oRoot, "tlds");
        foreach (int tldid in Paging.TldIdList)
        {
          XmlElement tldElement = (XmlElement)AddNode(tldsElement, "tld");
          AddAttribute(tldElement, "id", tldid.ToString());
        }
      }

      return requestDoc.InnerXml;
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      StringBuilder dataBuilder = new StringBuilder();
      dataBuilder.Append(ShopperID);
      dataBuilder.AppendFormat(".{0}", Paging.SortOrderField);
      dataBuilder.AppendFormat(".{0}", Paging.SortOrder == SortOrderType.Ascending ? "ASC" : "DESC");
      dataBuilder.AppendFormat(".{0}", !string.IsNullOrEmpty(Paging.SearchTerm) ? Paging.SearchTerm : "none");
      dataBuilder.AppendFormat(".{0}", Paging.RowsPerPage);
      dataBuilder.AppendFormat(".{0}", Paging.ExpirationDays.HasValue ? Paging.ExpirationDays.Value.ToString() : "none");
      dataBuilder.AppendFormat(".{0}", Paging.SummaryOnly ? "true" : "false");
      dataBuilder.AppendFormat(".{0}", Paging.StatusType.HasValue ? Paging.StatusType.Value : -1);
      
      if(Paging.IncludeBoundary && !string.IsNullOrEmpty(Paging.BoundaryField))
      {
        dataBuilder.AppendFormat(".{0}.{1}", Paging.BoundaryField, Paging.BoundaryFieldValue);

        if(!string.IsNullOrEmpty(Paging.BoundaryUniquifierField) && !string.IsNullOrEmpty(Paging.BoundaryUniquifierFieldValue))
        {
          dataBuilder.AppendFormat(".{0}.{1}", Paging.BoundaryUniquifierField, Paging.BoundaryUniquifierFieldValue);
        }

        dataBuilder.AppendFormat(".{0}", Paging.NavigatingForward ? "forward" : "backward");
      }

      if(Paging.TldIdList != null && Paging.TldIdList.Count > 0)
      {
        dataBuilder.AppendFormat(".{0}", "TldIds:");
        foreach (int tldId in Paging.TldIdList)
        {
          dataBuilder.AppendFormat("{0},", tldId);
        }
      }

      var data = Encoding.UTF8.GetBytes(dataBuilder.ToString());

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}
