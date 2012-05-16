using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.EcommPrunedActivationData.Interface
{
  public class ProductInfo
  {
    public const string PRODUCT_TYPE_INSTANTPAGE = "InstantPageSetup";
    public const string PRODUCT_TYPE_EMAIL = "EmailSetup";

    public const int ACTIVATION_STATUS_FAILURE = 0;
    public const int ACTIVATION_STATUS_PREPROCESS = 1;
    public const int ACTIVATION_STATUS_CHILD_INSERTED = 2;
    public const int ACTIVATION_STATUS_ACCOUNT_CREATED = 3;
    public const int ACTIVATION_STATUS_ACCOUNT_ACTIVATED = 4;
    public const int ACTIVATION_STATUS_BILLING_CONSOLIDATED = 5;
    private XmlNode _itemData;
    private List<ActivatedProducts> _products = new List<ActivatedProducts>();

    public ProductInfo(XmlNode itemNode)
    {
      _itemData = itemNode;
      foreach (XmlNode childNode in itemNode.ChildNodes)
      {
        ActivatedProducts currentProduct = new ActivatedProducts(childNode);
        _products.Add(currentProduct);
      }
    }

    public List<ActivatedProducts> ActivatedProducts
    {
      get
      {
        return _products;
      }
    }

    public int GdshopProductTypeID
    {
      get
      {
        return _itemData.GetAttribute<int>("gdshop_product_typeID", -1);
      }
    }

    public XmlNode ProductInfoNode
    {
      get
      {
        return _itemData;
      }
    }

    public int ProductID
    {
      get
      {
        return _itemData.GetAttribute<int>("pf_id", -1);
      }
    }

    public string ResourceID
    {
      get
      {
        return _itemData.GetAttribute<string>("resource_id", string.Empty);
      }
    }

    public string ExternalResourceID
    {
      get
      {
        return _itemData.GetAttribute<string>("externalResourceID", string.Empty).Trim(new char[] { '{', '}' });
      }
    }

    public int ActivationStatusID
    {
      get
      {
        return _itemData.GetAttribute<int>("gdshop_activationStatusID", -1);
      }
    }

  }
}
