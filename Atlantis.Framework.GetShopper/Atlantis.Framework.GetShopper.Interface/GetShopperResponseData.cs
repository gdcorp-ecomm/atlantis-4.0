using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetShopper.Interface
{
  public class GetShopperResponseData : IResponseData
  {
    // **************************************************************** //

    XmlDocument m_xdResponse = null;
    string m_sResponseXML = null;
    AtlantisException m_ex;

    // **************************************************************** //

    public GetShopperResponseData(string sResponseXML)
    {
      m_sResponseXML = sResponseXML;
      m_ex = null;
    }

    // **************************************************************** //

    public GetShopperResponseData(string sResponseXML, AtlantisException ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = ex;
    }

    // **************************************************************** //

    public GetShopperResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData,
                                   "GetShopperResponseData", 
                                   ex.Message.ToString(), 
                                   oRequestData.ToXML());
    }

    // **************************************************************** //

    public string GetField(string sName)
    {
      PopulateFromXML();

      string sValue = "";
      string sXPath = String.Format("/Shopper/Fields/Field[@Name='{0}']", sName);
      XmlNode xnField = m_xdResponse.SelectSingleNode(sXPath);

      if (xnField != null)
        sValue = xnField.InnerText;

      return sValue;
    }

    // **************************************************************** //

    public long GetInterestPref(long lCommTypeID, long lInterestTypeID)
    {
      PopulateFromXML();

      long lValue = 0;
      string sXPath = String.Format("/Shopper/Preferences/Interest[@CommTypeID='{0}' and @InterestTypeID='{1}']",
                                    lCommTypeID,
                                    lInterestTypeID);
      XmlElement xlInterest = m_xdResponse.SelectSingleNode(sXPath) as XmlElement;

      if (xlInterest != null)
        lValue = XmlConvert.ToInt32(xlInterest.GetAttribute("OptIn"));

      return lValue;
    }

    // **************************************************************** //

    public long GetCommunicationPref(long lCommTypeID)
    {
      PopulateFromXML();

      long lValue = 0;
      string sXPath = String.Format("/Shopper/Preferences/Communication[@CommTypeID='{0}']",
                                    lCommTypeID);
      XmlElement xlComm = m_xdResponse.SelectSingleNode(sXPath) as XmlElement;

      if (xlComm != null)
        lValue = XmlConvert.ToInt32(xlComm.GetAttribute("OptIn"));

      return lValue;
    }

    /******************************************************************************/

    private void PopulateFromXML()
    {
      if (null != m_xdResponse)
        return;

      m_xdResponse = new XmlDocument();

      m_xdResponse.LoadXml(m_sResponseXML);
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
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
