using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductOffer.Interface
{
  public class ProductOfferResponseData : IResponseData
  {
    Dictionary<int, string> _productOfferings;
    AtlantisException _exception;

    public ProductOfferResponseData(Dictionary<int, string> productOfferings)
    {
      _productOfferings = productOfferings;
    }

    public ProductOfferResponseData(Dictionary<int, string> productOfferings, AtlantisException exAtlantis)
    {
      _productOfferings = productOfferings;
      _exception = exAtlantis;
    }

    public ProductOfferResponseData(Dictionary<int, string> dictProdcutOfferings, 
                                    RequestData requestData, 
                                    Exception ex)
    {
      _productOfferings = dictProdcutOfferings;
      _exception = new AtlantisException(requestData,
                                   "ProductOfferResponseData",
                                   ex.Message,
                                   requestData.ToXML(), ex);
    }

    public Dictionary<int, string> ProductOfferings
    {
      get { return _productOfferings; }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("ProductOfferings");

      if (_productOfferings != null)
      {
        foreach (KeyValuePair<int, string> oPair in _productOfferings)
        {
          xtwResult.WriteStartElement("Product");
          xtwResult.WriteAttributeString("group_id", oPair.Key.ToString());
          xtwResult.WriteAttributeString("description", oPair.Value);
          xtwResult.WriteEndElement(); // Product
        }
      }

      xtwResult.WriteEndElement(); // ProductOfferings

      return sbResult.ToString();
    }

    #endregion
  }
}
