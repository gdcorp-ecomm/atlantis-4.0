using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainsBotSpins.Interface;
using Atlantis.Framework.DomainsBotSpins.Interface.domainsBotSpinsService;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotSpins.Impl
{
  public class DomainsBotSpinsRequest : IRequest
  {
    #region IRequestMembers

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        DomainsBotSpinsRequestData oDomainsBotSpinsRequestData = (DomainsBotSpinsRequestData)oRequestData;

        Domain[] oResponse;

        using (FirstImpact3 oRequest = new FirstImpact3())
        {
          oRequest.Url = ((WsConfigElement)oConfig).WSURL;
          oRequest.Timeout = (int)oDomainsBotSpinsRequestData.RequestTimeout.TotalMilliseconds;

          oResponse = oRequest.SearchAvailableDomainsWithTargets(oDomainsBotSpinsRequestData.SearchKey
                                                                  , GetTLDs(oDomainsBotSpinsRequestData.TLDs)
                                                                  , oDomainsBotSpinsRequestData.MaxResults
                                                                  , oDomainsBotSpinsRequestData.RemoveKeys
                                                                  , oDomainsBotSpinsRequestData.Filters
                                                                  , oDomainsBotSpinsRequestData.Targets
                                                                  , oDomainsBotSpinsRequestData.SupportedLang);
        }



        Dictionary<string, List<KeyValuePair<string, string>>> _domainList = new Dictionary<string, List<KeyValuePair<string, string>>>();;

        foreach (Domain DomainSuggestion in oResponse)
        {
          _domainList.Add(DomainSuggestion.DomainName, GetData(DomainSuggestion.Data[0]));
        }

        oResponseData = new DomainsBotSpinsResponseData(oResponse.Length, _domainList);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainsBotSpinsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainsBotSpinsResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

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

    #endregion

    private string GetTLDs(System.Collections.Generic.List<string> list)
    {
      return String.Join(", ", list.ToArray());
    }

  }
}

