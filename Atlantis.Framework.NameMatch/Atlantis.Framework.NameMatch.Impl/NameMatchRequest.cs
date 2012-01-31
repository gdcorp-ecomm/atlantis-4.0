using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.NameMatch.Interface;
using Service = Atlantis.Framework.NameMatch.Impl.NameMatchService;

namespace Atlantis.Framework.NameMatch.Impl
{
  public class NameMatchRequest : NameMatchRequestBase, IRequest
  {
    #region IRequestMembers

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        NameMatchRequestData oNameMatchRequestData = (NameMatchRequestData)oRequestData;

        AvailableDomain[] oResponse;

        using (Service.gdDppSearchWS oRequest = new Service.gdDppSearchWS())
        {
          oRequest.Url = ((WsConfigElement)oConfig).WSURL;
          oRequest.Timeout = (int)Math.Truncate(oNameMatchRequestData.RequestTimeout.TotalMilliseconds);

          oResponse = GetAvailableDomains(oRequest.dppAvailableDomains(GetRequestXML(oNameMatchRequestData)), oRequestData);
        }

        if (oResponse != null && oResponse.Length > 0)
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
        oResponseData = new NameMatchResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
