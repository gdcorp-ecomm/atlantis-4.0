using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Backorder.Interface
{
  public class BackorderResponseData : IResponseData
  {
    /******************************************************************************/

    string m_sResponseXML                 = null;
    AtlantisException m_ex                = null;
    Dictionary<string, int> m_dictDomains = null;

    /******************************************************************************/

    public BackorderResponseData(string sBackorderXML)
    {
      m_sResponseXML = sBackorderXML;
    }

    /******************************************************************************/

    public BackorderResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    /******************************************************************************/

    public BackorderResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData,
                                   "BackorderResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    /******************************************************************************/

    public KeyValuePair<string, int> FirstDomain
    {
      get
      {
        if (m_dictDomains == null)
        {
          m_dictDomains = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
          PopulateFromXML();
        }

        IEnumerator<KeyValuePair<string, int>> oEnum = m_dictDomains.GetEnumerator();
        oEnum.MoveNext();

        return oEnum.Current;
      }
    }

    /******************************************************************************/

    public Dictionary<string, int> Domains
    {
      get
      {
        if (m_dictDomains == null)
        {
          m_dictDomains = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
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

      XmlNodeList xnlDomains = xdDoc.SelectNodes("/results/domains/domain");

      foreach (XmlElement xlDomain in xnlDomains)
      {
        int iResultCode = -1;
        string sFullName = xlDomain.GetAttribute("name");

        int.TryParse(xlDomain.GetAttribute("result"), out iResultCode);

        m_dictDomains.Add(sFullName.ToUpper(), iResultCode);
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
