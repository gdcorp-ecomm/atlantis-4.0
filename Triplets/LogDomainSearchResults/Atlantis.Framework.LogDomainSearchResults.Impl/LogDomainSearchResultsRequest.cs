using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogDomainSearchResults.Impl.LogDomainSearchResultsWS;
using Atlantis.Framework.LogDomainSearchResults.Interface;

namespace Atlantis.Framework.LogDomainSearchResults.Impl
{
  public class LogDomainSearchResultsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      LogDomainSearchResultsResponseData responseData;

      var request = (LogDomainSearchResultsRequestData)requestData;

      using (var service = new Service
        {
          Url = ((WsConfigElement)config).WSURL,
          Timeout = (int)request.RequestTimeout.TotalMilliseconds
        })
      {
        try
        {
          service.LogDomainSearchResults(request.ToXML());
          responseData = new LogDomainSearchResultsResponseData();
        }
        catch (Exception ex)
        {
          responseData = new LogDomainSearchResultsResponseData(requestData, ex);
        }
      }

      return responseData;
    }

    #endregion
  }
}