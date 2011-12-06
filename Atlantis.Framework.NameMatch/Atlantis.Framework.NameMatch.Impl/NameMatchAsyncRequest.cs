using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.NameMatch.Interface;
using Service = Atlantis.Framework.NameMatch.Impl.NameMatchService;

namespace Atlantis.Framework.NameMatch.Impl
{
  public class NameMatchAsyncRequest : NameMatchRequestBase, IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      NameMatchRequestData oNameMatchRequestData = (NameMatchRequestData)oRequestData;

      Service.gdDppSearchWS oRequest = new Service.gdDppSearchWS();
      oRequest.Url = ((WsConfigElement)oConfig).WSURL;
      oRequest.Timeout = (int)Math.Truncate(oNameMatchRequestData.RequestTimeout.TotalMilliseconds);

      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oRequest, oState);

      IAsyncResult oAsyncResult = oRequest.BegindppAvailableDomains(GetRequestXML(oNameMatchRequestData), oCallback, oAsyncState);

      return oAsyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      NameMatchResponseData oResponseData = null;
      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        Service.gdDppSearchWS oRequest = (Service.gdDppSearchWS)oAsyncState.Request;
        AvailableDomain[] oResponse = GetAvailableDomains(oRequest.EnddppAvailableDomains(oAsyncResult), oAsyncState.RequestData);

        if (oResponse != null)
        {
          Dictionary<string, List<KeyValuePair<string, string>>> _domainList = new Dictionary<string, List<KeyValuePair<string, string>>>();

          foreach (AvailableDomain domainSuggestion in oResponse)
          {
            if (domainSuggestion.Data != null)
            {
              if (domainSuggestion.Data.Length > 0)
              {
                _domainList[domainSuggestion.DomainName] = GetData(domainSuggestion.Data[0]);
              }
            }
            else
            {
              throw new ArgumentException("No DomainsBotData");
            }
          }

          oResponseData = new NameMatchResponseData(oResponse.Length, _domainList, oResponse);
        }
        else
        {
          oResponseData = new NameMatchResponseData(false);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new NameMatchResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new NameMatchResponseData(oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
