using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FabDomains.Interface
{
  public class FabDomainsRequestData : RequestData
  {
    /******************************************************************************/

    const string m_sGetFormat = "&returncount={0}&allowedextensions={1}&search={2}&negotiable=1";

    /******************************************************************************/

    string m_sSLD = "";
    string m_sClientIPAddress = "";
    int m_iReturnCount = 10;
    int m_iMinPrice = 0;
    int m_iMaxPrice = 0;
    List<string> m_lstTLDs = new List<string>();

    /******************************************************************************/

    public FabDomainsRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public FabDomainsRequestData(string sShopperID,
                             string sSourceURL,
                             string sOrderID,
                             string sPathway,
                             int iPageCount,
                             string sSLD,
                             int iReturnCount,
                             string sTLD,
                             string sClientIPAddress)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SLD = sSLD;
      this.ReturnCount = iReturnCount;
      this.ClientIPAddress = sClientIPAddress;
      AddTLD(sTLD);
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public FabDomainsRequestData(string sShopperID,
                             string sSourceURL,
                             string sOrderID,
                             string sPathway,
                             int iPageCount,
                             string sSLD,
                             int iReturnCount,
                             List<string> oTLDs,
                             string sClientIPAddress)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SLD = sSLD;
      this.ReturnCount = iReturnCount;
      this.ClientIPAddress = sClientIPAddress;
      AddTLDs(oTLDs);
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public string GetQuery
    {
      get
      {
        StringBuilder sbResult = new StringBuilder();

        sbResult.AppendFormat(m_sGetFormat, ReturnCount,
                                            String.Join(",", m_lstTLDs.ToArray()),
                                            SLD);

        if (ClientIPAddress.Length > 0)
          sbResult.AppendFormat("&ip={0}", ClientIPAddress);

        if (m_iMinPrice < m_iMaxPrice)
          sbResult.AppendFormat("&minprice={0}&maxprice={1}", m_iMinPrice, m_iMaxPrice);

        return sbResult.ToString();
      }
    }

    /******************************************************************************/

    public string SLD
    {
      get { return m_sSLD; }
      set { m_sSLD = value; }
    }

    /******************************************************************************/

    public int MinPrice
    {
      get { return m_iMinPrice; }
      set { m_iMinPrice = value; }
    }

    /******************************************************************************/

    public int MaxPrice
    {
      get { return m_iMaxPrice; }
      set { m_iMaxPrice = value; }
    }

    /******************************************************************************/

    public int ReturnCount
    {
      get { return m_iReturnCount; }
      set { m_iReturnCount = value; }
    }

    /******************************************************************************/

    public string ClientIPAddress
    {
      get { return m_sClientIPAddress; }
      set
      {
        m_sClientIPAddress = "";
        IPAddress address = null;
        if (IPAddress.TryParse(value, out address))
          m_sClientIPAddress = address.ToString();
      }
    }

    /******************************************************************************/

    public void AddTLD(string sTLD)
    {
      m_lstTLDs.Add(sTLD.Trim().ToLower());
    }

    /******************************************************************************/

    public void AddTLDs(IEnumerable<string> oTLDs)
    {
      foreach (string sTLD in oTLDs)
        AddTLD(sTLD);
    }

    /******************************************************************************/

    #region RequestData Members

    /******************************************************************************/

    public override string GetCacheMD5()
    {
      throw new Exception("FabDomains is not a cacheable request.");
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
