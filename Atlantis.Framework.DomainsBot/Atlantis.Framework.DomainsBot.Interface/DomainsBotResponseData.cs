using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBot.Interface
{
  public class DomainsBotResponseData : IResponseData
  {
    /******************************************************************************/

    AtlantisException m_ex = null;
    List<string> m_lstDomainNames = new List<string>();
    int m_iAvailableResults = 0;

    /******************************************************************************/

    public DomainsBotResponseData(int iAvailableResults)
    {
      m_iAvailableResults = iAvailableResults;
    }

    /******************************************************************************/

    public DomainsBotResponseData(AtlantisException exAtlantis)
    {
      m_ex = exAtlantis;
    }

    /******************************************************************************/

    public DomainsBotResponseData(RequestData oRequestData, Exception ex)
    {
      m_ex = new AtlantisException(oRequestData,
                                   "DomainsBotResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    /******************************************************************************/

    public int AvailableResults
    {
      get { return m_iAvailableResults; }
    }

    /******************************************************************************/

    public void AddDomain(string sDomain)
    {
      m_lstDomainNames.Add(sDomain);
    }

    /******************************************************************************/

    public List<string> DomainNames
    {
      get
      {
        return m_lstDomainNames;
      }
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
      throw new NotImplementedException();
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
