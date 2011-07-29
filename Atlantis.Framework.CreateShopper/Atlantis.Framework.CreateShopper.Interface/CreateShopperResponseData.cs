using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CreateShopper.Interface
{
  public class CreateShopperResponseData : IResponseData
  {
    // **************************************************************** //

    XmlDocument m_xdResponse = new XmlDocument();
    string m_sResponseXML = null;
    AtlantisException m_ex;

    // **************************************************************** //

    public CreateShopperResponseData(string sResponseXML)
    {
      m_sResponseXML = sResponseXML;
      m_ex = null;
    }

    // **************************************************************** //
    public CreateShopperResponseData(string sResponseXML, AtlantisException ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = ex;
    }

    // **************************************************************** //

    public CreateShopperResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData,
                                   "ShopperResponseData",
                                   ex.Message.ToString(),
                                   oRequestData.ToXML());
    }

    // **************************************************************** //

    public string GetShopperId()
    {
      string sValue = "";

      m_xdResponse.LoadXml(m_sResponseXML);
      XmlNode xnField = m_xdResponse.SelectSingleNode("/Shopper");

      if (xnField != null)
      {
        XmlAttribute attr = (XmlAttribute)xnField.Attributes.GetNamedItem("ID");
        if (attr != null)
        {
          sValue = attr.InnerText;
        }
      }
      return sValue;
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("Shopper", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    // **************************************************************** //

    #region IResponseData Members

    // **************************************************************** //

    public string ToXML()
    {
      return m_sResponseXML;
    }

    // **************************************************************** //

    public AtlantisException GetException()
    {
      return m_ex;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
