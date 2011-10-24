using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainsBotSpins.Interface;
using Atlantis.Framework.DomainsBotSpins.Interface.domainsBotSpinsService;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotSpins.Impl
{
  public class DomainsBotSpinsAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      DomainsBotSpinsRequestData oDomainsBotSpinsRequestData = (DomainsBotSpinsRequestData)oRequestData;

      FirstImpact3 oRequest = new FirstImpact3();
      oRequest.Url = ((WsConfigElement)oConfig).WSURL;

      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oRequest, oState);

      IAsyncResult oAsyncResult = oRequest.BeginSearchAvailableDomainsWithTargets(oDomainsBotSpinsRequestData.SearchKey
                                                                                  , GetTLDs(oDomainsBotSpinsRequestData.TLDs)
                                                                                  , oDomainsBotSpinsRequestData.MaxResults
                                                                                  , oDomainsBotSpinsRequestData.RemoveKeys
                                                                                  , oDomainsBotSpinsRequestData.Filters
                                                                                  , oDomainsBotSpinsRequestData.Targets
                                                                                  , oDomainsBotSpinsRequestData.SupportedLang
                                                                                  , oCallback
                                                                                  , oAsyncState);
      return oAsyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      DomainsBotSpinsResponseData oResponseData = null;
      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        FirstImpact3 oRequest = (FirstImpact3)oAsyncState.Request;
        Domain[] oResponse = oRequest.EndSearchAvailableDomainsWithTargets(oAsyncResult) as Domain[];
        if (oResponse != null)
        {
          Dictionary<string, List<KeyValuePair<string, string>>> _domainList = new Dictionary<string, List<KeyValuePair<string, string>>>(); ;

          foreach (Domain DomainSuggestion in oResponse)
          {
            _domainList.Add(DomainSuggestion.DomainName, GetData(DomainSuggestion.Data[0]));
          }

          oResponseData = new DomainsBotSpinsResponseData(oResponse.Length, _domainList);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainsBotSpinsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainsBotSpinsResponseData(oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private List<KeyValuePair<string, string>> GetData(DomainData[] domainData)
    {
      List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

      foreach (DomainData item in domainData)
      {
        if (item.Data != null)
        {
          data.Add(new KeyValuePair<string, string>(item.Name, item.Data.ToString()));
        }
      }

      return data;
    }

    private string GetTLDs(System.Collections.Generic.List<string> list)
    {
      return String.Join(", ", list.ToArray());
    }
  }
}
