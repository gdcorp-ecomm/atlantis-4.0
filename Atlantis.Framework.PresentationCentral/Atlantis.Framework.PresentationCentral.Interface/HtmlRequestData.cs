using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresentationCentral.Interface
{
  [Serializable]
  [XmlRoot("PresentationCentral")]
  public class HtmlRequestData : RequestData
  {
    private int m_iPrivateLabelId;
    private string m_sShopperId;
    private HtmlParams m_Params;
    private HtmlXslt m_xslt;

    // **************************************************************** //

    private HtmlRequestData()
      : base("", "", "", "", 0)
    {
      // Needed to support serialization
    }

    // **************************************************************** //


    public HtmlRequestData(string transformName, string application, int iPrivateLabelId,
                            bool bIsSecure,
                            string sShopperID,
                            string sSourceURL,
                            string sOrderID,
                            string sPathway,
                            int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_iPrivateLabelId = iPrivateLabelId;
      m_sShopperId = sShopperID;
      ParamsElement = new HtmlParams(transformName, application, bIsSecure);
      XSLTElement = new HtmlXslt();
    }

    // **************************************************************** //

    [XmlAttribute("pl_id")]
    public int PrivateLabelId
    {
      get { return m_iPrivateLabelId; }
      set { m_iPrivateLabelId = value; }
    }

    // **************************************************************** //

    [XmlAttribute("shopper_id")]
    public string ShopperId
    {
      get { return m_sShopperId; }
      set { m_sShopperId = value; }
    }

    // **************************************************************** //

    [XmlElement("XSLTParams")]
    public HtmlParams ParamsElement
    {
      get { return m_Params; }
      set { m_Params = value; }
    }

    [XmlElement("XSLT")]
    public HtmlXslt XSLTElement
    {
      get { return m_xslt; }
      set { m_xslt = value; }
    }

    // **************************************************************** //

    #region RequestData Members

    // **************************************************************** //

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ToXML());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      return BitConverter.ToString(md5Bytes, 0).Replace("-", "");
    }

    // **************************************************************** //

    public override string ToXML()
    {
      StringBuilder sb = new StringBuilder(ObjectSerializer.SerializeToXml(this, true));
      while ((sb.Length > 0) && (sb[0] != '<'))
      {
        sb.Remove(0, 1);
      }
      return sb.ToString();
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }

}
