﻿using System.Xml;

namespace Atlantis.Framework.EcommActivationData.Interface
{
  public class ActivatedProducts
  {
    private XmlNode _productData;
    public ActivatedProducts(XmlNode productNode)
    {
      _productData = productNode;
    }
    
    public XmlNode ProductData
    {
      get
      {
        return _productData;
      }
    }

    public string ProductType
    {
      get
      {
        return _productData.Name;
      }
    }

    public string Domain
    {
      get
      {
        return _productData.GetAttribute<string>("domain", string.Empty);
      }
    }

    public string Title
    {
      get
      {
        return _productData.GetAttribute<string>("title", string.Empty);
      }
    }

    public string Description
    {
      get
      {
        return _productData.GetAttribute<string>("description", string.Empty);
      }
    }

    public int BackgroundID
    {
      get
      {
        return _productData.GetAttribute<int>("backgroundID", -1);
      }
    }

    public string Email
    {
      get
      {
        return _productData.GetAttribute<string>("email", string.Empty);
      }
    }

    public string PromoCoe
    {
      get
      {
        return _productData.GetAttribute<string>("promoCode", string.Empty);
      }
    }

    public string Password
    {
      get
      {
        return _productData.GetAttribute<string>("password", string.Empty);
      }
    }

    public int Diskspace
    {
      get
      {
        return _productData.GetAttribute<int>("diskspaceMB",24000);
      }
    }

    public string HostingType
    {
      get
      {
        return _productData.GetAttribute<string>("hosting_type", string.Empty);
      }
    }

    public string WordPressAdminEmail
    {
      get
      {
        return _productData.GetAttribute<string>("wordPressAdminEmail", string.Empty);
      }
    }

    public string WordPressBlogName
    {
      get
      {
        return _productData.GetAttribute<string>("wordPressBlogName", string.Empty);
      }
    }

  }
}
