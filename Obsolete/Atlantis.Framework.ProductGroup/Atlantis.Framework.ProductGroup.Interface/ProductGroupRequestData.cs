using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductGroup.Interface
{
  /// <summary>
  /// Requests Product Group data.  Cacheable.
  /// </summary>
  public class ProductGroupRequestData : RequestData
  {
    // **************************************************************** //

    int m_iUnifiedProductGroupID;

    // **************************************************************** //

    public ProductGroupRequestData(string sShopperID,
                                   string sSourceURL,
                                   string sOrderID,
                                   string sPathway,
                                   int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_iUnifiedProductGroupID = 0;
    }

    // **************************************************************** //


    public ProductGroupRequestData(string sShopperID,
                               string sSourceURL,
                               string sOrderID,
                               string sPathway,
                               int iPageCount,
                               int iUnifiedProductGroupID)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_iUnifiedProductGroupID = iUnifiedProductGroupID;
    }

    // **************************************************************** //

    public int UnifiedProductGroupID
    {
      get { return m_iUnifiedProductGroupID; }
      set { m_iUnifiedProductGroupID = value; }
    }

    // **************************************************************** //

    #region RequestData Members

    // **************************************************************** //

    public override string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("ProductGroup");

      xtwResult.WriteStartElement("param");
      xtwResult.WriteAttributeString("name", "n_gdshop_product_unifiedProductGroupID");
      xtwResult.WriteAttributeString("value", m_iUnifiedProductGroupID.ToString());
      xtwResult.WriteEndElement(); // param

      xtwResult.WriteEndElement(); // ProductGroup

      return sbResult.ToString();
    }

    // **************************************************************** //

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes
        = System.Text.ASCIIEncoding.ASCII.GetBytes(m_iUnifiedProductGroupID.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    // **************************************************************** //

    #endregion
  }
}
