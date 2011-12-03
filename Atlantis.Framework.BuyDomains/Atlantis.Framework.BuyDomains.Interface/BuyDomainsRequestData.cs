using System;
using System.Collections.Generic;
using System.Text;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyDomains.Interface
{
  public class BuyDomainsRequestData : RequestData
  {
    /******************************************************************************/

    string m_sSLD             = "";
    int m_iMaxResults         = 10;
    bool m_bRequirePrice      = true;
    bool m_bFastTransferOnly  = true;
    List<string> m_lstTLDs    = new List<string>();

    /******************************************************************************/

    public BuyDomainsRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public BuyDomainsRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount,
                                 string sSLD,
                                 int iMaxResults)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SLD = sSLD;
      this.MaxResults = iMaxResults;
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public BuyDomainsRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount,
                                 string sSLD,
                                 string sTLD,
                                 int iMaxResults)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SLD = sSLD;
      AddTLD(sTLD);
      this.MaxResults = iMaxResults;
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public BuyDomainsRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount,
                                 string sSLD,
                                 IEnumerable<string> oTLDs,
                                 int iMaxResults)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SLD = sSLD;
      AddTLDs(oTLDs);
      this.MaxResults = iMaxResults;
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public string GetQuery
    {
      get
      {
        StringBuilder sbResult = new StringBuilder();

        sbResult.AppendFormat("keyword={0}&maxResults={1}&", SLD.ToLower(), MaxResults);

        foreach (string sTLD in m_lstTLDs)
          sbResult.AppendFormat("tlds=.{0}&", sTLD);

        sbResult.AppendFormat("require_price={0}&fast-transfer-only={0}", RequirePrice ? "Y" : "N", 
                                                                          FastTransferOnly ? "Y" : "N");

        return sbResult.ToString();
      }
    }

    /******************************************************************************/

    public string SLD
    {
      get { return m_sSLD; }
      set { m_sSLD = value.Trim().ToLower(); }
    }

    /******************************************************************************/

    public int MaxResults
    {
      get { return m_iMaxResults; }
      set { m_iMaxResults = value; }
    }

    /******************************************************************************/

    public bool RequirePrice
    {
      get { return m_bRequirePrice; }
      set { m_bRequirePrice = value; }
    }

    /******************************************************************************/

    public bool FastTransferOnly
    {
      get { return m_bFastTransferOnly; }
      set { m_bFastTransferOnly = value; }
    }

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
      throw new Exception("BuyDomains is not a cacheable request.");
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
