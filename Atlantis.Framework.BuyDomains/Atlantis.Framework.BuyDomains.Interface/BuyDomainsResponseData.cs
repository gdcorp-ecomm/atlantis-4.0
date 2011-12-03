using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyDomains.Interface
{
  public class BuyDomainsResponseData : IResponseData
  {
    /******************************************************************************/

    string m_sResponseXML                                 = null;
    AtlantisException m_ex                                = null;
    Dictionary<string, BuyDomainAttributes> m_dictDomains = null;

    /******************************************************************************/

    public BuyDomainsResponseData(string sDomainXML)
    {
      m_sResponseXML = sDomainXML;
    }

    /******************************************************************************/

    public BuyDomainsResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    /******************************************************************************/

    public BuyDomainsResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData,
                                   "BuyDomainsResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    /******************************************************************************/

    public Dictionary<string, BuyDomainAttributes> Domains
    {
      get 
      {
        if (m_dictDomains == null)
        {
          m_dictDomains = new Dictionary<string, BuyDomainAttributes>(StringComparer.OrdinalIgnoreCase);
          PopulateFromXML();
        }

        return m_dictDomains; 
      }
    }

    /******************************************************************************/

    private void PopulateFromXML()
    {
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(m_sResponseXML);

      XmlNodeList xnlPremiumDomains = xdDoc.SelectNodes("/brokerageSearch/response/premiumdomain");

      foreach (XmlElement xlPremiumDomain in xnlPremiumDomains)
      {
        string sFullName = xlPremiumDomain.InnerXml;
        float fPrice = 0.0f;
        int iIsFastTransfer = 0;

        float.TryParse(xlPremiumDomain.GetAttribute("price"), out fPrice);
        int.TryParse(xlPremiumDomain.GetAttribute("is-fast-transfer"), out iIsFastTransfer);

        //m_dictDomains.Add(sFullName.ToUpper(), new BuyDomainAttributes(sFullName, fPrice, iIsFastTransfer == 1));

        m_dictDomains.Add(sFullName, new BuyDomainAttributes(sFullName, fPrice, iIsFastTransfer == 1));
      }
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    /******************************************************************************/

    #region IResponseData Members

    /******************************************************************************/

    public AtlantisException GetException()
    {
      return m_ex;
    }

    /******************************************************************************/

    public string ToXML()
    {
      return m_sResponseXML;
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
