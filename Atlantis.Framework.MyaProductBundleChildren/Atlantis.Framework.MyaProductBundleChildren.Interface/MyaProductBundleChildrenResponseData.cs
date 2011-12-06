using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MyaProductBundleChildren.Interface
{
  public class MyaProductBundleChildrenResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly AtlantisException _atlantisException;
    public IList<ChildProduct> BundleChildProducts { get; private set; }

    public bool IsSuccess { get; private set; }

    public MyaProductBundleChildrenResponseData()
    { }

    public MyaProductBundleChildrenResponseData(IList<ChildProduct> bundleChildProducts)
    {
      IsSuccess = true;
      BundleChildProducts = bundleChildProducts;
    }

    public MyaProductBundleChildrenResponseData(AtlantisException atlantisException)
    {
      BundleChildProducts = new List<ChildProduct>(1);
      _atlantisException = atlantisException;
    }

    public MyaProductBundleChildrenResponseData(RequestData requestData, Exception ex)
    {
      BundleChildProducts = new List<ChildProduct>(1);
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("MyaProductBundleChildren Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);                                   
    }


    #region IResponseData Members
    public string ToXML()
    {
      XDocument xDoc = new XDocument();
      XElement root = new XElement("children");

      foreach (ChildProduct cp in BundleChildProducts)
      {
        root.Add(new XElement("child",
          new XAttribute("child_resource_id", cp.BillingResourceId.ToString()),
          new XAttribute("commonName", cp.CommonName),
          new XAttribute("CustomerID", cp.CustomerId.ToString()),
          new XAttribute("externalResourceID", cp.OrionResourceId),
          new XAttribute("parent_bundle_id", cp.ParentBundleId.ToString()),
          new XAttribute("parent_bundle_product_typeID", cp.ParentBundleProductTypeId.ToString()),
          new XAttribute("pf_id", cp.ProductId.ToString()),
          new XAttribute("child_product_type_id", cp.ProductTypeId.ToString()),
          new XAttribute("recurring_payment", cp.RecurringPayment),
          new XAttribute("start_date", cp.StartDate.ToString())));
      }

      xDoc.Add(root);

      return xDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }
    #endregion

    #region ISessionSerializableResponse Members
    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      List<ChildProduct> bundleChildProducts = new List<ChildProduct>();
      int parentBillingResourceId = 0;

      if (!string.IsNullOrEmpty(sessionData))
      {
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(sessionData);
        XmlNodeList childProductNodes = xdoc.SelectNodes("children/child");

        if (childProductNodes != null)
        {
          foreach (XmlNode node in childProductNodes)
          {
            IDictionary<string, object> productProperties = new Dictionary<string, object>();

            foreach (XmlAttribute attribute in node.Attributes)
            {
              if (!productProperties.ContainsKey(attribute.Name))
              {
                if (attribute.Name.Equals("parent_bundle_id"))
                {
                  int.TryParse(attribute.Value, out parentBillingResourceId);
                }
                productProperties.Add(attribute.Name, attribute.Value);
              }
            }

            bundleChildProducts.Add(new ChildProduct(productProperties, parentBillingResourceId));
          }

          if (bundleChildProducts.Count > 0)
          {
            IsSuccess = true;
            BundleChildProducts = bundleChildProducts;
          }
        }
      }
    }
    #endregion
  }
}
