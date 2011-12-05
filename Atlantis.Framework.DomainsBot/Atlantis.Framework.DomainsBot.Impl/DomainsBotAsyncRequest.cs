using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainsBot.Impl.domainsBot;
using Atlantis.Framework.DomainsBot.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBot.Impl
{
  public class DomainsBotAsyncRequest : DomainsBotRequestBase, IAsyncRequest
  {
    /******************************************************************************/

    #region IAsyncRequest Members

    /******************************************************************************/

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      DomainsBotRequestData oDomainsBotRequestData = (DomainsBotRequestData)oRequestData;

      List<Tld> oTldList = GetTldList(oDomainsBotRequestData.TLDs);

      FirstImpact oRequest = new FirstImpact();
      oRequest.Url = ((WsConfigElement)oConfig).WSURL;
      oRequest.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oRequest, oState);

      IAsyncResult oAsyncResult = oRequest.BeginGetDomainsEx(oDomainsBotRequestData.SearchKey,
                                                             oDomainsBotRequestData.MaxResults,
                                                             oDomainsBotRequestData.ExcludeTaken,
                                                             oTldList.ToArray(),
                                                             oDomainsBotRequestData.AddPrefixes,
                                                             oDomainsBotRequestData.AddSuffixes,
                                                             oDomainsBotRequestData.AddDashes,
                                                             oDomainsBotRequestData.AddRelated,
                                                             oDomainsBotRequestData.AdvancedSplit,
                                                             oDomainsBotRequestData.BaseOnTop,
                                                             oDomainsBotRequestData.SessionId,
                                                             oCallback,
                                                             oAsyncState);
      return oAsyncResult;
    }

    /******************************************************************************/

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;
      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        FirstImpact oRequest = (FirstImpact)oAsyncState.Request;
        SearchResult oResponse = (SearchResult)oRequest.EndGetDomainsEx(oAsyncResult);

        oResponseData = new DomainsBotResponseData(oResponse.AvailableResults);
        foreach (Domain DomainSuggestion in oResponse.Domains)
        {
          ((DomainsBotResponseData)oResponseData).AddDomain(DomainSuggestion.Name);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainsBotResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainsBotResponseData(oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }

    /******************************************************************************/

    #endregion
  }
}
