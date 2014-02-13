using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogDomainSearchResults.Impl.LogDomainSearchResultsWS;
using Atlantis.Framework.LogDomainSearchResults.Interface;

namespace Atlantis.Framework.LogDomainSearchResults.Impl
{
  public class LogDomainSearchResultsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      LogDomainSearchResultsResponseData oResponseData;

      var request = (LogDomainSearchResultsRequestData)oRequestData;

      using (var service = new Service
        {
          Url = ((WsConfigElement)oConfig).WSURL,
          Timeout = (int)request.RequestTimeout.TotalMilliseconds
        })
      {
        try
        {
          service.LogDomainSearchResults(request.ToXML());
          oResponseData = new LogDomainSearchResultsResponseData();
        }
        catch (Exception ex)
        {
          oResponseData = new LogDomainSearchResultsResponseData(oRequestData, ex);
        }
      }

      return oResponseData;
    }

    #endregion
  }
}