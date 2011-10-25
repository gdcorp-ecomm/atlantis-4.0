using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;

namespace Atlantis.Framework.MyaRenewalBundleItems.Interface
{
  public class MyaRenewalBundleItemsResponseData : IResponseData
  {
    private AtlantisException _atlException;

    private bool _isSuccess;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    private List<RenewalBundleItem> _childItemList;
    public List<RenewalBundleItem> ChildItemList
    {
      get
      {
        return _childItemList;
      }
    }

    private string _bundleXML;
    public string BundleXML
    {
      get
      {
        if (_bundleXML == null)
        {          
          XmlDocument xdoc = new XmlDocument();          
          XmlElement bundleParent = (XmlElement) xdoc.CreateNode(XmlNodeType.Element, "BUNDLE", "");
          xdoc.AppendChild(bundleParent);
          foreach (var item in ChildItemList)
          {
            XmlElement childElement = (XmlElement) xdoc.CreateNode(XmlNodeType.Element, "BUNDLEITEM", "");
            bundleParent.AppendChild(childElement);

            XmlAttribute indexAttribute = childElement.OwnerDocument.CreateAttribute("index");
            childElement.Attributes.Append(indexAttribute);
            indexAttribute.Value = item.PresentationSequenceID.ToString();

            XmlAttribute formAttribute = childElement.OwnerDocument.CreateAttribute("form_id");
            childElement.Attributes.Append(formAttribute);
            formAttribute.Value = item.ResourceRecurringID + "_1";

          }
          _bundleXML = xdoc.InnerXml;
        }
        return _bundleXML;  
      }
      
    }

    public MyaRenewalBundleItemsResponseData(List<RenewalBundleItem> productList)
    {
      _isSuccess = true;
      _childItemList = productList;
    }

    public MyaRenewalBundleItemsResponseData(AtlantisException exAtlantis)
    {
      _atlException = exAtlantis;
    }

    public MyaRenewalBundleItemsResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {
      _atlException = new AtlantisException(oRequestData, "MyaRenewalBundleItemsResponseData", ex.Message, string.Empty);
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }
  }
}
