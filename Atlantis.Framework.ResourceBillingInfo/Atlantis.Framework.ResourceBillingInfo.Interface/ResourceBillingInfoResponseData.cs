using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using System.Xml;

namespace Atlantis.Framework.ResourceBillingInfo.Interface
{
  public class ResourceBillingInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties
    private readonly AtlantisException _exception;
    private bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public IList<GdshopResourceBillingInfo> ResourceBillingInfos { get; private set; }
    #endregion

    public ResourceBillingInfoResponseData()
    { }

    public ResourceBillingInfoResponseData(IList<GdshopResourceBillingInfo> resourceBillingInfos)
    {
      ResourceBillingInfos = resourceBillingInfos;
      _success = true;
    }

     public ResourceBillingInfoResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public ResourceBillingInfoResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "ResourceBillingInfoResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      var xDoc = new XDocument();
      var root = new XElement("resources");

      foreach (GdshopResourceBillingInfo resource in ResourceBillingInfos)
      {
        root.Add(new XElement("resource",
          new XAttribute("billingResourceId", resource.BillingResourceId.ToString()),
          new XAttribute("shopperId", resource.ShopperId),
          new XAttribute("productTypeId", resource.ProductTypeId.ToString()),
          new XAttribute("orderId", resource.OrderId),
          new XAttribute("rowId", resource.RowId.ToString()),
          new XAttribute("privateLabelId", resource.PrivateLabelId.ToString()),
          new XAttribute("pfid", resource.PfId.ToString()),
          new XAttribute("billingStatusId", resource.BillingStatusId.ToString()),
          new XAttribute("billingAttemptId", resource.BillingAttemptId.ToString()),
          new XAttribute("billingDate", resource.BillingDate.ToString()),
          new XAttribute("lastRenewalDate", resource.LastRenewalDate.ToString()),
          new XAttribute("expirationDate", resource.ExpirationDate.ToString()),
          new XAttribute("ppShopperProfileId", resource.PpShopperProfileId.HasValue ? resource.PpShopperProfileId.Value.ToString() : "null"),
          new XAttribute("originalListPrice", resource.OriginalListPrice.ToString()),
          new XAttribute("parentBundleResourceId", resource.ParentBundleResourceId.HasValue ? resource.ParentBundleResourceId.Value.ToString() : "null"),
          new XAttribute("parentProductTypeId", resource.ParentProductTypeId.HasValue ? resource.ParentProductTypeId.Value.ToString() : "null"),
          new XAttribute("freeProductPackageId", resource.FreeProductPackageId.HasValue ? resource.FreeProductPackageId.Value.ToString() : "null"),
          new XAttribute("cancelDate", resource.CancelDate.HasValue ? resource.CancelDate.Value.ToString() : "null"),
          new XAttribute("cancelledBy", string.IsNullOrEmpty(resource.CancelledBy) ? "null" : resource.CancelledBy),
          new XAttribute("createDate", resource.CreateDate.HasValue ? resource.CreateDate.Value.ToString() : "null"),
          new XAttribute("lastExpirationDate", resource.LastExpirationDate.HasValue ? resource.LastExpirationDate.Value.ToString() : "null"),
          new XAttribute("billingCredits", resource.BillingCredits.ToString()),
          new XAttribute("quantity", resource.Quantity.HasValue ? resource.Quantity.Value.ToString() : "null"),
          new XAttribute("isPastDue", resource.IsPastDue.ToString()),
          new XAttribute("isFree", resource.IsFree.ToString()),
          new XAttribute("isAutoRenew", resource.IsAutoRenew.ToString()),
          new XAttribute("parentResourceId", resource.ParentResourceId.HasValue ? resource.ParentResourceId.Value.ToString() : "null"),
          new XAttribute("isRenewalPriceLocked", resource.IsRenewalPriceLocked.ToString())));
      }

      xDoc.Add(root);

      return xDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion


    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (string.IsNullOrEmpty(sessionData))
        return;
     
      var rbis = new List<GdshopResourceBillingInfo>();

      var xdoc = new XmlDocument();
      xdoc.LoadXml(sessionData);
      XmlNodeList resourceNodes = xdoc.SelectNodes("resources/resource");

      if (resourceNodes == null)
        return;

      foreach (XmlNode node in resourceNodes)
      {
        IDictionary<string, object> billingPropertiesDictionary = new Dictionary<string, object>();

        foreach (XmlAttribute attribute in node.Attributes)
        {
          if (!billingPropertiesDictionary.ContainsKey(attribute.Name))
          {
            billingPropertiesDictionary.Add(attribute.Name, attribute.Value);
          }
        }
        rbis.Add(new GdshopResourceBillingInfo(billingPropertiesDictionary, true));
      }

      if (rbis.Count > 0)
      {
        _success = true;
        ResourceBillingInfos = rbis;
      }
    }
    #endregion
  }
}
