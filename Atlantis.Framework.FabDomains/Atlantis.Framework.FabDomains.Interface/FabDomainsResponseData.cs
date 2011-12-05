using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FabDomains.Interface
{
  public class FabDomainsResponseData : IResponseData
  {
    /******************************************************************************/

    string m_sResponseXML = null;
    AtlantisException m_ex = null;
    Dictionary<string, FabDomainAttributes> m_dictDomains = null;

    /******************************************************************************/

    public FabDomainsResponseData(string sDomainsXML)
    {
      m_sResponseXML = sDomainsXML;
    }

    /******************************************************************************/

    public FabDomainsResponseData(string sRequestXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sRequestXML;
      m_ex = exAtlantis;
    }

    /******************************************************************************/

    public FabDomainsResponseData(string sRequestXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sRequestXML;
      m_ex = new AtlantisException(oRequestData,
                                   "FabDomainsResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    /******************************************************************************/

    public Dictionary<string, FabDomainAttributes> Domains
    {
      get
      {
        if (m_dictDomains == null)
        {
          m_dictDomains = new Dictionary<string, FabDomainAttributes>(StringComparer.OrdinalIgnoreCase);
          PopulateListFromXML();
        }

        return m_dictDomains;
      }
    }

    /******************************************************************************/

    private void PopulateListFromXML()
    {
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(m_sResponseXML);

      XmlNodeList xnlResults = xdDoc.SelectNodes("/fabulousdomains/response/results/result");

      foreach (XmlElement xlResult in xnlResults)
      {
        string sDomainName = "";
        string sOwnerType = string.Empty;
        float fPrice = 0.0f;
        float fCommissionPct = 0.0f;
        float fCommissionBonusPct = 0.0f;
        float fWaterlinePrice = 0.0f;
        float fNegotiablePrice = 0.0f;

        XmlElement xlDomain = xlResult["ucdomain"];
        if (xlDomain != null)
          sDomainName = xlDomain.InnerText;

        XmlElement xlOwnerType = xlResult["owner_type"];
        if (xlOwnerType != null)
          sOwnerType = xlOwnerType.InnerText;

        XmlElement xlPrice = xlResult["price"];
        if (xlPrice != null)
          float.TryParse(xlPrice.InnerText, out fPrice);

        XmlElement xlCommissionPct = xlResult["commissionpct"];
        if (xlCommissionPct != null)
          float.TryParse(xlCommissionPct.InnerText, out fCommissionPct);

        XmlElement xlCommissionBonusPct = xlResult["commissionbonuspct"];
        if (xlCommissionBonusPct != null)
          float.TryParse(xlCommissionBonusPct.InnerText, out fCommissionBonusPct);

        XmlElement xlWaterlinePrice = xlResult["waterlineprice"];
        if (xlWaterlinePrice != null)
          float.TryParse(xlWaterlinePrice.InnerText, out fWaterlinePrice);

        XmlElement xlNegotiablePrice = xlResult["negotiableprice"];
        if (xlNegotiablePrice != null)
          float.TryParse(xlNegotiablePrice.InnerText, out fNegotiablePrice);

        //m_dictDomains.Add(sDomainName.ToUpper(), new FabDomainAttributes(sDomainName, sOwnerType,
        //  fPrice, fWaterlinePrice, fNegotiablePrice, fCommissionPct, fCommissionBonusPct));

        m_dictDomains.Add(sDomainName, new FabDomainAttributes(sDomainName, sOwnerType,
          fPrice, fWaterlinePrice, fNegotiablePrice, fCommissionPct, fCommissionBonusPct));
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
