using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBot.Interface
{
  /// <summary>
  /// Request for DomainsBot.  Non-cacheable
  /// </summary>
  public class DomainsBotRequestData : RequestData
  {
    /******************************************************************************/

    string m_sSearchKey = String.Empty;
    int m_iMaxResults = 10;
    bool m_bExcludeTaken = false;
    bool m_bAddPrefixes = false;
    bool m_bAddSuffixes = false;
    bool m_bAddDashes = false;
    bool m_bAddRelated = false;
    bool m_bAdvancedSplit = false;
    bool m_bBaseOnTop = false;
    List<string> m_lstTLDs = new List<string>();
    string m_sSessionId = String.Empty;

    /******************************************************************************/

    public DomainsBotRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public DomainsBotRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount,
                                 string sSearchKey,
                                 int iMaxResults,
                                 string sTLD)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SearchKey = sSearchKey;
      this.MaxResults = iMaxResults;
      AddTLD(sTLD);
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public DomainsBotRequestData(string sShopperID,
                             string sSourceURL,
                             string sOrderID,
                             string sPathway,
                             int iPageCount,
                             string sSearchKey,
                             int iMaxResults,
                             IEnumerable<string> oTLDs)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this.SearchKey = sSearchKey;
      this.MaxResults = iMaxResults;
      AddTLDs(oTLDs);
      RequestTimeout = TimeSpan.FromMilliseconds(3000);
    }

    /******************************************************************************/

    public string SearchKey
    {
      get { return m_sSearchKey; }
      set { m_sSearchKey = value; }
    }

    /******************************************************************************/

    public int MaxResults
    {
      get { return m_iMaxResults; }
      set { m_iMaxResults = value; }
    }

    /******************************************************************************/

    public bool AddDashes
    {
      get { return m_bAddDashes; }
      set { m_bAddDashes = value; }
    }

    /******************************************************************************/

    public bool AddRelated
    {
      get { return m_bAddRelated; }
      set { m_bAddRelated = value; }
    }

    /******************************************************************************/

    public bool AddPrefixes
    {
      get { return m_bAddPrefixes; }
      set { m_bAddPrefixes = value; }
    }

    /******************************************************************************/

    public bool AddSuffixes
    {
      get { return m_bAddSuffixes; }
      set { m_bAddSuffixes = value; }
    }

    /******************************************************************************/

    public bool ExcludeTaken
    {
      get { return m_bExcludeTaken; }
      set { m_bExcludeTaken = value; }
    }

    /******************************************************************************/

    public bool AdvancedSplit
    {
      get { return m_bAdvancedSplit; }
      set { m_bAdvancedSplit = value; }
    }

    /******************************************************************************/

    public bool BaseOnTop
    {
      get { return m_bBaseOnTop; }
      set { m_bBaseOnTop = value; }
    }

    /******************************************************************************/

    public string SessionId
    {
      get { return m_sSessionId; }
      set { m_sSessionId = value; }
    }

    /******************************************************************************/

    public List<string> TLDs
    {
      get { return m_lstTLDs; }
      set { m_lstTLDs = value; }
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
      throw new Exception("DomainsBot is not a cacheable request.");
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
